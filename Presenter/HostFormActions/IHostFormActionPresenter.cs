using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter.HostFormActions
{
    public interface IHostFormActionPresenter
    {
        EventHandler OnClosingHostForm { get; set; }
    }
}



//entonces, ihostformactionpresenter como nueva interfaz de la que hereda hoistformactionpresenter para que pueda escuchar imainformnavigationpresenter y enterarme cuando se cierra un hostform para sacarlo de la lista de presenters añadidos cuando se abre unopa nuievo



