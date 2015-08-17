using System;
using MusicXml.Domain;
using NUnit.Framework;
using System.Xml;

namespace MusicXml.Unit.Tests
{
    public class MusicXmlWriterTests
    {
        [TestFixture]
        public class WhenScoreIsEmpty
        {
            private XmlDocument _document;

            [TestFixtureSetUp]
            public void SetUp()
            {
                var emptyScore = new Score();
                using (XmlWriter writer = XmlWriter.Create("out.xml"))
                {
                    MusicXmlWriter.Write(emptyScore, writer);
                }

                _document = new XmlDocument();
                _document.Load("out.xml");
            }

            [Test]
            public void MovementTitleIsNotWritten()
            {
                Assert.That(_document.SelectSingleNode("score-partwise/movement-title"), Is.Null);
            }
        }
    }
}
