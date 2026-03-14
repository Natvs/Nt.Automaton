using System;
using System.Collections.Generic;
using System.Text;

namespace Nt.Automaton.Automatons.Exceptions
{
    internal class NullStateException(string message) : Exception(message) { }
}
