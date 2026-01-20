using Nt.Automaton.Actions;
using Nt.Automaton.States;

namespace Nt.Automaton.Transitions
{
    public interface ITransition<T>
    {
        T Value { get; }
        IState<T> NewState { get; }
        IAction<T>? Action { get; }
    }
}
