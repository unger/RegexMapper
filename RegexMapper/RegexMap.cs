namespace RegexMapper
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using SafeMapper;

    public class RegexMap
    {
        public IList<T> Matches<T>(string pattern, string input)
        {
            return this.Matches<T>(new Regex(pattern), input);
        }

        public IList<T> Matches<T>(Regex regex, string input)
        {
            var groupNames = regex.GetGroupNames().Skip(1).ToArray();
            return this.Matches<T>(regex, input, groupNames);
        }

        public IList<T> Matches<T>(Regex regex, string input, string[] groupNames)
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
                for (var i = 0; i < groupNames.Length; i++)
                {
                    var groupName = groupNames[i];

                    if ((i + 1) < match.Groups.Count && match.Groups[i + 1].Success)
                    {
                        if (dict.ContainsKey(groupName))
                        {
                            matchList.Add(dict);
                            dict = new Dictionary<string, string>();
                        }

                        dict.Add(groupName, match.Groups[i + 1].Value);
                    }
                }
            }

            matchList.Add(dict);

            return SafeMap.Convert<List<Dictionary<string, string>>, List<T>>(matchList);
        }

        private bool IsNumeric(string input)
        {
            return input.All(t => (t >= 48) || (t <= 57));
        }
    }
}
