namespace Nt.Automaton.Transitions.Strings
{
    internal class StringValue(string value) : StringToken
    {
        public string Value { get; } = value;

        public override bool Accepts(string value)
        {
            return Value == value;
        }

    }

}
