# YaWhois (dotnet) - Yet Another WHOIS Client Library


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
whois.ResponseParsed += Whois_ResponseParsed;

// make request
whois.Query("github.com");


static void Whois_ResponseParsed(object sender, YaWhoisClientEventArgs e)
{
    YaWhoisClient whois = (YaWhoisClient)sender;

    Console.WriteLine($"[server: {e.Server}]");
    Console.WriteLine($"[query: {e.Query}]");
    Console.WriteLine(e.Response);
}
```

### Async method

Same as above, but use `QueryAsync` method instead.

```C#
var whois = new YaWhoisClient();

whois.ResponseParsed += Whois_ResponseParsed;

// additional delegate for async queries
whois.ExceptionThrown += (o, e) =>
{
    Console.WriteLine(e.Exception.Message);
};

// use cancellation token if neccessary
var cts = new CancellationTokenSource();

whois.QueryAsync("github.com", token: cts.Token);

// ...
```

## Using delegates

All delegates have only two arguments:
* `object` sender (YaWhoisClient)
* `YaWhoisClientEventArgs` args

The `YaWhoisClientEventArgs` contains all information about your query:
* `object` Value - user object per query
* `IDataParser` Parser - parser to get referral information, you can set to yours
* `string` Server - selected server (readonly)
* `string` Query - adopted query to the selected server (readonly)
* `Encoding` Encoding - server encoding (readonly)
* `string` Response - when query is completed, it contains server response (readonly)
* `string` Referral - referral server if server response contains this information (readonly)
* `Exception` Exception - used by `QueryAsync()`; contains exception if smth goes wrong (readonly)

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

### BeforeSendRequest

This delegate called before request to a server.
Currently, this delegate has little purposes.
You may change `IDataParser` Parser at this moment.

## BeforeParseResponse

This one called after network request.
You may observe the `string` Response value.
You still may change `IDataParser` Parser at this moment.

## ResponseParsed

This one called after parsing server response to find out
the value of `Referral`.

This is last delegate to be called upon successful request.

Changing `IDataParser` Parser value will not give any results.

## ExceptionThrown

This is used only by `QueryAsync()` method.
When called the exception is set to `Exception` Exception
property of the `YaWhoisClientEventArgs` arguments.


## See also

* [RFC 3912][1] - WHOIS Protocol Specification
* [github: rfc1036/whois][2] - whois client written in C
* [github: flipbit/whois][3] - dotnet whois library


## License

This software is released under BSD 2-clause "Simplified" License.


[1]: https://tools.ietf.org/html/rfc3912
[2]: https://github.com/rfc1036/whois
[3]: https://github.com/flipbit/whois
