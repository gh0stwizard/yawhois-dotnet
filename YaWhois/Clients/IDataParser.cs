using System;

namespace YaWhois.Clients
{
    public interface IDataParser
    {
        string GetReferral(in string text);
    }
}
