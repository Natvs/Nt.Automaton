using Nt.Automaton.Tokens;

namespace Nt.Tests.Automaton.Instances
{
    internal class Token(string name) : IAutomatonToken<string>
    {
        public string Value { get; private set; } = name;
    }
}
