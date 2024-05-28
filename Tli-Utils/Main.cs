using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tli_Utils.Properties;
using static System.Net.Mime.MediaTypeNames;

namespace Tli_Utils
{
    public partial class Main : Form
    {
        bool gLanguageKor = true;

        public Main()
        {
            InitializeComponent();
            LoadResources();
            log4net.Config.XmlConfigurator.Configure();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new System.Drawing.Size(1024, 768);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            init();
        }

        private void ToggleLanguage()
        {
            if (gLanguageKor)
            {
                gLanguageKor = true;
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("ko-KR");
            }
            else
            {
                gLanguageKor = false;
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            }
            ReloadUI();
            LoadResources();
        }
        private void LoadResources()
        {
            //가이드 메인 버튼
            btnCool.Text = Language.calcCool;
            btnActivation.Text = Language.calcActivationMediumSkill;
            btnFreeze.Text = Language.calcFreeze;
            btnFrostRampage.Text = Language.FrostRampage;
            btnLightningShadow.Text = Language.Lightning_Shadow;
            btnNewGod.Text = Language.calcNewGod;

            //가이드 하단
            linkYoutube.Text = Language.strYoutube;
            linkCzz.Text = Language.strCzz;
            linkGithub.Text = Language.strGithub;

            //메인가기
            MetroTile[] arrGoHome = { btnMain0, btnMain1, btnMain2, btnMain3, btnMain4, btnMain5, };
            foreach (var goHome in arrGoHome)
            {
                if (goHome != null) {
                    goHome.Text = Language.strGoMain;
                }
            }

            //쿨타임 탭
            lb_mcrr.Text = Language.tab_1_My_cooldown_recovery_rate;
            lb_scc.Text = Language.tab_1_Skill_cooldown_calculate;
            lb_rsc.Text = Language.tab_1_Result_Skill_Cooldown;
            lb_gsc.Text = Language.tab_1_Goal_Skill_Cooldown;
            lb_rc.Text = Language.tab_1_Required_Cooldown;
            lb_wc.Text = Language.tab_1_Whether_cooldown;
        }

        private void ReloadUI()
        {
            Controls.Clear();
            InitializeComponent();
            init();
        }

        public void init()
        {
            if (gLanguageKor)
                btnLanguge.Text = "한 글";
            else
                btnLanguge.Text = "Eng";

            var sortedPages = mainTab.TabPages.Cast<TabPage>()
                .OrderBy(tp => tp.Name)
                .ToList();

            mainTab.TabPages.Clear();
            for (int i = 0; i < sortedPages.Count; i++)
            {
                var tabPage = sortedPages[i];
                tabPage.Text = GetTabPageText(i);
                mainTab.TabPages.Add(tabPage);
            }

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            lblVersion.Text = $"Ver: {version}";

            txtBaseFreezeTime.Text = DefineValues.BASE_FREEZE_TIME.ToString();
            txtCatPer.Text = DefineValues.BASE_CAT_TRIGGER_PERCENT.ToString();
            txtCatCool.Text = DefineValues.BASE_CAT_TRIGGER_COOLTIME.ToString();
            txtCatCnt.Text = DefineValues.BASE_CAT_TRIGGER_CNT.ToString();

            btnCool.Tag = DefineValues.TAB_COOLTIME;
            btnFreeze.Tag = DefineValues.TAB_FREEZE;
            btnLightningShadow.Tag = DefineValues.TAB_LIGHTNING_SHADOW;
            btnFrostRampage.Tag = DefineValues.TAB_FROSTFIRE_RAMPAGE;
            btnNewGod.Tag = DefineValues.TAB_NEW_GOD;
            btnActivation.Tag = DefineValues.TAB_ACTIVATION;
        }

        private string GetTabPageText(int index)
        {
            switch (index)
            {
                case 0: return Language.tab_0;
                case 1: return Language.tab_1;
                case 2: return Language.tab_2;
                case 3: return Language.tab_3;
                case 4: return Language.tab_4;
                case 5: return Language.tab_5;
                case 6: return Language.tab_6;
                default: return $"Tab {index + 1}";
            }
        }

