using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Tli_Utils
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            log4net.Config.XmlConfigurator.Configure();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new System.Drawing.Size(1024, 768);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            init();
        }

        public void init()
        {
            var sortedPages = mainTab.TabPages.Cast<TabPage>()
                .OrderBy(tp => tp.Name)
                .ToList();

            mainTab.TabPages.Clear();
            foreach (var tabPage in sortedPages)
            {
                mainTab.TabPages.Add(tabPage);
            }

            txtBaseFreezeTime.Text = DefineValues.BASE_FREEZE_TIME.ToString();
            txtCatPer.Text = DefineValues.BASE_CAT_TRIGGER_PERCENT.ToString();
            txtCatCool.Text = DefineValues.BASE_CAT_TRIGGER_COOLTIME.ToString();
            txtCatCnt.Text = DefineValues.BASE_CAT_TRIGGER_CNT.ToString();
        }


        private void txtMyCoolTime_TextChanged(object sender, EventArgs e)
        {
            string text = txtMyCoolTime.Text.Trim();
            // 공백 문자 제거
            text = text.Replace(" ", "");
            if(text.Length == 0 || text == ""){ 

            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(text, @"^[0-9]*\.?[0-9]*$"))
            {
                MessageBox.Show("숫자만 입력 가능합니다.");
                return;
            }
            else
            {
                decimal dMyCoolTime = string.IsNullOrEmpty(text) ? 0 : Convert.ToDecimal(text);
                string strRef = txtRefCoolTime.Text.Trim();
                decimal dRef = string.IsNullOrEmpty(strRef) ? 0 : Convert.ToDecimal(strRef);
                
                decimal dRtnValue = calcCoolTime(dMyCoolTime, dRef);
                txtResultCoolTime.Text = dRtnValue.ToString("F2");
                calcNeedCoolTime();
                checkCoolYN();
            }
        }

        private void txtRefCoolTime_TextChanged(object sender, EventArgs e)
        {
            string text = txtRefCoolTime.Text.Trim();
            // 공백 문자 제거
            text = text.Replace(" ", "");
            if (!System.Text.RegularExpressions.Regex.IsMatch(text, @"^[0-9]*\.?[0-9]*$"))
            {
                MessageBox.Show("숫자만 입력 가능합니다.");
                return;
            }
            else
            {
                decimal dRef = string.IsNullOrEmpty(text) ? 0 : Convert.ToDecimal(text);
                string strMycool = txtMyCoolTime.Text.Trim();
                decimal dMyCoolTime = string.IsNullOrEmpty(strMycool) ? 0 : Convert.ToDecimal(strMycool);

                decimal dRtnValue = calcCoolTime(dMyCoolTime, dRef);
                txtResultCoolTime.Text = dRtnValue.ToString("F2");
                calcNeedCoolTime();
                checkCoolYN();
            }
        }
        public decimal calcCoolTime(decimal myCool, decimal refCool)
        {
            decimal rtnValue = -1;
            decimal dMyCool = myCool / 100.0m;
            decimal dRefCool = refCool;

            //쿨타임 배율
            decimal dCoolRate = (1.0m / (1.0m + dMyCool));

            //쿨타임 계산
            rtnValue = dRefCool * dCoolRate;

            return rtnValue;
        }

        private void txtTargetCool_TextChanged(object sender, EventArgs e)
        {
            string strNeedCool = txtTargetCool.Text.Trim();
            // 공백 문자 제거
            strNeedCool = strNeedCool.Replace(" ", "");
            if (!System.Text.RegularExpressions.Regex.IsMatch(strNeedCool, @"^[0-9]*\.?[0-9]*$"))
            {
                MessageBox.Show("숫자만 입력 가능합니다.");
                return;
            }
            else
            {
                calcNeedCoolTime();
                checkCoolYN();
            }
        }
        public void calcNeedCoolTime()
        {
            decimal rtnValue = -1;
            decimal dMyCool = string.IsNullOrEmpty(txtMyCoolTime.Text.Trim()) ? 0 : Convert.ToDecimal(txtMyCoolTime.Text.Trim());
            decimal dSkillCool = string.IsNullOrEmpty(txtRefCoolTime.Text.Trim()) ? 0 : Convert.ToDecimal(txtRefCoolTime.Text.Trim());
            decimal dTargetCool = string.IsNullOrEmpty(txtTargetCool.Text.Trim()) ? 0 : Convert.ToDecimal(txtTargetCool.Text.Trim());
            if (dTargetCool > 0)
            {
                decimal dReqCool = (dSkillCool / dTargetCool) - 1.0m;
                dReqCool = dReqCool * 100.0m;
                rtnValue = dReqCool - dMyCool;
            }
            txtNeedCool.Text = rtnValue.ToString("F2");
        }
        public void checkCoolYN()
        {
            decimal dResultCool = string.IsNullOrEmpty(txtResultCoolTime.Text.Trim()) ? 0 : Convert.ToDecimal(txtResultCoolTime.Text.Trim());
            decimal dTargetCool = string.IsNullOrEmpty(txtTargetCool.Text.Trim()) ? 0 : Convert.ToDecimal(txtTargetCool.Text.Trim());
            if (dResultCool == dTargetCool) { 
                txtYNCool.Text = DefineValues.NOT_NEED;
                txtYNCool.ForeColor = Color.Green;
            }
            else { 
                txtYNCool.Text = DefineValues.NEED;
                txtYNCool.ForeColor = Color.Red;
            }

        }
        private void btnCool_Click(object sender, EventArgs e)
        {
            mainTab.SelectedIndex = DefineValues.TAB_COOLTIME;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            mainTab.SelectedIndex = DefineValues.TAB_MAIN;
        }

        private void txtAddFreezeTime1_TextChanged(object sender, EventArgs e)
        {
            decimal dSum = 0;
            MetroTextBox[] txtFreezeTimes = { txtAddFreezeTime1, txtAddFreezeTime2, txtAddFreezeTime3, txtAddFreezeTime4, txtAddFreezeTime5};
            foreach (MetroTextBox control in txtFreezeTimes)
            {
                string strText = control.Text.Trim();
                strText = strText.Replace(" ", "");
                
                if (!System.Text.RegularExpressions.Regex.IsMatch(strText, @"^[0-9]*\.?[0-9]*$"))
                {
                    MessageBox.Show("숫자만 입력 가능합니다.");
                    return;
                }
                else
                {
                    strText = string.IsNullOrEmpty(strText) ? "0" : strText;
                    dSum += Decimal.Parse(strText);
                }
            }
            calcFreeze(dSum);
        }
        public void calcFreeze(decimal _dSum)
        {
            decimal dSum = (_dSum/100.0m) + 1.0m;
            decimal dResultFreezeTime = DefineValues.BASE_FREEZE_TIME * dSum;
            txtResultFreezeTime.Text = dResultFreezeTime.ToString("F2");
            decimal dArcticCnt = dResultFreezeTime / 0.2m;
            if(dArcticCnt > 20)
            {
                dArcticCnt = 20;
            }
            txtArcticCnt.Text = dArcticCnt.ToString("F2");
            txtArcticPer.Text = (dArcticCnt * 12.0m).ToString("F2") + "%";

        }

        private void btnFreeze_Click(object sender, EventArgs e)
        {
            mainTab.SelectedIndex = DefineValues.TAB_FREEZE;
        }

        private void btnLightningShadow_Click(object sender, EventArgs e)
        {
            mainTab.SelectedIndex = DefineValues.TAB_Lightning_Shadow;
        }

        private void txtMovePerLS_TextChanged(object sender, EventArgs e)
        {
            string strText = txtMovePerLS.Text.Trim();
            strText = strText.Replace(" ", "");
            if (!System.Text.RegularExpressions.Regex.IsMatch(strText, @"^[0-9]*\.?[0-9]*$"))
            {
                MessageBox.Show("숫자만 입력 가능합니다.");
                return;
            }
            else
            {
                decimal dMovePer = string.IsNullOrEmpty(strText) ? 0 : Convert.ToDecimal(strText);
                decimal sToM = (1.0m + (dMovePer/100.0m)) * 6.0m;
                decimal mToS = 1.0m / sToM;
                txtStoMLS.Text = sToM.ToString("F2");
                txtMtoSLS.Text = mToS.ToString("F2");
                if (cbFast.Checked)
                {
                    txtSpeedPerSkill.Enabled = false;
                    txtSpeedPerSkill.Text = txtStoMLS.Text.Trim();
                }
            }
        }

        private void cbFast_CheckedChanged(object sender, EventArgs e)
        {
            if(cbFast.Checked)
            {
                txtSpeedPerSkill.Enabled = false;
                txtSpeedPerSkill.Text = txtStoMLS.Text.Trim();
            }
            else
            {
                txtSpeedPerSkill.Text = "0";
                txtSpeedPerSkill.Enabled = true;
            }
        }

        private void txtSpeedPerSkill_TextChanged(object sender, EventArgs e)
        {
            MetroTextBox textBox = sender as MetroTextBox;
            string textValue = textBox.Text;
            if (!System.Text.RegularExpressions.Regex.IsMatch(textValue, @"^[0-9]*\.?[0-9]*$"))
            {
                MessageBox.Show("숫자만 입력 가능합니다.");
                return;
            }
            else
            {
                decimal dSpeedPerSkill = string.IsNullOrEmpty(txtSpeedPerSkill.Text.Trim()) ? 1.0m : Convert.ToDecimal(txtSpeedPerSkill.Text.Trim());
                decimal dCatPer = string.IsNullOrEmpty(txtCatPer.Text.Trim()) ? 1.0m : Convert.ToDecimal(txtCatPer.Text.Trim());
                decimal dCatCool = string.IsNullOrEmpty(txtCatCool.Text.Trim()) ? 1.0m : Convert.ToDecimal(txtCatCool.Text.Trim());
                decimal dCatCnt = string.IsNullOrEmpty(txtCatCnt.Text.Trim()) ? 1.0m : Convert.ToDecimal(txtCatCnt.Text.Trim());
                if (dSpeedPerSkill <= 0) dSpeedPerSkill = 1.0m;
                if (dCatPer <= 0) dCatPer = 1.0m;
                if (dCatCool <= 0) dCatCool = 1.0m;
                if (dCatCnt <= 0) dCatCnt = 1.0m;

                txtResultSkill.Text = (dSpeedPerSkill * 4.0m).ToString("F2");
                decimal resultCatMaxTrigger = 4.0m / dCatCool;
                txtCatMaxTrigger.Text = resultCatMaxTrigger.ToString("F2");
                decimal resultCatExpectTrigger = resultCatMaxTrigger * (dCatPer / 100.0m);
                txtCatExpectTrigger.Text = resultCatExpectTrigger.ToString("F2");
                txtCatResultTrigger.Text = (resultCatExpectTrigger * dCatCnt).ToString("F2");
            }
        }
    }
}
