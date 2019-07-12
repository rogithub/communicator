# Communicator

This is a proof of concept to send events between different dotnet/js applications.


## Contents
### Server
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

  StringSerializedMessage(D data, M metaData);
```

### Default Metadata
Since metadata would be so common there is a default implementation that is a generic list of KeyValue objects.
You can find that class under Communicator.Core namespace.

```cs
List<KeyValue> metadata = new List<KeyValue>();

```
Convinient helper methods are included under the same namespace to add/read values preventing key dupplication.

```cs
metadata.set("user", username);
var id = metadata.get("id");
```


## Library Usage

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

### Sending my first event
In order to send events we instantiate an IEventSender object then we use
one of its methods to send all three kinds of messages. Make sure you check all cool overloads.

```cs
var send = source.GetEventSender(serializer);
send.String(new EventInfo("Chat"), new StringMessage(message, metadata));
send.Serialized(new EventInfo("Person"), new StringSerializedMessage<Person>(p, metadata));
send.Binary(new EventInfo("File"), new BinaryMessage(bytes, metadata));
```


### Receiving messages
From a different application (or the same one) you create an IObservableFactory
using a live (opened) IEventSource.

```cs
var factory = source.GetObservablesFactory(deserializer);

var chatObservable = factory.GetString("Chat");
var fileObservable = factory.GetBinary("File");
var personObservable = factory.GetSerialized<Person>("Person");
```

Once we got an IObservable we can attach as many handlers (IObserver) as we want like:

```cs
IObserver myObserver = ...
var chatObservable.Subscribe(myObserver);
```

C# Introduced, IObserver<T> and IObservable<T> which will help push-based notification,
also known as the observer design pattern. The IObservable<T> interface represents
the class that sends notifications (the provider); the IObserver<T> interface represents
the class that receives them (the observer) this pattern can be even better by installing
[reactivex](https://www.nuget.org/packages/System.Reactive). 


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

## Next steps
I would like to add groups support for reducing event names collision.
Also it would be nice to write a JavaScript version of the Communicator 
library.

## See also
[reactive.io](http://reactivex.io/)
