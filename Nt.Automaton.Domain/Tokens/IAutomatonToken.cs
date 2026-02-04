namespace Nt.Automaton.Tokens
{

    public interface IAutomatonToken<T>
    {
        T Value { get; }
    }
}
