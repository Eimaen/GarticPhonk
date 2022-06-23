using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GarticTest.Model.Messages.Server
{
    public enum GarticGamemode
    {
        Default = 1,
        DMCA = 8,
        Hidden = 3,
        Animation = 11,
        IceStuff = 9,
        Adjust = 15,
        Ranked = 10,
        Express = 6,
        Sandwich = 5,
        Crowd = 7,
        Background = 14,
        Solo = 13
    }

    internal class GarticGamemodeSwitchMessage : GarticServerMessage
    {
        public override GarticMessageType Type => GarticMessageType.GamemodeSwitch;
        public GarticGamemode Gamemode;

        public GarticGamemodeSwitchMessage(object data)
        {
            Gamemode = (GarticGamemode)int.Parse(data.ToString());
        }
    }
}
