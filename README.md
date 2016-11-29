# Switchboard

Why

Because asynchronous, cross-process messaging is an effective way to scale an application out, not up. It also drives a clear separation of concerns, leading to better design.

What

A lightweight message broker that receives messages from a queue and passes them to handlers based on message type. Message handlers are implemented as plug-ins that the broker discovers and loads at runtime using MEF.

Each message handler is loaded in its own app domain allowing isolation of types between handlers. The host is type agnostic so the only components that need to know a type are the sender and the handler. This also means that the sender does not need to take a dependency on Switchboard - it can interact directly with the queue.

How

1. Install the broker Windows service

The broker can be hosted by any process, but it is typically deployed as a Windows service.

2. Configure the broker

The broker needs to know the path(s) to your queue(s) and the root folder for your handlers.

3. Write a handler

Inherit HandlerBase from Switchboard.Common. Set the HandledTypes array to indicate the message type(s) you are interested in and then override ProcessMessageData to get the message instances handed to you when they come down the queue.

4. Drop your handler in a folder

Create a folder under the root that you configured for the broker. Include your handler and any other dependent assemblies. Each handler you create can have its own folder and each folder will become its own app domain - so there won't be any conflicts if there are duplicate assemblies across folders.

5. Start the broker service

The broker will load up your handler(s), attach to the queues and start listening. If a message comes along, the broker will query your handler and, if the handler is interested in that message type, the host will call the handler's ProcessMessageData method, passing the message.
