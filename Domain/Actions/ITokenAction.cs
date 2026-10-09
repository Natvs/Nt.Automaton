using Nt.Automaton.Tokens;

namespace Nt.Automaton.Actions
{
    public interface ITokenAction<T> 
    {
        void Perform(IAutomatonToken<T> token);
    }

}
