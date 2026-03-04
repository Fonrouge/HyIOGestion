using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WinformsUI.Forms.Base;

namespace WinformsUI.Infrastructure.UserInterface.Windowing
{
    public class VerticalTileStrategy : ILayoutStrategy
    {
        public void Arrange(Rectangle clientArea, IEnumerable<Form> forms)
        {
            List<Form> listForms = forms.ToList();

            int count = listForms.Count;

            if (count == 0) return;

            int baseWidth = clientArea.Width / count;
            int remainder = clientArea.Width % count;

            for (int i = 0; i < count; i++)
            {
                int finalWidth = (i == count - 1) ? baseWidth + remainder : baseWidth;

                listForms[i].Size = new Size(finalWidth, clientArea.Height);
                listForms[i].Location = new Point(i * baseWidth, 0);
            }
        }
    }
}
