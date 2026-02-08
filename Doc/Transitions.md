# Transitions

- [Common features of a transition](#common-features-of-a-transition)
- [Types of transitions](#types-of-transitions)
    - [Transition](#transition) 

## Common features of a transition

Transitions have some common features
- A field `Value` that is the type of tokens to parse (`string`, `int`, `char`...)
- A field `Target` that points to the state the transitions leads to
- A field `Action` that represents the action associated to the transition

All of these features describe an interface `ITransition` that any transition should implement.

## Types of transitions
As for now, only one type of transition is implemented in this project:
- [Transition](#transition)

### Transition

**Field**
|Name|Type|Description|
|----|----|-----------|
|Value|string|String value associated to the transition|
|Target|IState|State associated to the transition|
|Action|IAction|Action associated to the transition|

**Methods**
|Name|Parameters|Return type|Description|
|----|----------|-----------|-----------|
|Transition(string value, IState newstate)|Value and state linked to this transition|Transition|Instantiates a new state with a value and a destination state|
|Transition(string Value, IState newstate, IAction action)|Value, state and action linkied to this transition|Transition|Instantiates a new states with a value, a destination state and an action linked to it|