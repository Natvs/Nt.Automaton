using Nt.Automaton.Actions;
using Nt.Automaton.Events;
using Nt.Automaton.Tokens;
using Nt.Automaton.Transitions;

namespace Nt.Automaton.States.Decorators
{
    public class ActionState<T> : IActionState<T>
    {
        public event EventHandler? Activated;
        public event EventHandler? Deactivated;
        public event EventHandler<TransitionEventArgs<T>>? Reach;
        public event EventHandler<TransitionEventArgs<T>>? Leave;

        private IState<T> State { get; }

        public IAction Action { get; private set; }
        private Func<bool> Condition = () => true;
        public bool IsFinal => State.IsFinal;

        public ActionState(IState<T> state, IAction action) {
            State = state;
            Action = action;

            State.Activated += (sender, args) => { Activated?.Invoke(this, args); };
            State.Deactivated += (sender, args) => { Deactivated?.Invoke(this, args); };
            State.Leave += (sender, args) => { Leave?.Invoke(this, args); };
            State.Reach += (sender, args) => { Reach?.Invoke(this, args); };

            State.Activated += (sender, args) => { if (Condition()) Action.Perform(); };
        }

        public IState<T> SetDefault(ITransition<T> transition) => State.SetDefault(transition);
        public IActionState<T> SetAction(IAction action)
        {
            Action = action;
            return this;
        }
        public IActionState<T> OnCondition(Func<bool> condition)
        {
            Condition = condition;
            return this;
        }
        public IFinalState<T> SetFinal() => new FinalState<T>(this);

        public IState<T> AddTransition(ITransition<T> transition) => State.AddTransition(transition);
        public IState<T> OverwriteTransition(ITransition<T> transition) => State.OverwriteTransition(transition);
        public void ClearTransitions() => State.ClearTransitions();
        public int GetTransitionsCount() => State.GetTransitionsCount();
        public IEnumerable<T> ListTransitions() => State.ListTransitions();
        public ITransition<T>? GetTransition(T token) => State.GetTransition(token);

        public IState<T> Read(IAutomatonToken<T> token) => State.Read(token);

        // Events

        public void Activate() => State.Activate();
        public void Deactivate() => State.Deactivate();

        public void OnReach(TransitionEventArgs<T> args) => State.OnReach(args);
        public void OnLeave(TransitionEventArgs<T> args) => State.OnLeave(args);

    }
}
