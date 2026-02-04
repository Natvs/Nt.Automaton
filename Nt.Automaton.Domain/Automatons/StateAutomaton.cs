using Nt.Automaton.Automatons.Exceptions;
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
        public IState<T> InitialState { get; } = initialState;
        public IState<T> CurrentState { get; private set; } = initialState;

        /// <summary>
        /// Read a token from the current state and goes to the next state.
        /// </summary>
        /// <param name="token">Automation token to read</param>
        /// <exception cref="NullStateException">The current state may be null</exception>
        public void Read(IAutomatonToken<T> token)
        {
            if (CurrentState == null) { throw new NullStateException("Current state is null"); }
            CurrentState = CurrentState.Read(token);
        }
    }

    /// <summary>
    /// Represents an automaton with backward functionality
    /// </summary>
    /// <param name="initialState">Initial state of the automaton</param>
    public class StackAutomaton<T>(IState<T> initialState) : IAutomaton<T>
    {
        public IState<T> InitialState { get; } = initialState;
        public IState<T> CurrentState { get; private set; } = initialState;
        private Stack<IState<T>> StackState { get; } = new();

        /// <summary>
        /// Read a token from the current state and goes to the next state. 
        /// Go back after all actions (state and transitions actions) are executed.
        /// </summary>
        /// <param name="token">Automation token to read</param>
        /// <exception cref="NullStateException">The current state may be null</exception>
        public void Read(IAutomatonToken<T> token)
        {
            if (CurrentState == null) { throw new NullStateException("Current state is null"); }

            CurrentState.StateLeft += Push;
            var new_state = CurrentState.Read(token);

            if (StackState.Count > 0) CurrentState = StackState.Pop();
        }

        private void Push(object? sender, StateEventArgs<T> e)
        {
            CurrentState.StateLeft -= Push;
            if (e.Transition == null) return;

            StackState.Push(e.Transition.Target);
            CurrentState = e.Transition.Target;
        }

    }

}
