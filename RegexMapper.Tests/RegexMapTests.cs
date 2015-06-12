namespace RegexMapper.Tests
{
    using System.Text.RegularExpressions;

    using NUnit.Framework;

    using RegexMapper.Tests.Model;

    [TestFixture]
    public class RegexMapTests
    {
        [Test]
        public void TestMapping_WithGroupNamesDefinedInRegex()
        {
            var mapper = new RegexMap();

            var result = mapper.Matches<TestModel>(
                            new Regex(@"{Id:(?<Id>\d*),Name:""(?<Name>[^""]*)""}"),
                            @"{Id:1,Name:""Test1""},{Id:12,Name:""Test12""},{Id:123,Name:""Test123""},{Id:1234,Name:""Test1234""}");

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual("Test1", result[0].Name);
            Assert.AreEqual(1234, result[3].Id);
            Assert.AreEqual("Test1234", result[3].Name);
        }

        [Test]
        public void TestMapping_WithGroupNamesDefinedInFunction()
        {
            var mapper = new RegexMap();

            var result = mapper.Matches<TestModel>(
                            new Regex(@"{Id:(\d*),Name:""([^""]*)""}"),
                            @"{Id:1,Name:""Test1""},{Id:12,Name:""Test12""},{Id:123,Name:""Test123""},{Id:1234,Name:""Test1234""}",
                            new[] { "Id", "Name" });

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual("Test1", result[0].Name);
            Assert.AreEqual(1234, result[3].Id);
            Assert.AreEqual("Test1234", result[3].Name);
        }
    }
}
