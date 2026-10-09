using Nt.Automaton.Automatons.Exceptions;
using Nt.Automaton.Events;
using Nt.Automaton.States;
using Nt.Automaton.Tokens;

namespace Nt.Automaton.Automatons
{

    /// <summary>
    /// Represents an automaton
    /// </summary>
    /// <param name="initialState">Initial state of the automaton</param>
    public class StateAutomaton<T>(IState<T> initialState) : IAutomaton<T>
    {
        private IState<T> InitialState { get; } = initialState;
        public IState<T> CurrentState { get; private set; } = initialState;
        public bool IsValid => CurrentState.IsFinal;

        public void Read(IAutomatonToken<T> token)
        {
            if (CurrentState == null) { throw new NullStateException("Current state is null"); }
            CurrentState = CurrentState.Read(token);
            if (CurrentState.IsFinal) FinalStateReached?.Invoke(this, new StateEventArgs<T>(CurrentState));
        }

        public event EventHandler<StateEventArgs<T>>? FinalStateReached;


    }

}
