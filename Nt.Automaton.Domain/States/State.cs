using Nt.Automaton.Actions;
using Nt.Automaton.States.Decorators;
using Nt.Automaton.States.Exceptions;
using Nt.Automaton.Tokens;
using Nt.Automaton.Transitions;
using System.Transactions;

namespace Nt.Automaton.States
{

    /// <summary>
    /// Represents a state within a finite state machine, including its transitions, actions, and default behavior.
    /// </summary>
    public class State<T>() : IState<T>
    {
        public List<ITransition<T>> Transitions { get; } = [];
        public IState<T>? DefaultState { get; private set; }
        public ITokenAction<T>? DefaulAction { get; private set; }

        public void Activate() { }
        public void Deactivate() { }

        public IState<T> SetDefault(IState<T> defaultState)
        {
            DefaultState = defaultState;
            return this;
        }     
        public IState<T> SetDefault(IState<T> defaultState, ITokenAction<T> defaultAction)
        {
            DefaultState = defaultState;
            DefaulAction = defaultAction;
            return this;
        }
        public IActionState<T> SetAction(IAction action)
        {
            return new ActionState<T>(this, action);
        }

        public void AddTransition(ITransition<T> transition)
        {
            Transitions.Add(transition);
        }
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
        public void AddTransitions(ICollection<ITransition<T>> transitions)
        {
            foreach (var transition in transitions)
            {
                Transitions.Add(transition);
            }
        }

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

        protected virtual IState<T> TargetNewState(ITransition<T> transition, IAutomatonToken<T> token)
        {
            var args = new StateEventArgs<T>(transition);

            // Leaves the current state then performs the transition action
            OnLeft(args);
            transition.Action?.Perform(token);

            // Enters the new state
            transition.Target.OnReached(args);

            return transition.Target;
        }

        protected virtual IState<T> TargetDefaultState(IAutomatonToken<T> token)
        {
            if (DefaultState == null) throw new NoDefaultStateException();
            var args = new StateEventArgs<T>(new Transition<T>(token.Value, DefaultState));

            // Leaves the current state then performs the transition action
            OnLeft(args);
            DefaulAction?.Perform(token);

            // Enters the default state
            DefaultState.OnReached(args);

            return DefaultState!;
        }

        public void OnReached(StateEventArgs<T> args)
        {
            StateReached?.Invoke(this, args);
            Activate();
        }

        public void OnLeft(StateEventArgs<T> args)
        {
            Deactivate();
            StateLeft?.Invoke(this, args);
        }

        public event EventHandler<StateEventArgs<T>>? StateReached;

        public event EventHandler<StateEventArgs<T>>? StateLeft;
    }

}
