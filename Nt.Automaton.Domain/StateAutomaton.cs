using Nt.Automaton.States;
using Nt.Automaton.Tokens;

namespace Nt.Automaton
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
        /// Reads a token from the current state and updates current state to the next state
        /// </summary>
        /// <param name="token">Automation token to read</param>
        public void Read(IAutomatonToken<T> token)
        {
            if (CurrentState == null) { return; }
            CurrentState = CurrentState.Read(token);
        }
    }

}
