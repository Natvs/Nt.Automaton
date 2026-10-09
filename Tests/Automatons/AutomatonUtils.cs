using Nt.Automaton.Actions;
using Nt.Automaton.Automatons;
using Nt.Automaton.States;
using Nt.Automaton.Tokens;
using Nt.Automaton.Transitions;
using Nt.Tests.Automaton.Automatons.Instances;

namespace Nt.Tests.Automaton.Automatons
{
    internal class AutomatonUtils
    {
        public static void StateSequence(IState<string> initial, List<(IState<string>, string)> states, ITokenAction<string>? action = null)
        {
            var lastState = initial;
            foreach (var (state, word) in states)
            {
                var token = new AutomatonToken<string>(word);
                ITransition<string> transition = new Transition<string>(token, state);
                if (action != null) transition = transition.SetAction(action);
                lastState.AddTransition(transition);
                lastState = state;
            }
        }

        public static void Read(IAutomaton<string> automaton, List<string> words)
        {
            foreach (var word in words) automaton.Read(new Token(word));
        }
    }
}
