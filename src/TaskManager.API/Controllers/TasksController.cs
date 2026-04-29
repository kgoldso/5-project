using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Services.Interfaces;
using TaskManager.Shared.DTOs;

namespace TaskManager.API.Controllers;

/// <summary>
/// Контроллер для CRUD-операций с задачами.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController(ITaskService taskService) : ControllerBase
{
    /// <summary>Получить все задачи указанного процесса.</summary>
    [HttpGet("by-process/{processId:guid}")]
    public async Task<IActionResult> GetByProcess(Guid processId)
    {
        var tasks = await taskService.GetByProcessIdAsync(processId);
        return Ok(tasks);
    }

    /// <summary>Получить задачу по идентификатору.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var task = await taskService.GetByIdAsync(id);
        if (task is null)
            return NotFound(new ApiErrorResponse(404, "Задача не найдена."));

        return Ok(task);
    }

    /// <summary>Создать новую задачу.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TaskItemCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest(new ApiErrorResponse(400, "Название задачи обязательно."));

        var task = await taskService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    /// <summary>Обновить задачу.</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] TaskItemCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest(new ApiErrorResponse(400, "Название задачи обязательно."));

        var task = await taskService.UpdateAsync(id, dto);
        return Ok(task);
    }

    /// <summary>Удалить задачу.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await taskService.DeleteAsync(id);
        return NoContent();
    }
}
