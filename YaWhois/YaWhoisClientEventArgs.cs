using System;
using System.Text;
using YaWhois.Clients;

namespace YaWhois
{
    public class YaWhoisClientEventArgs : EventArgs
    {
        public object Value { get; set; }
        public IDataParser Parser { get; set; }
        public string Server { get; internal set; }
        public string Query { get; internal set; }
        public Encoding Encoding { get; internal set; }
        public string Response { get; internal set; }
        public string Referral { get; internal set; }
        public Exception Exception { get; internal set; }
    }
}
