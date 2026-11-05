using Nt.Automaton.Actions;
using Nt.Automaton.Events;
using Nt.Automaton.States;
using Nt.Automaton.Tokens;

namespace Nt.Automaton.Transitions.Decorators
{
    public class ActionTransition<T>(ITransition<T> transition, ITokenAction<T> action) : IActionTransition<T>
    {
        public IAutomatonToken<T> Token => Transition.Token;
        public IState<T> Target => Transition.Target;
        public ITransition<T> Transition => transition;

        public ITokenAction<T> Action { get; private set; } = action;
        private Func<bool> condition = () => true;

        public bool Accepts(IAutomatonToken<T> token) => Transition.Accepts(token);

        public IActionTransition<T> SetAction(ITokenAction<T> action)
        {
            Action = action;
            return this;
        }

        public IActionTransition<T> OnCondition(Func<bool> condition)
        {
            this.condition = condition;
            return this;
        }


        public void Trigger(IAutomatonToken<T> token)
        {
            if (condition()) Action.Perform(token);
            Transition.Trigger(token);
            Triggered?.Invoke(this, new TokenEventArgs<T>(token));
        }

        public event EventHandler<TokenEventArgs<T>>? Triggered;
    }
}
