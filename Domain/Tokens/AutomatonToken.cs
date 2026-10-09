namespace Nt.Automaton.Tokens
{
    public class AutomatonToken<T>(T value) : IAutomatonToken<T>
    {
        public T Value { get; private set; } = value;
    }
}
