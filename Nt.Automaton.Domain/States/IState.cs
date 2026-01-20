using Nt.Automaton.Actions;
using Nt.Automaton.Tokens;

namespace Nt.Automaton.States
{
    public interface IState<T>
    {
        IAction<T>? Action { get; }
        IState<T> Read(IAutomatonToken<T> token);
    }
}
