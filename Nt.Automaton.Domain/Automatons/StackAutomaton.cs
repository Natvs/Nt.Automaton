using Nt.Automaton.Automatons.Exceptions;
using Nt.Automaton.States;
using Nt.Automaton.States.Exceptions;
using Nt.Automaton.Tokens;
using Nt.Automaton.Transitions;
using System.Reflection;

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

        public void Push(IState<T> new_state)
        {
            if (CurrentState != null) Stack.Push(CurrentState);
            CurrentState = new_state;
            StatePushed?.Invoke(this, EventArgs.Empty);

            if (CurrentState.IsFinal)
            {
                CurrentState.Deactivate();
                Pop();
                CurrentState.Activate();
            }
        }
        private void Push(object? sender, StateEventArgs<T> e)
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
        public bool IsEmpty()
        {
            return Stack.Count == 0 && CurrentState == null;
        }


    }

}
