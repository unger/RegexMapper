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
            var groupNames = regex.GetGroupNames();

            var matchList = (from Match match in regex.Matches(input)
                             select
                                 groupNames.ToDictionary(
                                     groupName => groupName,
                                     groupName => match.Groups[groupName].Value)).ToList();

            return SafeMap.Convert<List<Dictionary<string, string>>, List<T>>(matchList);
        }

        public IList<T> Matches<T>(Regex regex, string input, string[] groupNames)
        {
            var matchList = new List<Dictionary<string, string>>();

            foreach (Match match in regex.Matches(input))
            {
                var dict = new Dictionary<string, string>();

                for (int i = 0; i < groupNames.Length; i++)
                {
                    var groupName = groupNames[i];

                    if ((i + 1) < match.Groups.Count)
                    {
                        dict.Add(groupName, match.Groups[i + 1].Value);
                    }
                }

                matchList.Add(dict);
            }

            return SafeMap.Convert<List<Dictionary<string, string>>, List<T>>(matchList);
        }

        private bool IsNumeric(string input)
        {
            return input.All(t => (t >= 48) || (t <= 57));
        }
    }
}
