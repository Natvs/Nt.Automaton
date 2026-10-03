using Nt.Automaton.Actions;
using Nt.Automaton.States.Decorators;
using Nt.Automaton.Tokens;
using Nt.Automaton.Transitions;

namespace Nt.Automaton.States
{
    public interface IState<T>
    {
        /// <summary>
        /// Sets the default state for this instance.
        /// </summary>
        /// <param name="defaultState">The state to use as the default.</param>
        /// <returns>The current instance with the default state set.</returns>
        IState<T> SetDefault(IState<T> defaultState);

        /// <summary>
        /// Sets the default state and action for this instance.
        /// </summary>
        /// <param name="defaultState">The state to use as the default.</param>
        /// <param name="defaultAction">The action to use as the default.</param>
        /// <returns>The current instance with the updated default state and action.</returns>
        IState<T> SetDefault(IState<T> defaultState, ITokenAction<T> defaultAction);

        /// <summary>
        /// Adds a transition from this state to an other one.
        /// </summary>
        /// <param name="transition">The transition to add.</param>
        void AddTransition(ITransition<T> transition);

        /// <summary>
        /// Replaces any existing transition with the same value by the new transition.
        /// </summary>
        /// <param name="transition">The transition to add or overwrite in the collection.</param>
        void OverwriteTransition(ITransition<T> transition);

        /// <summary>
        /// Adds a collection of transitions, from this state to other states.
        /// </summary>
        /// <param name="transitions">A list of transitions to add.</param>
        void AddTransitions(ICollection<ITransition<T>> transitions);

        /// <summary>
        /// Reads a token and gets the next state
        /// </summary>
        /// <param name="token">Automaton token to read</param>
        /// <returns>Next state of the automaton after reading the token</returns>
        /// <remarks>In case of multiple transitions with same symbol, only the first action added will be performed</remarks>
        /// <exception cref="NoDefaultStateException">It might be that no default state was set for this state</exception>
        IState<T> Read(IAutomatonToken<T> token);

        // Methods for decorators

        /// <summary>
        /// Sets an action to be performed when this state is reached.
        /// </summary>
        /// <param name="action">Action to perform.</param>
        /// <returns>The current instance with the action set.</returns>
        IActionState<T> SetAction(IAction action);

        // Events

        /// <summary>
        /// Activates this state, allowing it to perform any necessary setup or initialization.
        /// </summary>
        void Activate();
        /// <summary>
        /// Deactivates this state, allowing it to perform any necessary cleanup or teardown.
        /// </summary>
        void Deactivate();

        /// <summary>
        /// Triggers the <see cref="Reach"/> event
        /// </summary>
        /// <param name="args">Event arguments</param>
        void OnReach(StateEventArgs<T> args);
        /// <summary>
        /// Triggers the <see cref="Leave"/> event
        /// </summary>
        /// <param name="args"></param>
        void OnLeave(StateEventArgs<T> args);

        /// <summary>
        /// Event triggered when this state is activated.
        /// </summary>
        event EventHandler? Activated;
        /// <summary>
        /// Event triggered when this state is deactivated.
        /// </summary>
        event EventHandler? Deactivated;

        /// <summary>
        /// Event triggered after a transition that targets this state is taken.
        /// </summary>
        event EventHandler<StateEventArgs<T>>? Reach;

        /// <summary>
        /// Event triggered before a transition that departs from this state is taken.
        /// </summary>
        event EventHandler<StateEventArgs<T>>? Leave;


    }
}
