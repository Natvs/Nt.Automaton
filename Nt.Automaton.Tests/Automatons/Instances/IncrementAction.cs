using Nt.Automaton.Actions;
using Nt.Automaton.Tokens;

namespace Nt.Tests.Automaton.Automatons.Instances
{
    internal class IncrementAction : IAction, ITokenAction<string>
    {
        public int Count { get; private set; } = 0;
        private Func<bool> Condition { get; set; } = () => true;

        public IncrementAction SetCondition(Func<bool> condition)
        {
            Condition = condition;
            return this;
        }

        public void Perform()
        {
            if (Condition())
            {
                Count++;
            }
        }

        public void Perform(IAutomatonToken<string> token)
        {
            if (Condition())
            {
                Count++;
            }
        }
    }

}
