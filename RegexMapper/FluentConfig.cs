namespace RegexMapper
{
    using System;

    public class FluentConfig
    {
        private readonly RegexMapConfiguration config;

        public FluentConfig()
        {
            this.config = new RegexMapConfiguration();
        }

        public FluentConfig Trim()
        {
            this.config.GlobalStringOperation |= StringOperation.Trim;
            return this;
        }

        public FluentConfig UpperCaseFirst()
        {
            this.config.GlobalStringOperation |= StringOperation.UpperCaseFirst;
            return this;
        }

        public FluentConfig HtmlDecode()
        {
            this.config.GlobalStringOperation |= StringOperation.HtmlDecode;
            return this;
        }

        public FluentConfig FormatProvider(IFormatProvider provider)
        {
            this.config.FormatProvider = provider;
            return this;
        }

        public RegexMapConfiguration BuildConfiguration()
        {
            return this.config;
        }
    }
}
