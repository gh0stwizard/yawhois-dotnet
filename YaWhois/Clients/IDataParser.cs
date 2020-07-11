using System;
using System.Collections.Generic;
using System.Text;

namespace YaWhois.Clients
{
    public interface IDataParser
    {
        string GetReferral(in string text);
    }
}
