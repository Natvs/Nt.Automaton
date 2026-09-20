using Nt.Automaton.States;
using Nt.Automaton.Tokens;
using Nt.Automaton.Transitions;
using Nt.Tests.Automaton.States.Instances;

namespace Nt.Tests.Automaton.States.Decorators
{
    public class ActionStateTest
    {
        // Target states

        [Fact]
        public void ActionState_DefaultTransition_ValidState()
        {
            var initial = new State<string>();
            initial.SetDefault(initial);

            var new_state = initial.Read(new AutomatonToken<string>("a"));

            Assert.Equal(initial, new_state);
        }

        [Fact]
        public void ActionState_MultipleDefaultTransition_ValidState()
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
        public void ActionState_Transition_ValidState()
        {
            var initial = new State<string>();
            var second = new State<string>();
            var token = new AutomatonToken<string>("a");
            initial.AddTransition(new Transition<string>(token, second));

            var new_state = initial.Read(token);

            Assert.Equal(second, new_state);
        }

        // Actions

        [Fact]
        public void ActionState_DefaultTransition_StateActionPerformed()
        {
            var initial = new State<string>().SetAction(new ThrowStateErrorAction());
            initial.SetDefault(initial);

            Assert.Throws<StateErrorException>(() => initial.Read(new AutomatonToken<string>("a")));
        }

        [Fact]
        public void ActionState_Transition_StateActionPerformed()
        {
            var initial = new State<string>();
            var second = new State<string>().SetAction(new ThrowStateErrorAction());
            var token = new AutomatonToken<string>("a");
            initial.AddTransition(new Transition<string>(token, second));

            Assert.Throws<StateErrorException>(() => initial.Read(token));
        }

        // Events

        [Fact]
        public void ActionState_DefaultTransition_LeftEventTriggered()
        {
            var initial = new State<string>();
            bool left_triggered = false;
            initial.SetDefault(initial);
            initial.StateLeft += (state, token) => { left_triggered = true; };

            initial.Read(new AutomatonToken<string>("a"));

            Assert.True(left_triggered);
        }

        [Fact]
        public void ActionState_DefaultTransition_ReachedEventTriggered()
        {
            var initial = new State<string>();
            bool reached_triggered = false;
            initial.SetDefault(initial);
            initial.StateReached += (state, token) => { reached_triggered = true; };

            initial.Read(new AutomatonToken<string>("a"));

            Assert.True(reached_triggered);
        }

        [Fact]
        public void ActionState_Transition_LeftEventTriggered()
        {
            var initial = new State<string>();
            var second = new State<string>();
            bool left_triggered = false;
            var token = new AutomatonToken<string>("a");
            initial.AddTransition(new Transition<string>(token, second));
            initial.StateLeft += (state, token) => { left_triggered = true; };

            initial.Read(token);

            Assert.True(left_triggered);
        }

        [Fact]
        public void ActionState_Transition_ReachedEventTriggered()
        {
            var initial = new State<string>();
            var second = new State<string>();
            bool reached_triggered = false;
            var token = new AutomatonToken<string>("a");
            initial.AddTransition(new Transition<string>(token, second));
            second.StateReached += (state, token) => { reached_triggered = true; };

            initial.Read(token);

            Assert.True(reached_triggered);
        }
    }
}
