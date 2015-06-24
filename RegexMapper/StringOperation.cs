namespace RegexMapper
{
    using System;

    [Flags]
    public enum StringOperation
    {
        None = 0,
        Trim = 1,
        HtmlDecode = 2,
        UpperCaseFirst = 4,
    }
}