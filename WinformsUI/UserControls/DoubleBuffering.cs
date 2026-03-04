using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace WinformsUI.UserControls
{
    internal static class DoubleBuffering
    {
        internal static void TryForAllControls(ControlCollection c)
        {

            foreach (var control in c)
            {
                if (control is Control cx)
                {
                    try
                    {
                        var prop = c.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                        if (prop != null) prop.SetValue(cx, true, null);
                    }
                    catch { }
                }
            }
            
        }
    }
}
