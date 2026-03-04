using BLL.DTOs;
using BLL.LogicLayers;
using BLL.LogicLayers.Sales;
using Presenter.ForSale;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinformsUI.Infrastructure.Translations;

namespace WinformsUI.Features.SaleCRUDL
{
    public partial class CreateSaleForm : Form, ICreateSaleView
    {
        private readonly ITranslatableControlsManager _transMgr;
        public CreateSaleForm
        (
            ITranslatableControlsManager transMgr
        )
        {
            _transMgr = transMgr;

            InitializeComponent();
            AddTranslatables();
        }


        private void AddTranslatables()
        {
            _transMgr.AddParentedObjects<Label>(this.Controls, "Text");
            _transMgr.AddParentedObjects<Button>(this.Controls, "Text");

            ApplyTranslation();

            _transMgr.AddFormNotify(this);
        }

        public void NotifiedByTranslationManager() => ApplyTranslation();
        public void ApplyTranslation() => _transMgr.Apply();



        public event EventHandler<SaleDTO> CreateSaleRequested;

        public void ShowOperationResult(OperationResult<SaleDTO> opRes)
        {
            throw new NotImplementedException();
        }
    }
}
