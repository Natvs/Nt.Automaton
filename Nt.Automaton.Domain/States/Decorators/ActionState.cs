using Nt.Automaton.Actions;
using Nt.Automaton.States.Exceptions;
using Nt.Automaton.Tokens;
using Nt.Automaton.Transitions;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Nt.Automaton.States.Decorators
{
    public class ActionState<T> : IActionState<T>
    {
        public IState<T> State { get; }

        public IAction Action { get; private set; }

        public ActionState(IState<T> state, IAction action, bool auto_trigger = false) {
            State = state;
            Action = action;

            State.StateLeft += (sender, args) => { OnLeft(args); };
            State.StateReached += (sender, args) => { OnReached(args); };
        }

        public void Activate()
        {
            Action.Perform();
            State.Activate();
        }
        public void Deactivate() => State.Deactivate();

        public IState<T> SetDefault(IState<T> defaultState) => State.SetDefault(defaultState);
        public IState<T> SetDefault(IState<T> defaultState, ITokenAction<T> defaultAction) => State.SetDefault(defaultState, defaultAction);
        public IActionState<T> SetAction(IAction action)
        {
            Action = action;
            return this;
        }

        public void AddTransition(ITransition<T> transition) => State.AddTransition(transition);
        public void OverwriteTransition(ITransition<T> transition) => State.OverwriteTransition(transition);
        public void AddTransitions(ICollection<ITransition<T>> transitions) => State.AddTransitions(transitions);

        public IState<T> Read(IAutomatonToken<T> token) => State.Read(token);

        // Events

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
