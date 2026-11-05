namespace Nt.Automaton.Transitions
{
    public interface IConditionalTransition<T> : ITransition<T>
    {
        /// <summary>
        /// Set a condition for this transition to be taken
        /// </summary>
        /// <param name="condition">A function that returns a boolean indicating if the transition should be taken</param>
        ITransition<T> OnCondition(Func<bool> condition);
    }
}
