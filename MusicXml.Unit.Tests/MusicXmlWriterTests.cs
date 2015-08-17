using System;
using MusicXml.Domain;
using NUnit.Framework;
using System.Xml;
using Microsoft.XmlDiffPatch;
using System.IO;

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

        [TestFixture]
        public class ReadingAndWritingAFileProducesOriginalInput
        {
            [Test]
            public void MusicXmlWithChords()
            {
                const string sourcePath = @"TestData/MusicXmlOneMeasure.xml";
                var document = WriteAndLoadScore(MusicXmlParser.GetScore(sourcePath));

                // TODO diff xml in the test
                XmlDiffOptions diffOptions = XmlDiffOptions.IgnoreComments | XmlDiffOptions.IgnoreDtd | XmlDiffOptions.IgnoreWhitespace | XmlDiffOptions.IgnoreXmlDecl;
                var diff = new XmlDiff(diffOptions);


                var same = false;
                var readerSettings = new XmlReaderSettings()
                {
                    DtdProcessing = DtdProcessing.Ignore
                };
                using (var originalReader = XmlReader.Create(sourcePath, readerSettings))
                using (var outputReader = XmlReader.Create("out.xml", readerSettings))
                using (XmlWriter diffgramWriter = XmlWriter.Create("diff.xml"))
                {
                    same = diff.Compare(originalReader, outputReader, diffgramWriter);
                }

                if(!same)
                {
                    var view = new XmlDiffView();
                    using (var reader = XmlReader.Create(sourcePath, readerSettings))
                    using (var diffgramReader = XmlReader.Create("diff.xml"))
                    {
                        view.Load(reader, diffgramReader);

                        using (var htmlWriter = new StreamWriter("diff.html"))
                        {
                            htmlWriter.Write("<html><body><table>");
                            view.GetHtml(htmlWriter);
                            htmlWriter.Write("</table></body></html>");
                            Console.WriteLine("Open {0} to view xml differences", Path.GetFullPath("diff.html"));
                        }
                    }

                    Assert.Fail("The xml written does not match the xml originally read.");
                }
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
