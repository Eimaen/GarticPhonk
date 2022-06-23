using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarticTest.Model.Messages.Server
{
    internal class GarticDisconnectMessage : GarticServerMessage
    {
        public override GarticMessageType Type => GarticMessageType.Disconnect;

        public int Reason;

        public GarticDisconnectMessage(object data)
        {
            Reason = int.Parse(data.ToString());
        }
    }
}
