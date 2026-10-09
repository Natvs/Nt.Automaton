using Nt.Automaton.Actions;
using Nt.Automaton.Tokens;

namespace Nt.Tests.Automaton.Transitions.Instances
{
    public class TriggerTokenAction<T> : ITokenAction<T>
    {

        public bool Triggered { get; private set; } = false;

        public void Perform(IAutomatonToken<T> token)
        {
            Triggered = true;
        }

    }
}
