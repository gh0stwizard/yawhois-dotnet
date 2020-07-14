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


## See also

* [RFC 3912][1] - WHOIS Protocol Specification
* [github: rfc1036/whois][2] - whois client written in C
* [github: flipbit/whois][3] - dotnet whois library


## License

This software is released under BSD 2-clause "Simplified" License.


[1]: https://tools.ietf.org/html/rfc3912
[2]: https://github.com/rfc1036/whois
[3]: https://github.com/flipbit/whois
