using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinformsUI.Infrastructure.UserInterface.Windowing
{
    public class SmartGridStrategy : ILayoutStrategy
    {
        public void Arrange(Rectangle clientArea, IEnumerable<Form> forms)
        {
            var formList = forms.Where(f => f.Visible).ToList();
            int count = formList.Count;
            if (count == 0) return;

            // Matemática para definir filas y columnas cuadradas
            int cols = (int)Math.Ceiling(Math.Sqrt(count));
            int rows = (int)Math.Ceiling((double)count / cols);

            int w = clientArea.Width / cols;
            int h = clientArea.Height / rows;

            for (int i = 0; i < count; i++)
            {
                var form = formList[i];
                form.WindowState = FormWindowState.Normal;

                int currentRow = i / cols;
                int currentCol = i % cols;

                int x = clientArea.X + (currentCol * w);
                int y = clientArea.Y + (currentRow * h);

                form.Bounds = new Rectangle(x, y, w, h);
                form.BringToFront();
            }
        }
    }
}
