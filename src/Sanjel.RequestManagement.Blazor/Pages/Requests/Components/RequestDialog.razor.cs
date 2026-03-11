using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Sanjel.RequestManagement.Blazor.Pages.Requests.ViewModels;
using Sanjel.RequestManagement.Entities.Entities;

namespace Sanjel.RequestManagement.Blazor.Pages.Requests.Components;

/// <summary>
/// Code-behind for the RequestDialog component used in create and edit modal dialogs.
/// </summary>
public partial class RequestDialog : ComponentBase
{
	/// <summary>
	/// The ViewModel binding the form to the Request entity data.
	/// </summary>
	[Parameter]
	[EditorRequired]
	public RequestViewModel ViewModel { get; set; } = default!;

	/// <summary>
	/// When true the dialog is in edit mode; when false it is in create mode.
	/// </summary>
	[Parameter]
	public bool IsEdit { get; set; }

	/// <summary>
	/// Callback invoked with the validated ViewModel when the form is submitted.
	/// </summary>
	[Parameter]
	public EventCallback<RequestViewModel> OnSave { get; set; }

	/// <summary>
	/// Callback invoked when the user cancels the operation.
	/// </summary>
	[Parameter]
	public EventCallback OnCancel { get; set; }

	/// <summary>
	/// Available Status options for the dropdown.
	/// </summary>
	protected static readonly List<EnumOption<StatusEnum>> StatusOptions =
		Enum.GetValues<StatusEnum>()
			.Select(v => new EnumOption<StatusEnum>(v, v.ToString()))
			.ToList();

	/// <summary>
	/// Available Priority options for the dropdown.
	/// </summary>
	protected static readonly List<EnumOption<PriorityEnum>> PriorityOptions =
		Enum.GetValues<PriorityEnum>()
			.Select(v => new EnumOption<PriorityEnum>(v, v.ToString()))
			.ToList();

	/// <summary>
	/// The EditContext driving Blazor form validation.
	/// </summary>
	protected EditContext EditContext { get; private set; } = default!;

	/// <summary>
	/// Indicates that an async save operation is in progress.
	/// </summary>
	protected bool IsSaving { get; private set; }

	/// <summary>
	/// Displays a server-side or unexpected error returned from the save operation.
	/// </summary>
	protected string? FormError { get; private set; }

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		this.EditContext = new EditContext(this.ViewModel);
		this.FormError = null;
	}

	/// <summary>
	/// Handles valid form submission by invoking the OnSave callback.
	/// </summary>
	protected async Task HandleValidSubmitAsync()
	{
		this.IsSaving = true;
		this.FormError = null;
		try
		{
			await this.OnSave.InvokeAsync(this.ViewModel);
		}
		catch (Exception ex)
		{
			this.FormError = $"Operation failed: {ex.Message}";
		}
		finally
		{
			this.IsSaving = false;
		}
	}

	/// <summary>
	/// Invokes the OnCancel callback when the user cancels the dialog.
	/// </summary>
	protected async Task HandleCancelAsync()
	{
		await this.OnCancel.InvokeAsync();
	}

	/// <summary>
	/// Projection type used to populate enum dropdowns.
	/// </summary>
	public sealed record EnumOption<TEnum>(TEnum value, string label);
}
