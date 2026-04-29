using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Regions;
using TaskManager.Client.Helpers;
using TaskManager.Client.Services.Interfaces;
using TaskManager.Domain.Enums;
using TaskManager.Shared.DTOs;

namespace TaskManager.Client.ViewModels;

/// <summary>
/// ViewModel списка задач процесса с поддержкой CRUD.
/// </summary>
public class TaskListViewModel : ViewModelBase, INavigationAware
{
    private readonly ITaskApiService _taskService;
    private readonly IRegionManager _regionManager;

    private Guid _processId;
    private string _processTitle = string.Empty;
    private bool _isEditing;
    private Guid? _editingTaskId;

    // Поля формы редактирования
    private string _editTitle = string.Empty;
    private string _editDescription = string.Empty;
    private TaskItemStatus _editStatus = TaskItemStatus.NotStarted;
    private TaskPriority _editPriority = TaskPriority.Medium;
    private DateTime? _editDueDate;

    public ObservableCollection<TaskItemDto> Tasks { get; } = [];

    public string ProcessTitle
    {
        get => _processTitle;
        set => SetProperty(ref _processTitle, value);
    }

    public bool IsEditing
    {
        get => _isEditing;
        set => SetProperty(ref _isEditing, value);
    }

    public string EditTitle
    {
        get => _editTitle;
        set { SetProperty(ref _editTitle, value); SaveTaskCommand.RaiseCanExecuteChanged(); }
    }

    public string EditDescription
    {
        get => _editDescription;
        set => SetProperty(ref _editDescription, value);
    }

    public TaskItemStatus EditStatus
    {
        get => _editStatus;
        set => SetProperty(ref _editStatus, value);
    }

    public TaskPriority EditPriority
    {
        get => _editPriority;
        set => SetProperty(ref _editPriority, value);
    }

    public DateTime? EditDueDate
    {
        get => _editDueDate;
        set => SetProperty(ref _editDueDate, value);
    }

    /// <summary>Доступные статусы для ComboBox.</summary>
    public TaskItemStatus[] Statuses => Enum.GetValues<TaskItemStatus>();

    /// <summary>Доступные приоритеты для ComboBox.</summary>
    public TaskPriority[] Priorities => Enum.GetValues<TaskPriority>();

    public DelegateCommand LoadCommand { get; }
    public DelegateCommand NewTaskCommand { get; }
    public DelegateCommand<TaskItemDto> EditTaskCommand { get; }
    public DelegateCommand<TaskItemDto> DeleteTaskCommand { get; }
    public DelegateCommand SaveTaskCommand { get; }
    public DelegateCommand CancelEditCommand { get; }
    public DelegateCommand BackCommand { get; }

    public TaskListViewModel(ITaskApiService taskService, IRegionManager regionManager)
    {
        _taskService = taskService;
        _regionManager = regionManager;

        LoadCommand = new DelegateCommand(async () => await LoadTasksAsync());
        NewTaskCommand = new DelegateCommand(OnNewTask);
        EditTaskCommand = new DelegateCommand<TaskItemDto>(OnEditTask);
        DeleteTaskCommand = new DelegateCommand<TaskItemDto>(async t => await OnDeleteTaskAsync(t));
        SaveTaskCommand = new DelegateCommand(async () => await OnSaveTaskAsync(), () => !string.IsNullOrWhiteSpace(EditTitle));
        CancelEditCommand = new DelegateCommand(() => IsEditing = false);
        BackCommand = new DelegateCommand(OnBack);
    }

    /// <summary>Загружает задачи процесса.</summary>
    private async Task LoadTasksAsync()
    {
        await ExecuteAsync(async () =>
        {
            var tasks = await _taskService.GetByProcessIdAsync(_processId);
            Tasks.Clear();
            foreach (var t in tasks)
                Tasks.Add(t);
        });
    }

    /// <summary>Открывает форму создания новой задачи.</summary>
    private void OnNewTask()
    {
        _editingTaskId = null;
        EditTitle = string.Empty;
        EditDescription = string.Empty;
        EditStatus = TaskItemStatus.NotStarted;
        EditPriority = TaskPriority.Medium;
        EditDueDate = null;
        IsEditing = true;
    }

    /// <summary>Открывает форму редактирования задачи.</summary>
    private void OnEditTask(TaskItemDto task)
    {
        _editingTaskId = task.Id;
        EditTitle = task.Title;
        EditDescription = task.Description ?? string.Empty;
        EditStatus = task.Status;
        EditPriority = task.Priority;
        EditDueDate = task.DueDate;
        IsEditing = true;
    }

    /// <summary>Сохраняет новую или обновляет существующую задачу.</summary>
    private async Task OnSaveTaskAsync()
    {
        await ExecuteAsync(async () =>
        {
            var dto = new TaskItemCreateDto(EditTitle, EditDescription, EditStatus, EditPriority, EditDueDate, _processId);

            if (_editingTaskId is null)
                await _taskService.CreateAsync(dto);
            else
                await _taskService.UpdateAsync(_editingTaskId.Value, dto);

            IsEditing = false;
            await LoadTasksAsync();
        });
    }

    /// <summary>Удаляет задачу.</summary>
    private async Task OnDeleteTaskAsync(TaskItemDto task)
    {
        var confirmMessage = System.Windows.Application.Current.TryFindResource("ConfirmDelete") as string
                             ?? "Вы уверены, что хотите удалить?";

        if (!ErrorHandler.Confirm(confirmMessage)) return;

        await ExecuteAsync(async () =>
        {
            await _taskService.DeleteAsync(task.Id);
            await LoadTasksAsync();
        });
    }

    /// <summary>Возврат к списку процессов.</summary>
    private void OnBack()
    {
        _regionManager.RequestNavigate("ContentRegion", "ProcessListView");
    }

    public void OnNavigatedTo(NavigationContext navigationContext)
    {
        _processId = navigationContext.Parameters.GetValue<Guid>("ProcessId");
        ProcessTitle = navigationContext.Parameters.GetValue<string>("ProcessTitle") ?? string.Empty;
        LoadCommand.Execute();
    }

    public bool IsNavigationTarget(NavigationContext navigationContext) => true;
    public void OnNavigatedFrom(NavigationContext navigationContext) { }
}
