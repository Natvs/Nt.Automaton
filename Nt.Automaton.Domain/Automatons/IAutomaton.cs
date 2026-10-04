using Nt.Automaton.States;
using Nt.Automaton.Tokens;

namespace Nt.Automaton.Automatons
{
    public interface IAutomaton<T>
    {
        /// <summary>
        /// Gets the current state of the automaton.
        /// </summary>
        IState<T>? CurrentState { get; }

        /// <summary>
        /// Read a token from the current state and goes to the next state.
        /// </summary>
        /// <param name="token">Automation token to read</param>
        /// <exception cref="NullStateException">The current state may be null</exception>
        void Read(IAutomatonToken<T> token);
    }
}
