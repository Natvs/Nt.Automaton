using Nt.Automaton.Tokens;

namespace Nt.Tests.Automaton.Automatons.Instances
{
    internal class Token(string name) : IAutomatonToken<string>
    {
        public string Value { get; private set; } = name;
    }
}
