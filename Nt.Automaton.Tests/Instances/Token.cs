using Nt.Automaton;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nt.Tests.Automaton.Instances
{
    internal class Token(string name) : IAutomatonToken
    {
        public string Name { get; private set; } = name;
    }
}
