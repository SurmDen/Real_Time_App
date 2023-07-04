using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Authentication.Models
{
    public class Message
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string Text { get; set; }

        public Dialog Dialog { get; set; }

        public long DialogId { get; set; }
    }
}
