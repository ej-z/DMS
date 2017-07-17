using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using System.IO;
using System.Windows.Media.Imaging;

namespace DocumentManipulation
{
    public class DocManp
    {
        string attributeRegexExpr = @"{{([a-zA-Z0-9]+)}}";

        string bitRegexExpr = @"\[\[([a-zA-Z0-9]+)\]\]";

        string repeaterRegexExpr = @"{{([a-zA-Z0-9]+)\.([a-zA-Z0-9]+)}}";

        string photoExpr = @"\[\[.*\]\]";

        public DocInputs ReadDoc(string srcfilename)
        {
            DocInputs inputs = new DocInputs();
            using (var document = WordprocessingDocument.Open(srcfilename, false))
            {     
                var main = document.MainDocumentPart;
                var doc = main.Document;
                var body = doc.Body;

                foreach (TableRow row in body.Descendants<Table>().Last().Descendants<TableRow>())
                {
                    var cells = row.Descendants<TableCell>().ToArray();
                    inputs.AddInput(cells[0].InnerText, cells[1].InnerText, cells[2].InnerText);
                }
            }
            return inputs;
        }
        public void CreateDoc(DocInputs inputs, string srcfilename, string tarfilename)
        {
            using (var mainDoc = WordprocessingDocument.Open(srcfilename, false))
            using (var resultDoc = WordprocessingDocument.Create(tarfilename, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
            {
                foreach (var part in mainDoc.Parts)
                    resultDoc.AddPart(part.OpenXmlPart, part.RelationshipId);

                var main = resultDoc.MainDocumentPart;
                var doc = main.Document;
                var body = doc.Body;
                Regex attributeRegex = new Regex(attributeRegexExpr);
                Regex bitRegex = new Regex(bitRegexExpr);
                Regex repeaterRegex = new Regex(repeaterRegexExpr);
                Regex photoRegex = new Regex(photoExpr);
                foreach (Text text in body.Descendants<Text>())
                {
                    MatchCollection mc1 = attributeRegex.Matches(text.Text);
                    foreach (Match m in mc1)
                    {
                        var name = m.Groups[1].Value;
                        text.Text = text.Text.Replace(name.ToAttributeString(), inputs.Attributes[name].FinalValue);
                    }
                }
                
                foreach (Table t in body.Descendants<Table>().Where(tbl => bitRegex.IsMatch(tbl.InnerText)))
                {
                    MatchCollection mc1 = bitRegex.Matches(t.InnerText);
                    List<string> Groups = (from Match m in mc1 select m.Groups[1].Value).ToList();

                    var availableBitAttributes =
                        inputs.Attributes.Values.Where(x => x.Type == "Bit" && x.FinalValue == "True")
                            .Select(x => (BitAttribute) x)
                            .Where(x => Groups.Contains(x.Group))
                            .GroupBy(x => x.Group)
                            .ToDictionary(x => x.Key, x => x.ToList());

                    var maxGroupCount = availableBitAttributes.Values.OrderByDescending(x => x.Count).First().Count;
                    var row = t.Descendants<TableRow>().First(tbl => bitRegex.IsMatch(tbl.InnerText));
                    for (int i = 0; i < maxGroupCount - 1; i++)
                    {
                        var newRow = new TableRow();
                        foreach (TableCell c in row.Descendants<TableCell>())
                        {
                            newRow.Append(c.CloneNode(true));
                        }
                        t.Append(newRow.CloneNode(true));
                    }

                    for (int i = 0; i < maxGroupCount; i++)
                    {
                        var r = t.Descendants<TableRow>().Skip(i).First(tbl => bitRegex.IsMatch(tbl.InnerText));
                        foreach (Text text in r.Descendants<Text>())
                        {
                            MatchCollection mc2 = bitRegex.Matches(text.Text);
                            foreach (Match m in mc2)
                            {
                                var group = m.Groups[1].Value;
                                if (availableBitAttributes.ContainsKey(group) && i < availableBitAttributes[group].Count)
                                    text.Text = text.Text.Replace(group.ToGroupString(), availableBitAttributes[group][i].Label);
                                else
                                    text.Text = string.Empty;
                            }
                        }
                    }
                }          

                
                foreach (Table t in body.Descendants<Table>().Where(tbl => repeaterRegex.IsMatch(tbl.InnerText)))
                {
                    var repeaterName = repeaterRegex.Match(t.InnerText).Groups[1].Value;
                    var repeater = inputs.Repeaters[repeaterName];
                    var row = t.Descendants<TableRow>().First(tbl => repeaterRegex.IsMatch(tbl.InnerText));
                    for (int i = 0; i < repeater.Count - 1; i++)
                    {
                        var newRow = new TableRow();
                        foreach (TableCell c in row.Descendants<TableCell>())
                        {
                            newRow.Append(c.CloneNode(true));
                        }
                        t.Append(newRow.CloneNode(true));
                    }

                    for(int i = 0; i< repeater.Count;i++)
                    {
                        var r = t.Descendants<TableRow>().Skip(i).First(tbl => repeaterRegex.IsMatch(tbl.InnerText));
                        foreach (Text text in r.Descendants<Text>())
                        {
                            MatchCollection mc1 = repeaterRegex.Matches(text.Text);
                            foreach (Match m in mc1)
                            {
                                var name = m.Groups[2].Value;
                                var value = string.Empty;
                                if (name == "Photono")
                                    value = i.ToString();
                                else
                                    value = repeater.GetAttribute(i, name).FinalValue;
                                text.Text = text.Text.Replace(name.ToRepeaterString(m.Groups[1].Value), value);
                            }
                        }
                    }
                }

                foreach (Text text in body.Descendants<Text>())
                {
                    MatchCollection mc1 = photoRegex.Matches(text.Text);
                    foreach (Match m in mc1)
                    {
                        text.Text = string.Empty;
                        {
                            var repeater = inputs.Repeaters["Sample"];
                            Paragraph para = text.Ancestors<Paragraph>().FirstOrDefault();
                            if (para == null)
                                break;

                            for (int i = 0; i < repeater.Count; i++)
                            {
                                ImagePart imagePart = main.AddImagePart(ImagePartType.Jpeg);
                                var location = repeater.GetAttribute(i, "Photo").FinalValue;
                                if (!string.IsNullOrEmpty(location))
                                {
                                    using (FileStream stream = new FileStream(location, FileMode.Open))
                                    {
                                        imagePart.FeedData(stream);
                                    }
                                }
                                AddImageToBody(para, main.GetIdOfPart(imagePart), location);
                                Run run = para.AppendChild(new Run());
                                run.AppendChild(new Text("\r\nPhotograph " + i+1 + ". " + repeater.GetAttribute(i, "PhotoDescription").FinalValue + "\r\n\r\n"));
                            }
                        }
                    }
                }

                var configTable = body.Descendants<Table>().Last();
                configTable.Remove();
                var lastBreak = doc.Descendants<Break>().Last();
                lastBreak.Remove();
            }
        }

        private static void AddImageToBody(Paragraph para, string relationshipId, string filename)
        {
            var img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri(filename, UriKind.RelativeOrAbsolute);
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.EndInit();
            var widthPx = img.PixelWidth;
            var heightPx = img.PixelHeight;
            var horzRezDpi = img.DpiX;
            var vertRezDpi = img.DpiY;
            const int emusPerInch = 914400;
            const int emusPerCm = 360000;
            var maxWidthCm = 16.51;
            var widthEmus = (long)(widthPx / horzRezDpi * emusPerInch);
            var heightEmus = (long)(heightPx / vertRezDpi * emusPerInch);
            var maxWidthEmus = (long)(maxWidthCm * emusPerCm);
            if (widthEmus > maxWidthEmus)
            {
                var ratio = (heightEmus * 1.0m) / widthEmus;
                widthEmus = maxWidthEmus;
                heightEmus = (long)(widthEmus * ratio);
            }
            // Define the reference of the image.
            var element =
                 new Drawing(
                     new DW.Inline(
                         new DW.Extent() { Cx = widthEmus, Cy = heightEmus },
                         new DW.EffectExtent()
                         {
                             LeftEdge = 0L,
                             TopEdge = 0L,
                             RightEdge = 0L,
                             BottomEdge = 0L
                         },
                         new DW.DocProperties()
                         {
                             Id = (UInt32Value)1U,
                             Name = "Picture 1"
                         },
                         new DW.NonVisualGraphicFrameDrawingProperties(
                             new A.GraphicFrameLocks() { NoChangeAspect = true }),
                         new A.Graphic(
                             new A.GraphicData(
                                 new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(
                                         new PIC.NonVisualDrawingProperties()
                                         {
                                             Id = (UInt32Value)0U,
                                             Name = "New Bitmap Image.jpg"
                                         },
                                         new PIC.NonVisualPictureDrawingProperties()),
                                     new PIC.BlipFill(
                                         new A.Blip(
                                             new A.BlipExtensionList(
                                                 new A.BlipExtension()
                                                 {
                                                     Uri =
                                                        "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                 })
                                         )
                                         {
                                             Embed = relationshipId,
                                             CompressionState =
                                             A.BlipCompressionValues.Print
                                         },
                                         new A.Stretch(
                                             new A.FillRectangle())),
                                     new PIC.ShapeProperties(
                                         new A.Transform2D(
                                             new A.Offset() { X = 0L, Y = 0L },
                                             new A.Extents() { Cx = 990000L, Cy = 792000L }),
                                         new A.PresetGeometry(
                                             new A.AdjustValueList()
                                         )
                                         { Preset = A.ShapeTypeValues.Rectangle }))
                             )
                             { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                     )
                     {
                         DistanceFromTop = (UInt32Value)0U,
                         DistanceFromBottom = (UInt32Value)0U,
                         DistanceFromLeft = (UInt32Value)0U,
                         DistanceFromRight = (UInt32Value)0U,
                         EditId = "50D07946"
                     });

            // Append the reference to body, the element should be in a Run.
            para.AppendChild(new Run(element));
        }
    }
}
