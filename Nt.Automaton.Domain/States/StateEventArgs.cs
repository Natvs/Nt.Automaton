using Nt.Automaton.Transitions;

namespace Nt.Automaton.States
{
    public class StateEventArgs<T> : EventArgs
    {
        public ITransition<T>? Transition { get; }
        public StateEventArgs(ITransition<T> transition)
        {
            Transition = transition;
        }
    }
}
