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

        public static readonly string KR = "KR";
        public static readonly string EN = "EN";
        public static readonly string RU = "RU";

        public static readonly string API_VERSION = "https://api.github.com/repos/nextkaki/Tli-Utils/releases/latest";
        public static readonly string API_RELEASE = "https://github.com/nextkaki/Tli-Utils/releases";
        public static readonly string API_README = "https://raw.githubusercontent.com/nextkaki/Tli-Utils/main/README.md";

        public static readonly int BASE_MAX_MONSTER_LEVEL = 90;
        public static readonly int BASE_MIN_MONSTER_ARMOR = 0;
        public static readonly int BASE_MAX_MONSTER_ARMOR = 27273;

        public static readonly int TAB_MAIN = 0;
        public static readonly int TAB_COOLTIME = 1;
        public static readonly int TAB_FREEZE = 2;
        public static readonly int TAB_FROSTFIRE_RAMPAGE = 3;
        public static readonly int TAB_NEW_GOD = 4;
        public static readonly int TAB_ACTIVATION = 5;
        public static readonly int TAB_ARMOR_CALC = 6;
        public static readonly int TAB_SELENA_CALC = 7;
        public static readonly int TAB_DMG_CALC = 8;


        public static readonly int BASE_CAT_TRIGGER_PERCENT = 30;
        public static readonly decimal BASE_CAT_TRIGGER_COOLTIME = 0.5m;
        public static readonly int BASE_CAT_TRIGGER_CNT = 10;

        public static readonly decimal BASE_FREEZE_TIME = 1.5m;
        public static readonly decimal BASE_FROSTFIRE_RAMPAGE_COOL = 10.0m;
        public static readonly decimal BASE_FROSTFIRE_RAMPAGE_DURATION = 5.0m;

        public static readonly decimal BASE_NEW_GOD_AFFECTED = 0.10m;
        public static readonly decimal BASE_NEW_GOD_REALM = 0.25m;


    }
}
