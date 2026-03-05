using BLL.DTOs;
using BLL.LogicLayers;
using Presenter.ForSupplier;
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

namespace WinformsUI.Forms.SupplierCRUDL
{
    public partial class CreateSupplierForm : Form, ICreateSupplierView
    {
        private readonly ITranslatableControlsManager _transMgr;
        
        public CreateSupplierForm
        (
            ITranslatableControlsManager transMgr
        )
        {
            transMgr = _transMgr;

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
        public event EventHandler<SupplierDTO> CreateSupplierRequested;

        public void ShowOperationResult(OperationResult<SupplierDTO> opRes)
        {
            throw new NotImplementedException();
        }
    }
}
