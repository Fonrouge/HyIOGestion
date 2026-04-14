using SharedAbstractions.ArchitecturalMarkers;
using BLL.DTOs; 
using System;
using System.Collections.Generic;

namespace SharedAbstractions.Interfaces
{
    // T debe ser una clase y cumplir con IDto
    public interface ICrudView<T> : IView where T : class, IDto
    {
        // --- Eventos Comunes (Nombres estandarizados) ---
        event EventHandler CreateRequested;
        event EventHandler<T> UpdateRequested;
        event EventHandler<T> DeleteRequested;
        event EventHandler ListAllRequested; 
        event EventHandler CloseRequested; 
        event EventHandler OnceLoadedAdvice; 

        // --- Métodos Comunes ---
        void ShowOperationResult(OperationResult<T> opRes);
        void CachingList(IEnumerable<T> list); // Nombre genérico
        void FillDGV();
        void OpenCreationView();
        void OpenUpdateView();
        void CloseView();
        void Dispose();
        void SelectFirstGridRow();
    }
}