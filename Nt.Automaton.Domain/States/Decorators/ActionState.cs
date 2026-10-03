using Nt.Automaton.Actions;
using Nt.Automaton.Tokens;
using Nt.Automaton.Transitions;

namespace Nt.Automaton.States.Decorators
{
    public class ActionState<T> : IActionState<T>
    {
        public IState<T> State { get; }

        public IAction Action { get; private set; }

        public ActionState(IState<T> state, IAction action) {
            State = state;
            Action = action;

            State.Leave += (sender, args) => { OnLeave(args); };
            State.Reach += (sender, args) => { OnReach(args); };
        }

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

        public void OnReach(StateEventArgs<T> args)
        {
            Reach?.Invoke(this, args);
            Activate();
        }
        public void OnLeave(StateEventArgs<T> args)
        {
            Deactivate();
            Leave?.Invoke(this, args);
        }

        public void Activate()
        {
            Action.Perform();
            State.Activate();
            Activated?.Invoke(this, EventArgs.Empty);
        }
        public void Deactivate()
        {
            State.Deactivate();
            Deactivated?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler? Activated;
        public event EventHandler? Deactivated;
        public event EventHandler<StateEventArgs<T>>? Reach;
        public event EventHandler<StateEventArgs<T>>? Leave;
    }
}
