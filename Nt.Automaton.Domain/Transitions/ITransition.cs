using Nt.Automaton.Actions;
using Nt.Automaton.States;

namespace Nt.Automaton.Transitions
{
    public interface ITransition
    {
        string Value { get; }
        IState NewState { get; }
        IAction? Action { get; }
    }
}
