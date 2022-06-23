using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarticTest.Model.Messages
{
    internal abstract class GarticServerMessage : GarticMessage
    {
        public override string Serialize()
        {
            throw new InvalidOperationException("Server messages are not meant to be serialized.");
        }
    }
}
