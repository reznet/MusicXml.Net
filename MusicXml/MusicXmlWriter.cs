using MusicXml.Domain;
using System.Xml;

namespace MusicXml
{
    public static class MusicXmlWriter
    {
        public static void Write(Score score, XmlWriter writer)
        {
            writer.WriteStartElement("score-partwise");
            // REVIEW: what version should we be writing?
            writer.WriteAttributeString("version", "3.0");

            writer.WriteEndElement();
        }
    }
}
