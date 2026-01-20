using Nt.Automaton.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nt.Tests.Automaton.Instances
{
    internal class Token(string name) : IAutomatonToken<string>
    {
        public string Value { get; private set; } = name;
    }
}
