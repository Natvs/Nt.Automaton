using System.Text.RegularExpressions;

namespace Nt.Automaton.Transitions.Strings
{
    internal class RegexValue(string pattern) : StringToken
    {
        public string Pattern { get; } = "^" + pattern + "$";
        public override bool Accepts(string value)
        {
            return Regex.IsMatch(value, Pattern);
        }
    }

}
