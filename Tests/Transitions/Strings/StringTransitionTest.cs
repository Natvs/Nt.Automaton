

using Nt.Automaton.States;
using Nt.Automaton.Transitions.Strings;

namespace Nt.Tests.Automaton.Transitions.Strings
{
    public class StringTransitionTest
    {
        [Fact]
        public void StringTransition_Accept_ValidToken()
        {
            var target = new State<string>();
            var transition = new StringTransition(target).Include("token");

            var accepted = transition.Accepts("token");

            Assert.True(accepted);
        }

        [Fact]
        public void StringTransition_Accept_InvalidToken()
        {
            var target = new State<string>();
            var transition = new StringTransition(target).Include("token");

            var accepted = transition.Accepts("invalid");

            Assert.False(accepted);
        }

        // Collection of strings

        [Fact]
        public void StringTransition_Accept_ValidTokenInCollection()
        {
            var target = new State<string>();
            var transition = new StringTransition(target).Include(["token1", "token2", "token3" ]);

            var accepted = transition.Accepts("token2");

            Assert.True(accepted);
        }

        [Fact]
        public void StringTransition_Accept_TokenOutOfCollection()
        {
            var target = new State<string>();
            var transition = new StringTransition(target).Include(["token1", "token2", "token3" ]);

            var accepted = transition.Accepts("invalid");

            Assert.False(accepted);
        }

        // Regular expression

        [Fact]
        public void StringTransition_Accept_TokenMatchPattern()
        {
            var target = new State<string>();
            var transition = new StringTransition(target).AddPattern(@"token\d+");

            var accepted = transition.Accepts("token123");

            Assert.True(accepted);
        }

        [Fact]
        public void StringTransition_Accept_TokenDoesNotMatchPattern()
        {
            var target = new State<string>();
            var transition = new StringTransition(target).AddPattern(@"token\d+");

            var accepted = transition.Accepts("token");

            Assert.False(accepted);
        }

    }
}
