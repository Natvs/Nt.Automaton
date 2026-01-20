using Nt.Automaton.Tokens;

namespace Nt.Automaton
{
    public interface IAutomaton
    {
        void Read(IAutomatonToken token);
    }
}
