using Nt.Automaton.Automatons.Exceptions;
using Nt.Automaton.States;
using Nt.Automaton.States.Exceptions;
using Nt.Automaton.Tokens;
using System.Reflection;

namespace Nt.Automaton.Automatons
{
    /// <summary>
    /// Represents an automaton with backward functionality
    /// </summary>
    /// <param name="initialState">Initial state of the automaton</param>
    public class StackAutomaton<T> : IAutomaton<T>
    {

        #region Constructors and build methods

        public StackAutomaton() { }

        public StackAutomaton<T> SetAutoPerformAction()
        {
            AutoPerformAction = true;
            return this;
        }

        #endregion

        #region Public

        public IState<T>? CurrentState { get; private set; }

        /// <summary>
        /// Read a token from the current state and push it one the stack. 
        /// </summary>
        /// <param name="token">Automation token to read</param>
        /// <exception cref="NullStateException">The current state may be null</exception>
        public void Read(IAutomatonToken<T> token)
        {
            if (CurrentState == null) { throw new NullStateException("Current state is null"); }

            try
            {
                CurrentState.StateLeft += Push;
                CurrentState.Read(token);
            }
            catch (NoDefaultStateException)
            {
                CurrentState.StateLeft -= Push;
                Pop(AutoPerformAction);
            }
        }

        /// <summary>
        /// Push the current state onto the stack.
        /// </summary>
        /// <param name="new_state">The state to transition to.</param>
        /// <param name="performAction">Whether to perform the action linked to the state. Default is false.</param>
        public void Push(IState<T> new_state, bool performAction = false)
        {
            if (CurrentState != null) Stack.Push(CurrentState);
            CurrentState = new_state;
            StatePushed?.Invoke(this, EventArgs.Empty);
            if (performAction) CurrentState.Action?.Perform();
        }

        /// <summary>
        /// Pop the last state from the stack and goes back to it.
        /// </summary>
        /// <param name="performAction">Whether to perform the action linked to the state. Default is false.</param>
        public void Pop(bool performAction = false)
        {
            if (Stack.Count > 0) 
            {
                CurrentState = Stack.Pop();
                if (performAction) CurrentState.Action?.Perform();
            }
            else CurrentState = null;
            StatePopped?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Determine whether the stack contains no elements.
        /// </summary>
        /// <returns>true if the stack is empty; otherwise, false.</returns>
        public bool IsEmpty()
        {
            return Stack.Count == 0 && CurrentState == null;
        }

        /// <summary>
        /// Occur after a state is pushed onto the stack.
        /// </summary>
        public event EventHandler? StatePushed;

        /// <summary>
        /// Occur after a state is removed from the stack.
        /// </summary>
        public event EventHandler? StatePopped;

        #endregion

        #region Private
        private bool AutoPerformAction { get; set; } = false;
        private Stack<IState<T>> Stack { get; } = new();
        private void Push(object? sender, StateEventArgs<T> e)
        {
            CurrentState?.StateLeft -= Push;

            if (e.Transition == null) return;
            Push(e.Transition.Target);
        }

        #endregion

    }

}
