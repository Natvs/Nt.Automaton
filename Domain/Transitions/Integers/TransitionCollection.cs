namespace Nt.Automaton.Transitions.Integers
{
    internal class TransitionCollection(ICollection<int> tokens) : TransitionToken
    {
        public ICollection<int> Values { get; } = tokens;

        public override bool Accepts(int value)
        {
            return Values.Contains(value);
        }
    }

}
