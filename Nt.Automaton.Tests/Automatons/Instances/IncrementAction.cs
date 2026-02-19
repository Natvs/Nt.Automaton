using Nt.Automaton.Actions;
using Nt.Automaton.Tokens;

namespace Nt.Tests.Automaton.Automatons.Instances
{
    internal class IncrementAction : IAction, ITokenAction<string>
    {
        public int Count { get; private set; } = 0;

        public void Perform()
        {
            Count++;
        }

        public void Perform(IAutomatonToken<string> token)
        {
            Count++;
        }
    }


}
