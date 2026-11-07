namespace Nt.Automaton.Transitions.Integers
{
    internal class TransitionInterval(int min, int max) : TransitionToken
    {
        public int Min { get; } = min;
        public int Max { get; } = max;

        public override bool Accepts(int value)
        {
            return value >= Min && value <= Max;
        }
    }

}
