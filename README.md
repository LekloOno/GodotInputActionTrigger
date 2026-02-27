# GIAT - Godot Input Action Trigger

GIAT describes generic actions, their inputs and tiggers, and how such components interract together.

It is thought in a Tree like structure to seamlessly integrates within the Godot editor.

## Goal

The goal of this plugin is to provide actions that can easily be swapped to various execution context, and branched in flexible ways to various trigger contexts.

### Example - A jump

Let's take the example of a jump.

The behavior of the jump will pretty much always be the same - modify the upward velocity of the player.

However, there is a wide range of way we could trigger such a jump.

**Simplest setup**  
If I take the jump, and put it in a new blank scene, I want it to work as is, with no more dependencies than what it strictly needs to.

You're on ground, you press jump, the player jump !

So the only component you should need is - something that handles the input and use it to trigger the jump, and something that detects if the player is on ground so the jump knows if it can jump or not.

**Buffer**  
Now, in this simple scene, maybe you want to add a buffer behavior.

If you press space bar just a little too early, you don't jump as you're not grounded yet. You wish the jump to be permissive if the delay is short enough, or if the player holds space bar.

You would typically implement this by having the space input buffered, and have some external trigger try to consume that input.

The input handler tries to trigger the action, and stores the input if the action couldn't be performed.

In parallel, you have a buffer consumer - when you land on the ground, it checks if an input is buffered, and triggers the action if so.

**Sequence**  
Having component that can be used independantly, in a blank scene like that, is great, notably because it enables extensive testability.  

However, such independance has a cost - coherence and predictability.  
Later, you might have a wide range of other movement mechanics, and having all these component act independantly introduce unpredictability.

If the ground movement performs after the jump, the behavior will be slightly different than if it did before. This seems like a detail, but in games with crucial complex physics-based movements, this can become a real problem.

So, you wish you could take this same independant jump component, and now bind it in a deterministic orchestrator.

**Short circuit**  
Additionnaly, maybe you want some other components to be able to act as disguised input sender. Like an enemy that takes control of the player for a brief moment.


## Concept

What we can conclude from the list of scenarios described above is as follows -
- Actions behavior should be decouplated from their trigger
- Various triggers (like, an input handler, and multiple buffer consumers) can act on a single action

To build the most flexible structure, the proposed architecture is as follows.

### Base - IAT

There are three fundamental components in GIAT: **Inputs**, **Actions**, and **Triggers**.

**Input** is a data that is transmitted along the communication channels of GIAT structures.

**Action** is something that, provided an *Input* of type `T`, tries to perform some action, and returns whether or not it successfully performed it.

**Trigger** is something that provides an *Input* of type `T` to an *Action* of matching input type. Unlike action, it does not expose any interface to the world, it's just a tag that declares its intention of triggering an action.

Further behavior are derived from these three simple components.

### Trigger semantic

As mentionned, triggers are a tag, not an exposed interface. The meaning of this tag changes depending on the structural context.

A free trigger is the simplest, it defines freely its action(s).

A node trigger on the other hand defines automatically its action(s) based on structural identity.
- If it has children actions, it becomes the prioritary trigger of these.
- If it has a parent action, it becomes a subsidiary trigger of it.

It defaults to the first if it has both a parent and children actions.

To avoid infinite recursion, a parent trigger is always prioritary.  
If a Trigger is also an action, and its parent action is also a trigger, the it must have children actions and cannot use its parent as its action.

The concrete meaning of this will appear clearer later.

### Input Handler

An input handler is an hybrid composed component.

It introduces a new concept, the **Producer**.
It decribe components that produce unhandled *Inputs*, that is, without triggering directly an *Action*.

#### General definition
An input handler is an **AP(T)**.

Externally :
- An input handler is exposed as an **Action**.
  
Internally :  
- It is a sporadic **Producer**.  
- It is a (node) **Trigger** in some structural context.

#### Examples

When the input handler is the root of a GIAT structure, it acts as an **APT**.

**Trigger** | When the input is pressed - the handler transmits it to the action it is attached to.  
**Producer** | If the action fails, it stores the input in a buffer.  
**Action** | Other triggers can then try to consume the buffer, to transmit it to the input handler's action.

This enables the two first goal - a simple scene, in which you can also add a buffer mechanism.

You would have
```
└── space handler       <---+
    ├── jump                |
    └── (on land trigger) --+
```

Now, if the input handler has a trigger parent, this signals it that it should not take the trigger responsibility anymore, since this trigger is a prioritary trigger.  

It is now an **AP**.

**Producer** | When the input is pressed, it stores the input in a buffer.  
**Action** | Other triggers can then try to consume the buffer, to transmit it to the input handler's action.

This enables the third goal, while remaining compatible with the first one. We just have to change the structure.

You would have
```
└── orchestrator trigger
    ├── space handler
    │   └── jump
    ├── shift handler
    │   └── sprint
    ...
```


