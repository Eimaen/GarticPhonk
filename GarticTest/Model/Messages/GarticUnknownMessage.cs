using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarticTest.Model.Messages
{
    internal class GarticUnknownMessage : GarticMessage
    {
        public override GarticMessageType Type => GarticMessageType.Unknown;

        public override string Serialize()
        {
            throw new InvalidOperationException();
        }
    }
}
