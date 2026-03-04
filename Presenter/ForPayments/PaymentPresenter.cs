using BLL.LogicLayers;
using BLL.LogicLayers.Payments;
using BLL.DTOs;
using SharedAbstractions.ArchitecturalMarkers;
using System;
using System.Linq;

namespace Presenter.ForPayments
{
    public class PaymentPresenter : IPresenter
    {
        private readonly IPaymentView _view;
        private readonly IUCGetAllPayments _ucGetAll;
        private readonly IUCCreatePayment _ucCreate;
        private readonly IUCUpdatePayment _ucUpdate;
        private readonly IUCDeletePayment _ucDelete;

        public PaymentPresenter
        (
            IPaymentView view,
            IUCGetAllPayments ucGetAll,
            IUCCreatePayment ucCreate,
            IUCUpdatePayment ucUpdate,
            IUCDeletePayment ucDelete
        )
        {
            _view = view;
            _ucGetAll = ucGetAll;
            _ucCreate = ucCreate;
            _ucUpdate = ucUpdate;
            _ucDelete = ucDelete;

            WireEvents();
            ApplyDarkTheme();
        }

        private void WireEvents()
        {
            // Mapeo de eventos GENÉRICOS (ICrudView<EmployeeDTO>)
            _view.CreateRequested += (s, e) => OnOpenCreationForm();
            _view.UpdateRequested += (s, e) => UpdatePayment(e);
            _view.DeleteRequested += (s, e) => DeletePayment(e);

            // Evento para listar (renombrado de CachingAll... a ListAll...)
            _view.ListAllRequested += (s, e) => GetAllPayments();
            
        }
        private void ApplyDarkTheme() => _view.ApplyGlobalPalette();
        private void OnOpenCreationForm() => _view.OpenCreationForm();

        private async void UpdatePayment(PaymentDTO e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var opRes = await _ucUpdate.Execute(e);
            ShowResult(opRes);

            // Refrescar lista si salió bien
            if (!opRes.Errors.Any()) GetAllPayments();
        }

        private async void DeletePayment(PaymentDTO e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var opRes = await _ucDelete.Execute(e);
            ShowResult(opRes);

            // Refrescar lista si salió bien
            if (!opRes.Errors.Any()) GetAllPayments();
        }

        private async void GetAllPayments()
        {
            var tuple = await _ucGetAll.ExecuteAsync();

            var employeeList = tuple.Item1;
            var opResult = tuple.Item2;    


            if (!opResult.Success)
            {
                ShowResult(opResult);
            }
            else
            {
                try
                {
                    // Método genérico de ICrudView
                    _view.CachingList(employeeList);
                }
                finally
                {
                    _view.FillDGV();
                }
            }
        }

        private void ShowResult(OperationResult<PaymentDTO> opRes)
            => _view.ShowOperationResult(opRes);
    }
}