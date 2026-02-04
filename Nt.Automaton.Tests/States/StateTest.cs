using Nt.Automaton.States;
using Nt.Automaton.Tokens;
using Nt.Automaton.Transitions;

namespace Nt.Tests.Automaton.States
{
    public class StateTest
    {
        // Target states

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
        public void State_Transition_ValidState()
        {
            var initial = new State<string>();
            var second = new State<string>();
            initial.AddTransition(new Transition<string>("a", second));

            var new_state = initial.Read(new AutomatonToken<string>("a"));

            Assert.Equal(second, new_state);
        }

        // Actions

        [Fact]
        public void State_DefaultTransition_StateActionPerformed()
        {
            var initial = new State<string>(new ThrowStateErrorAction());
            initial.SetDefault(initial);

            Assert.Throws<StateErrorException>(() => initial.Read(new AutomatonToken<string>("a")));
        }

        [Fact]
        public void State_Transition_StateActionPerformed()
        {
            var initial = new State<string>();
            var second = new State<string>(new ThrowStateErrorAction());
            initial.AddTransition(new Transition<string>("a", second));

            Assert.Throws<StateErrorException>(() => initial.Read(new AutomatonToken<string>("a")));
        }

        // Events

        [Fact]
        public void State_DefaultTransition_LeftEventTriggered()
        {
            var initial = new State<string>();
            bool left_triggered = false;
            initial.SetDefault(initial);
            initial.StateLeft += (state, token) => { left_triggered = true; };

            initial.Read(new AutomatonToken<string>("a"));

            Assert.True(left_triggered);
        }

        [Fact]
        public void State_DefaultTransition_ReachedEventTriggered()
        {
            var initial = new State<string>();
            bool reached_triggered = false;
            initial.SetDefault(initial);
            initial.StateReached += (state, token) => { reached_triggered = true; };

            initial.Read(new AutomatonToken<string>("a"));

            Assert.True(reached_triggered);
        }

        [Fact]
        public void State_Transition_LeftEventTriggered()
        {
            var initial = new State<string>();
            var second = new State<string>();
            bool left_triggered = false;
            initial.AddTransition(new Transition<string>("a", second));
            initial.StateLeft += (state, token) => { left_triggered = true; };

            initial.Read(new AutomatonToken<string>("a"));

            Assert.True(left_triggered);
        }

        [Fact]
        public void State_Transition_ReachedEventTriggered()
        {
            var initial = new State<string>();
            var second = new State<string>();
            bool reached_triggered = false;
            initial.AddTransition(new Transition<string>("a", second));
            second.StateReached += (state, token) => { reached_triggered = true; };

            initial.Read(new AutomatonToken<string>("a"));

            Assert.True(reached_triggered);
        }
    }
}
