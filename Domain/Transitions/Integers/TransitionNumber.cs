namespace Nt.Automaton.Transitions.Integers
{
    internal class TransitionNumber(int value) : TransitionToken
    {
        public int Value { get; } = value;

        public override bool Accepts(int value)
        {
            return Value == value;
        }
    }

}
