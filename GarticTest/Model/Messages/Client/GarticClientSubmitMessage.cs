using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarticTest.Model.Messages.Client
{
    internal class GarticClientSubmitMessage : GarticClientMessage
    {
        public override GarticMessageType Type => GarticMessageType.Submit;

        public GarticClientSubmitMessage() { }

        public override string Serialize()
        {
            return $"42[2,{(int)Type},true]";
        }
    }
}
