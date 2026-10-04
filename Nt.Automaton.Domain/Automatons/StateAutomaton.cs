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

        public void Read(IAutomatonToken<T> token)
        {
            if (CurrentState == null) { throw new NullStateException("Current state is null"); }
            CurrentState = CurrentState.Read(token);
        }
    }

}
