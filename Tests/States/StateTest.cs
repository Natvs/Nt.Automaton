using Nt.Automaton.States;
using Nt.Automaton.Tokens;
using Nt.Automaton.Transitions;
using Nt.Tests.Automaton.Instances;

namespace Nt.Tests.Automaton.States
{
    public class StateTest
    {
        // Target states

        [Fact]
        public void State_DefaultTransition_ValidState()
        {
            var initial = new State<string>();
            initial.SetDefault(new Transition<string>(initial));

            var new_state = initial.Read(new AutomatonToken<string>("a"));

            Assert.Equal(initial, new_state);
        }

        [Fact]
        public void State_MultipleDefaultTransition_ValidState()
        {
            var initial = new State<string>();
            initial.SetDefault(new Transition<string>(initial));

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

        // Information about transitions

        [Fact]
        public void State_GetTransitionsCount_ShouldReturnTheNumberOfTransitions()
        {
            var initial = new State<string>();
            var second = new State<string>();

            initial.AddTransition(new Transition<string>("a", second));
            initial.AddTransition(new Transition<string>("b", second));

            Assert.Equal(2, initial.GetTransitionsCount());
        }

        [Fact]
        public void State_ClearTransitions_ShouldntContainAnyTransition()
        {
            var initial = new State<string>();
            var second = new State<string>();

            initial.AddTransition(new Transition<string>("a", second));
            initial.AddTransition(new Transition<string>("b", second));
            initial.ClearTransitions();

            Assert.Equal(0, initial.GetTransitionsCount());
        }

        [Fact]
        public void State_ListTransitions_ShouldContainTokens()
        {
            var initial = new State<string>();
            var second = new State<string>();

            initial.AddTransition(new Transition<string>("a", second));
            initial.AddTransition(new Transition<string>("b", second));         
            var tokens = initial.ListTransitions();

            Assert.Contains("a", tokens);
            Assert.Contains("b", tokens);
        }

        [Fact]
        public void State_GetTransition_ShouldReturnTheTransition()
        {
            var initial = new State<string>();
            var second = new State<string>();
            var token = new AutomatonToken<string>("a");
            var transition = new Transition<string>(token, second);

            initial.AddTransition(transition);
            var retrieved_transition = initial.GetTransition(token.Value);

            Assert.Equal(transition, retrieved_transition);
        }

        [Fact]
        public void State_GetTransition_ShouldReturnNullIfNotFound()
        {
            var initial = new State<string>();

            var retrieved_transition = initial.GetTransition("a");

            Assert.Null(retrieved_transition);
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
        public void State_RaiseEventsInRigthSequence_OnDefaultTransition()
        {
            var initial = new State<string>();
            var second = new State<string>();
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

        // Actions 

        [Fact]
        public void State_PerformAction_OnDefaultTransition()
        {
            var action = new IncrementAction();
            var initital = new State<int>();
            var second = new State<int>().SetAction(action);

            initital.SetDefault(new Transition<int>(second));
            initital.Read(new AutomatonToken<int>(1));

            Assert.Equal(1, action.Count);
        }

        [Fact]
        public void State_PerformActionWithCondition_OnDefaultTransition()
        {
            var condition_met = false;
            var action = new IncrementAction();
            var initital = new State<int>();
            var second = new State<int>().SetAction(action).OnCondition(() => condition_met);
            var token = new AutomatonToken<int>(1);

            initital.SetDefault(new Transition<int>(second));
            initital.Read(token);

            Assert.Equal(0, action.Count);

            condition_met = true;
            initital.Read(token);

            Assert.Equal(1, action.Count);
        }

        [Fact]
        public void State_PerformAction_OnTransition()
        {
            var action = new IncrementAction();
            var initital = new State<int>();
            var second = new State<int>().SetAction(action);
            var token = new AutomatonToken<int>(1);

            initital.AddTransition(new Transition<int>(token, second));
            initital.Read(token);

            Assert.Equal(1, action.Count);
        }
    }
}
