using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tli_Utils
{
    public static class Common
    {
        public static bool IsNumericInputValid(MetroTextBox textBox)
        {
            string strText = textBox.Text.Trim();
            strText = strText.Replace(" ", "");
            
            // 빈 문자열 체크
            if (string.IsNullOrEmpty(strText))
            {
                return false;
            }

            // 소수점만 있는지 체크
            if (strText == ".")
            {
                return false;
            }

            // 마이너스만 있는지 체크
            if (strText == "-")
            {
                return false;
            }

            if (!Regex.IsMatch(strText, @"^-?[0-9]*\.?[0-9]*$"))
            {
                return false;
            }
            return true;
        }

        public static decimal getCoolDown(decimal _ref)
        {
            decimal rtnValue = 0.0m;
            rtnValue = (1.0m / (1.0m + _ref));
            return rtnValue;
        }

        public static decimal getUserArmorPhysicalDamageReduction(int _armour, int _monsterlv)
        {
            decimal rtnValue = 0.0m;
            decimal dArmor = (decimal)_armour;
            rtnValue = dArmor / ((0.9m * dArmor) + 3000 + (300 * _monsterlv));
            return rtnValue;
        }
        public static decimal getUserArmorNonPhysicalDamageReduction(decimal _ref)
        {
            decimal rtnValue = 0.0m;
            rtnValue = _ref * 0.6m;
            return rtnValue;
        }
        public static decimal getMonsterArmorPhysicalDamageReduction(int _armour)
        {
            decimal rtnValue = 0.0m;
            decimal dArmor = (decimal)_armour;
            rtnValue = dArmor / ((0.9m * dArmor) + 30000);
            return rtnValue;
        }
        public static int CalculateArmor(int level, int maxLevel, int minArmor, int maxArmor)
        {
            return minArmor + ((int)(maxArmor - minArmor) / (maxLevel - 1)) * (level - 1);
        }
        public static string removeComma(string _ref)
        {
            string rtnValue = "";
            rtnValue = _ref.Replace(",", "");
            return rtnValue;
        }
        public static Decimal CalcPercent(Decimal _ref)
        {
            return (_ref / 100.0m);
        }

        public static decimal ParseTextBoxToDecimal(MetroTextBox textBox, CultureInfo cultureInfo)
        {
            string text = textBox.Text.Trim();

            // 유효하지 않은 형식 처리
            if (string.IsNullOrWhiteSpace(text) || text == ".")
            {
                text = "0";
            }

            if (decimal.TryParse(text, NumberStyles.Number, cultureInfo, out decimal dPoint))
            {
                return dPoint;
            }
            else
            {
                return 0.0m;
            }
        }

    }
}
