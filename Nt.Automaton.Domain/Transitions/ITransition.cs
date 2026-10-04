using Nt.Automaton.Actions;
using Nt.Automaton.Events;
using Nt.Automaton.States;
using Nt.Automaton.Tokens;
using Nt.Automaton.Transitions.Decorators;

namespace Nt.Automaton.Transitions
{
    public interface ITransition<T>
    {
        IAutomatonToken<T> Token { get; }
        IState<T> Target { get; }

        /// <summary>
        /// Defines if a given token is accepted by this transition.
        /// </summary>
        /// <param name="token">The token to check</param>
        /// <returns>A boolean indicating if the token is accepted</returns>
        bool Accepts(IAutomatonToken<T> token);

        /// <summary>
        /// Set an action to this transition. The action will be executed when the transition is triggered.
        /// </summary>
        /// <param name="action">Action to set on this transition</param>
        /// <returns>The new transition with the action</returns>
        IActionTransition<T> SetAction(ITokenAction<T> action);

        /// <summary>
        /// Triggers this transition with the given token.
        /// </summary>
        /// <param name="token">The token to trigger the transition with</param>
        void Trigger(IAutomatonToken<T> token);

        event EventHandler<TokenEventArgs<T>>? Triggered;
    }
}
