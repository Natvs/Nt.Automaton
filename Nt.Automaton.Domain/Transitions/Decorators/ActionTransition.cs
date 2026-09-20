using Nt.Automaton.Actions;
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

        public bool Accepts(IAutomatonToken<T> token) => Transition.Accepts(token);

        public IActionTransition<T> SetAction(ITokenAction<T> action)
        {
            Action = action;
            return this;
        }

        public void Trigger(IAutomatonToken<T> token)
        {
            Action.Perform(token);
            Transition.Trigger(token);
        }

    }
}
