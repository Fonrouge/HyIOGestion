using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winforms.Theme;

namespace WinformsUI.UserControls.CustomStatusBar
{
    public partial class CustomStatusBar : UserControl
    {
        enum BackUpStatus
        {
            UP_TO_DATE = 0,
            WARNING = 1,
            OUTDATED = 2
        }


        public Label LblBackUpTrafficLight => lblBackUpTrafficLight;
        public Label TxtBackUpName => txtBackUpName;
        public Label TxtBackUpStatus => txtBackUpStatus;
        public Label TxtWelcome => txtWelcome;
        public Label TxtUserName => txtUserName;
        public Label TxtLoginTimeIndicator => txtLoginTimeIndicator;
        public Label TxtLoginTime => txtLoginTime;
        public Button BtnMyAccount => btnMyAccount;
        public TableLayoutPanel MainTableLayoutPnl => mainTableLayoutPnl;


        public CustomStatusBar()
        {
            InitializeComponent();

            DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());



        }
        public void ThemingNotifiedByConfigurationsModule()
        {
            DarkTheme.Apply(this, DarkTheme.GetCurrentPalette());
            MessageBox.Show("asd");
        }

    }
}
