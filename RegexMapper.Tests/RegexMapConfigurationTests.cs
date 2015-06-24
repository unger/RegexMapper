namespace RegexMapper.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class RegexMapConfigurationTests
    {
        [TestCase(null, Result = null)]
        [TestCase("", Result = "")]
        [TestCase("Test", Result = "Test")]
        [TestCase("Test ", Result = "Test")]
        [TestCase(" Test", Result = "Test")]
        [TestCase(" Test ", Result = "Test")]
        public string ProcessGlobalStringOperations_Trim(string value)
        {
            var config = new RegexMapConfiguration { GlobalStringOperation = StringOperation.Trim };

            return config.ProcessGlobalStringOperations(value);
        }

        [TestCase(null, Result = null)]
        [TestCase("", Result = "")]
        [TestCase("test", Result = "Test")]
        [TestCase("Test", Result = "Test")]
        [TestCase("åäö", Result = "Åäö")]
        public string ProcessGlobalStringOperations_UppercaseFirst(string value)
        {
            var config = new RegexMapConfiguration { GlobalStringOperation = StringOperation.UpperCaseFirst };

            return config.ProcessGlobalStringOperations(value);
        }

        [TestCase(null, Result = null)]
        [TestCase("", Result = "")]
        [TestCase("Test", Result = "Test")]
        [TestCase("&aring;&auml;&ouml;", Result = "åäö")]
        public string ProcessGlobalStringOperations_HtmlDecode(string value)
        {
            var config = new RegexMapConfiguration { GlobalStringOperation = StringOperation.HtmlDecode };

            return config.ProcessGlobalStringOperations(value);
        }

        [TestCase(null, Result = null)]
        [TestCase("", Result = "")]
        [TestCase("test", Result = "Test")]
        [TestCase("&aring;&auml;&ouml;", Result = "Åäö")]
        public string ProcessGlobalStringOperations_HtmlDecode_UpperCaseFirst(string value)
        {
            var config = new RegexMapConfiguration { GlobalStringOperation = StringOperation.HtmlDecode | StringOperation.UpperCaseFirst };

            return config.ProcessGlobalStringOperations(value);
        }

        [TestCase(null, Result = null)]
        [TestCase("", Result = "")]
        [TestCase(" test ", Result = "Test")]
        [TestCase(" &aring;&auml;&ouml; ", Result = "Åäö")]
        public string ProcessGlobalStringOperations_HtmlDecode_UpperCaseFirst_Trim(string value)
        {
            var config = new RegexMapConfiguration { GlobalStringOperation = StringOperation.HtmlDecode | StringOperation.UpperCaseFirst | StringOperation.Trim };

            return config.ProcessGlobalStringOperations(value);
        }
    }
}
