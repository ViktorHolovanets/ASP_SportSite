using SportSite.Models.Db;
using System.Xml.Linq;

namespace SportSite.Models.SignalR
{
    public class MessageSignalR
    {
        public int CountMessage { get; set; }=0;
        public Message? Message { get; set; }
    }
}
