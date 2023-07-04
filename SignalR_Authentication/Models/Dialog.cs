using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Authentication.Models
{
    public class Dialog
    {
        public long Id { get; set; }

        public string ChatName { get; set; }

        public List<Message> Messages { get; set; }
    }
}
