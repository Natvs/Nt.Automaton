using Nt.Automaton.Actions;
using Nt.Automaton.States;

namespace Nt.Automaton.Transitions
{
    public interface ITransition<T>
    {
        T Value { get; }
        IState<T> Target { get; }
        IAction<T>? Action { get; }
    }
}
