using Nt.Automaton.Automatons.Exceptions;
using Nt.Automaton.Events;
using Nt.Automaton.States;
using Nt.Automaton.States.Exceptions;
using Nt.Automaton.Tokens;

namespace Nt.Automaton.Automatons
{
    /// <summary>
    /// Represents an automaton with backward functionality
    /// </summary>
    /// <param name="initialState">Initial state of the automaton</param>
    public class StackAutomaton<T>() : IStackAutomaton<T>
    {

        public IState<T>? CurrentState { get; private set; }
        private Stack<IState<T>> Stack { get; } = new();
        public bool IsValid => IsEmpty;
        public bool IsEmpty => CurrentState == null && Stack.Count == 0;

        public event EventHandler<StateEventArgs<T>>? FinalStateReached;
        public event EventHandler? StatePushed;
        public event EventHandler? StatePopped;

        public void Read(IAutomatonToken<T> token)
        {
            if (CurrentState == null) { throw new NullStateException("Can't read from a null state"); }

            try
            {
                CurrentState.Leave += Push;
                CurrentState.Read(token);
            }
            catch (NoDefaultTransitionException)
            {
                CurrentState.Leave -= Push;
                Pop();
            }
        }

        public void Push(IState<T> target)
        {
            if (CurrentState != null) Stack.Push(CurrentState);
            CurrentState = target;
            StatePushed?.Invoke(this, EventArgs.Empty);

            if (CurrentState.IsFinal)
            {
                CurrentState.Deactivate();
                Pop();
                CurrentState.Activate();
                FinalStateReached?.Invoke(this, new StateEventArgs<T>(CurrentState));
            }
        }
        private void Push(object? sender, TransitionEventArgs<T> e)
        {
            var state = (IState<T>)sender!;

            if (state is null) throw new NullStateException("Can't push a null state");
            state.Leave -= Push;

            if (e.Transition == null) return;
            Push(e.Transition.Target);
        }
        public void Pop()
        {
            if (CurrentState is null) throw new NullStateException("Can't pop from an empty stack");
            if (Stack.Count > 0)
            {
                CurrentState = Stack.Pop();
            }
            else CurrentState = null;
            StatePopped?.Invoke(this, EventArgs.Empty);
        }

    }

}
