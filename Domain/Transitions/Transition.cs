using Nt.Automaton.Actions;
using Nt.Automaton.Events;
using Nt.Automaton.States;
using Nt.Automaton.Tokens;
using Nt.Automaton.Transitions.Decorators;

namespace Nt.Automaton.Transitions
{

    /// <summary>
    /// Represents a transition in an automaton from a state to an other state.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value">The value that triggers the transition</param>
    /// <param name="newState">The state to which the transition leads when the specified value is read</param>
    public class Transition<T> : ITransition<T>, IConditionalTransition<T>
    {
        public Transition(IState<T> newState)
        {
            Token = null;
            Target = newState;
        }

        public Transition(IAutomatonToken<T> value, IState<T> newState)
        {
            Token = value;
            Target = newState;
        }

        public Transition(T value, IState<T> newState)
        {
            Token = new AutomatonToken<T>(value);
            Target = newState;
        }

        private Func<bool> condition = () => true;
        public IAutomatonToken<T>? Token { get; }
        public IState<T> Target { get; }

        public ITransition<T> OnCondition(Func<bool> condition)
        {
            this.condition = condition;
            return this;
        }
        public bool Accepts(IAutomatonToken<T> token)
        {
            if (!condition()) return false;
            if (Token == null || Token.Value == null) return true;
            return Token.Value.Equals(token.Value);
        }

        public IActionTransition<T> SetAction(ITokenAction<T> action)
        {
            return new ActionTransition<T>(this, action);
        }

        public void Trigger(IAutomatonToken<T> token) 
        {
            Triggered?.Invoke(this, new TokenEventArgs<T>(token));
        }

        public event EventHandler<TokenEventArgs<T>>? Triggered;
    }

}
