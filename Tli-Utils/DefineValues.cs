using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tli_Utils
{
    public class DefineValues
    {
        public static readonly string NEED = "필요";
        public static readonly string NOT_NEED = "불필요";


        public static readonly int TAB_MAIN = 0;
        public static readonly int TAB_COOLTIME = 1;
        public static readonly int TAB_FREEZE = 2;
        public static readonly int TAB_LIGHTNING_SHADOW = 3;
        public static readonly int TAB_FROSTFIRE_RAMPAGE = 4;
        public static readonly int TAB_NEW_GOD = 5;
        public static readonly int TAB_ACTIVATION = 6;

        public static readonly int BASE_CAT_TRIGGER_PERCENT = 30;
        public static readonly decimal BASE_CAT_TRIGGER_COOLTIME = 0.5m;
        public static readonly int BASE_CAT_TRIGGER_CNT = 10;

        public static readonly decimal BASE_FREEZE_TIME = 1.5m;
        public static readonly decimal BASE_FROSTFIRE_RAMPAGE_COOL = 11.0m;
        public static readonly decimal BASE_FROSTFIRE_RAMPAGE_DURATION = 5.0m;


    }
}
