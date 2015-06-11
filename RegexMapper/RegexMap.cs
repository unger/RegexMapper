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
    }
}
