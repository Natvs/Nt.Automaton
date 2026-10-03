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
            var token = new AutomatonToken<string>("a");
            initial.AddTransition(new Transition<string>(token, second));

            var new_state = initial.Read(token);

            Assert.Equal(second, new_state);
        }

        // Events

        [Fact]
        public void State_RaiseEvents_OnTransition()
        {
            var initial = new State<string>();
            var second = new State<string>();
            var token = new AutomatonToken<string>("a");
            bool deactivated_raised = false, leave_raised = false, reach_raised = false, activated_raised = false;

            initial.AddTransition(new Transition<string>(token, second));
            initial.Deactivated += (sender, args) => deactivated_raised = true;
            initial.Leave += (sender, args) => leave_raised = true;
            second.Reach += (sender, args) => reach_raised = true;
            second.Activated += (sender, args) => activated_raised = true;
            initial.Read(token);

            Assert.True(deactivated_raised);
            Assert.True(leave_raised);
            Assert.True(reach_raised);
            Assert.True(activated_raised);
        }

        [Fact]
        public void State_RaiseEventsInRigthSequence_OnTransition()
        {
            var initial = new State<string>();
            var second = new State<string>();
            var token = new AutomatonToken<string>("a");
            int counter = 0;
            int deactivated = 0, leave = 0, reach = 0, activated = 0;

            initial.AddTransition(new Transition<string>(token, second));
            initial.Deactivated += (sender, args) => { deactivated = counter; counter++; };
            initial.Leave += (sender, args) => { leave = counter; counter++; };
            second.Reach += (sender, args) => { reach = counter; counter++; };
            second.Activated += (sender, args) => { activated = counter; counter++; };
            initial.Read(token);

            Assert.Equal(0, deactivated);
            Assert.Equal(1, leave);
            Assert.Equal(2, reach);
            Assert.Equal(3, activated);
        }

        [Fact]
        public void State_RaiseEvents_OnDefaultTransition()
        {
            var initial = new State<string>();
            var second = new State<string>();
            var token = new AutomatonToken<string>("a");
            bool deactivated_raised = false, leave_raised = false, reach_raised = false, activated_raised = false;

            initial.SetDefault(second);
            initial.Deactivated += (sender, args) => deactivated_raised = true;
            initial.Leave += (sender, args) => leave_raised = true;
            second.Reach += (sender, args) => reach_raised = true;
            second.Activated += (sender, args) => activated_raised = true;
            initial.Read(token);

            Assert.True(deactivated_raised);
            Assert.True(leave_raised);
            Assert.True(reach_raised);
            Assert.True(activated_raised);
        }

        [Fact]
        public void State_RaiseEventsInRigthSequence_OnDefaultTransition()
        {
            var initial = new State<string>();
            var second = new State<string>();
            var token = new AutomatonToken<string>("a");
            int counter = 0;
            int deactivated = 0, leave = 0, reach = 0, activated = 0;

            initial.SetDefault(second);
            initial.Deactivated += (sender, args) => { deactivated = counter; counter++; };
            initial.Leave += (sender, args) => { leave = counter; counter++; };
            second.Reach += (sender, args) => { reach = counter; counter++; };
            second.Activated += (sender, args) => { activated = counter; counter++; };
            initial.Read(token);

            Assert.Equal(0, deactivated);
            Assert.Equal(1, leave);
            Assert.Equal(2, reach);
            Assert.Equal(3, activated);
        }

    }
}
