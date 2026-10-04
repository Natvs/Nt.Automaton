using Nt.Automaton.Events;
using Nt.Automaton.States;
using Nt.Automaton.Tokens;

namespace Nt.Automaton.Automatons
{
    public interface IAutomaton<T>
    {
        /// <summary>
        /// Get the current state of the automaton.
        /// </summary>
        IState<T>? CurrentState { get; }
        /// <summary>
        /// Indicate whether the automaton is in a valid state (i.e., has reached a final state).
        /// </summary>
        bool IsValid { get; }


        /// <summary>
        /// Occur when the automaton reaches a final state.
        /// </summary>
        event EventHandler<StateEventArgs<T>>? FinalStateReached;

        /// <summary>
        /// Read a token from the current state and goes to the next state.
        /// </summary>
        /// <param name="token">Automation token to read</param>
        /// <exception cref="NullStateException">The current state may be null</exception>
        void Read(IAutomatonToken<T> token);
    }
}
