namespace RegexMapper
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    using SafeMapper;

    public class RegexMap<T>
    {
        private readonly RegexMapConfiguration config;

        private readonly IFormatProvider formatProvider;

        public RegexMap()
            : this(StringOperation.None)
        {
        }

        public RegexMap(StringOperation operations)
            : this(new RegexMapConfiguration { GlobalStringOperation = operations })
        {
            this.formatProvider = CultureInfo.InvariantCulture;
        }

        public RegexMap(RegexMapConfiguration config)
        {
            this.config = config;
        }

        public T Match(string pattern, string input)
        {
            return this.Matches(pattern, input).FirstOrDefault();
        }

        public T Match(Regex regex, string input)
        {
            return this.Matches(regex, input).FirstOrDefault();
        }

        public T Match(Regex regex, string input, string[] groupNames)
        {
            return this.Matches(regex, input, groupNames).FirstOrDefault();
        }

        public IList<T> Matches(string pattern, string input)
        {
            return this.Matches(new Regex(pattern), input);
        }

        public IList<T> Matches(Regex regex, string input)
        {
            var groupNames = regex.GetGroupNames().Skip(1).ToArray();
            return this.Matches(regex, input, groupNames);
        }

        public IList<T> Matches(Regex regex, string input, string[] groupNames)
        {
            var matchList = new List<Dictionary<string, string>>();

            // Fix groupnames
            for (int i = 0; i < groupNames.Length; i++)
            {
                if (this.IsNumeric(groupNames[i]))
                {
                    groupNames[i] = string.Format("Property{0}", groupNames[i]);
                }
            }

            var dict = new Dictionary<string, string>();
            foreach (Match match in regex.Matches(input))
            {
                var foundSuccessGroup = false;
                var notmatchedGroupNames = new List<string>();
                for (var i = 0; i < groupNames.Length; i++)
                {
                    var groupName = groupNames[i];

                    if ((i + 1) < match.Groups.Count && match.Groups[i + 1].Success)
                    {
                        foundSuccessGroup = true;
                        if (dict.ContainsKey(groupName))
                        {
                            matchList.Add(dict);
                            dict = new Dictionary<string, string>();
                            foreach (var notmatchedGroupName in notmatchedGroupNames)
                            {
                                dict.Add(notmatchedGroupName, null);
                            }
                        }

                        var value = this.config.ProcessGlobalStringOperations(match.Groups[i + 1].Value);

                        dict.Add(groupName, value);
                    }
                    else
                    {
                        // To preserve order of matched groups mark the groupName as used
                        // only if it has not already found a group with successful match
                        if (!foundSuccessGroup)
                        {
                            notmatchedGroupNames.Add(groupName);
                            if (!dict.ContainsKey(groupName))
                            {
                                dict.Add(groupName, null);
                            }
                        }
                    }
                }
            }

            matchList.Add(dict);

            if (typeof(T) == typeof(string) || typeof(T) == typeof(decimal) || typeof(T).IsPrimitive)
            {
                var values = new List<string>();
                foreach (var d in matchList)
                {
                    values.AddRange(d.Values);
                }

                return SafeMap.Convert<List<string>, List<T>>(values, formatProvider);
            }

            return SafeMap.Convert<List<Dictionary<string, string>>, List<T>>(matchList, formatProvider);
        }

        private bool IsNumeric(string input)
        {
            return input.All(t => (t >= 48) && (t <= 57));
        }
    }
}
