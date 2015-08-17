using MusicXml.Domain;
using System.Xml;
using System;
using System.Globalization;

namespace MusicXml
{
    public static class MusicXmlWriter
    {
        public static void Write(Score score, XmlWriter writer)
        {
            writer.WriteStartElement("score-partwise");
            // REVIEW: what version should we be writing?
            writer.WriteAttributeString("version", "3.0");

            WritePartsList(score, writer);

            foreach(var part in score.Parts)
            {
                WritePart(part, writer);
            }

            writer.WriteEndElement();
        }

        private static void WritePart(Part part, XmlWriter writer)
        {
            writer.WriteStartElement("part");
            writer.WriteAttributeString("id", part.Id);

            for(int i = 0; i < part.Measures.Count; i++)
            {
                WriteMeasure(part.Measures[i], i + 1, writer);
            }

            writer.WriteEndElement();
        }

        private static void WriteMeasure(Measure measure, int measureNumber, XmlWriter writer)
        {
            writer.WriteStartElement("measure");
            writer.WriteAttributeString("number", measureNumber.ToString(CultureInfo.InvariantCulture));

            if(measure.Attributes != null)
            {
                WriteMeasureAttributes(measure.Attributes, writer);
            }

            writer.WriteEndElement();
        }

        private static void WriteMeasureAttributes(MeasureAttributes attributes, XmlWriter writer)
        {
            writer.WriteStartElement("attributes");

            writer.WriteElementString("divisions", attributes.Divisions.ToString());

            if(attributes.Key != null)
            {
                writer.WriteStartElement("key");
                writer.WriteElementString("fifths", attributes.Key.Fifths.ToString());
                // TODO Key.Mode
                writer.WriteEndElement();
            }

            if(attributes.Time != null)
            {
                writer.WriteStartElement("time");
                writer.WriteElementString("beats", attributes.Time.Beats.ToString());
                writer.WriteElementString("beat-type", attributes.Time.Mode);
                // TODO: time symbol
                writer.WriteEndElement();
            }

            if(attributes.Clef != null)
            {
                writer.WriteStartElement("clef");
                writer.WriteElementString("sign", attributes.Clef.Sign);
                writer.WriteElementString("line", attributes.Clef.Line.ToString());
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        private static void WritePartsList(Score score, XmlWriter writer)
        {
            writer.WriteStartElement("part-list");

            foreach(var part in score.Parts)
            {
                WritePartInPartsList(part, writer);
            }

            writer.WriteEndElement();
        }

        private static void WritePartInPartsList(Part part, XmlWriter writer)
        {
            writer.WriteStartElement("score-part");

            writer.WriteAttributeString("id", part.Id);

            if(!string.IsNullOrEmpty(part.Name))
            {
                writer.WriteElementString("part-name", part.Name);
            }

            writer.WriteEndElement();
        }
    }
}
