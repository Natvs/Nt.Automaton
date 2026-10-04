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
            var initial = new State<string>().SetAction(new EmptyAction());
            initial.SetDefault(new Transition<string>(initial));

            var new_state = initial.Read(new AutomatonToken<string>("a"));

            Assert.Equal(initial, new_state);
        }

        [Fact]
        public void ActionState_MultipleDefaultTransition_ValidState()
        {
            var initial = new State<string>().SetAction(new EmptyAction());
            initial.SetDefault(new Transition<string>(initial));

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
            var initial = new State<string>().SetAction(new EmptyAction());
            var second = new State<string>().SetAction(new EmptyAction());
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
            initial.SetDefault(new Transition<string>(initial));

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
        public void ActionState_RaiseEvents_OnTransition()
        {
            var initial = new State<string>().SetAction(new EmptyAction());
            var second = new State<string>().SetAction(new EmptyAction());
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
        public void ActionState_RaiseEventsInRigthSequence_OnTransition()
        {
            var initial = new State<string>().SetAction(new EmptyAction());
            var second = new State<string>().SetAction(new EmptyAction());
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
        public void ActionState_RaiseEvents_OnDefaultTransition()
        {
            var initial = new State<string>().SetAction(new EmptyAction());
            var second = new State<string>().SetAction(new EmptyAction());
            var token = new AutomatonToken<string>("a");
            bool deactivated_raised = false, leave_raised = false, reach_raised = false, activated_raised = false;

            initial.SetDefault(new Transition<string>(second));
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
        public void ActionState_RaiseEventsInRigthSequence_OnDefaultTransition()
        {
            var initial = new State<string>().SetAction(new EmptyAction());
            var second = new State<string>().SetAction(new EmptyAction());
            var token = new AutomatonToken<string>("a");
            int counter = 0;
            int deactivated = 0, leave = 0, reach = 0, activated = 0;

            initial.SetDefault(new Transition<string>(second));
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
