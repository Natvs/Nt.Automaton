# Nt.Automaton

- [Introduction](#introduction)
- [Features](#features)
- [Using an automaton](#using-an-automaton)
	- [Creating a token](#creating-a-token)	 
	- [Defining actions](#defining-actions)
	- [Creating states and transition](#creating-states-and-transitions)
- [Customising the automate](#customising-the-automate)
	- [Custom states](#custom-states)
	- [Custom transitions](#custom-transitions)

## Introduction
Nt.Automaton is an automate containing a set of states and transitions between those states.
Each state or transition can be linked to an action, so that any structure or information for the user can be processed from a simple text.

Applications of this library include:
- Parsing structured text formats (used for example in compilers or reading datas from files)
- Updating user interfaces based on input or events
- Behavior of entities (like PNJ in a video game or other simulations)
- Any king of workflow or any other state-based logic

## Features
- Define states and transitions for an automate.
- Link actions to states and transitions.
- Iterate over a list of tokens and process them according to the defined automate.
- Easily extendable for custom behavior.
- Lightweight and efficient.

## Using an automaton

### Creating a token
The automaton functions by processing tokens that implement the `IAutomatonToken` interface.
Such interface only requires a `string Name` property with a public getter, which represents the value of the token.

Example of a custom token implementation:
```csharp
using Nt.Automaton.Tokens;

public class MyToken(string name) : IAutomatonToken
{
    public string Name { get; } = name;
}
```

The advantage of this interface is that you can extend any existing class to implement it, and then transform them into automaton tokens (without having to create new tokens).

### Defining actions
When a state is reached or a transition is taken, an action can be executed. These actions can be whatever you prefer and are yours to implement.
An action must implement the `IAction` interface which requires a single method `Perform(IAutomatonToken token)` where `token` is the current token being processed by the automaton.

Example of a custom action implementation:
```csharp
using Nt.Automaton.Actions;

public class MyAction : IAction
{
	public void Perform(IAutomatonToken token)
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

# Define the tokens used in the automaton
var tokenA = new MyToken("A");
var tokenB = new MyToken("B");

# Define the actions that are triggered
var action = new MyAction();

# Create a new state
var stateA = new State();

# Create a new state with an action to trigger when entering the state
var stateB = new State(new MyAction());

# Add a default state to transfer to when no transition is valid
stateA.SetDefault(stateB);

# Add a default state with an action to trigger when transferring to the default state
stateB.SetDefault(stateA, action);

# Add a transition from stateA to stateB when the token read is "B"
stateA.AddTransition(new Transition(tokenB, stateB)));

# Add a transition from stateB to stateA with an action when the token read is "A"
stateB.AddTransition(new Transition(tokenA, stateA, action));
```

## Customising the automate
It is possible to create your own automatons by implementing the `IAutomaton` interface and the method `Read(IAutomatonToken token)`.

Here is a list of available implenented automatons from the library:

#### State Automaton

The `StateAutomaton` is a common automaton type with always one active state. It handles one token at a time.
**Fields**
|Name|Type|Description|
|----|----|-----------|
|InitialState|IState|The initial state, set in the constructor of the automaton|
|CurrentState|IState|The current state of the automaton.|

**Methods**
|Name|Parameters|Return Type|Description|
|----|----------|-----------|-----------|
|StateAutomaton(IState initialState)|initial state of the automaton|StateAutomaton|Default constructor of a new instance of StateAutomaton|
|Read(IAutomatonToken token)|token to read|void|Processes the given token and updates the current state accordingly.|

---

### Custom states
By extending the `IState` interface, it is possible to create other types of states. This interface has a field `IAction Action` that can be null and a method `IState Read(IAutomatonToken token)`.

Here is a list of implemented states in this library:

#### State

This kind of state can have only one action, a list of transitions, a default state and a default action. It can only take one transition at a time.

**Fields**
|Name|Type|Description|
|----|----|-----------|
|Action|IAction|The action to be executed when entering this state.|
|Transitions|List<ITransition>|A list of transitions that can be taken from this state.|
|DefaultState|IState|The state to transfer to when none of the above transitions are valid.|
|DefaultAction|IAction|The action to be executed when transferring to the default state.|

**Methods**
|Name|Parameters|Return Type|Description|
|----|----------|-----------|-----------|
|State()| |State|Default constructor of a new instance of State without action.|
|State(IAction action)|action to execute when entering the state|State|Constructor of a new instance of State with an action.|
|SetDefault(IState state)|state to return when read if no transitions are valid|State|Set the default state to return when no transitions are valid.|
|SetDefault(IState state, IAction action)|state and action to perform when read if no transitions are valid|State|Set the default state and actions when no transitions are valid.|
|AddTransition(ITransition transitions)|transition to add|void|Adds a transition to the list of transitions.|
|AddTransitions(ICollection<ITransition> transition)|collection of transitions to add|void|Shortcut for adding multiple transitions.|
|OverwriteTransition(ITransition transition)|transition to overwrite|void|Overwrites an existing transition in the list of transitions.|
|Read(IAutomatonToken token)|token to read to take a transition|State|Returns the target state of the right transition, or the default one if there is no such transition.|

**Rules for triggering actions**

All actions are performed when the method `Read(token)` is called.

When a transition is taken:
- If a transition with the token read exists:
	1. The action `Action` associated to the transition is performed.
	2. The action `Action` of the target state is triggered.
	3. The target state is returned
- If no transitions with the token read exists:
    1. The default action `DefaultAction` is performed
	2. The default state is returned

---

### Custom transitions
You can declare your own transitions by extending the `ITransition` interface, and implementing the three fields `string Value` (value for the transition to be taken), `IState NewState` (usually destination of the transition) and `IAction Action` (action linked to this transition).

This library already contains some implementations:

#### Transition
This transition contains two constructors and only implement the three fields. The interpretation of each field is up to the state when taking the transition.

**Field**
|Name|Type|Description|
|----|----|-----------|
|Value|string|String value associated to the transition|
|NewState|IState|State associated to the transition|
|Action|IAction|Action associated to the transition|

**Methods**
|Name|Parameters|Return type|Description|
|----|----------|-----------|-----------|
|Transition(string value, IState newstate)|Value and state linked to this transition|Transition|Instantiates a new state with a value and a destination state|
|Transition(string Value, IState newstate, IAction action)|Value, state and action linkied to this transition|Transition|Instantiates a new states with a value, a destination state and an action linked to it|