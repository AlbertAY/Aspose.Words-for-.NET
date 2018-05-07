﻿// Copyright (c) 2001-2017 Aspose Pty Ltd. All Rights Reserved.
//
// This file is part of Aspose.Words. The source code in this file
// is only intended as a supplement to the documentation, and is provided
// "as is", without warranty of any kind, either expressed or implied.
//////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using Aspose.Words;
using Aspose.Words.Markup;
using NUnit.Framework;
using System.IO;

namespace ApiExamples
{
    /// <summary>
    /// Tests that verify work with structured document tags in the document 
    /// </summary>
    [TestFixture]
    internal class ExStructuredDocumentTag : ApiExampleBase
    {
        [Test]
        public void RepeatingSection()
        {
            //ExStart
            //ExFor:StructuredDocumentTag.SdtType
            //ExSummary:Shows how to get type of structured document tag.
            Document doc = new Document(MyDir + "TestRepeatingSection.docx");

            NodeCollection sdTags = doc.GetChildNodes(NodeType.StructuredDocumentTag, true);

            foreach (StructuredDocumentTag sdTag in sdTags)
            {
                Console.WriteLine("Type of this SDT is: {0}",sdTag.SdtType);
            }
            //ExEnd
            StructuredDocumentTag sdTagRepeatingSection = (StructuredDocumentTag)sdTags[0];
            Assert.AreEqual(SdtType.RepeatingSection, sdTagRepeatingSection.SdtType);

            StructuredDocumentTag sdTagRichText = (StructuredDocumentTag)sdTags[1];
            Assert.AreEqual(SdtType.RichText, sdTagRichText.SdtType);
        }

        [Test]
        public void CheckBox()
        {
            //ExStart
            //ExFor:StructuredDocumentTag.#ctor(DocumentBase, SdtType, MarkupLevel)
            //ExFor:StructuredDocumentTag.Checked
            //ExSummary:Show how to create and insert checkbox structured document tag.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            StructuredDocumentTag sdtCheckBox = new StructuredDocumentTag(doc, SdtType.Checkbox, MarkupLevel.Inline);
            sdtCheckBox.Checked = true;

            // Insert content control into the document
            builder.InsertNode(sdtCheckBox);
            //ExEnd
            MemoryStream dstStream = new MemoryStream();
            doc.Save(dstStream, SaveFormat.Docx);

            NodeCollection sdts = doc.GetChildNodes(NodeType.StructuredDocumentTag, true);

            StructuredDocumentTag sdt = (StructuredDocumentTag)sdts[0];
            Assert.AreEqual(true, sdt.Checked);
        }

        [Test]
        public void CreatingCustomXml()
        {
            //ExStart
            //ExFor:CustomXmlPart
            //ExFor:CustomXmlPartCollection.Add(String, String)
            //ExFor:XmlMapping.SetMapping(CustomXmlPart, String, String)
            //ExSummary:Shows how to create structured document tag with a custom XML data.
            Document doc = new Document();
            // Add test XML data part to the collection.
            CustomXmlPart xmlPart = doc.CustomXmlParts.Add(Guid.NewGuid().ToString("B"), "<root><text>Hello, World!</text></root>");

            StructuredDocumentTag sdt = new StructuredDocumentTag(doc, SdtType.PlainText, MarkupLevel.Block);
            sdt.XmlMapping.SetMapping(xmlPart, "/root[1]/text[1]", "");

            doc.FirstSection.Body.AppendChild(sdt);

            doc.Save(MyDir + @"\Artifacts\SDT.CustomXml.docx");
            //ExEnd
            Assert.IsTrue(DocumentHelper.CompareDocs(MyDir + @"\Artifacts\SDT.CustomXml.docx", MyDir + @"\Golds\SDT.CustomXml Gold.docx"));
        }

        [Test]
        public void ClearTextFromSdt()
        {
            //ExStart
            //ExFor:StructuredDocumentTag.Clear
            //ExSummary:Shows how to delete content of StructuredDocumentTag elements.
            Document doc = new Document(MyDir + "TestRepeatingSection.docx");

            NodeCollection sdts = doc.GetChildNodes(NodeType.StructuredDocumentTag, true);
            Assert.IsNotNull(sdts);

            foreach (StructuredDocumentTag sdt in sdts)
            {
                sdt.Clear();
            }
            //ExEnd
            MemoryStream dstStream = new MemoryStream();
            doc.Save(dstStream, SaveFormat.Docx);

            sdts = doc.GetChildNodes(NodeType.StructuredDocumentTag, true);

            Assert.AreEqual("Enter any content that you want to repeat, including other content controls. You can also insert this control around table rows in order to repeat parts of a table.\r", sdts[0].GetText());
            Assert.AreEqual("Click here to enter text.\f", sdts[2].GetText());
        }

        [Test, Order(1)]
        public void SetSdtColor()
        {
            //ExStart
            //ExFor:StructuredDocumentTag.Color
            //ExSummary:Shows how to set color of a content control.
            Document doc = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc);

            StructuredDocumentTag sdtPlainText = new StructuredDocumentTag(doc, SdtType.PlainText, MarkupLevel.Inline);
            // The Color affects content control in two situations:
            // 1) MSW highlights the background of the content control when the mouse moves over the content control. This helps user to identify that it is a content control.
            //    The color of highlighting is a bit "softer", than the Color.For example, MSW highlights background with the pink color, when Color is Red.
            // 2) When user interacts (editing, picking, etc) with the content control, the border of content control is colored with the Color.
            sdtPlainText.Color = Color.Red;
            
            builder.InsertNode(sdtPlainText);

            doc.Save(MyDir + @"\Artifacts\StructuredDocumentTag.SetStructuredDocumentTagColor.docx");
            //ExEnd
        }

        [Test, Order(2)]
        public void ChangeSdtColor()
        {
            Document doc = new Document(MyDir + @"\Artifacts\StructuredDocumentTag.SetStructuredDocumentTagColor.docx");

            StructuredDocumentTag sdt = (StructuredDocumentTag)doc.GetChild(NodeType.StructuredDocumentTag, 0, true);
            Assert.AreEqual(Color.Red.ToArgb(), sdt.Color.ToArgb());

            sdt.Color = Color.Green;

            MemoryStream dstStream = new MemoryStream();
            doc.Save(dstStream, SaveFormat.Docx);

            StructuredDocumentTag sdtNew = (StructuredDocumentTag)doc.GetChild(NodeType.StructuredDocumentTag, 0, true);
            Assert.AreEqual(Color.Green.ToArgb(), sdtNew.Color.ToArgb());
        }

        [Test, Order(3)]
        public void EditColorOfExistingSdt()
        {
            Document doc = new Document(MyDir + "Sdt.docx");

            StructuredDocumentTag sdt = (StructuredDocumentTag)doc.GetChild(NodeType.StructuredDocumentTag, 0, true);
            Assert.AreEqual(Color.Red.ToArgb(), sdt.Color.ToArgb());

            sdt.Color = Color.Green;

            MemoryStream dstStream = new MemoryStream();
            doc.Save(dstStream, SaveFormat.Docx);

            StructuredDocumentTag sdtNew = (StructuredDocumentTag)doc.GetChild(NodeType.StructuredDocumentTag, 0, true);
            Assert.AreEqual(Color.Green.ToArgb(), sdtNew.Color.ToArgb());
        }
    }
}
