using Nt.Automaton.Actions;
using Nt.Automaton.Tokens;
using Nt.Automaton.Transitions;

namespace Nt.Automaton.States.Decorators
{
    public class FinalState<T> : IFinalState<T>
    {
        private IState<T> State { get; }
        private Func<bool> Condition = () => true;

        public event EventHandler? Activated;
        public event EventHandler? Deactivated;
        public event EventHandler<StateEventArgs<T>>? Reach;
        public event EventHandler<StateEventArgs<T>>? Leave;

        public bool IsFinal => Condition();

        public FinalState(IState<T> state)
        {
            State = state;

            State.Leave += (sender, args) => { Leave?.Invoke(this, args); };
            State.Reach += (sender, args) => { Reach?.Invoke(this, args); };
            State.Activated += (sender, args) => { Activated?.Invoke(this, args); };
            State.Deactivated += (sender, args) => { Deactivated?.Invoke(this, args); };
        }

        public IState<T> SetDefault(ITransition<T> transition) => State.SetDefault(transition);

        public void AddTransition(ITransition<T> transition) => State.AddTransition(transition);

        public void OverwriteTransition(ITransition<T> transition) => State.OverwriteTransition(transition);


        public IState<T> Read(IAutomatonToken<T> token) => State.Read(token);

        public IActionState<T> SetAction(IAction action) => State.SetAction(action);

        public IFinalState<T> SetFinal()
        {
            Condition = () => true;
            return this;
        }
        public IFinalState<T> OnCondition(Func<bool> condition)
        {
            Condition = condition;
            return this;
        }

        public void Activate() => State.Activate();

        public void Deactivate() => State.Deactivate();

        public void OnReach(StateEventArgs<T> args) => State.OnReach(args);

        public void OnLeave(StateEventArgs<T> args) => State.OnLeave(args);

    }
}
