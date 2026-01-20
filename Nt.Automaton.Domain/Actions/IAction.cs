using Nt.Automaton.Tokens;

namespace Nt.Automaton.Actions
{
    public interface IAction<T> 
    {
        void Perform(IAutomatonToken<T> token);
    }

}
