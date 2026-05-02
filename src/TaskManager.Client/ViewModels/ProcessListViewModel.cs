using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Regions;
using TaskManager.Client.Helpers;
using TaskManager.Client.Services.Interfaces;
using TaskManager.Shared.DTOs;

namespace TaskManager.Client.ViewModels;

/// <summary>
/// Управление списком процессов пользователя.
/// </summary>
public class ProcessListViewModel : ViewModelBase, INavigationAware
{
    private readonly IProcessApiService _processService;
    private readonly IRegionManager _regionManager;
    private ProcessDto? _selectedProcess;
    private bool _isEditing;
    private string _editTitle = string.Empty;
    private string _editDescription = string.Empty;
    private Guid? _editingProcessId;

    public ObservableCollection<ProcessDto> Processes { get; } = [];

    public ProcessDto? SelectedProcess
    {
        get => _selectedProcess;
        set => SetProperty(ref _selectedProcess, value);
    }

    public bool IsEditing
    {
        get => _isEditing;
        set => SetProperty(ref _isEditing, value);
    }

    public string EditTitle
    {
        get => _editTitle;
        set { SetProperty(ref _editTitle, value); SaveProcessCommand.RaiseCanExecuteChanged(); }
    }

    public string EditDescription
    {
        get => _editDescription;
        set => SetProperty(ref _editDescription, value);
    }

    public DelegateCommand LoadCommand { get; }
    public DelegateCommand NewProcessCommand { get; }
    public DelegateCommand<ProcessDto> EditProcessCommand { get; }
    public DelegateCommand<ProcessDto> DeleteProcessCommand { get; }
    public DelegateCommand<ProcessDto> OpenProcessCommand { get; }
    public DelegateCommand SaveProcessCommand { get; }
    public DelegateCommand CancelEditCommand { get; }

    public ProcessListViewModel(IProcessApiService processService, IRegionManager regionManager)
    {
        _processService = processService;
        _regionManager = regionManager;

        LoadCommand = new DelegateCommand(async () => await LoadProcessesAsync());
        NewProcessCommand = new DelegateCommand(OnNewProcess);
        EditProcessCommand = new DelegateCommand<ProcessDto>(OnEditProcess);
        DeleteProcessCommand = new DelegateCommand<ProcessDto>(async p => await OnDeleteProcessAsync(p));
        OpenProcessCommand = new DelegateCommand<ProcessDto>(OnOpenProcess);
        SaveProcessCommand = new DelegateCommand(async () => await OnSaveProcessAsync(), () => !string.IsNullOrWhiteSpace(EditTitle));
        CancelEditCommand = new DelegateCommand(OnCancelEdit);
    }

    private async Task LoadProcessesAsync()
    {
        await ExecuteAsync(async () =>
        {
            var processes = await _processService.GetAllAsync();
            Processes.Clear();
            foreach (var p in processes)
                Processes.Add(p);
        });
    }

    private void OnNewProcess()
    {
        _editingProcessId = null;
        EditTitle = string.Empty;
        EditDescription = string.Empty;
        IsEditing = true;
    }

    private void OnEditProcess(ProcessDto process)
    {
        _editingProcessId = process.Id;
        EditTitle = process.Title;
        EditDescription = process.Description ?? string.Empty;
        IsEditing = true;
    }

    private async Task OnSaveProcessAsync()
    {
        await ExecuteAsync(async () =>
        {
            var dto = new ProcessCreateDto(EditTitle, EditDescription);

            if (_editingProcessId is null)
                await _processService.CreateAsync(dto);
            else
                await _processService.UpdateAsync(_editingProcessId.Value, dto);

            IsEditing = false;
            await LoadProcessesAsync();
        });
    }

    private async Task OnDeleteProcessAsync(ProcessDto process)
    {
        var confirmMessage = System.Windows.Application.Current.TryFindResource("ConfirmDelete") as string
                             ?? "Вы уверены, что хотите удалить этот процесс?";

        if (!ErrorHandler.Confirm(confirmMessage)) return;

        await ExecuteAsync(async () =>
        {
            await _processService.DeleteAsync(process.Id);
            await LoadProcessesAsync();
        });
    }

    // Переход к просмотру задач выбранного процесса
    private void OnOpenProcess(ProcessDto process)
    {
        var parameters = new NavigationParameters
        {
            { "ProcessId", process.Id },
            { "ProcessTitle", process.Title }
        };
        _regionManager.RequestNavigate("ContentRegion", "TaskListView", parameters);
    }

    private void OnCancelEdit() => IsEditing = false;

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        LoadCommand.Execute();
    }

    public bool IsNavigationTarget(NavigationContext navigationContext) => true;
    public void OnNavigatedFrom(NavigationContext navigationContext) { }
}
