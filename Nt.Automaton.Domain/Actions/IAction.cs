using Nt.Automaton.Tokens;

namespace Nt.Automaton.Actions
{
    public interface IAction 
    {
        void Perform(IAutomatonToken token);
    }

}
