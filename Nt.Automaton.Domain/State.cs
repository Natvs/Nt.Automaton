using Nt.Automaton;
using Nt.Automaton.Exceptions;

namespace Nt.Syntax
{
    public class State
    {
        public List<Transition> Transitions { get; } = [];
        public State? DefaultState { get; private set; }
        public IAction? DefaulAction { get; private set; }
        public IAction? Action { get; }

        public State() { }
        public State(IAction action)
        {
            Action = action;
        }

        public State SetDefault(State defaultState)
        {
            DefaultState = defaultState;
            return this;
        }
        public State SetDefault(State defaultState, IAction defaultAction)
        {
            DefaultState = defaultState;
            DefaulAction = defaultAction;
            return this;
        }

        public void AddTransition(string value, State state)
        {
            Transitions.Add(new Transition(value, state));
        }
        public void AddTransition(string value, State state, IAction action)
        {
            Transitions.Add(new Transition(value, state, action));
        }

        public void OverwriteTransition(string value, State state)
        {
            List<Transition> toRemove = [];
            foreach (var t in Transitions)
            {
                if (t.Value.Equals(value)) toRemove.Add(t);
            }
            foreach (var t in toRemove)
            {
                Transitions.Remove(t);
            }
            Transitions.Add(new Transition(value, state));
        }
        public void OverwriteTransition(string value, State state, IAction action)
        {
            List<Transition> toRemove = [];
            foreach (var t in Transitions)
            {
                if (t.Value.Equals(value)) toRemove.Add(t);
            }
            foreach (var t in toRemove)
            {
                Transitions.Remove(t);
            }
            Transitions.Add(new Transition(value, state, action));
        }

        public void AddTransitions(List<string> values, State state)
        {
            foreach (string value in values)
            {
                Transitions.Add(new Transition(value, state));
            }
        }
        public void AddTransitions(List<string> values, State state, IAction action)
        {
            foreach (string value in values)
            {
                Transitions.Add(new Transition(value, state, action));
            }
        }

        /// <summary>
        /// Reads a token and gets the next state
        /// </summary>
        /// <param name="token">Automaton token to read</param>
        /// <returns>Next state of the automaton after reading the token</returns>
        /// <remarks>In case of multiple transitions with same symbol, only the first action added will be performed</remarks>
        /// <exception cref="NoDefaultStateException">It might be that no default state was set for this state</exception>
        public State Read(IAutomatonToken token)
        {
            if (DefaultState == null) throw new NoDefaultStateException();
            foreach (Transition transition in Transitions)
            {
                if (transition.Value.Equals(token.Name))
                {
                    transition.Action?.Perform(token);
                    transition.NewState.Action?.Perform(token);
                    return transition.NewState;
                }
            }
            DefaulAction?.Perform(token);
            DefaultState.Action?.Perform(token);
            return DefaultState;
        }
            
    }
}
