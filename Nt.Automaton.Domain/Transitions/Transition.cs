using Nt.Automaton.Actions;
using Nt.Automaton.States;

namespace Nt.Automaton.Transitions
{

    /// <summary>
    /// Represents a transition in an automaton from a state to an other state
    /// </summary>
    public class Transition<T> : ITransition<T>
    {
        public T Value { get; }
        public IState<T> NewState { get; }
        public IAction<T>? Action { get; }

        /// <summary>
        /// Initializes a new transition with the specified transition value and target state.
        /// </summary>
        /// <param name="value">The value that triggers the transition.</param>
        /// <param name="newState">The state to which the transition leads when the specified value is read.</param>
        public Transition(T value, IState<T> newState)
        {
            Value = value;
            NewState = newState;
        }

        /// <summary>
        /// Initializes a new transition with the specified transition value and target state. Also specifies an action to perform when the transition is taken.
        /// </summary>
        /// <param name="value">The value that triggers the transition.</param>
        /// <param name="newState">The state to which the transition leads when the specified value is read.</param>
        /// <param name="action">The action to perform when this transition is taken</param>
        public Transition(T value, IState<T> newState, IAction<T> action)
        {
            Value = value;
            NewState = newState;
            Action = action;
        }
    }

}
