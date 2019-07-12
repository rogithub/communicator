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

Exits predefined classes to send the three kind of messages:

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

