using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.API.Helpers
{
    public class SendSMS
    {
        public string Username = "M8r3Luhk";
        //   public string Password = "Aqp8qPKZ2E";

        public string Password = "6997d527439623407d7a71a035fcbec1563ba25ef989eb248a91340b4dce3ff3";
        public int Language { get; set; }
        public string Sender = "Almostakbal";
        public string Mobile { get; set; }
        public string Message { get; set; }
        public string DelayUntil { get; set; }
    }
}
