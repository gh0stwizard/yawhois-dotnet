using System;
using System.Collections.Generic;
using System.Text;

namespace YaWhois.Data
{
    public partial class Assignments
    {
        public struct TldOptions
        {
            public Hints Hint;
            public string Server;
        }

        public enum Hints
        {
            NONE,
            EXTERNAL, //x01
            NOSERVER, //0x03
            CSRNIC, //x04
            AFILIAS, //x08
            IPv4, //x0c
            IPv6, //x0d
        }
    }
}
