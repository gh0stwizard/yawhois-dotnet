using System;
using System.Collections.Generic;
using System.Text;

namespace YaWhois.Data
{
    public partial class Assignments
    {
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


        public static readonly string[] RipeServers =
        {
            "whois.ripe.net",
            "whois.apnic.net",
            "whois.afrinic.net",
            "rr.arin.net",
            "whois.nic.fr",
            "rr.level3.net",
            "rr.ntt.net",
            "whois.tcinet.ru",
            "whois.ripn.net",
            "whois.arnes.si",
            "whois.nic.ir",
            "whois.ra.net",
        };
    }
}
