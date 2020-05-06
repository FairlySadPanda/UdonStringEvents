# UdonStringEvents
A system by which complex events can be sent over the network in Udon for VRChat. Written in U#.

This includes a simple chat system.

An overview of the system can be found here: https://www.youtube.com/watch?v=77Z2nvkVsf4&feature=youtu.be
(Note: the Youtube video is a bit out of date)

# COMPONENTS

1) UdonStringEvents

UdonStringEvent hides away event management behind syncing strings, meaning you can avoid having data races between updated synced vars and custom network events. Two different kinds of event receivers exist - the basic one is a shotgun which blasts an event with payload out to all world members, and the PlayerEvent version targets the specific owner of the receiver with the event.

2) UdonChat

An implementation of UdonStringEvent that makes a text chat interface for VRC. The latest prefab is always available on the Releases page.

3) UdonKeyboard

A UK English Keyboard built in Udon. Easy to modify for other layouts. Feeds text into an InputField by default, but this is also easy to change.

# TECHNICAL NOTES

When implementing UdonStringEvents you'll probably want to unpick how UdonChat was made first, as it'll illuminate how to make the systems go. There's no example code for PlayerEventReceivers yet, unfortunately.

EventReceivers pass all events to EventHandlers. An EventHandler is an abstract class that can be described as follows:

EventHandler {
  string characterName;
  string newEvent;
  void HandleEvent();
}

An EventHandler is an UdonBehaviour, meaning this is a clean exit point into UdonGraph or other Udon implementations(!). As in, yes, you can use UdonStringEvents and its friends if you write in UdonGraph, as long as your EventHandler graph implements the above interface.
