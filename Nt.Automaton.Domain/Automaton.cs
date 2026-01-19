using Nt.Parser.Structures;
using Nt.Syntax;

namespace Nt.Automaton
{

    /// <summary>
    /// Represents an automaton
    /// </summary>
    /// <param name="initialState">Initial state of the automaton</param>
    public class Automaton(State initialState)
    {
        public State CurrentState { get; private set; } = initialState;

        /// <summary>
        /// Reads a token from the current state and updates current state to the next state
        /// </summary>
        /// <param name="token">Automation token to read</param>
        public void Read(IAutomatonToken token)
        {
            if (CurrentState == null) { return; }
            CurrentState = CurrentState.Read(token);
        }
    }
}
