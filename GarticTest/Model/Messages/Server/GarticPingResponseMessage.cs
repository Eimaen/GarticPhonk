using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarticTest.Model.Messages.Server
{
    internal class GarticPingResponseMessage : GarticServerMessage
    {
        public override GarticMessageType Type => GarticMessageType.PingResponse;
    }
}
