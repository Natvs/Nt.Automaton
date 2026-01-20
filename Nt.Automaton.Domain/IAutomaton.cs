using Nt.Automaton.Tokens;

namespace Nt.Automaton
{
    public interface IAutomaton<T>
    {
        void Read(IAutomatonToken<T> token);
    }
}
