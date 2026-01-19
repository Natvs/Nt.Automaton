using Nt.Automaton;

namespace Nt.Syntax
{
    /// <summary>
    /// Represents a transition in an automaton from a state to an other state
    /// </summary>
    public class Transition
    {
        public string Value { get; }
        public State NewState { get; }
        public IAction? Action { get; }

        public Transition(string value, State newState)
        {
            Value = value;
            NewState = newState;
        }
        public Transition(string value, State newState, IAction action)
        {
            Value = value;
            NewState = newState;
            Action = action;
        }
    }
}
