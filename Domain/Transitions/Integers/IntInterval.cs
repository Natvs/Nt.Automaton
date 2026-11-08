namespace Nt.Automaton.Transitions.Integers
{
    internal class IntInterval(int min, int max) : IntToken
    {
        public int Min { get; } = min;
        public int Max { get; } = max;

        public override bool Accepts(int value)
        {
            return value >= Min && value <= Max;
        }
    }

}
