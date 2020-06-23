# UdonStringEvents

A system by which complex events can be sent over the network in Udon for VRChat. Written in U#.

This includes a simple chat system.

An overview of the system can be found here: https://www.youtube.com/watch?v=77Z2nvkVsf4&feature=youtu.be
(Note: the Youtube video is quite out of date)

# HOW TO USE THIS CODE

Assuming you are using UdonSharp:

1. Import the UdonStringEvents prefab into your world and modify the EventHandler.cs file with new event names.
2. Inside any UdonSharp file, pass in the imported EventReceiver object.
3. In any function where you want to send an event with a payload over the network, call the receiver's SendEvent(string eventName, string payload) method with a good event name.
4. In the EventHandler UdonSharp file, add a new case to the switch within Handle(). Match it to the name of the event you named in step 3.
5. In the case, do whatever you like. The private var EventHandler.newEvent contains a comma-separated string array formed as [eventName, payload, clock], where clock is an incrementing integer that is equal to the total number of events sent. If you send a CSV string as a payload, this will be reflected in the received event.

Take a look at UdonChat for an example of this in use.

# COMPONENTS

1. UdonStringEvents

UdonStringEvent hides away event management behind syncing strings, meaning you can avoid having data races between updated synced vars and custom network events.

2. UdonChat

An implementation of UdonStringEvent that makes a text chat interface for VRC. The latest prefab is always available on the Releases page.

3. UdonKeyboard

A UK English Keyboard built in Udon. Easy to modify for other layouts. Feeds text into an InputField by default, but this is also easy to change. It is built into the UdonChat prefab.

# TECHNICAL NOTES

When implementing UdonStringEvents you'll probably want to unpick how UdonChat was made first, as it'll illuminate how to make the systems go brrrr.

EventReceivers pass all events to EventHandlers. An EventHandler is an abstract class that can be described as follows:

EventHandler {
string characterName;
string newEvent;
void HandleEvent();
}

An EventHandler is an UdonBehaviour, meaning this is a clean exit point into UdonGraph or other Udon implementations. As in, yes, you can use UdonStringEvents and its friends if you write in UdonGraph, as long as your EventHandler graph implements the above interface.

There are three problems with this system:

1. Speed. This is the slowest possible way to send events in Udon.
2. Network load. This is a high-intensity system and will negatively effect your network performance if abused.
3. Event volume. If you send more than one event every half second or so via this system, there is a risk that the earlier of the two events is not received. Take
   care to only use this system when you need a payload, and use SendCustomNetworkEvent otherwise.
