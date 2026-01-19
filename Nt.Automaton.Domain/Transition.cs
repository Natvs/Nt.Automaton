namespace Nt.Automaton
{
    /// <summary>
    /// Represents a transition in an automaton from a state to an other state
    /// </summary>
    public class Transition
    {
        public string Value { get; }
        public State NewState { get; }
        public IAction? Action { get; }

        /// <summary>
        /// Initializes a new transition with the specified transition value and target state.
        /// </summary>
        /// <param name="value">The value that triggers the transition.</param>
        /// <param name="newState">The state to which the transition leads when the specified value is read.</param>
        public Transition(string value, State newState)
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
        public Transition(string value, State newState, IAction action)
        {
            Value = value;
            NewState = newState;
            Action = action;
        }
    }
}
