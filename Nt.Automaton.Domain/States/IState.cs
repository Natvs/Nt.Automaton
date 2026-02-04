using Nt.Automaton.Actions;
using Nt.Automaton.Tokens;
using Nt.Automaton.Transitions;

namespace Nt.Automaton.States
{
    public interface IState<T>
    {
        IAction<T>? Action { get; }

        IState<T> Read(IAutomatonToken<T> token);
        void AddTransition(ITransition<T> transition);

        /// <summary>
        /// Triggers the <see cref="StateReached"/> event
        /// </summary>
        /// <param name="args">Event arguments</param>
        void OnReached(StateEventArgs<T> args);
        /// <summary>
        /// Triggers the <see cref="StateLeft"/> event
        /// </summary>
        /// <param name="args"></param>
        void OnLeft(StateEventArgs<T> args);

        event EventHandler<StateEventArgs<T>> StateReached;
        event EventHandler<StateEventArgs<T>> StateLeft;
    }
}
