using MusicXml.Domain;
using System.Xml;
using System;

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

            writer.WriteEndElement();
        }

        private static void WritePartsList(Score score, XmlWriter writer)
        {
            writer.WriteStartElement("part-list");

            writer.WriteEndElement();
        }
    }
}
