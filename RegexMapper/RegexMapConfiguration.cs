namespace RegexMapper
{
    using System;
    using System.Globalization;
    using System.Web;

    public class RegexMapConfiguration
    {
        private IFormatProvider formatProvider;

        public StringOperation GlobalStringOperation { get; set; }

        public IFormatProvider FormatProvider
        {
            get
            {
                return this.formatProvider ?? (this.formatProvider = CultureInfo.InvariantCulture);
            }

            set
            {
                this.formatProvider = value;
            }
        }

        public string ProcessGlobalStringOperations(string value)
        {
            if (value == null)
            {
                return null;
            }

            if ((this.GlobalStringOperation & StringOperation.Trim) == StringOperation.Trim)
            {
                value = value.Trim();
            }

            if ((this.GlobalStringOperation & StringOperation.HtmlDecode) == StringOperation.HtmlDecode)
            {
                value = HttpUtility.HtmlDecode(value);
            }

            if ((this.GlobalStringOperation & StringOperation.UpperCaseFirst) == StringOperation.UpperCaseFirst)
            {
                if (value.Length > 0)
                {
                    value = value[0].ToString(CultureInfo.InvariantCulture).ToUpper() + value.Substring(1);
                }
            }

            return value;
        }
    }
}
