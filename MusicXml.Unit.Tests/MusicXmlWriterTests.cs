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
            public void RootElementIsScorePartwise()
            {
                Assert.That(_document.DocumentElement.Name, Is.EqualTo("score-partwise"));
            }

            [Test]
            public void VersionIsThreeDotZero()
            {
                Assert.That(_document.DocumentElement.GetAttribute("version"), Is.EqualTo("3.0"));
            }

            [Test]
            public void EmptyPartListIsWritten()
            {
                var partList = _document.DocumentElement.SelectSingleNode("/score-partwise/part-list");
                Assert.That(partList, Is.Not.Null);
                Assert.That(partList.ChildNodes.Count, Is.EqualTo(0));
            }

            [Test]
            public void MovementTitleIsNotWritten()
            {
                Assert.That(_document.SelectSingleNode("score-partwise/movement-title"), Is.Null);
            }
        }

        public class WhenScoreHasSinglePart
        {
            [Test]
            public void ThenPartListIsWritten()
            {
                var score = new Score();
                score.Parts.Add(new Part());

                var document = WriteAndLoadScore(score);

                var partList = document.SelectSingleNode("score-partwise/part-list");
                Assert.That(partList, Is.Not.Null);
            }

            [Test]
            public void AndIdSetThenIdWritten()
            {
                var score = new Score();
                score.Parts.Add(new Part() { Id = "P1" });

                var document = WriteAndLoadScore(score);

                Assert.That(document.SelectSingleNode("/score-partwise/part-list/score-part[@id='P1']"), Is.Not.Null);
            }

            public void AndNameSetThenNameWritten()
            {
                var score = new Score();
                score.Parts.Add(new Part() { Id = "P1", Name = "Part 1" });

                var document = WriteAndLoadScore(score);

                Assert.That(document.SelectSingleNode("/score-partwise/part-list/score-part[@id='P1']/part-name[text()='Part 1']"), Is.Not.Null);
            }
        }

        private static XmlDocument WriteAndLoadScore(Score score)
        {
            using (XmlWriter writer = XmlWriter.Create("out.xml"))
            {
                MusicXmlWriter.Write(score, writer);
            }

            var document = new XmlDocument();
            document.Load("out.xml");

            return document;
        }
    }
}
