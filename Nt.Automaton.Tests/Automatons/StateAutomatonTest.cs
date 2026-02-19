using Nt.Automaton.Automatons;
using Nt.Automaton.States;
using Nt.Tests.Automaton.Automatons.Instances;

using static Nt.Tests.Automaton.Automatons.AutomatonUtils;

namespace Nt.Tests.Automaton.Automatons
{

    public class StateAutomatonTest
    {

        [Fact]
        public void StateAutomaton_SingleTransition_ValidState()
        {
            State<string> initial = new(), state1 = new();
            StateSequence(initial, [(state1, "a")]);

            var automaton = new StateAutomaton<string>(initial);
            Read(automaton, ["a"]);

            Assert.Equal(state1, automaton.CurrentState);
        }

        [Fact]
        public void StateAutomaton_MultipleTransitions_ValidState()
        {
            State<string> initial = new(), state1 = new(), state2 = new(), state3 = new(), state4 = new();
            StateSequence(initial, [(state1, "a"), (state2, "b"), (state3, "c"), (state4, "d")]);

            var automaton = new StateAutomaton<string>(initial);
            Read(automaton, ["a", "b", "c", "d"]);

            Assert.Equal(state4, automaton.CurrentState);
        }

        [Fact]
        public void StateAutomaton_SingleTransition_ValidStateAction()
        {
            var action = new IncrementAction();
            State<string> initial = new(), state1 = new(action);
            StateSequence(initial, [(state1, "a")]);

            var automaton = new StateAutomaton<string>(initial);
            Read(automaton, ["a"]);

            Assert.Equal(1, action.Count);
        }

        [Fact]
        public void StateAutomaton_MultipleTransitions_ValidStateAction()
        {
            var action = new IncrementAction();
            State<string> initial = new(), state1 = new(action), state2 = new(action), state3 = new(action), state4 = new(action);
            StateSequence(initial, [(state1, "a"), (state2, "b"), (state3, "c"), (state4, "d")]);

            var automaton = new StateAutomaton<string>(initial);
            Read(automaton, ["a", "b", "c", "d"]);

            Assert.Equal(4, action.Count);
        }

        [Fact]
        public void StateAutomaton_SingleTransition_ValidTransitionAction()
        {
            var action = new IncrementAction();
            State<string> initial = new(), state1 = new();
            StateSequence(initial, [(state1, "a")], action);

            var automaton = new StateAutomaton<string>(initial);
            Read(automaton, ["a"]);

            Assert.Equal(1, action.Count);
        }

        [Fact]
        public void StateAutomaton_MultipleTransitions_ValidTransitionAction()
        {
            var action = new IncrementAction();
            State<string> initial = new(), state1 = new(), state2 = new(), state3 = new(), state4 = new();
            StateSequence(initial, [(state1, "a"), (state2, "b"), (state3, "c"), (state4, "d")], action);

            var automaton = new StateAutomaton<string>(initial);
            Read(automaton, ["a", "b", "c", "d"]);

            Assert.Equal(4, action.Count);
        }

        [Fact]
        public void StateAutomaton_DefaultTransition_ValidState()
        {
            State<string> initial = new(), state1 = new();
            initial.SetDefault(state1);

            var automaton = new StateAutomaton<string>(initial);
            Read(automaton, ["a"]);

            Assert.Equal(state1, automaton.CurrentState);
        }

        [Fact]
        public void StateAutomaton_DefaultTransition_ValidStateAction()
        {
            var action = new IncrementAction();
            State<string> initial = new(), state1 = new(action);
            initial.SetDefault(state1);

            var automaton = new StateAutomaton<string>(initial);
            Read(automaton, ["a"]);

            Assert.Equal(1, action.Count);
        }

        [Fact]
        public void StateAutomaton_DefaultTransition_ValidTransitionAction()
        {
            var action = new IncrementAction();
            State<string> initial = new(), state1 = new();
            initial.SetDefault(state1, action);

            var automaton = new StateAutomaton<string>(initial);
            Read(automaton, ["a"]);

            Assert.Equal(1, action.Count);
        }
    }
}
