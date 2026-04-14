using BLL.DTOs;
using Shared.ArchitecturalMarkers;
using Shared.Enums;
using System;
using System.Collections.Generic;

namespace Presenter.MainFormNavigation
{
    /// <summary>
    /// Defines the contract for the Main Form view.
    /// This interface implements the Passive View pattern, allowing the Presenter to coordinate 
    /// application shell logic and window management without coupling to the WinForms framework.
    /// </summary>
    public interface IMainFormNavigation
    {
        /// <summary>Gets or sets the current visual tiling strategy (e.g., Vertical, Horizontal) for internal windows.</summary>
        LayoutTypeEnum CurrentLayoutType { get; set; }

        /// <summary>Gets or sets the active window management mode (Tabbed vs. Dashboard).</summary>
        WindowManagementModeEnum WindowManageMode { get; set; }

        // --- Application Control Events ---

        /// <summary>Occurs when the user requests to terminate the application.</summary>
        event EventHandler CloseRequested;

        /// <summary>Occurs when the user clicks the minimize button of the main shell.</summary>
        event EventHandler MinimizeRequested;

        /// <summary>Occurs when the user requests to toggle the main window between Maximized and Normal states.</summary>
        event EventHandler ExpandContractRequested;

        /// <summary>Occurs when a specific tiling layout is requested to rearrange active internal windows.</summary>
        event EventHandler<LayoutTypeEnum> ApplyLayoutRequested;

        // --- Module Navigation Events ---

        /// <summary>Occurs when the user requests to open the Client management module.</summary>
        event EventHandler OpenClientModuleRequested;

        /// <summary>Occurs when the user requests to open the Employee management module.</summary>
        event EventHandler OpenEmployeeModuleRequested;

        /// <summary>Occurs when the user requests to open the Payment processing module.</summary>
        event EventHandler OpenPaymentModuleRequested;

        /// <summary>Occurs when the user requests to open the Products catalog module.</summary>
        event EventHandler OpenProductsModuleRequested;

        /// <summary>Occurs when the user requests to open the System Configuration module.</summary>
        event EventHandler OpenConfigsModuleRequested;

        /// <summary>Occurs when the user requests to open the Sales management module.</summary>
        event EventHandler OpenSaleModuleRequested;

        /// <summary>Occurs when the user requests to open the Suppliers management module.</summary>
        event EventHandler OpenSuppliersModuleRequested;

        // --- Window Management & Lifecycle Events ---

        /// <summary>Fired when a new internal window is created, allowing the Presenter to initialize its specific Sub-Presenter.</summary>
        event EventHandler<IHostFormActions> InternalWindowCreated;

        /// <summary>Occurs when the user requests to switch the UI between Tabbed and Dashboard management modes.</summary>
        event EventHandler ChangeWindowManagementMode;

        /// <summary>Occurs while the main window is being resized, used to dynamically update the internal layout.</summary>
        event EventHandler OnResizingWindow;

        /// <summary>Occurs when the view needs to refresh its display title based on the application's current context.</summary>
        event EventHandler UpdatingTitleRequested;

        /// <summary>Fired when the form is fully charged.</summary>
        event EventHandler OnceLoadedAdvice;


        // --- View Action Methods ---
        
        /// <summary>
        /// Displays the outcome of a use case execution, presenting any specific validation or business rules errors for the Client.
        /// </summary>
        /// <param name="opRes">The operation result object to be evaluated and displayed.</param>
        void ShowOperationResult(OperationResult<UsuarioDTO> opRes);

        /// <summary>Sets the text for the main window's title bar.</summary>
        /// <param name="title">The string value for the window title.</param>
        void SetTitle(string title);

        /// <summary>Gracefully shuts down the application.</summary>
        void CloseApp();

        /// <summary>Minimizes the main application window to the taskbar.</summary>
        void MinimizeWindow();

        /// <summary>Updates the visual state of the window (Maximized/Normal) to match the current system state.</summary>
        void UpdateWindowState();

        // --- Navigation Execution Methods ---

        /// <summary>Executes the logic to initialize and display the Clients module form.</summary>
        void OpenClientsFrm();

        /// <summary>Executes the logic to initialize and display the Employee module form.</summary>
        void OpenEmployeeFrm();

        /// <summary>Executes the logic to initialize and display the Sales module form.</summary>
        void OpenSaleFrm();

        /// <summary>Executes the logic to initialize and display the Payment module form.</summary>
        void OpenPaymentFrm();

        /// <summary>Executes the logic to initialize and display the Product module form.</summary>
        void OpenProductFrm();

        /// <summary>Executes the logic to initialize and display the Supplier module form.</summary>
        void OpenSupplierFrm();

        /// <summary>Executes the logic to initialize and display the System Configuration form.</summary>
        void OpenConfigsFrm();

        /// <summary>Triggers the translation engine to update UI controls based on the global language setting.</summary>
        void ApplyTranslation();

        /// <summary>
        /// Updates the session information displayed in the window's status bar.
        /// </summary>
        /// <param name="loggedUserName">The login name of the authenticated user.</param>
        /// <param name="currentUserName">The display name or active profile context.</param>
        void SetStatusBarInfo(string loggedUserName, string currentUserName);

        /// <summary>
        /// Refreshes the UI text that indicates the current window management mode (e.g., "Tabbed Mode").
        /// </summary>
        /// <remarks>This is independent of the tiling strategy used when in Dashboard mode.</remarks>
        void UpdateWindowManageText();

        /// <summary>
        /// Updates the Window title.
        /// </summary>
        /// <remarks>Since the MainForm uses a custom chrome/border, this primarily ensures the title is correctly reflected in the Windows Taskbar.</remarks>
        void UpdateTitle();

        /// <summary>
        /// Visually arranges the provided collection of hosted windows according to a layout strategy.
        /// </summary>
        /// <param name="layoutType">The tiling strategy to apply (Vertical, Horizontal, etc.).</param>
        /// <param name="objsForTiling">The collection of active window interfaces to be rearranged.</param>
        void TileWindows(LayoutTypeEnum layoutType, IEnumerable<IHostFormActions> objsForTiling);

        /// <summary>
        /// Retrieves all currently active and non-disposed internal windows.
        /// This method is crucial for the Presenter to orchestrate layouts without direct access to the UI's private containers.
        /// </summary>
        /// <returns>An enumerable collection of interfaces for the active hosted windows.</returns>
        IEnumerable<IHostFormActions> GetActiveInternalWindows();
    }
}