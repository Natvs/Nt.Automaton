using Nt.Automaton.States;
using Nt.Automaton.Tokens;

namespace Nt.Automaton.Transitions.Integers
{

    public class IntTransition : Transition<int>
    {
        private int? Min { get; set; }
        private int? Max { get; set; }
        private ICollection<TransitionToken> Tokens { get; } = [];

        public IntTransition(IState<int> newState) : base(newState) { }

        /// <summary>
        /// Set the minimum value (included) acceptable by the transition
        /// </summary>
        /// <param name="min">Minimum value (included)</param>
        /// <returns>The instance of transition for method chaining</returns>
        public IntTransition SetMin(int min)
        {
            Min = min;
            return this;
        }

        /// <summary>
        /// Set the maximum value (included) acceptable by the transition
        /// </summary>
        /// <param name="max">Maximm value (included)</param>
        /// <returns>The instance of transition for method chaining</returns>
        public IntTransition SetMax(int max)
        {
            Max = max;
            return this;
        }


        /// <summary>
        /// Add a value that can be accepted by this transition
        /// </summary>
        /// <param name="value">The value to accept</param>
        /// <returns>The instance of transition for method chaining</returns>
        public IntTransition Include(int value)
        {
            Tokens.Add(new TransitionNumber(value));
            return this;
        }

        /// <summary>
        /// Add an interval of acceptable values for this transition
        /// </summary>
        /// <param name="min">The minimum value (included)</param>
        /// <param name="max">The maximum value (included)</param>
        /// <returns>The instance of transition for method chaining</returns>
        public IntTransition Include(int min, int max)
        {
            Tokens.Add(new TransitionInterval(min, max));
            return this;
        }

        /// <summary>
        /// Add a collection of integer to accept for this transition
        /// </summary>
        /// <param name="values">The collection of integers to accept</param>
        /// <returns>The instance of transition for method chaining</returns>
        public IntTransition Include(ICollection<int> values)
        {
            Tokens.Add(new TransitionCollection(values));
            return this;
        }

        public override sealed bool Accepts(IAutomatonToken<int> token)
        {
            return Accepts(token.Value);
        }
        public bool Accepts(int token)
        {
            if (!base.Accepts(new AutomatonToken<int>(token))) return false;
            if (Min != null && token >= Min) return true;
            if (Max != null && token <= Max) return true;
            foreach (var intToken in Tokens)
            {
                if (intToken.Accepts(token)) return true;
            }
            return false;
        }
    }

}
