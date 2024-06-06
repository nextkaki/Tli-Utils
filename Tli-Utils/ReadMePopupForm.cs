using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tli_Utils
{
    public partial class ReadMePopupForm : Form
    {
        public ReadMePopupForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        public ReadMePopupForm(string content)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            txtReadMe.Text = "";
            
            string startMarker = "## 현재 진행 상태";
            string endMarker = "## 소개";

            int startIndex = content.IndexOf(startMarker);
            int endIndex = content.IndexOf(endMarker);

            if (startIndex != -1 && endIndex != -1 && startIndex < endIndex)
            {
                // 시작 인덱스부터 끝 인덱스 이전까지의 부분 문자열을 추출합니다.
                string result = content.Substring(startIndex, endIndex - startIndex);
                result = result.Replace("-", "\r\n - ");
                txtReadMe.Text = result;
            }

            

        }

        private void btnClosePopup_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
