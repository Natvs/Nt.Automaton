# States

- [Common features of a state](#common-features-of-a-state)
- [Different types of states](#different-type-of-states)
	- [State](#state)
	- [StillState](#stillstate)

## Common features of a state

All states have common features
- A field `Action` that describes an action linked to the state
- A method `Read(IAutomatonToken token)` where token is the token to read
- A method `AddTransition(ITransition transition)` to add transitions from this state
- Two events `OnReached` and `OnLeft` to trigger when the state is reached or left.

These features describes the interface `IState` that all states must implement. The difference between the different instances of states that exist is the interpretation of the fields, methods and events.

## Different type of states

Here is a list of implemented states in this library:
- [State](#state)
- [StillState](#stillstate)

### State

The state `State` is a state that handles the associated action automatically when reading. You do not have to trigger the state manually.

It also has fields `DefaultState` and `DefaultAction` when the token is none of the registered transitions.

**Fields**
|Name|Type|Description|
|----|----|-----------|
|Action|IAction|The action to be executed when entering this state.|
|Transitions|List<ITransition>|A list of transitions that can be taken from this state.|
|DefaultState|IState|The state to transfer to when none of the above transitions are valid.|
|DefaultAction|IAction|The action to be executed when transferring to the default state.|

**Constructors**
|Name|Parameters|Description|
|----|----------|-----------|
|State()| |Returns a new instance of State without action.|
|State(IAction action)|action linked to the state|Returns a new instance of State with an action.|

**Methods**
|Name|Parameters|Return Type|Description|
|----|----------|-----------|-----------|
|SetDefault(IState state)|state to return when read if no transitions are valid|State|Set the default state to return when no transitions are valid.|
|SetDefault(IState state, IAction action)|state and action to perform when read if no transitions are valid|State|Set the default state and actions when no transitions are valid.|
|AddTransition(ITransition transitions)|transition to add|void|Adds a transition to the list of transitions.|
|AddTransitions(ICollection<ITransition> transitions)|collection of transitions to add|void|Shortcut for adding multiple transitions.|
|OverwriteTransition(ITransition transition)|transition to overwrite|void|Overwrites an existing transition in the list of transitions.|
|Read(IAutomatonToken token)|token to read to take a transition|State|Returns the target state of the right transition, or the default one if there is no such transition.|

**Events**
|Name|Parameters|Description|
|----|----------|-----------|
|StateReached|StateEventsArgs e|Event triggered when a state is reached (after a transition).|
|StateLeft|StateEventsArgs e|Event triggered when a state is left (before a transition).|

**Rules for triggering actions and events**

All actions and events are performed when the method `Read(token)` is called.

When a token is read:
- If a transition with the token read exists:
	1. The event `StateLeft` of the current state is invoked.
	2. The action `Action` associated to the transition is performed.
	3. The event `StateReached` of the target is invoked.
	4. The action `Action` of the target state is performed.
- If no transitions with the token read exists:
	1. The event `StateLeft` of the current state is invoked.
	2. The action `DefaultAction` is performed.
	3. The event `StateReached` of the default target state is invoked.
	4. The action `Action` of the default target state is performed.

### StillState

The state `StillState` is a state that do not trigger the action linked to the state after a transition. You do have to perform the action manually.

It also has fields `DefaultState` and `DefaultAction` when the token is none of the registered transitions.

**Fields**
|Name|Type|Description|
|----|----|-----------|
|Action|IAction|The action to be executed when entering this state.|
|Transitions|List<ITransition>|A list of transitions that can be taken from this state.|
|DefaultState|IState|The state to transfer to when none of the above transitions are valid.|
|DefaultAction|IAction|The action linked to transitions to the default state.|

**Constructors**
|Name|Parameters|Description|
|----|----------|-----------|
|StillState()| |Returns a new instance of StillState without action.|
|StillState(IAction action)|action linked to the state|Returns a new instance of StillState with an action.|

**Methods**
|Name|Parameters|Return Type|Description|
|----|----------|-----------|-----------|
|SetDefault(IState state)|state to return when read if no transitions are valid|StillState|Set the default state to return when no transitions are valid.|
|SetDefault(IState state, IAction action)|state and action to perform when read if no transitions are valid|StillState|Set the default state and actions when no transitions are valid.|
|AddTransition(ITransition transitions)|transition to add|void|Adds a transition to the list of transitions.|
|AddTransitions(ICollection<ITransition> transitions)|collection of transitions to add|void|Shortcut for adding multiple transitions.|
|OverwriteTransition(ITransition transition)|transition to overwrite|void|Overwrites an existing transition in the list of transitions.|
|Read(IAutomatonToken token)|token to read to take a transition|State|Returns the target state of the right transition, or the default one if there is no such transition.|

**Events**
|Name|Parameters|Description|
|----|----------|-----------|
|StateReached|StateEventsArgs e|Event triggered when a state is reached (after a transition).|
|StateLeft|StateEventsArgs e|Event triggered when a state is left (before a transition).|

**Rules for triggering actions and events**

All actions and events are performed when the method `Read(token)` is called.

When a token is read:
- If a transition with the token read exists:
	1. The event `StateLeft` of the current state is invoked.
	2. The action `Action` associated to the transition is performed.
	3. The event `StateReached` of the target is invoked.
- If no transitions with the token read exists:
	1. The event `StateLeft` of the current state is invoked.
	2. The action `DefaultAction` is performed.
	3. The event `StateReached` of the default target state is invoked.

The action `Action` linked to the event is never triggered when reading. You have to execute it manually.