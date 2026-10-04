using Nt.Automaton.States;

namespace Nt.Automaton.Events
{
    public class StateEventArgs<T> : EventArgs
    {

        public IState<T> State { get; }

        public StateEventArgs(IState<T> state)
        {
            State = state;
        }

    }
}
