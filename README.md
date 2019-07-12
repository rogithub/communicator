# Communicator

This is a proof of concept to send events between different dotnet/js applications.


## Contents
### Server:
This is a
[signalr](https://dotnet.microsoft.com/apps/aspnet/real-time) server in charge of receiving events and broadcasting them over listeners.
### Communicator
This is the library you will install on your projects to be able to send and receive messages.
### Chat
This is a demo implementation of the Communicator library.

## The library
### There are three types of messages:

* String
* Binary
* Serialized

However a message by itself is of little value. Most of the time what we want is to send a message accompanied of some kind of meta data.
So that a message in the Communicator library has this form:

```cs
public interface IMessage<D, M>
{
    D Data { get; set; }
    M MetaData { get; set; }
}
```

Predefined classes exist to send the three kind of messages:

```cs
  StringMessage(string data, T metaData);

  BinaryMessage(byte[] data, T metaData);

  StringSerializedMessage(T data, T metaData);
```

### Default Metadata
Since metadata would be so common there is a default implementation that is a generic list of KeyValue objects. You can find that class under:

```cs
Communicator.Core.KeyValue
```

## Library Installation
Library is provided as a nuget package.
[https://www.nuget.org/packages/Communicator](https://www.nuget.org/packages/Communicator)

## Library Usage:

### Create an EventSource
First step is get an IEventSource object passing the server url as argument.

```cs
string url = "http://localhost:5000/communicator";
IEventSource source = EventSourceFactory.Get(url); 
```

### Open a connection to the server
Before any event takes place we must open a connection.

```cs
source.Connect().GetAwaiter().GetResult(); 
```

### Sending my first event.
In order to send events we instantiate an IEventSender object then we use
one of its methods to send all three kinds of messages. Make sure you check all cool overloads.

```cs
var sender = source.GetEventSender(serializer);
sender.String(new EventInfo("Chat"), new StringMessage(message, metadata));
sender.Serialized(new EventInfo("Person"), new StringSerializedMessage<Person>(p, metadata));
sender.Binary(new EventInfo("File"), new BinaryMessage(bytes, metadata));
```


### Receiving messages
From a different application (or the same one) you create an IObservablesFactory
using a live (opened) IEventSource.

```cs
var factory = source.GetObservablesFactory(jsonSerializer);

var onChatObservable = factory.GetString("Chat");
var onFileObservable = factory.GetBinary("File");
var onPersonObservable = factory.GetSerialized<Person>("Person");
```

Once we got an IObservable we can attach as many handlers (IObserver) as we want like:

```cs
IObserver myObserver = ...
var onChatObservable.Subscribe(myObserver);
```

C# Introduced, IObserver<T> and IObservable<T> which will help push-based notification,
also known as the observer design pattern. The IObservable<T> interface represents
the class that sends notifications (the provider); the IObserver<T> interface represents
the class that receives them (the observer) this pattern can be even better by installing [reactivex](reactivex.io). 


## Library Installation
Library is provided as a nuget package.

[communicator](https://www.nuget.org/packages/Communicator)

## Deployment
1. Clone or download this repo.
2. Run server.
```bash
cd Server
dotnet run
```
3. Run as many instances of chat as you want.
You might have to modify url inside Chat/Program.cs to point to your server url.
```bash
cd Chat
dotnet run [username]
```

## See also
[reactivex](reactivex.io)

