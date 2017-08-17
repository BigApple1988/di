using System.Collections.Generic;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudContainer.Tests
{
    [TestFixture]
    public class SimpleTagsParser_Should
    {
        private SimpleTagsParser parser;
        private string text = "simple\r\nSimple\r\ntext\r\nparser\r\n  \r\n \r\n\r\n\r\nв";
        [SetUp]
        public void SetUp()
        {
            var boringWords = A.Fake<IBoringWordsService>();
            A.CallTo(() => boringWords.GetBoringWords()).Returns(new HashSet<string>() {"в"});
            parser = new SimpleTagsParser(boringWords);
        }

        [Test]
        public void ExcludeBoringWords_IfContains()
        {
            parser.ParseTags(text).Should().NotContain(t => t.Item1 == "в");
        }

        [Test]
        public void ReturnTwoSimples_WhenDifferentCase()
        {
            parser.ParseTags(text).Should().Contain(t=>t.Item1=="simple"&&t.Item2==2);
        }

        [Test]
        public void ReturnOneWord_WhenOneWord()
        {
            parser.ParseTags(text).Should().Contain(t => t.Item1 == "text" && t.Item2 == 1);
        }

        [Test]
        public void NotContainNullOrEmptyOrCrRows_WhenExists()
        {
            parser.ParseTags(text).Should().NotContain(t => t.Item1 == ""||t.Item1==null||t.Item1=="\r"||t.Item1=="\n"||t.Item1=="\r\n");
        }


    }
}