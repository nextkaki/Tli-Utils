using MetroFramework.Controls;
using System;
using System.Collections.Generic;
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
    }
}
