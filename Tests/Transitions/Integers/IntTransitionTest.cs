using Nt.Automaton.States;
using Nt.Automaton.Transitions.Integers;

namespace Nt.Tests.Automaton.Transitions.Integers
{
    public class IntTransitionTest
    {
        [Fact]
        public void IntTransition_Accept_ValidToken()
        {
            var target = new State<int>();
            var transition = new IntTransition(target).Include(1);

            var accepted = transition.Accepts(1);

            Assert.True(accepted);
        }

        [Fact]
        public void IntTransition_Accept_InvalidToken()
        {
            var target = new State<int>();
            var transition = new IntTransition(target).Include(1);

            var accepted = transition.Accepts(2);

            Assert.False(accepted);
        }

        // Range of integers

        [Fact]
        public void IntTransition_Accept_ValidTokenInRange()
        {
            var target = new State<int>();
            var transition = new IntTransition(target).Include(1, 10);

            var accepted = transition.Accepts(5);

            Assert.True(accepted);
        }

        [Fact]
        public void IntTransition_Accept_MinimumOfARange()
        {
            var target = new State<int>();
            var transition = new IntTransition(target).Include(1, 10);

            var accepted = transition.Accepts(1);

            Assert.True(accepted);
        }

        [Fact]
        public void IntTransition_Accept_MaximumOfARange()
        {
            var target = new State<int>();
            var transition = new IntTransition(target).Include(1, 10);

            var accepted = transition.Accepts(10);

            Assert.True(accepted);
        }

        [Fact]
        public void IntTransition_Accept_GreaterThanRange()
        {
            var target = new State<int>();
            var transition = new IntTransition(target).Include(1, 10);

            var accepted = transition.Accepts(11);

            Assert.False(accepted);
        }

        [Fact]
        public void IntTransition_Accept_LowerThanRange()
        {
            var target = new State<int>();
            var transition = new IntTransition(target).Include(1, 10);

            var accepted = transition.Accepts(0);

            Assert.False(accepted);
        }

        // Collection of integers

        [Fact]
        public void IntTransition_Accept_ValidTokenInCollection()
        {
            var target = new State<int>();
            var transition = new IntTransition(target).Include(new List<int> { 1, 2, 3 });
            var accepted = transition.Accepts(2);
            Assert.True(accepted);
        }

        [Fact]
        public void IntTransition_Accept_TokenOutOfCollection()
        {
            var target = new State<int>();
            var transition = new IntTransition(target).Include(new List<int> { 1, 2, 3 });
            var accepted = transition.Accepts(4);
            Assert.False(accepted);
        }

        // Maximum and minimum values

        [Fact]
        public void IntTransition_Accept_GreaterThanMinimum()
        {
            var target = new State<int>();
            var transition = new IntTransition(target).SetMin(5);

            var accepted = transition.Accepts(6);

            Assert.True(accepted);
        }

        [Fact]
        public void IntTransition_Accept_LowerThanMinimum()
        {
            var target = new State<int>();
            var transition = new IntTransition(target).SetMin(5);

            var accepted = transition.Accepts(4);

            Assert.False(accepted);
        }

        [Fact]
        public void IntTransition_Accept_LowerThanMaximum()
        {
            var target = new State<int>();
            var transition = new IntTransition(target).SetMax(5);

            var accepted = transition.Accepts(4);

            Assert.True(accepted);
        }

        [Fact]
        public void IntTransition_Accept_GreaterThanMaximum()
        {
            var target = new State<int>();
            var transition = new IntTransition(target).SetMax(5);

            var accepted = transition.Accepts(6);

            Assert.False(accepted);
        }
    }
}
