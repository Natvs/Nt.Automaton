using Nt.Automaton.Actions;
using Nt.Automaton.States.Exceptions;
using Nt.Automaton.Tokens;
using Nt.Automaton.Transitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nt.Automaton.States
{
    public class StillState<T> : IState<T>
    {
        public List<ITransition<T>> Transitions { get; } = [];
        public IState<T>? DefaultState { get; private set; }
        public IAction<T>? DefaulAction { get; private set; }
        public IAction<T>? Action { get; }

        /// <summary>
        /// Initializes a new instance of the State class.
        /// </summary>
        public StillState() { }
        /// <summary>
        /// Initializes a new instance of the State class with the specified action.
        /// </summary>
        /// <param name="action">The action to associate with this state.</param>
        public StillState(IAction<T> action)
        {
            Action = action;
        }

        /// <summary>
        /// Sets the default state for this instance.
        /// </summary>
        /// <param name="defaultState">The state to use as the default.</param>
        /// <returns>The current instance with the default state set.</returns>
        public StillState<T> SetDefault(IState<T> defaultState)
        {
            DefaultState = defaultState;
            return this;
        }
        /// <summary>
        /// Sets the default state and action for this instance.
        /// </summary>
        /// <param name="defaultState">The state to use as the default.</param>
        /// <param name="defaultAction">The action to use as the default.</param>
        /// <returns>The current instance with the updated default state and action.</returns>
        public StillState<T> SetDefault(IState<T> defaultState, IAction<T> defaultAction)
        {
            DefaultState = defaultState;
            DefaulAction = defaultAction;
            return this;
        }

        /// <summary>
        /// Adds a transition from this state to an other one.
        /// </summary>
        /// <param name="transition">The transition to add.</param>
        public void AddTransition(ITransition<T> transition)
        {
            Transitions.Add(transition);
        }
        /// <summary>
        /// Replaces any existing transition with the same value by the new transition.
        /// </summary>
        /// <param name="transition">The transition to add or overwrite in the collection.</param>
        public void OverwriteTransition(ITransition<T> transition)
        {
            List<ITransition<T>> toRemove = [];
            foreach (var t in Transitions)
            {
                if (t.Value != null && t.Value.Equals(transition.Value)) toRemove.Add(t);
            }
            foreach (var t in toRemove)
            {
                Transitions.Remove(t);
            }
            Transitions.Add(transition);
        }
        /// <summary>
        /// Adds a collection of transitions, from this state to other states.
        /// </summary>
        /// <param name="transitions">A list of transitions to add.</param>
        public void AddTransitions(ICollection<ITransition<T>> transitions)
        {
            foreach (var transition in transitions)
            {
                Transitions.Add(transition);
            }
        }

        /// <summary>
        /// Reads a token and gets the next state
        /// </summary>
        /// <param name="token">Automaton token to read</param>
        /// <returns>Next state of the automaton after reading the token</returns>
        /// <remarks>In case of multiple transitions with same symbol, only the first action added will be performed</remarks>
        /// <exception cref="NoDefaultStateException">It might be that no default state was set for this state</exception>
        public IState<T> Read(IAutomatonToken<T> token)
        {
            foreach (var transition in Transitions)
            {
                if (transition.Value == null) throw new NullTransitionTokenValue();
                if (transition.Value.Equals(token.Value))
                {
                    return TargetNewState(transition, token);
                }
            }
            return TargetDefaultState(token);
        }

        private IState<T> TargetNewState(ITransition<T> transition, IAutomatonToken<T> token)
        {
            transition.Action?.Perform(token);

            var args = new StateEventArgs<T>(transition);
            OnLeft(args);
            transition.NewState.OnReached(args);

            return transition.NewState;
        }
        private IState<T> TargetDefaultState(IAutomatonToken<T> token)
        {
            if (DefaultState == null) throw new NoDefaultStateException();
            DefaulAction?.Perform(token);

            var args = new StateEventArgs<T>(new Transition<T>(token.Value, DefaultState!));
            OnLeft(args);
            DefaultState!.OnReached(args);

            return DefaultState!;
        }

        public void OnReached(StateEventArgs<T> args)
        {
            StateReached?.Invoke(this, args);
        }
        public void OnLeft(StateEventArgs<T> args)
        {
            StateLeft?.Invoke(this, args);
        }

        /// <summary>
        /// Event triggered after a transition that targets this state is taken.
        /// </summary>
        public event EventHandler<StateEventArgs<T>> StateReached;

        /// <summary>
        /// Event triggered before a transition that departs from this state is taken.
        /// </summary>
        public event EventHandler<StateEventArgs<T>> StateLeft;
    }
}
