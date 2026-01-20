using Nt.Automaton;
using Nt.Automaton.States;
using Nt.Automaton.Transitions;
using Nt.Tests.Automaton.Instances;

namespace Nt.Tests.Automaton
{
    public class StateAutomatonTest
    {

        [Fact]
        public void AutomatonTransition_Test1()
        {
            State<string> initial = new(), state1 = new();
            StateSequence(initial, [(state1, "a")]);

            var automaton = new StateAutomaton<string>(initial);
            Read(automaton, ["a"]);

            Assert.Equal(state1, automaton.CurrentState);
        }

        [Fact]
        public void AutomatonTransition_Test2()
        {
            State<string> initial = new(), state1 = new(), state2 = new(), state3 = new(), state4 = new();
            StateSequence(initial, [(state1, "a"), (state2, "b"), (state3, "c"), (state4, "d")]);

            var automaton = new StateAutomaton<string>(initial);
            Read(automaton, ["a", "b", "c", "d"]);

            Assert.Equal(state4, automaton.CurrentState);
        }

        [Fact]
        public void AutomatonDefaultTransition_Test()
        {
            State<string> initial = new(), state1 = new(), state2 = new(), state3 = new(), state4 = new();
            StateSequence(initial, [(state1, "a"), (state2, "b"), (state3, "c"), (state4, "d"), (state1, "e"), (state2, "f"), (state3, "g")]);

            var automaton = new StateAutomaton<string>(initial);
            Read(automaton, ["a", "b", "c", "d", "f", "g", "e"]);

            Assert.Equal(initial, automaton.CurrentState);
        }

        private static void StateSequence(State<string> initial, List<(State<string>, string)> states)
        {
            initial.SetDefault(initial);
            var lastState = initial;
            foreach (var (state, word) in states)
            {
                state.SetDefault(initial);
                lastState.AddTransition(new Transition<string>(word, state));
                lastState = state;
            }
        }

        private static void Read(StateAutomaton<string> automaton, List<string> words)
        {
            foreach (var word in words) automaton.Read(new Token(word));
        }
    }
}
