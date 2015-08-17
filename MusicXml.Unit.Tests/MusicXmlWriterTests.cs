﻿using System;
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
            [TestFixture]
            public class AndPartIsEmpty
            {
                private XmlDocument _document;

                [Test]
                public void PartListIsWritten()
                {
                    var emptyScore = new Score();
                    emptyScore.Parts.Add(new Part());
                    using (XmlWriter writer = XmlWriter.Create("out.xml"))
                    {
                        MusicXmlWriter.Write(emptyScore, writer);
                    }

                    _document = new XmlDocument();
                    _document.Load("out.xml");

                    var scorePart = _document.SelectSingleNode("score-partwise/part-list/score-part");
                    Assert.That(scorePart, Is.Not.Null);
                    Assert.That(scorePart.Attributes["id"], Is.EqualTo("P1"));
                }
            }
        }
    }
}
