using BLL.LogicLayers;
using SharedAbstractions.Interfaces;
using System.Collections;
using System.Collections.Generic;

namespace Presenter.ForProducts
{
    public interface IProductView : ICrudView<ProductDTO> 
    {
        void SetSearchFilters<T>(IEnumerable<T> categories) where T : CategoryDTO;
    }
}
