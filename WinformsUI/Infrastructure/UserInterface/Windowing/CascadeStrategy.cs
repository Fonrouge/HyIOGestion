using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinformsUI.Infrastructure.UserInterface.Windowing
{
    public class CascadeStrategy : ILayoutStrategy
    {
        private const int Offset = 25; // Desplazamiento en píxeles

        public void Arrange(Rectangle clientArea, IEnumerable<Form> forms)
        {
            var formList = forms.Where(f => f.Visible).ToList();
            if (formList.Count == 0) return;

            int w = (int)(clientArea.Width * 0.6); // Ocupa el 60% del ancho
            int h = (int)(clientArea.Height * 0.6);

            for (int i = 0; i < formList.Count; i++)
            {
                var form = formList[i];
                form.WindowState = FormWindowState.Normal;

                int x = clientArea.X + (i * Offset);
                int y = clientArea.Y + (i * Offset);

                // Evitar que se salga de la pantalla si hay muchas ventanas
                if (x + w > clientArea.Right) x = clientArea.X;
                if (y + h > clientArea.Bottom) y = clientArea.Y;

                form.Bounds = new Rectangle(x, y, w, h);
                form.BringToFront();
            }
        }
    }
}
