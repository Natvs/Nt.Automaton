using Nt.Automaton.States;
using Nt.Automaton.Tokens;
using Nt.Automaton.Transitions;

namespace Nt.Tests.Automaton.Transitions
{
    public class TransitionTest
    {

        [Fact]
        public void Transition_Accept_ValidToken()
        {
            var token = new AutomatonToken<string>("a");
            var target = new State<string>();
            var transition = new Transition<string>(token, target);

            var accepted = transition.Accepts(token);

            Assert.True(accepted);
        }

        [Fact]
        public void Transition_Accept_NoToken()
        {
            var token = new AutomatonToken<string>("a");
            var target = new State<string>();
            var transition = new Transition<string>(target);

            var accepted = transition.Accepts(token);

            Assert.True(accepted);
        }

        [Fact]
        public void Transition_Accept_InvalidToken()
        {
            var token = new AutomatonToken<string>("a");
            var target = new State<string>();
            var transition = new Transition<string>(token, target);

            var accepted = transition.Accepts(new AutomatonToken<string>("b"));

            Assert.False(accepted);
        }

        [Fact]
        public void Transition_Accept_NoTokenOnCondition()
        {
            var token = new AutomatonToken<int>(1);
            var target = new State<int>();
            var condition_met = false;
            var transition = new Transition<int>(target).OnCondition(() => condition_met);

            Assert.False(transition.Accepts(token));
            condition_met = true;
            Assert.True(transition.Accepts(token));
        }

        [Fact]
        public void Transition_Accept_ValidTokenOnCondition()
        {
            var token = new AutomatonToken<int>(1);
            var target = new State<int>();
            var condition_met = false;
            var transition = new Transition<int>(token, target).OnCondition(() => condition_met);

            Assert.False(transition.Accepts(token));
            condition_met = true;
            Assert.True(transition.Accepts(token));
        }

        [Fact]
        public void Transition_Accept_InvalidTokenOnCondition()
        {
            var token = new AutomatonToken<int>(1);
            var target = new State<int>();
            var condition_met = false;
            var transition = new Transition<int>(token, target).OnCondition(() => condition_met);

            Assert.False(transition.Accepts(new AutomatonToken<int>(2)));
            condition_met = true;
            Assert.False(transition.Accepts(new AutomatonToken<int>(2)));
        }

        // Events

        [Fact]
        public void Transition_Trigger_RaiseEvent()
        {
            var token = new AutomatonToken<string>("a");
            var target = new State<string>();
            var transition = new Transition<string>(token, target);
            bool triggered = false;

            transition.Triggered += (sender, args) => { triggered = true; };
            transition.Trigger(token);
            
            Assert.True(triggered);
        }

    }
}
