﻿namespace RegexMapper.Tests
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using NUnit.Framework;

    using RegexMapper.Tests.Model;

    [TestFixture]
    public class RegexMapTests
    {
        [Test]
        public void Matches_WithGroupNamesDefinedInRegex()
        {
            var mapper = new RegexMap<TestModel>();

            var result = mapper.Matches(
                            @"{Id:(?<Id>\d*),Name:""(?<Name>[^""]*)""}",
                            @"{Id:1,Name:""Test1""},{Id:12,Name:""Test12""},{Id:123,Name:""Test123""},{Id:1234,Name:""Test1234""}");

            Assert.AreEqual(
                new[]
                    {
                        new TestModel { Id = 1, Name = "Test1" }, 
                        new TestModel { Id = 12, Name = "Test12" },
                        new TestModel { Id = 123, Name = "Test123" }, 
                        new TestModel { Id = 1234, Name = "Test1234" }
                    },
                result);
        }

        [Test]
        public void Matches_WithGroupNamesDefinedInFunction()
        {
            var mapper = new RegexMap<TestModel>();

            var result = mapper.Matches(
                            new Regex(@"{Id:(\d*),Name:""([^""]*)""}"),
                            @"{Id:1,Name:""Test1""},{Id:12,Name:""Test12""},{Id:123,Name:""Test123""},{Id:1234,Name:""Test1234""}",
                            new[] { "Id", "Name" });

            Assert.AreEqual(
                new[]
                    {
                        new TestModel { Id = 1, Name = "Test1" }, 
                        new TestModel { Id = 12, Name = "Test12" },
                        new TestModel { Id = 123, Name = "Test123" }, 
                        new TestModel { Id = 1234, Name = "Test1234" }
                    },
                result);
        }

        [Test]
        public void Matches_WithToManyGroupNamesDefinedInFunction()
        {
            var mapper = new RegexMap<TestModel>();

            var result = mapper.Matches(
                            new Regex(@"{Id:(\d*),Name:""([^""]*)""}"),
                            @"{Id:1,Name:""Test1""},{Id:12,Name:""Test12""},{Id:123,Name:""Test123""},{Id:1234,Name:""Test1234""}",
                            new[] { "Id", "Name", "Property3" });

            Assert.AreEqual(
                new[]
                    {
                        new TestModel { Id = 1, Name = "Test1" }, 
                        new TestModel { Id = 12, Name = "Test12" },
                        new TestModel { Id = 123, Name = "Test123" }, 
                        new TestModel { Id = 1234, Name = "Test1234" }
                    },
                result);
        }

        [Test]
        public void Matches_WithMultipleMatchesMappedToOneObject()
        {
            var mapper = new RegexMap<TestModel>();

            var result = mapper.Matches(
                            new Regex(@"Id:(?<Id>\d*)|Name:""(?<Name>[^""]*)"""),
                            @"{Id:1,Name:""Test1""},{Id:12,Name:""Test12""},{Id:123,Name:""Test123""},{Id:1234,Name:""Test1234""}");

            Assert.AreEqual(
                new[]
                    {
                        new TestModel { Id = 1, Name = "Test1" }, 
                        new TestModel { Id = 12, Name = "Test12" },
                        new TestModel { Id = 123, Name = "Test123" }, 
                        new TestModel { Id = 1234, Name = "Test1234" }
                    },
                result);
        }

        [Test]
        public void Matches_WithMultipleNonSymetricMatchesMappedToOneObject()
        {
            var mapper = new RegexMap<TestModel>();

            var result = mapper.Matches(
                            new Regex(@"Id:(?<Id>\d*)|Name:""(?<Name>[^""]*)"""),
                            @"{Name:""Test1""},{Id:1}");

            Assert.AreEqual(
                new[]
                    {
                        new TestModel { Name = "Test1" }, 
                        new TestModel { Id = 1 }
                    },
                result);
        }

        [Test]
        public void Matches_WithMultipleNonSymetricMatchesMappedToOneObjectAndPreserveOrder()
        {
            var mapper = new RegexMap<TestModel>();

            var result = mapper.Matches(
                            new Regex(@"Id:(?<Id>\d*)|Name:""(?<Name>[^""]*)"""),
                            @"{Id:1},{Id:12,Name:""Test12""},{Name:""Test123""},{Id:1234,Name:""Test1234""}");

            Assert.AreEqual(
                new[]
                    {
                        new TestModel { Id = 1 }, 
                        new TestModel { Id = 12, Name = "Test12" },
                        new TestModel { Name = "Test123" }, 
                        new TestModel { Id = 1234, Name = "Test1234" }
                    },
                result);

            Assert.AreEqual(
                new[]
                    {
                        new TestModel { Id = 1 }, 
                        new TestModel { Id = 12, Name = "Test12" },
                        new TestModel { Name = "Test123" }, 
                        new TestModel { Id = 1234, Name = "Test1234" }
                    },
                result);
        }

        [Test]
        public void Matches_WithUnamedGroupsShouldReturnDictionaryWithCorrectKeys()
        {
            var mapper = new RegexMap<Dictionary<string, string>>();

            var result = mapper.Matches(
                            new Regex(@"{Id:(\d*),Name:""([^""]*)""}"),
                            @"{Id:1,Name:""Test1""},{Id:12,Name:""Test12""},{Id:123,Name:""Test123""},{Id:1234,Name:""Test1234""}");

            Assert.AreEqual(
                new[]
                    {
                        new Dictionary<string, string> { { "Property1", "1" }, { "Property2", "Test1" } }, 
                        new Dictionary<string, string> { { "Property1", "12" }, { "Property2", "Test12" } }, 
                        new Dictionary<string, string> { { "Property1", "123" }, { "Property2", "Test123" } }, 
                        new Dictionary<string, string> { { "Property1", "1234" }, { "Property2", "Test1234" } }
                    },
                result);
        }

        [Test]
        public void Matches_TestRegexMapOptionsTrim()
        {
            var mapper = new RegexMap<TestModel>(StringOperation.Trim);

            var result = mapper.Matches(
                            @"<id>(?<Id>.*?)</id><name>(?<Name>.*?)</name>",
                            @"<id>  1  </id><name>  Test1  </name>");

            Assert.AreEqual(
                new[]
                    {
                        new TestModel { Id = 1, Name = "Test1" }
                    },
                result);
        }

        [Test]
        public void Matches_TestRegexMapOptionsUpperCaseFirst()
        {
            var mapper = new RegexMap<TestModel>(StringOperation.UpperCaseFirst);

            var result = mapper.Matches(
                            @"<id>(?<Id>.*?)</id>|<name>(?<Name>.*?)</name>",
                            @"<id>1</id><name>test1</name><id>2</id><name>t</name><id>3</id><name></name><id>4</id>");

            Assert.AreEqual(
                new[]
                    {
                        new TestModel { Id = 1, Name = "Test1" },
                        new TestModel { Id = 2, Name = "T" },
                        new TestModel { Id = 3, Name = string.Empty },
                        new TestModel { Id = 4 }
                    },
                result);
        }

        [Test]
        public void Matches_TestRegexMapOptionsHtmlDecode()
        {
            var mapper = new RegexMap<TestModel>(StringOperation.HtmlDecode);

            var result = mapper.Matches(
                            @"<id>(?<Id>.*?)</id><name>(?<Name>.*?)</name>",
                            @"<id>1</id><name>T&auml;st1</name>");

            Assert.AreEqual(
                new[]
                    {
                        new TestModel { Id = 1, Name = "Täst1" }
                    },
                result);
        }

        [Test]
        public void Matches_TestStringArray()
        {
            var mapper = new RegexMap<string>();

            var result = mapper.Matches(
                            @"<([^/]+?)>",
                            @"<id>1</id><name>test</name>");

            Assert.AreEqual(
                new[]
                    {
                        "id",
                        "name"
                    },
                result);
        }

        [Test]
        public void Matches_TestIntArray()
        {
            var mapper = new RegexMap<int>();

            var result = mapper.Matches(
                            @"<.+?>([^<]*)</.+?>",
                            @"<id>1</id><name>2</name>");

            Assert.AreEqual(
                new[]
                    {
                        1,
                        2
                    },
                result);
        }

        [Test]
        public void Match_TestDecimal()
        {
            var mapper = new RegexMap<decimal>();

            var result = mapper.Match(
                            @"([\d]+\.[\d]+)",
                            @"<number>1.45</number>");

            Assert.AreEqual(1.45m, result);
        }
    }
}
