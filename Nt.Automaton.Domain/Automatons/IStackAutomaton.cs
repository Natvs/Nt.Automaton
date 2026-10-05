using Nt.Automaton.States;
using Nt.Automaton.Tokens;

namespace Nt.Automaton.Automatons
{
    public interface IStackAutomaton<T>: IAutomaton<T>
    {
        /// <summary>
        /// Determine whether the stack is empty.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Occur after a state is removed from the stack.
        /// </summary>
        event EventHandler? StatePopped;

        /// <summary>
        /// Occur after a state is pushed onto the stack.
        /// </summary>
        event EventHandler? StatePushed;

        /// <summary>
        /// Pop the last state from the stack and goes back to it.
        /// </summary>
        void Pop();

        /// <summary>
        /// Push the current state onto the stack.
        /// </summary>
        /// <param name="target">The state to transition to.</param>
        void Push(IState<T> target);
    }
}