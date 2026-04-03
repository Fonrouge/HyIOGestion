using BLL.DTOs;
using BLL.LogicLayers.Clients;
using SharedAbstractions.ArchitecturalMarkers;
using SharedAbstractions.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Presenter.ForClient
{
    public class CreateClientPresenter : IPresenter
    {

        private readonly ICreateClientView _view;
        private readonly IUCCreateClient _useCaseCreate;

        public CreateClientPresenter
        (
            ICreateClientView view,
            IUCCreateClient useCaseCreate
        )
        {
            _view = view;
            _useCaseCreate = useCaseCreate;

            WireViewEvents();
            ApplyDarkTheme();
            FillDropDownData();
        }

        private void WireViewEvents()
        {
            _view.CreateClientRequested += (sender, e) => OnCreateClientRequested(e);
        }

        private void FillDropDownData()
        {
            var datasourceDocs = Enum.GetValues(typeof(DocTypesEnum))
            .Cast<DocTypesEnum>()
            .Select(d => new { Id = d.GetDocInfo().Id, Display = d.GetDocInfo().Description }).ToList();

            _view.FillClientDocTypes(datasourceDocs);


            var datasourceCountries = Enum.GetValues(typeof(CountriesEnum))
            .Cast<CountriesEnum>()
            .Select(d => new { Id = d.GetCountriesInfo().Id, Display = d.GetCountriesInfo().Description }).ToList();

            _view.FillCountries(datasourceCountries);
        }

        private void ApplyDarkTheme() => _view.ApplyGlobalPalette();
        private async Task OnCreateClientRequested(ClientDTO clientData)
        {
            var opRes = await _useCaseCreate.ExecuteAsync(clientData);


            _view.ShowOperationResult(opRes);
        }
    }


}
