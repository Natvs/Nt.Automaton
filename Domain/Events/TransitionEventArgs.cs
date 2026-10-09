using Nt.Automaton.Transitions;

namespace Nt.Automaton.Events
{
    public class TransitionEventArgs<T> : EventArgs
    {
        public ITransition<T> Transition { get; }
        public TransitionEventArgs(ITransition<T> transition)
        {
            Transition = transition;
        }
    }
}
