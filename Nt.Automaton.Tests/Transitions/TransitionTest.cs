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
        public void Transition_Accept_InvalidToken()
        {
            var token = new AutomatonToken<string>("a");
            var target = new State<string>();
            var transition = new Transition<string>(token, target);

            var accepted = transition.Accepts(new AutomatonToken<string>("b"));

            Assert.False(accepted);
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
