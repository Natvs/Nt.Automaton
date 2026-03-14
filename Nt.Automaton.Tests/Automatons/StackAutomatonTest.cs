using Nt.Automaton.Automatons;
using Nt.Automaton.States;
using Nt.Tests.Automaton.Automatons.Instances;

using static Nt.Tests.Automaton.Automatons.AutomatonUtils;

namespace Nt.Tests.Automaton.Automatons
{
    public class StackAutomatonTest
    {
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

        [Fact]
        public void StackAutomaton_SingleTransition_ValidStateAction()
        {
            var action = new IncrementAction();
            State<string> initial = new(), state1 = new(action);
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
            State<string> initial = new(), state1 = new(action);
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
            State<string> initial = new(), state1 = new(action), state2 = new(action), state3 = new(action), state4 = new(action);
            StateSequence(initial, [(state1, "a"), (state2, "b"), (state3, "c"), (state4, "d")]);

            var automaton = new StackAutomaton<string>();
            automaton.Push(initial);
            Read(automaton, ["a", "b", "c", "d"]);
            Assert.Equal(4, action.Count);
        }

        [Fact]
        public void StackAutomaton_MultipleBackwardTransitionsWithoutAutoPerform_ValidStateAction()
        {
            var action = new IncrementAction();
            State<string> initial = new(), state1 = new(action), state2 = new(action), state3 = new(action), state4 = new(action);
            StateSequence(initial, [(state1, "a"), (state2, "b"), (state3, "c"), (state4, "d")]);

            var automaton = new StackAutomaton<string>();
            automaton.Push(initial);
            Read(automaton, ["a", "b", "c", "d", "e", "e", "e", "e"]);

            Assert.Equal(4, action.Count);
        }

        [Fact]
        public void StackAutomaton_MultipleBackwardTransitionsWithAutoPerform_ValidStateAction()
        {
            var action = new IncrementAction();
            State<string> initial = new(), state1 = new(action), state2 = new(action), state3 = new(action), state4 = new(action);
            StateSequence(initial, [(state1, "a"), (state2, "b"), (state3, "c"), (state4, "d")]);

            var automaton = new StackAutomaton<string>().SetAutoPerformAction();
            automaton.Push(initial);
            Read(automaton, ["a", "b", "c", "d", "e", "e", "e", "e"]);

            Assert.Equal(7, action.Count);
        }

        [Fact]
        public void StackAutomaton_SingleTransition_ValidTransitionAction()
        {
            var action = new IncrementAction();
            State<string> initial = new(), state1 = new();
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
            State<string> initial = new(), state1 = new(), state2 = new(), state3 = new(), state4 = new();
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
            State<string> initial = new(), state1 = new(), state2 = new(), state3 = new(), state4 = new();
            StateSequence(initial, [(state1, "a"), (state2, "b"), (state3, "c"), (state4, "d")], action);

            var automaton = new StackAutomaton<string>();
            automaton.Push(initial);
            Read(automaton, ["a", "b", "c", "d", "e", "e", "e", "e"]);

            Assert.Equal(4, action.Count);
        }
    }
}
