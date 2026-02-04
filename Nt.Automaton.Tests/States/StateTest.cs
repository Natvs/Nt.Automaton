using Nt.Automaton.States;
using Nt.Automaton.Tokens;
using Nt.Automaton.Transitions;

namespace Nt.Tests.Automaton.States
{
    public class StateTest
    {
        [Fact]
        public void State_DefaultTransition_ValidState()
        {
            var initial = new State<string>();
            initial.SetDefault(initial);

            var new_state = initial.Read(new AutomatonToken<string>("a"));

            Assert.Equal(initial, new_state);
        }

        [Fact]
        public void State_MultipleDefaultTransition_ValidState()
        {
            var initial = new State<string>();
            initial.SetDefault(initial);

            IState<string> new_state = initial;
            foreach (var letter in new List<string>(["a", "b", "c", "d", "e", "f"]))
            {
                new_state = initial.Read(new AutomatonToken<string>(letter));
            }

            Assert.Equal(initial, new_state);
        }

        [Fact]
        public void State_DefaultTransition_StateActionPerformed()
        {
            var initial = new State<string>(new ThrowStateErrorAction());
            initial.SetDefault(initial);

            Assert.Throws<StateErrorException>(() => initial.Read(new AutomatonToken<string>("a")));
        }

        [Fact]
        public void State_Transition_ValidState()
        {
            var initial = new State<string>();
            var second = new State<string>();
            initial.AddTransition(new Transition<string>("a", second));

            var new_state = initial.Read(new AutomatonToken<string>("a"));

            Assert.Equal(second, new_state);
        }

        [Fact]
        public void State_Transition_StateActionPerformed()
        {
            var initial = new State<string>();
            var second = new State<string>(new ThrowStateErrorAction());
            initial.AddTransition(new Transition<string>("a", second));

            Assert.Throws<StateErrorException>(() => initial.Read(new AutomatonToken<string>("a")));
        }
    }
}
