using Microsoft.Extensions.DependencyInjection;
using Shared;
using Shared.Services.Searching;
using System;
using WinformsUI.Infrastructure.Translations;

namespace WinformsUI.UserControls.CustomDGV
{
    internal class CustomDGVFactory : ICustomDGVFactory
    {
        private readonly IServiceProvider _sp;
        private readonly IListFilterSortProvider _lfsp;
        private readonly IApplicationSettings _appSet;


        public CustomDGVFactory
        (
            IServiceProvider sp,
            IListFilterSortProvider lfsp,
            IApplicationSettings appSet
        )
        {
            _sp = sp;
            _lfsp = lfsp;
            _appSet = appSet;
        }

        public CustomDGVForm Create(ITranslatableControlsManager transMgr)
            => ActivatorUtilities.CreateInstance<CustomDGVForm>(_sp, _lfsp, _appSet, transMgr);

    }
}
