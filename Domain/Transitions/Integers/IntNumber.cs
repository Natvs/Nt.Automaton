namespace Nt.Automaton.Transitions.Integers
{
    internal class IntNumber(int value) : IntToken
    {
        public int Value { get; } = value;

        public override bool Accepts(int value)
        {
            return Value == value;
        }
    }

}
