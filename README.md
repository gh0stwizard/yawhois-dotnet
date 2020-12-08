# YaWhois (dotnet) - Yet Another WHOIS Client Library
![build](https://github.com/gh0stwizard/yawhois-dotnet/workflows/.NET%20Core/badge.svg)
[![codecov](https://codecov.io/gh/gh0stwizard/yawhois-dotnet/branch/master/graph/badge.svg?token=KG7XWT8NVR)](https://codecov.io/gh/gh0stwizard/yawhois-dotnet)

## Description

This is a WHOIS client library written in .Net Standard 2.0 specification.


## Why?

There are some WHOIS libraries were written in dotnet, but none of them
satisfied of mine use cases. Previously, I have worked with [whois][2] program
written by Marco d'Itri under Unix-like systems. And I like it.

`YaWhois` repeats logic done by `whois` program, specifically:
* Smart whois server selection for each query object.
* Server response processing (parsing) leaved to the application developer.


## Usage

### Hello, YaWhois!

```C#
var whois = new YaWhoisClient();
var response = whois.Query("github.com");
Console.WriteLine(response);
```

### Using delegates

`YaWhois` is using delegates to be easy in use.

```C#
var whois = new YaWhoisClient();

// set delegate when responses received
whois.WhenResponseParsed += Whois_ResponseParsed;

// make request
whois.Query("github.com");


static void Whois_ResponseParsed(object sender, YaWhoisClientEventArgs e)
{
    Console.WriteLine($"[server: {e.Server}]");
    Console.WriteLine($"[query: {e.Query}]");
    Console.WriteLine(e.Response);
}
```

### Async method

Same as above, but use `QueryAsync` method instead.

```C#
var whois = new YaWhoisClient();

whois.WhenResponseParsed += Whois_ResponseParsed;

// QueryAsync() never throws exceptions, so handle exceptions in this way.
whois.WhenExceptionThrown += (o, e) =>
{
    Console.WriteLine(e.Exception.Message);
};

// use cancellation token if neccessary
var cts = new CancellationTokenSource();

whois.QueryAsync("github.com", token: cts.Token);

// ...
```


## Event Handlers

All delegates (event handlers) have only two arguments:
* `object` **sender** (`YaWhoisClient`)
* `YaWhoisClientEventArgs` **args**

The `YaWhoisClientEventArgs` contains all information about your query:
* `object` **Value** - user object per query
* `IDataParser` **Parser** - parser to get referral information, you can set to yours
* `string` **Server** - selected server (readonly)
* `string` **Query** - adopted query to the selected server (readonly)
* `Encoding` **Encoding** - server encoding (readonly)
* `string` **Response** - when query is completed, it contains server response (readonly)
* `string` **Referral** - referral server if server response contains this information (readonly)
* `Exception` **Exception** - used by `QueryAsync()`; contains exception if smth goes wrong (readonly)

General usage for user objects is passing them to `Query()` or `QueryAsync()`:

```C#
// your object for the query below
var mydata = new MyData();

whois.Query("github.com", value: mydata);


// access your data in any delegate, for instance
static void Whois_Delegate(object sender, YaWhoisClientEventArgs e)
{
    var data = (MyData)e.Value;
    // ...
}
```

### WhenRequestReady

This delegate called before request to a server.
Currently, this delegate has little purposes.
You may change `IDataParser` **Parser** at this moment.

### WhenResponseReceived

This one called after network request and response has been received.
You may observe the `string` Response value.
You still may change `IDataParser` **Parser** at this moment.

### WhenResponseParsed

This one called after parsing server response to find out
the value of `Referral`.

This is last delegate to be called upon successful request.

Changing `IDataParser` **Parser** value will not give any results.

### WhenExceptionThrown

This is used only by `QueryAsync()` method.
When called the exception is set to `Exception` Exception
property of the `YaWhoisClientEventArgs` arguments.


## Recursive queries

`YaWhoisClient` does recursive queries when its find referral from
the server response. If you wish disable this behaviour you may
set your dummy `IDataParser` **Parser**.

An example to query IANA:

```C#
var whois = new YaWhoisClient();

// set delegate when responses received
whois.WhenResponseParsed += Whois_ResponseParsed;

// make request to IANA
whois.Query("github.com", "whois.iana.org");

// This delegate will be called for whois.iana.org response
// and a referral one (if it exists).
static void Whois_ResponseParsed(object sender, YaWhoisClientEventArgs e)
{
    Console.WriteLine($"[server: {e.Server}]");
    Console.WriteLine();
    Console.WriteLine(e.Response);
}
```

An example how to disable recursive queries to all servers:
```C#
// Alternatively this could be done inside of WhenResponseReceived
whois.WhenRequestReady += (o, e) =>
{
    e.Parser = null; // since 1.0.6 version
};
```

For the `YaWhois` before 1.0.6 version use this approach:
```C#
class MyDummyParser : YaWhois.Clients.IDataParser
{
    public string GetReferral(in string text)
    {
        return null;
    }
}

whois.WhenRequestReady += (o, e) =>
{
    e.Parser = new MyDummyParser();
};
```

Another way per a server (since 1.0.6 version):
```C#
var whois = new YaWhoisClient();

// This also could be done between queries and in delegates.
whois.RegisterParserByServer("whois.iana.org", null);
whois.RegisterParserByServer("whois.arin.net", null);

whois.Query("github.com", "whois.iana.org");
whois.Query("67.227.191.5"); // whois.arin.net
```

To remove the parser per server use the `UnregisterParserByServer()` method:
```C#
whois.UnregisterParserByServer("whois.arin.net");
```


## Exceptions

There are special exceptions which may be thrown by `YaWhoisClient`:
* `NoServerException` - when unable to find a server for this kind of object
* `UnknownNetworkException` - probably invalid AS number or IP address has been passed
* `ExternalWhoisException` - the server infromation is accessed by external resource

The `Query()` method throws exceptions.

The `QueryAsync()` method does not throws exceptions, instead it calls
the `WhenExceptionThrown` delegate (see above for details).


## Properties

Currently there are only properties for `TcpClient`:

* `ConnectTimeout` (default: 15) - connection timeout in seconds
* `ReadWriteTimeout` (default: 30) - timeout in seconds for read/write operations

You may adjust any of them at any time, but most useful cases are:

* After initializing `YaWhoisClient` object.
* With in `WhenRequestReady` delegate depending on query or server values.


## See also

* [RFC 3912][1] - WHOIS Protocol Specification
* [github: rfc1036/whois][2] - whois client written in C
* [github: flipbit/whois][3] - dotnet whois library


## License

This software is released under BSD 2-clause "Simplified" License.


[1]: https://tools.ietf.org/html/rfc3912
[2]: https://github.com/rfc1036/whois
[3]: https://github.com/flipbit/whois
