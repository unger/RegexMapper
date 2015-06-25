namespace RegexMapper.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class FluentConfigTests
    {
        [Test]
        public void BuildConfiguration_None()
        {
            var config = new FluentConfig().BuildConfiguration();

            Assert.True(config.GlobalStringOperation == StringOperation.None);
        }

        [Test]
        public void BuildConfiguration_Trim()
        {
            var config = new FluentConfig().Trim().BuildConfiguration();

            Assert.True(config.GlobalStringOperation == StringOperation.Trim);
        }

        [Test]
        public void BuildConfiguration_UpperCaseFirst()
        {
            var config = new FluentConfig().UpperCaseFirst().BuildConfiguration();

            Assert.True(config.GlobalStringOperation == StringOperation.UpperCaseFirst);
        }

        [Test]
        public void BuildConfiguration_HtmlDecode()
        {
            var config = new FluentConfig().HtmlDecode().BuildConfiguration();

            Assert.True(config.GlobalStringOperation == StringOperation.HtmlDecode);
        }

        [Test]
        public void BuildConfiguration_Trim_UpperCaseFirst_HtmlDecode()
        {
            var config = new FluentConfig().Trim().UpperCaseFirst().HtmlDecode().BuildConfiguration();

            Assert.True((config.GlobalStringOperation & StringOperation.HtmlDecode) == StringOperation.HtmlDecode);
        }

    }
}
