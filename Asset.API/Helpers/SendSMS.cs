using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asset.API.Helpers
{
    public class SendSMS
    {
        public string Username = "M8r3Luhk";
        //public string Password = "Aqp8qPKZ2E";

        public string Password = "6997d527439623407d7a71a035fcbec1563ba25ef989eb248a91340b4dce3ff3";
        public int Language { get; set; }
        //Sender Token
        //  public string Sender = "Almostakbal";
        public string Sender = "c42418000f698a4c3205ee3ad20878bd0eac2114e8ea1c79afb52545e6eb229b";
        //Test Token
        // public string Sender = "b611afb996655a94c8e942a823f1421de42bf8335d24ba1f84c437b2ab11ca27";


        public string Mobile { get; set; }
        public string Message { get; set; }
        public string DelayUntil { get; set; }
        public int Environment { get; set; }
    }
}
