using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml;

namespace DocumentManipulation
{
    public class DocManp
    {
        string attributeRegexExpr = @"{{([a-zA-Z0-9]+)\|(String|Number|Date|TextArea)}}";

        string repeaterRegexExpr = @"{{([a-zA-Z0-9]+)\|([a-zA-Z0-9]+)\|(String|Number|Date|TextArea)}}";

        public DocInputs ReadDoc(string srcfilename)
        {
            DocInputs inputs = new DocInputs();
            Regex attributeRegex = new Regex(attributeRegexExpr);
            Regex repeaterRegex = new Regex(repeaterRegexExpr);
            using (var document = WordprocessingDocument.Open(srcfilename, false))
            {     
                var main = document.MainDocumentPart;
                var doc = main.Document;
                var body = doc.Body;
                foreach (Paragraph para in body.Descendants<Paragraph>())
                {
                    MatchCollection mc1 = attributeRegex.Matches(para.InnerText);
                    foreach (Match m in mc1)
                    {
                        inputs.AddAttribute(m.Groups[1].Value, m.Groups[2].Value);
                    }
                }

                foreach (Table t in body.Descendants<Table>().Where(tbl => repeaterRegex.IsMatch(tbl.InnerText)))
                {
                    var repeaterName = repeaterRegex.Match(t.InnerText).Groups[1].Value;
                    var repeater = inputs.AddRepeater(repeaterName);
                    int pos = repeater.AddAttributeCollection();
                    MatchCollection mc1 = repeaterRegex.Matches(t.InnerText);
                    foreach (Match m in mc1)
                    {
                        repeater.AddAttribute(pos, m.Groups[2].Value, m.Groups[3].Value);
                    }
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
                foreach (Text text in body.Descendants<Text>())
                {
                    MatchCollection mc1 = attributeRegex.Matches(text.Text);
                    foreach (Match m in mc1)
                    {
                        var name = m.Groups[1].Value;
                        text.Text = text.Text.Replace(name.ToAttributeString(m.Groups[2].Value), inputs.Attributes[name].Value);
                    }
                }

                Regex repeaterRegex = new Regex(repeaterRegexExpr);
                foreach (Table t in body.Descendants<Table>().Where(tbl => repeaterRegex.IsMatch(tbl.InnerText)))
                {
                    var repeaterName = repeaterRegex.Match(t.InnerText).Groups[1].Value;
                    var repeater = inputs.Repeaters[repeaterName];
                    var row = t.Descendants<TableRow>().Skip(1).First();
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
                        var r = t.Descendants<TableRow>().Skip(1+i).First();
                        foreach (Text text in r.Descendants<Text>())
                        {
                            MatchCollection mc1 = repeaterRegex.Matches(text.Text);
                            foreach (Match m in mc1)
                            {
                                var name = m.Groups[2].Value;
                                text.Text = text.Text.Replace(name.ToRepeaterString(m.Groups[1].Value,m.Groups[3].Value), repeater.GetAttribute(i,name).Value);
                            }
                        }
                    }
                }
            }       
        } 
    }
}