        private void BtnTab_Click(object sender, EventArgs e)
        {
            MetroTile btn = sender as MetroTile;
            if (btn != null && btn.Tag != null)
            {
                int tabIndex = (int)btn.Tag;
                mainTab.SelectedIndex = tabIndex;
            }
        }


        private void txtMyCoolTime_TextChanged(object sender, EventArgs e)
        {
            MetroTextBox textBox = sender as MetroTextBox;
            if (textBox != null)
            {
                if (Common.IsNumericInputValid(textBox))
                {
                    string text = textBox.Text.Trim();
                    decimal dMyCoolTime = string.IsNullOrEmpty(text) ? 0 : Convert.ToDecimal(text);
                    string strRef = txtRefCoolTime.Text.Trim();
                    decimal dRef = string.IsNullOrEmpty(strRef) ? 0 : Convert.ToDecimal(strRef);

                    decimal dRtnValue = calcCoolTime(dMyCoolTime, dRef);
                    txtResultCoolTime.Text = dRtnValue.ToString("F2");
                    calcNeedCoolTime();
                    checkCoolYN();
                }
            }
        }

        private void txtRefCoolTime_TextChanged(object sender, EventArgs e)
        {
            MetroTextBox textBox = sender as MetroTextBox;
            if (textBox != null)
            {
                if (Common.IsNumericInputValid(textBox))
                {
                    string text = textBox.Text.Trim();
                    decimal dRef = string.IsNullOrEmpty(text) ? 0 : Convert.ToDecimal(text);
                    string strMycool = txtMyCoolTime.Text.Trim();
                    decimal dMyCoolTime = string.IsNullOrEmpty(strMycool) ? 0 : Convert.ToDecimal(strMycool);

                    decimal dRtnValue = calcCoolTime(dMyCoolTime, dRef);
                    txtResultCoolTime.Text = dRtnValue.ToString("F2");
                    calcNeedCoolTime();
                    checkCoolYN();
                }
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
            MetroTextBox textBox = sender as MetroTextBox;
            if (textBox != null)
            {
                if (Common.IsNumericInputValid(textBox))
                {
                    calcNeedCoolTime();
                    checkCoolYN();
                }
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

        private void btnHome_Click(object sender, EventArgs e)
        {
            mainTab.SelectedIndex = DefineValues.TAB_MAIN;
        }

        private void txtAddFreezeTime1_TextChanged(object sender, EventArgs e)
        {
            decimal dSum = 0;
            MetroTextBox[] txtFreezeTimes = { txtAddFreezeTime1, txtAddFreezeTime2, txtAddFreezeTime3, txtAddFreezeTime4, txtAddFreezeTime5 };
            foreach (MetroTextBox control in txtFreezeTimes)
            {
                if (control != null)
                {
                    if (Common.IsNumericInputValid(control))
                    {
                        string strText = control.Text.Trim();
                        strText = strText.Replace(" ", "");
                        strText = string.IsNullOrEmpty(strText) ? "0" : strText;
                        dSum += Decimal.Parse(strText);
                    }
                }
            }
            calcFreeze(dSum);
        }
        public void calcFreeze(decimal _dSum)
        {
            decimal dSum = (_dSum / 100.0m) + 1.0m;
            decimal dResultFreezeTime = DefineValues.BASE_FREEZE_TIME * dSum;
            txtResultFreezeTime.Text = dResultFreezeTime.ToString("F2");
            decimal dArcticCnt = dResultFreezeTime / 0.2m;
            if (dArcticCnt > 20)
            {
                dArcticCnt = 20;
            }
            txtArcticCnt.Text = dArcticCnt.ToString("F2");
            txtArcticPer.Text = (dArcticCnt * 12.0m).ToString("F2") + "%";

        }

        private void txtMovePerLS_TextChanged(object sender, EventArgs e)
        {
            MetroTextBox textBox = sender as MetroTextBox;
            if (textBox != null)
            {
                if (Common.IsNumericInputValid(textBox))
                {
                    string strText = textBox.Text.Trim();
                    decimal dMovePer = string.IsNullOrEmpty(strText) ? 0 : Convert.ToDecimal(strText);
                    decimal sToM = (1.0m + (dMovePer / 100.0m)) * 6.0m;
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
        }

        private void cbFast_CheckedChanged(object sender, EventArgs e)
        {
            if (cbFast.Checked)
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
            if (textBox != null)
            {
                if (Common.IsNumericInputValid(textBox))
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

        private void metroLink1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/@Kaki_TV");
        }

        private void metroLink2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://chzzk.naver.com/8eac3d6cdac51bbceb794196cd4e6a15");
        }

        private void metroLink4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/nextkaki/Tli-Utils");
        }

        private void txtRampageCool_TextChanged(object sender, EventArgs e)
        {
            MetroTextBox textBox = sender as MetroTextBox;
            if (textBox != null)
            {
                if (Common.IsNumericInputValid(textBox))
                {
                    decimal dRate = 1 / (1 + (decimal.Parse(textBox.Text.Trim()) / 100.0m));
                    decimal dResultRapageCool = DefineValues.BASE_FROSTFIRE_RAMPAGE_COOL * dRate;
                    txtResultRampageCool.Text = dResultRapageCool.ToString("F2");
                    calcRampage();
                }
            }
        }

        private void txtRampageDuration_TextChanged(object sender, EventArgs e)
        {
            MetroTextBox textBox = sender as MetroTextBox;
            if (textBox != null)
            {
                if (Common.IsNumericInputValid(textBox))
                {
                    decimal dResultRapageDuration = DefineValues.BASE_FROSTFIRE_RAMPAGE_DURATION * (1 + (decimal.Parse(textBox.Text.Trim()) / 100.0m));
                    txtResultRampageDuration.Text = dResultRapageDuration.ToString("F2");
                    calcRampage();
                }
            }
        }

        public void calcRampage()
        {
            decimal dRampageCool = decimal.Parse(txtResultRampageCool.Text.Trim());
            decimal dRampageDuration = decimal.Parse(txtResultRampageDuration.Text.Trim());
            decimal dResult = dRampageCool - dRampageDuration;
            if (dResult > 0)
            {
                lblResultRampage.Text = dResult.ToString("F2") + "만큼 지속시간이 필요하다.\r\n쿨타임 or 스킬 효과 지속시간을 챙겨주세요.";
            }
            else
            {
                lblResultRampage.Text = "스킬 효과 지속시간이 충분합니다.";
            }
        }

        private void txtNewGod_TextChanged(object sender, EventArgs e)
        {
            calcNewGod();
        }

     
        public void calcNewGod()
        {
            MetroTextBox[] txtNewGod = { txtNewGodPoint, txtNewGodCnt, txtNewGodEtcEffect};
            bool bPass = true;
            foreach (MetroTextBox control in txtNewGod) {
                if (control != null)
                {
                    if (!Common.IsNumericInputValid(control))
                    {
                        bPass = false; // 하나라도 유효하지 않으면 false로 설정
                    }
                }
                else
                {
                    bPass = false;
                }
            }
            if (bPass)
            {
                decimal dPoint = decimal.Parse(txtNewGod[0].Text.Trim());
                decimal dBasePoint = dPoint * 100.0m;
                txtExNewGodPoint.Text = dBasePoint.ToString("F2");
                dPoint = dPoint / 100.0m;

                decimal dCnt = decimal.Parse(txtNewGod[1].Text.Trim());
                if(dCnt >= 0 )
                {
                    bool bPeacefulRealm = cbPeacefulRealm.Checked; //만계의 일상
                    decimal dRateRealm = bPeacefulRealm ? 0.20m : 0;
                    decimal dRatePoint = dCnt * 0.20m;
                    decimal dEtcEffect = decimal.Parse(txtNewGod[2].Text.Trim());
                    dEtcEffect = dEtcEffect / 100.0m;
                    decimal dTotalRate = dRateRealm + dRatePoint + dEtcEffect;

                    txtEachEffect.Text = (dBasePoint * (1 + dTotalRate)).ToString("F2");
                    txtTotalEffect.Text = (dBasePoint * (1 + dTotalRate) * dCnt).ToString("F2");
                }
            }
        }

        private void Language_CheckedChanged(object sender, EventArgs e)
        {
            if (gLanguageKor) 
                gLanguageKor = false;
            else
                gLanguageKor = true;

            ToggleLanguage();
        }

    }
}
