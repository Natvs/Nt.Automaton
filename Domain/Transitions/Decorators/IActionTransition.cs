using System;
using System.Collections.Generic;
using System.Text;

namespace Nt.Automaton.Transitions.Decorators
{
    public interface IActionTransition<T> : ITransition<T>
    {
        /// <summary>
        /// Set a condition for the action to be executed when this transition is triggered.
        /// </summary>
        /// <param name="condition">A function that returns a boolean indicating if the action should be executed</param>
        IActionTransition<T> OnCondition(Func<bool> condition);
    }
}
