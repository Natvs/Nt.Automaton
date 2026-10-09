using Nt.Automaton.Actions;
using Nt.Automaton.Events;
using Nt.Automaton.States.Decorators;
using Nt.Automaton.States.Exceptions;
using Nt.Automaton.Tokens;
using Nt.Automaton.Transitions;
using System.Diagnostics;
using System.Transactions;

namespace Nt.Automaton.States
{

    /// <summary>
    /// Represents a state within a finite state machine, including its transitions, actions, and default behavior.
    /// </summary>
    public class State<T>() : IState<T>
    {
        public List<ITransition<T>> Transitions { get; } = [];
        public ITransition<T>? DefaultTransition { get; private set; }
        public bool IsFinal { get => false; }

        public IState<T> SetDefault(ITransition<T> transition)
        {
            DefaultTransition = transition;
            return this;
        }
        public IActionState<T> SetAction(IAction action)
        {
            return new ActionState<T>(this, action);
        }

        public IFinalState<T> SetFinal()
        {
            return new FinalState<T>(this);
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
                if (t.Accepts(transition.Token)) toRemove.Add(t);
            }
            foreach (var t in toRemove)
            {
                Transitions.Remove(t);
            }
            Transitions.Add(transition);
        }

        public IState<T> Read(IAutomatonToken<T> token)
        {
            foreach (var transition in Transitions)
            {
                if (transition.Accepts(token))
                {
                    return TargetNewState(transition, token);
                }
            }
            if (DefaultTransition is null) throw new NoDefaultTransitionException();
            return TargetNewState(DefaultTransition, token);
        }

        protected virtual IState<T> TargetNewState(ITransition<T> transition, IAutomatonToken<T> token)
        {
            var args = new TransitionEventArgs<T>(transition);

            // Leaves the current state then performs the transition action
            OnLeave(args);
            transition.Trigger(token);

            // Enters the new state
            transition.Target.OnReach(args);

            return transition.Target;
        }

        public void Activate() 
        {
            Activated?.Invoke(this, new EventArgs());
        }
        public void Deactivate() 
        {
            Deactivated?.Invoke(this, new EventArgs());
        }
        public void OnReach(TransitionEventArgs<T> args)
        {
            Reach?.Invoke(this, args);
            Activate();
        }
        public void OnLeave(TransitionEventArgs<T> args)
        {
            Deactivate();
            Leave?.Invoke(this, args);
        }

        public event EventHandler? Activated;
        public event EventHandler? Deactivated;
        public event EventHandler<TransitionEventArgs<T>>? Reach;
        public event EventHandler<TransitionEventArgs<T>>? Leave;
    }

}
