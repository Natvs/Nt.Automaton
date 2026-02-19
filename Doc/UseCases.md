# Use cases

Here you will find some use cases of automatons and the differents combinations of components used for each one of them.

---

## Text parsing

### Context
You have one component for the user to fill. You have a text file written by a client that describes how to fill the component. You would like to read the text file and fill your component with the informations in it. Of course, some formats like `JSON` or `XML` can easily be parsed using already existing libraries. However these formats are not the most user-friendly ones, and you want the user to write content in a more natural language. So you decide of a syntax to use and inform the user about that.

### Problem
The client provides you with such a file. Now you have to parse it and fill your component. 

### Solution
This project can widely be used for parsing any kind of tokens sequence, including text tokens.

**Remarks**: The first step is to split the file into tokens. This can be done using [Nt.Parser](https://github.com/Natvs/Nt.Parser) project for example (in that case, you'll have to create a token class that extends both `Nt.Automaton.Token.IAutomatonToken` and `Nt.Parser.Symbols.ISymbol`). In the following, the token resulting from the parsing is called `Token`.

### Components used

- **Automaton**: `Nt.Automaton.Automatons.StateAutomaton`
- **State**: `Nt.Automaton.States.StillState`
- **Transition**: `Nt.Automaton.Transitions.Transition`

### Example

This example will fill a grid from the content of the file. The algorithm here is still simple for demonstration purpose. You may actually use if for more complex ones.

**Input**
The first line is the dimension of the array. The following is the content.

```
10, 15
X O X X X X X X X X X X X X X
X O X O O O X O O O X O O O X
X O X O X O X O X O X O X O X
X O X O X O X O X O X O X O X
X O X O X O X O X O X O X O X
X O X O X O X O X O X O X O X
X O X O X O X O X O X O X O X
X O X O X O X O X O X O X O X
X O O O X O O O X O O O X O X
X X X X X X X X X X X X X O X
```

After parsing, you get a list like `['10', '15', 'X', '0', 'X', 'X', ...]`

**Grid**
```csharp
class Grid() 
{
    public int LinesCount { get; private set; }
    public int ColumnsCount { get; private set; }
    public int[] Content { get; private set; }

    private int _currentLine = 0;
    private int _currentColumn = 0;

    private void increment() {
        if (_currentColumn == ColumnsCount) {
            _currentColumn = 0; _currentLine ++;
            return;
        }
        _currentColumn++;
    }

    public void SetLines(int lines) => LinesCount = lines;

    public void SetColumns(int colums) => ColumnsCount = columns;

    public void InitContent() => Content = new int[LinesCount * ColumnsCount];

    public void Add(int value) {
        Content[_currentLine*ColumnsCount + _currentColumn] = value;
        increment()
    }
}

```

**Actions**
```csharp
using ITokenAction = Nt.Automatons.Actions.ITokenAction<string>;

class SetLinesAction(Grid grid) : ITokenAction {
    void Perform(IAutomatonToken token) {
        var lines = (int)token.Value;
        grid.SetLines(lines);
    }
}

class SetColumnsAction(Grid grid) : ITokenAction {
    void Perform(IAutomatonToken token) {
        var columns = (int)token.Value;
        grid.SetColumns(columns);
        grid.Init();
    }
}

class AddOAction(Grid grid) : ITokenAction {
    void Perform(IAutomatonToken token) {
        grid.Add(0)
    }
}

class AddXAction(Grid grid): ITokenAction {
    void Perform(IAutomatonToken token) {
        grid.Add(1)
    }
}
```

**Grid service**
```csharp
using Automaton = Nt.Automaton.Automatons.StateAutomaton<string>;
using State = Nt.Automaton.Automatons.StillState<string>;
using Transition = Nt.Automaton.Transitions.Transition<string>;

class GridService(Grid grid) {
    Automaton Automaton { get; } = new Automaton(new State());

    public void FromTokens(List<Token> tokens) {
        SetAutomaton();

        for (var token in tokens) {
            Automaton.Read(token);
        }
    }

    void SetAutomaton() {
        var readLineState = Automaton.CurrentState;
        var readColumnState = new State();
        var fillGridState = new State();

        readLineState.AddDefault(readColumnState, new SetLinesAction(grid));
        readColumnState.AddDefault(fillGridState, new SetColumnsAction(grid));
        fillGridState.AddTransition(new Transition("X", fillGridState, new AddXAction(grid);))
        fillGridState.AddTransition(new Transition("O", fillGridState, new AddOAction(grid);))
    }
}
```


---

## Configuration with interface

### Context
You have a structure that is a global configuration for other components.

### Problem
You would like the client to edit the configuration at runtime, so you can't generate the configuration from a file and parse it (like the example before). A solution is to add a user interface that allows the client to view and edit different parts of the configuration dynamically. The structure of `StackAutomaton` is made for that scenario. You focus on the code for each part of the configuration, and the automaton handles the orchestration of these different steps for you.

### Components used

- **Automaton** : `Nt.Automaton.Automatons.StackAutomaton`
- **State** : `Nt.Automaton.States.State`
- **Transition** : `Nt.Automaton.Transitions.Transition`

### Example
You have a configuration with many parameters:
- `ENABLE`: boolean
- `research configuration`
    - `ITERATIONS`: int
    - `MODE`: STANDARD|LOW|HIGH
- ...

Using a command line application, you want to let the user edit it dynamically (by following a dynamic interface).

> All the parameters here are only for demonstration purpose and aren't supposed to represent any configuration of a real system.

The automaton can be seen as a graph with nodes and leaves, where nodes are states asking the user about a field to edit and each field has a leave node to set a new value. For better understanding,, the graph may have the same structure has the configuration.

**Configuration**
```csharp
class Configuration {
    boolean Enabled { get; set; } = false;
    ResearchConfiguration ResearchConfiguration { get; } = new();
}

class ResearchConfiguration {
    int Iterations { get; set; } = 10;
    Modes Mode { get; set; } = Modes.STANDARD;
}

enum Modes {STANDARD, LOW, HIGH}
```

**Actions**
```csharp
using IAction = Nt.Automaton.Actions.IAction<int>;
using AutomatonToken = Nt.Automaton.Tokens.AutomatonToken<int>;
using State = Nt.Automaton.States.State<int>;
using StackAutomaton = Nt.Automaton.Automatons.StackAutomaton<int>;

// This is the first action triggered when the client opens the configuration edition service
class BaseAction(StackAutomaton automaton): IAction {
    static void SetAutomaton(State parent, Automaton automaton, Config config) {
        var enableState = new State(new EnableAction(automaton, config));
        var researchState = new State(new ResearchState(automaton));

        parent.AddTransition(new Transition(1, enableState));
        parent.AddTransition(new Transition(2, researchState));

        ResearchAction.SetAutomaton(researchState, automaton, config);
    }

    void Perform() {
        Console.Writeline("Select a field to edit:");
        Console.Writeline("1. ENABLE");
        Console.Writeline("2. Configure research");
        Console.Writeline("3. Exit");
    }
}

// Once the client enters the "Configure reserach" from the previous action, this action is triggered
class ResearchAction(StackAutomaton automaton): IAction {
    static void SetAutomaton(State parent, Automaton automaton, Config config) {
        var iterationsState = new State(new EnableAction(automaton, config));
        var modeState = new State(new EnableAction(automaton, config)); 

        parent.AddTransition(new Transition(1, iterationState));
        parent.AddTransition(new Transition(2, modeState));
    }

    void Perform() {
        Console.Writeline("Select a field to edit research:");
        Console.Writeline("1. ITERATIONS");
        Console.Writeline("2. MODE");
        Console.Writeline("3. Exit");
    }
}

// The following actions are linked to the final nodes
class EnableAction(StackAutomaton automaton, Configuration config): IAction {
    void Perform() {
        Console.Writeline("Select a value:");
        Console.Writeline("1. On");
        Console.Writeline("2. Off");
        Console.Writeline("3. Cancel");

        var answer = Console.Readline;
        if (config == "1") config.Enabled = true;
        if (config == "2") config.Enabled = false;
    }
}

class IterationsAction(StackAutomaton automaton, Configuration config): IAction {
    void Perform() {
        Console.Writeline("Enter the number of iterations (default is 10):");

        var answer = Console.Readline();
        config.Iterations = (int)answer;
    }
}

class ModeAction(StackAutomaton automaton, Configuration config): IAction {
    void Perform() {
        Console.Writeline("Select the mode to set:")
        Console.Writeline("1. Standard");
        Console.Writeline("2. Low");
        Console.Writeline("3. High");
        Console.Writeline("4. Cancel");

        var answer = Console.Readline;
        config.Mode = answer switch {
            "1" => Modes.STANDARD,
            "2" => Modes.LOW,
            "3" => Modes.HIGH,
            _ => config.Mode
        };
    }
}
```

**Configuration service**
```csharp
using Automaton = Nt.Automaton.Automatons.StackAutomaton<int>;

class ConfigurationService(Configuration config) {
    Automaton Automaton { get; } = new Automaton().SetAutoPerformAction();

    void StartUI() {
        // Construct the automaton structure
        var initialState = new State(new BaseAction(Automaton, config));
        BaseAction.SetAutomaton(initialState);

        // Iterate until the user escapes from the initial state
        Automaton.Push(initialState, true);
        while (!Automaton.IsEmpty()) {
            var answer = (int)Console.Readline();
            Automaton.Read(new AutomatonToken(answer));
        }
    }
}
```