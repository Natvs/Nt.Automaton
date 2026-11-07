using Nt.Automaton.States;
using Nt.Automaton.Tokens;

namespace Nt.Automaton.Transitions.Strings
{
    public class StringTransition : Transition<string>
    {
        private ICollection<StringToken> Tokens { get; } = [];

        public StringTransition(IState<string> newState) : base(newState) { }

        /// <summary>
        /// Includes a string as an acceptable token for this transition
        /// </summary>
        /// <param name="value">String to accept</param>
        /// <returns></returns>
        public StringTransition Include(string value)
        {
            Tokens.Add(new StringValue(value));
            return this;
        }

        /// <summary>
        /// Includes a collection of strings as acceptable tokens for this transition
        /// </summary>
        /// <param name="values">Collection of strings to accept</param>
        /// <returns></returns>
        public StringTransition Include(ICollection<string> values)
        {
            foreach (var value in values)
            {
                Tokens.Add(new StringValue(value));
            }
            return this;
        }

        /// <summary>
        /// Includes all strings following a pattern as acceptable tokens for this transition
        /// </summary>
        /// <param name="pattern">Pattern that tokens must match to be accepted</param>
        /// <returns></returns>
        public StringTransition AddPattern(string pattern)
        {
            Tokens.Add(new RegexValue(pattern));
            return this;
        }

        public override sealed bool Accepts(IAutomatonToken<string> token)
        {
            return Accepts(token.Value);
        }
        public bool Accepts(string token)
        {
            if (!base.Accepts(new AutomatonToken<string>(token))) return false;
            foreach (var stringToken in Tokens)
            {
                if (stringToken.Accepts(token)) return true;
            }
            return false;
        }
    }

}
