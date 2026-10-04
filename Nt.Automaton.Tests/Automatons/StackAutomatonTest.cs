using Nt.Automaton.Automatons;
using Nt.Automaton.States;
using Nt.Automaton.Transitions;
using Nt.Tests.Automaton.Automatons.Instances;

using static Nt.Tests.Automaton.Automatons.AutomatonUtils;

namespace Nt.Tests.Automaton.Automatons
{
    public class StackAutomatonTest
    {

        // Target States

        [Fact]
        public void StackAutomaton_SingleTransition_ValidState()
        {
            State<string> initial = new(), state1 = new();
            StateSequence(initial, [(state1, "a")]);

            var automaton = new StackAutomaton<string>();
            automaton.Push(initial);
            Read(automaton, ["a"]);

            Assert.Equal(state1, automaton.CurrentState);
        }

        [Fact]
        public void StackAutomaton_SingleBackwardTransition_ValidState()
        {
            State<string> initial = new(), state1 = new();
            StateSequence(initial, [(state1, "a")]);

            var automaton = new StackAutomaton<string>();
            automaton.Push(initial);
            Read(automaton, ["a", "b"]);

            Assert.Equal(initial, automaton.CurrentState);
        }

        [Fact]
        public void StackAutomaton_MultipleTransitions_ValidState()
        {
            State<string> initial = new(), state1 = new(), state2 = new(), state3 = new(), state4 = new();
            StateSequence(initial, [(state1, "a"), (state2, "b"), (state3, "c"), (state4, "d")]);

            var automaton = new StackAutomaton<string>();
            automaton.Push(initial);
            Read(automaton, ["a", "b", "c", "d"]);

            Assert.Equal(state4, automaton.CurrentState);
        }

        [Fact]
        public void StackAutomaton_MultipleBackwardTransitions_ValidState()
        {
            State<string> initial = new(), state1 = new(), state2 = new(), state3 = new(), state4 = new();
            StateSequence(initial, [(state1, "a"), (state2, "b"), (state3, "c"), (state4, "d")]);

            var automaton = new StackAutomaton<string>();
            automaton.Push(initial);
            Read(automaton, ["a", "b", "c", "d", "e", "e", "e", "e"]);

            Assert.Equal(initial, automaton.CurrentState);
        }

        // State Actions

        [Fact]
        public void StackAutomaton_SingleTransition_ValidStateAction()
        {
            var action = new IncrementAction();
            var initial = new State<string>();
            var state1 = new State<string>().SetAction(action);
            StateSequence(initial, [(state1, "a")]);

            var automaton = new StackAutomaton<string>();
            automaton.Push(initial);
            Read(automaton, ["a"]);

            Assert.Equal(1, action.Count);
        }

        [Fact]
        public void StackAutomaton_SingleBackwardTransition_ValidStateAction()
        {
            var action = new IncrementAction();
            var initial = new State<string>();
            var state1 = new State<string>().SetAction(action);
            StateSequence(initial, [(state1, "a")]);

            var automaton = new StackAutomaton<string>();
            automaton.Push(initial);
            Read(automaton, ["a", "b"]);

            Assert.Equal(1, action.Count);
        }

        [Fact]
        public void StackAutomaton_MultipleTransitions_ValidStateAction()
        {
            var action = new IncrementAction();
            var initial = new State<string>();
            var state1 = new State<string>().SetAction(action);
            var state2 = new State<string>().SetAction(action);
            var state3 = new State<string>().SetAction(action);
            var state4 = new State<string>().SetAction(action);
            StateSequence(initial, [(state1, "a"), (state2, "b"), (state3, "c"), (state4, "d")]);

            var automaton = new StackAutomaton<string>();
            automaton.Push(initial);
            Read(automaton, ["a", "b", "c", "d"]);
            Assert.Equal(4, action.Count);
        }

        [Fact]
        public void StackAutomaton_MultipleBackwardTransitions_ValidStateAction()
        {
            var action = new IncrementAction();
            var initial = new State<string>();
            var state1 = new State<string>().SetAction(action);
            var state2 = new State<string>().SetAction(action);
            var state3 = new State<string>().SetAction(action);
            var state4 = new State<string>().SetAction(action);
            state4.SetDefault(new Transition<string>(state4));
            StateSequence(initial, [(state1, "a"), (state2, "b"), (state3, "c"), (state4, "d")]);

            var automaton = new StackAutomaton<string>();
            automaton.Push(initial);
            Read(automaton, ["a", "b", "c", "d", "e", "e", "e", "e"]);

            Assert.Equal(8, action.Count);
        }

        // Transition Actions

        [Fact]
        public void StackAutomaton_SingleTransition_ValidTransitionAction()
        {
            var action = new IncrementAction();
            var initial = new State<string>();
            var state1 = new State<string>();
            StateSequence(initial, [(state1, "a")], action);

            var automaton = new StackAutomaton<string>();
            automaton.Push(initial);
            Read(automaton, ["a"]);

            Assert.Equal(1, action.Count);
        }

        [Fact]
        public void StackAutomaton_MultipleTransitions_ValidTransitionAction()
        {
            var action = new IncrementAction();
            var initial = new State<string>();
            var state1 = new State<string>();
            var state2 = new State<string>();
            var state3 = new State<string>();
            var state4 = new State<string>();
            StateSequence(initial, [(state1, "a"), (state2, "b"), (state3, "c"), (state4, "d")], action);

            var automaton = new StackAutomaton<string>();
            automaton.Push(initial);
            Read(automaton, ["a", "b", "c", "d"]);

            Assert.Equal(4, action.Count);
        }

        [Fact]
        public void StackAutomaton_MultipleBackwardTransitions_ValidTransitionAction()
        {
            var action = new IncrementAction();
            var initial = new State<string>();
            var state1 = new State<string>();
            var state2 = new State<string>();
            var state3 = new State<string>();
            var state4 = new State<string>();
            StateSequence(initial, [(state1, "a"), (state2, "b"), (state3, "c"), (state4, "d")], action);

            var automaton = new StackAutomaton<string>();
            automaton.Push(initial);
            Read(automaton, ["a", "b", "c", "d", "e", "e", "e", "e"]);

            Assert.Equal(4, action.Count);
        }

        // Final states

        [Fact]
        public void StackAutomaton_FinalTransition_ShouldGoBack()
        {
            var initial = new State<string>();
            var final = new State<string>().SetFinal();
            StateSequence(initial, [(final, "a")]);

            var automaton = new StackAutomaton<string>();
            automaton.Push(initial);
            Read(automaton, ["a"]);

            Assert.Equal(initial, automaton.CurrentState);
        }

        [Fact]
        public void StackAutomaton_FinalTransition_ActivateActionOnFinalNode()
        {
            var action = new IncrementAction();
            var initial = new State<string>();
            var final = new State<string>().SetFinal().SetAction(action);
            StateSequence(initial, [(final, "a")]);

            var automaton = new StackAutomaton<string>();
            automaton.Push(initial);
            Read(automaton, ["a"]);

            Assert.Equal(1, action.Count);
        }

        [Fact]
        public void StackAutomaton_FinalTransition_ActivateActionOnGoingBack()
        {
            var action = new IncrementAction();
            var initial = new State<string>().SetAction(action);
            var final = new State<string>().SetFinal();
            StateSequence(initial, [(final, "a")]);

            var automaton = new StackAutomaton<string>();
            automaton.Push(initial);
            Read(automaton, ["a"]);

            Assert.Equal(1, action.Count);
        }

    }
}
