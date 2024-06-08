﻿using MetroFramework.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Http;
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
        bool gCheckPopup = false;
        string gCurrentVersion = ""; 
        string gLatestVersion = ""; 
        int[] gMonsterArmorByLevel = new int[DefineValues.BASE_MAX_MONSTER_LEVEL];

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
            btnArmorCalc.Text = Language.Calculating_Armor_Reduction;
            btnNotice.Text = Language.txtAnnouncements;
            btnCheckVersion.Text = Language.txtVersionCheck;

            //가이드 하단
            linkYoutube.Text = Language.strYoutube;
            linkCzz.Text = Language.strCzz;
            linkGithub.Text = Language.strGithub;
            metroLink1.Text = Language.All_responsibility;


            //메인가기
            MetroTile[] arrGoHome = { btnMain0, btnMain1, btnMain2, btnMain3, btnMain4, btnMain5, btnMain6 };
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

            //동결 계산기
            lb_fdd.Text = Language.tab_2_Freeze_default_duration;
            lb_ffd.Text = Language.tab_2_Final_freeze_duration;
            lb_ofd.Text = Language.tab_2_Owned_freeze_duration;
            lb_gewf.Text = Language.tab_2_Gain_Extreme_when_freezing;

            //라이트닝 쉐도우
            lb_bacs.Text = Language.tab_3_bag_attack_cast_speed;
            lb_fftc.Text = Language.tab_3_ff_trigger_chance;
            lb_ffcd.Text = Language.tab_3_ff_Cooldown;
            lb_ffc.Text = Language.tab_3_ff_Count;
            lb_tna4.Text = Language.tab_3_Total_number_of_attacks_in_4_seconds;
            lb_mnffa4.Text = Language.tab_3_Maximum_number_of_ff_activations_in_4_seconds;
            lb_ent.Text = Language.tab_3_Estimated_number_of_triggers;
            lb_tnoffsc.Text = Language.tab_3_Total_number_of_ff__shock_counts__in_4_seconds_Number_of_Static_Meteor_Shock_Counts_;
            lb_mms.Text = Language.tab_3_My_movement_speed;
            lb_dts.Text = Language.tab_3_Distance_traveled_in_1_second;
            lb_ttm.Text = Language.tab_3_Time_taken_to_move_1M;
            cbFast.Text = Language.tab_3_Use_Talent_Lightning_Fast;

            //얼음불 폭주
            lb_fre.Text = Language.tab_4_Frostfire_Rampage_exp;
            lb_t4_mcrr.Text = Language.tab_4_My_Cooldown_Recovery_Rate;
            lb_msed.Text = Language.tab_4_My_Skill_Effect_Duration;
            lblResultRampage.Text = Language.tab_4_Result;

            //질서혼란 계산기
            lb_popp.Text = Language.tab_5_Percentage_of_option_per_1_point_of_Order_Chaos;
            lb_nns.Text = Language.tab_5_Number_of_neighboring_slabs;
            lb_ogte.Text = Language.tab_5_Other_God_Talent_Effect;
            cbPeacefulRealm.Text = Language.tab_5_Order_Chaos_Use;
            lb_apes.Text = Language.tab_5_At_points_for_each_slab;
            lb_teasc.Text = Language.tab_5_Total_effect_of_all_slabs_combined;
            lb_explain.Text = Language.tab_5_explain;
            lbl_tab5_example.Text = Language.tab_5_example;
            lbl_mini_calc1.Text = Language.tab_5_mini_calc;

            //촉발체 계산기
            lb_title_cwr.Text = Language.tab_6_Calculating_Wind_Rhythm;
            lb_wrc.Text = Language.tab_6_Wind_Rhythm_Cooldown;
            lb_cwrcs.Text = Language.tab_6_Combined_Wind_Rhythm_Cast_Speed;
            lb_ccs.Text = Language.tab_6_Character_Cast_Speed;
            lb_accs.Text = Language.tab_6_Add_Character_Cast_Speed;
            lb_ccrr.Text = Language.tab_6_Character_Cooldown_Recovery_Rate;
            lb_ftc.Text = Language.tab_6_Final_Trigger_Cooldown;
            lb_ncps.Text = Language.tab_6_Number_of_casts_per_second;

            //아머 감면 계산기
            lbl_armor_explain1.Text = Language.tab_7_explain1;
            lbl_armor_explain2.Text = Language.tab_7_explain2;
            lbl_uav.Text = Language.tab_7_User_Armor_Value;
            lbl_ml.Text = Language.tab_7_Monster_Level;
            lbl_mav.Text = Language.tab_7_Monster_Armor_Value;
            lbl_mbr.Text = Language.tab_7_Monster_Base_Resistance;
            lbl_uadp.Text = Language.tab_7_User_Armor_Damage_Penetration;
            lbl_urp.Text = Language.tab_7_User_Resistance_Penetration;
            lbl_fd.Text = Language.tab_7_Expected__Final_Damage;
            lbl_apdr.Text = Language.tab_7_User__Armor_Physical_Damage_Reduction;
            lbl_anpdr.Text = Language.tab_7_User__Armor_Non_Physical_Damage_Reduction;
            lbl_fcr.Text = Language.tab_7_Final_Calculation_Result;
            lbl_eapdr.Text = Language.tab_7_Enemy__Physical_Damage_Reduction_Rate_by_Armor;
            lbl_eanpdr.Text = Language.tab_7_Enemy__Non_Physical_Damage_Reduction_Rate_by_Armor;
            lbl_eedrr.Text = Language.tab_7_Enemy__Elemental_Damage_Reduction_Rate_by_Resistance;
            lbl_fpdmm.Text = Language.tab_7_Final__Monster_Physical_Damage_Multiplier;
            lbl_fmedm.Text = Language.tab_7_Final__Monster_Elemental_Damage_Multiplier;
            lbl_ubd.Text = Language.tab_7_User_Base_Damage;
            btn_50m.Text = Language.tab_7_50m;
            btn_500m.Text = Language.tab_7_500m;
            btn_5b.Text = Language.tab_7_5b;
            btn_50b.Text = Language.tab_7_50b;
            btn_500b.Text = Language.tab_7_500b;
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
            gCurrentVersion = version.ToString().Replace(".", "");

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
            btnArmorCalc.Tag = DefineValues.TAB_ARMOR_CALC;

            createMonsterArmor();
            LoadReadMeAsync();
            CheckForUpdateAsync();
        }
        private async void CheckForUpdateAsync()
        {
            string latestVersion = await GetLatestVersionAsync();
            latestVersion = latestVersion.Replace("v", "");
            gLatestVersion = latestVersion;
            if (IsNewVersionAvailable(gCurrentVersion, latestVersion))
            {
                ShowUpdatePopup(latestVersion);
            }
        }
        private async Task<string> GetLatestVersionAsync()
        {
            string apiUrl = "https://api.github.com/repos/nextkaki/Tli-Utils/releases/latest";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "request"); // GitHub API를 사용하기 위한 User-Agent 설정
                try
                {
                    string response = await client.GetStringAsync(apiUrl);
                    JObject json = JObject.Parse(response);
                    return json["tag_name"].ToString(); // 최신 릴리스의 태그 이름을 버전으로 사용
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"최신 버전을 확인하는 중 오류가 발생했습니다: {ex.Message}");
                    return gCurrentVersion; // 오류 발생 시 현재 버전을 반환
                }
            }
        }
        private bool IsNewVersionAvailable(string currentVersion, string latestVersion)
        {
            int nCurrent = int.Parse(currentVersion);
            int nLatest = int.Parse(latestVersion);
            return nLatest > nCurrent;
        }

        private void ShowUpdatePopup(string latestVersion)
        {
            string message = $"새로운 버전({latestVersion})이 업데이트되었습니다.\n업데이트 페이지로 이동하시겠습니까?";
            string caption = "업데이트 알림";
            var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("https://github.com/nextkaki/Tli-Utils/releases");
            }
        }

        private async void LoadReadMeAsync()
        {
            if (gCheckPopup)
                return;

            gCheckPopup = true;
            string readMeUrl = "https://raw.githubusercontent.com/nextkaki/Tli-Utils/main/README.md";
            string readMeContent = await GetReadMeContentAsync(readMeUrl);

            ShowReadMePopup(readMeContent);
        }

        private async Task<string> GetReadMeContentAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    return await client.GetStringAsync(url);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"README.md 파일을 가져오는 중 오류가 발생했습니다: {ex.Message}");
                    return string.Empty;
                }
            }
        }

        private void ShowReadMePopup(string content)
        {
            ReadMePopupForm popupForm = new ReadMePopupForm(content);
            popupForm.ShowDialog();
        }

        private void createMonsterArmor()
        {
            for (int level = 1; level <= DefineValues.BASE_MAX_MONSTER_LEVEL; level++)
            {
                gMonsterArmorByLevel[level - 1] =
                    Common.CalculateArmor(
                        level,
                        DefineValues.BASE_MAX_MONSTER_LEVEL,
                        DefineValues.BASE_MIN_MONSTER_ARMOR,
                        DefineValues.BASE_MAX_MONSTER_ARMOR
                        );
            }
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
                case 7: return Language.tab_7;

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
                //if (!cbFast.Checked) return;
                if (!textBox.Enabled) return;

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
                //txtSpeedPerSkill.Enabled = false;
                txtSpeedPerSkill.Text = txtStoMLS.Text.Trim();
            }
            else
            {
                txtSpeedPerSkill.Text = "0";
                //txtSpeedPerSkill.Enabled = true;
            }
        }

        private void txtSpeedPerSkill_TextChanged(object sender, EventArgs e)
        {
            MetroTextBox textBox = sender as MetroTextBox;
            if (textBox != null)
            {
                if (string.IsNullOrEmpty(textBox.Text) || textBox.Text == "0") return;
                if (!textBox.Enabled) return;

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
                if (gLanguageKor)
                    lblResultRampage.Text = dResult.ToString("F2") + "만큼 지속시간이 필요하다.\r\n쿨타임 or 스킬 효과 지속시간을 챙겨주세요.";
                else
                    lblResultRampage.Text = dResult.ToString("F2") + " duration is required.\r\nIncrease the cooldown or skill effect duration.";
            }
            else
            {
                if (gLanguageKor)
                    lblResultRampage.Text = "스킬 효과 지속시간이 충분합니다.";
                else
                    lblResultRampage.Text = "Skill effect duration is long enough.";
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

                decimal dCnt = decimal.Parse(txtNewGod[1].Text.Trim());
                if(dCnt >= 0 )
                {
                    bool bPeacefulRealm = cbPeacefulRealm.Checked; //만계의 일상,재난
                    decimal dRateRealm = bPeacefulRealm ? 0.25m : 0;
                    decimal dRatePoint = dCnt * 0.20m; //효과 비율 %
                    //decimal dEtcEffect = decimal.Parse(txtNewGod[2].Text.Trim());
                    //dEtcEffect = dEtcEffect / 100.0m;

                    decimal dFirstEffectCalc = dBasePoint * (1 + dRatePoint);
                    decimal dSecondEffectCalc = dFirstEffectCalc * (1 + dRateRealm);
                    
                    txtEachEffect.Text = dSecondEffectCalc.ToString("F2");
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

        private void cbPeacefulRealm_CheckedChanged(object sender, EventArgs e)
        {
            calcNewGod();
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://game.naver.com/lounge/Torchlight_Infinite/home");
        }
        private void txtActivation_TextChanged(object sender, EventArgs e)
        {
            calcWindRhythm();
        }
        public void calcWindRhythm()
        {
            MetroTextBox[] txtWind = { txtWindCool, txtWindCastSum, txtPlayerCast, txtPlayerCastAdd, txtPlayerCool };
            bool bPass = true;
            foreach (MetroTextBox control in txtWind)
            {
                if (control != null) { 
                    if (!Common.IsNumericInputValid(control)) {
                        bPass = false; // 하나라도 유효하지 않으면 false로 설정
                    }
                }
                else { 
                    bPass = false;
                }
            }
            if(bPass)
            {
                int nPlayerCast = int.Parse(txtPlayerCast.Text.Trim());
                int nPlayerCastAdd = int.Parse(txtPlayerCastAdd.Text.Trim());
                int nResultCast = nPlayerCast + nPlayerCastAdd;

                int nWindCastValue = int.Parse(txtWindCastSum.Text.Trim());

                int nResultWindCool = nResultCast * (nWindCastValue/100);

                int nPlayerCool = int.Parse(txtPlayerCool.Text.Trim());
                decimal dResultBaseCool = (decimal)(nResultWindCool + nPlayerCool) / 100 ;
                
                decimal dRateCool = Common.getCoolDown(dResultBaseCool);
                decimal dWindCool = decimal.Parse(txtWindCool.Text.Trim());

                decimal dResultCool = dWindCool * dRateCool;
                if(dResultCool > 0)
                {
                    txtWindResultCool.Text = dResultCool.ToString("F2");
                    txtWindTotalCast.Text = (1 / dResultCool).ToString("F2");
                }
            }
        }

        private void calcArmor(object sender, EventArgs e)
        {
            MetroTextBox[] txtArmor = { txtUserArmor, txtMonsterLv };
            bool bPass = true;
            foreach (MetroTextBox control in txtArmor)
            {
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
                int nUserArmor = int.Parse(txtUserArmor.Text.Trim());
                int nMonsterLv = int.Parse(txtMonsterLv.Text.Trim());
                nMonsterLv = nMonsterLv > 90 ? 90 : nMonsterLv;
                nMonsterLv = nMonsterLv < 1 ? 1 : nMonsterLv;
                txtMonsterArmor.Text = gMonsterArmorByLevel[nMonsterLv-1].ToString();

                decimal dResultPhyReduceDmg = Common.getUserArmorPhysicalDamageReduction(nUserArmor,nMonsterLv);
                decimal dResultNonPhyReduceDmg = Common.getUserArmorNonPhysicalDamageReduction(dResultPhyReduceDmg);
                txtUserPhyReduce.Text = (dResultPhyReduceDmg * 100).ToString("F2") + "%";
                txtUserNonPhyReduce.Text = (dResultNonPhyReduceDmg * 100).ToString("F2") + "%";
            }
        }

        private void calcArmorDmg(object sender, EventArgs e)
        {
            MetroTextBox[] txtArmorDmg = { txtMonsterBaseResist,txtUserArmorPen,txtUserResistPen,txtUserBaseDmg };
            bool bPass = true;
            foreach (MetroTextBox control in txtArmorDmg)
            {
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
                int nMonsterArmor = int.Parse(txtMonsterArmor.Text.Trim());
                decimal dMonsterArmorPercent = Common.getMonsterArmorPhysicalDamageReduction(nMonsterArmor);
                decimal dUserArmorPen = decimal.Parse(txtUserArmorPen.Text.Trim());

                decimal dResultMonsterPhyReduce = dMonsterArmorPercent - (dUserArmorPen / 100.0m);
                txtMonsterArmorPhyReduce.Text = (dResultMonsterPhyReduce * 100).ToString("F2") + "%";

                decimal dResultMonsterNonPhyReduce = (dMonsterArmorPercent * 0.6m) - (dUserArmorPen / 100.0m);
                txtMonsterArmorNonPhyReduce.Text = (dResultMonsterNonPhyReduce * 100).ToString("F2") + "%";

                decimal dUserResistPen = decimal.Parse(txtUserResistPen.Text.Trim());
                decimal dMonsterResist = decimal.Parse(txtMonsterBaseResist.Text.Trim());
                decimal dResultMonsterResistReduce = dMonsterResist - dUserResistPen;
                txtMonsterResistReduce.Text = dResultMonsterResistReduce.ToString("F2") + "%";

                decimal dFinalPhyReduce = (1 - dResultMonsterPhyReduce) -1;
                txtMonsterFinalPhyReduce.Text = ((1 + dFinalPhyReduce)*100).ToString("F2") + "%";
                
                decimal dFinalNonPhyReduce =
                    (1 - dResultMonsterNonPhyReduce) * (1 - (dResultMonsterResistReduce/100)) - 1;
                txtMonsterFinalNonPhyReduce.Text = ((1 + dFinalNonPhyReduce)*100).ToString("F2") + "%";

                string _strUserBaseDmg = Common.removeComma(txtUserBaseDmg.Text.Trim());
                decimal dBaseUserDmg = decimal.Parse(_strUserBaseDmg);
                dBaseUserDmg *= 1000;
                txtFinalExpectPhyDmg.Text = (dBaseUserDmg * (1 + dFinalPhyReduce)).ToString("N0");
                txtFinalExpectEleDmg.Text = (dBaseUserDmg * (1 + dFinalNonPhyReduce)).ToString("N0");
            }
        }

        private void setBaseUserDmg_Click(object sender, EventArgs e)
        {
            // 클릭된 버튼의 Tag 속성을 사용하여 처리
            MetroTile dmgBtn = sender as MetroTile;
            if (dmgBtn != null)
            {
                int value = Convert.ToInt32(dmgBtn.Tag);
                txtUserBaseDmg.Text = value.ToString();
            }
        }

        private void btnNotice_Click(object sender, EventArgs e)
        {
            gCheckPopup = false;
            LoadReadMeAsync();
        }

        private void btnCheckVersion_Click(object sender, EventArgs e)
        {
            CheckForUpdateAsync();
            if (IsNewVersionAvailable(gCurrentVersion, gLatestVersion))
            {
                ShowUpdatePopup(gLatestVersion);
            }
            else
            {
                MessageBox.Show(Language.Latest_version_status);
            }
        }

        private void txtMiniCalc1_TextChanged(object sender, EventArgs e)
        {
            MetroTextBox[] txtCalc = { txtMiniCalc1, txtMiniCalc2, txtMiniCalc3, txtMiniCalc4 };
            bool bPass = true;
            foreach (MetroTextBox control in txtCalc)
            {
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
                int nResult = 0;
                foreach (MetroTextBox control in txtCalc)
                {
                    nResult += int.Parse(control.Text.Trim());
                }
                txtMiniCalcResult.Text = nResult.ToString();
            }
        }
    }
}
