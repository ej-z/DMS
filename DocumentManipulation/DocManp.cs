﻿using System;
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
        string attributeRegexExpr = @"{{([a-zA-Z0-9]+)}}";

        string bitRegexExpr = @"\[\[([a-zA-Z0-9]+)\]\]";

        string repeaterRegexExpr = @"{{([a-zA-Z0-9]+)\|([a-zA-Z0-9]+)}}";

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
                foreach (Text text in body.Descendants<Text>())
                {
                    MatchCollection mc1 = attributeRegex.Matches(text.Text);
                    foreach (Match m in mc1)
                    {
                        var name = m.Groups[1].Value;
                        text.Text = text.Text.Replace(name.ToAttributeString(), inputs.Attributes[name].FinalValue);
                    }
                }


                Regex bitRegex = new Regex(bitRegexExpr);
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
                        var r = t.Descendants<TableRow>().First(tbl => bitRegex.IsMatch(tbl.InnerText));
                        foreach (Text text in r.Descendants<Text>())
                        {
                            MatchCollection mc2 = bitRegex.Matches(text.Text);
                            foreach (Match m in mc1)
                            {
                                var group = m.Groups[1].Value;
                                text.Text = text.Text.Replace(group.ToGroupString(), availableBitAttributes[group][i].Label);
                            }
                        }
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
                                text.Text = text.Text.Replace(name.ToRepeaterString(m.Groups[1].Value), repeater.GetAttribute(i,name).Value);
                            }
                        }
                    }
                }


            }
        } 
    }
}
