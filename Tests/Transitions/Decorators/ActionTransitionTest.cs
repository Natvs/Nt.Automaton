using Nt.Automaton.States;
using Nt.Automaton.Tokens;
using Nt.Automaton.Transitions;
using Nt.Tests.Automaton.Instances;

namespace Nt.Tests.Automaton.Transitions.Decorators
{
    public class ActionTransitionTest
    {

        [Fact]
        public void ActionTransition_Accept_ValidToken()
        {
            var token = new AutomatonToken<string>("a");
            var target = new State<string>();
            var transition = new Transition<string>(token, target).SetAction(new EmptyTokenAction<string>());

            var accepted = transition.Accepts(token);

            Assert.True(accepted);
        }

        [Fact]
        public void ActionTransition_Accept_InvalidToken()
        {
            var token = new AutomatonToken<string>("a");
            var target = new State<string>();
            var transition = new Transition<string>(token, target).SetAction(new EmptyTokenAction<string>());

            var accepted = transition.Accepts(new AutomatonToken<string>("b"));

            Assert.False(accepted);
        }

        // Action

        [Fact]
        public void ActionTransition_Trigger_ExecuteAction()
        {
            var token = new AutomatonToken<string>("a");
            var target = new State<string>();
            var action = new TriggerTokenAction<string>();
            var transition = new Transition<string>(token, target).SetAction(action);

            transition.Trigger(token);

            Assert.True(action.Triggered);
        }

        [Fact]
        public void ActionTransition_Trigger_OnCondition()
        {
            var token = new AutomatonToken<int>(1);
            var target = new State<int>();
            var action = new TriggerTokenAction<int>();
            var condition_met = false;
            var transition = new Transition<int>(token, target).SetAction(action).OnCondition(() => condition_met);

            transition.Trigger(token);
            Assert.False(action.Triggered);

            condition_met = true;
            transition.Trigger(token);
            Assert.True(action.Triggered);
        }

        // Events

        [Fact]
        public void ActionTransition_Trigger_RaiseEvent()
        {
            var token = new AutomatonToken<string>("a");
            var target = new State<string>();
            var transition = new Transition<string>(token, target).SetAction(new EmptyTokenAction<string>());
            bool triggered = false;

            transition.Triggered += (sender, args) => { triggered = true; };
            transition.Trigger(token);

            Assert.True(triggered);
        }

    }
}
