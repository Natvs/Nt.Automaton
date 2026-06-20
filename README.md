# Nt.Automaton

- [Introduction](#introduction)
- [Features](#features)
- [Using an automaton](#using-an-automaton)
	- [Creating a token](#creating-a-token)	 
	- [Defining actions](#defining-actions)
	- [Creating states and transition](#creating-states-and-transitions)
- [Customising the automaton](#customising-the-automaton)
	- [Custom states](#custom-states)
	- [Custom transitions](#custom-transitions)

## Introduction
Nt.Automaton is an automate containing a set of states and transitions between those states.
Each state or transition can be linked to an action, so that any structure or information for the user can be processed from any sequences.
The entire automaton project is made for a generic type. From string to complex structures, there are possibility to read almost any data.

Applications of this library include:
- Parsing structured text formats (used for example in compilers or reading datas from files)
- Updating user interfaces based on input or events
- Behavior of entities (like PNJ in a video game or other simulations)
- Any kind of workflow or any other state-based logic

## Features
- Define states and transitions for an automate.
- Link actions to states and transitions.
- Iterate over a list of tokens and process them according to the defined automate.
- Easily extendable for custom behavior.
- Lightweight and efficient.

## Using an automaton

### Creating a token
The automaton functions by processing tokens that implement the `IAutomatonToken` interface.
Such interface only requires a `Name` property with a public getter, which represents the value of the token and can be of any type.

Example of the already implemented token:
```csharp
public class AutomatonToken<T>(T name) : IAutomatonToken<T>
{
    public T Name { get; } = name;
}

// you need to specify the type of the token, for example AutomatonToken<string>
```

The advantage of `IAutomatonToken` is that you can extend any existing class to implement it, and then transform them into automaton tokens (without having to create a new class for tokens).

### Defining actions
When a state is reached or a transition is taken, an action can be executed. These actions can be whatever you prefer and are yours to implement.
An action must implement the `IAction` interface which requires a single method `Perform(IAutomatonToken token)` where `token` is the current token being processed by the automaton.

Example of a custom action implementation:
```csharp
using Nt.Automaton.Actions;

public class MyAction : IAction<string> // or int, char... 
{
	public void Perform(IAutomatonToken<string> token)
	{
		// Your action logic here
	}
}
```

## Creating states and transitions
A state represents a specific condition or situation in the automate. The specificity of `Nt.Automaton` is the possibility to declare actions linked to states or transitions.

The base structure you will need is an instance of `StateAutomaton`, which contains states and transitions.
This state automaton is an implementation of `IAutomaton` with one current state at a time. See below for more automatons.

You'll need to define an initial state to initialize the automaton, and then you can add states and transitions as needed.

1. Create a default state
```csharp
using  Nt.Automaton.States;

var defaultState = new State();
```

2. Create an automaton with the default state
```csharp
using Nt.Automaton;

var automaton = new StateAutomaton(defaultState);
```

3. Add states and transitions
```csharp
using Nt.Automaton.States;

// Define the tokens used in the automaton
var tokenA = new MyToken("A");
var tokenB = new MyToken("B");

// Define the actions that are triggered
var action = new MyAction();

// Create a new state
var stateA = new State();

// Create a new state with an action to trigger when entering the state
var stateB = new State(new MyAction());

// Add a default state to transfer to when no transition is valid
stateA.SetDefault(stateB);

// Add a default state with an action to trigger when transferring to the default state
stateB.SetDefault(stateA, action);

// Add a transition from stateA to stateB when the token read is "B"
stateA.AddTransition(new Transition(tokenB, stateB)));

// Add a transition from stateB to stateA with an action when the token read is "A"
stateB.AddTransition(new Transition(tokenA, stateA, action));
```

## Customising the automaton
A particularity of `Nt.Automaton` is that components are largely customisable, including the automatons. This project includes some implementations for quick use like `StateAutomaton`, but feel free to create your own implementations at any time by extending the `IAutomaton` interface.

Two implementations already exists:
- `Nt.Automaton.Automatons.StateAutomaton`
- `Nt.Automaton.Automatons.StackAutomaton`

See the [automatons documentation](Doc/Automaton.md) for more details.

---

### Custom states
By extending the `IState` interface, it is possible to create other types of states than the default one `State`. 

Two implementations are available:
- `Nt.Automaton.States.State`
- `Nt.Automaton.States.StillState`

See the [states documentation](Doc/States.md) for more details.

---

### Custom transitions
Similarly, you can declare your own transitions by extending the `ITransition` interface.

One implementation is available:
- `Nt.Automaton.Transitions.Transition`

See the [transitions documention](Doc/Transitions.md) for more details.