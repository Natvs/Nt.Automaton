using Nt.Automaton.Actions;
using Nt.Automaton.Automatons;
using Nt.Automaton.States;
using Nt.Automaton.Transitions;
using Nt.Tests.Automaton.Automatons.Instances;

namespace Nt.Tests.Automaton.Automatons
{
    internal class AutomatonUtils
    {
        public static void StateSequence(State<string> initial, List<(State<string>, string)> states, ITokenAction<string>? action = null)
        {
            var lastState = initial;
            foreach (var (state, word) in states)
            {
                lastState.AddTransition(new Transition<string>(word, state, action));
                lastState = state;
            }
        }

        public static void Read(IAutomaton<string> automaton, List<string> words)
        {
            foreach (var word in words) automaton.Read(new Token(word));
        }
    }
}
