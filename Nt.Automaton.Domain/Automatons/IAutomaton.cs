using Nt.Automaton.Tokens;

namespace Nt.Automaton.Automatons
{
    public interface IAutomaton<T>
    {
        void Read(IAutomatonToken<T> token);
    }
}
