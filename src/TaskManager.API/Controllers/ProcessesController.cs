using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Services.Interfaces;
using TaskManager.Shared.DTOs;

namespace TaskManager.API.Controllers;

/// <summary>
/// Контроллер для CRUD-операций с процессами.
/// Все эндпоинты требуют авторизации.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProcessesController(IProcessService processService) : ControllerBase
{
    /// <summary>Получить все процессы текущего пользователя.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetCurrentUserId();
        var processes = await processService.GetAllByOwnerAsync(userId);
        return Ok(processes);
    }

    /// <summary>Получить процесс по идентификатору.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var process = await processService.GetByIdAsync(id);
        if (process is null)
            return NotFound(new ApiErrorResponse(404, "Процесс не найден."));

        return Ok(process);
    }

    /// <summary>Создать новый процесс.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProcessCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest(new ApiErrorResponse(400, "Название процесса обязательно."));

        var userId = GetCurrentUserId();
        var process = await processService.CreateAsync(userId, dto);
        return CreatedAtAction(nameof(GetById), new { id = process.Id }, process);
    }

    /// <summary>Обновить существующий процесс.</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProcessCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest(new ApiErrorResponse(400, "Название процесса обязательно."));

        var userId = GetCurrentUserId();
        var process = await processService.UpdateAsync(id, userId, dto);
        return Ok(process);
    }

    /// <summary>Удалить процесс.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = GetCurrentUserId();
        await processService.DeleteAsync(id, userId);
        return NoContent();
    }

    /// <summary>Извлекает идентификатор пользователя из JWT claims.</summary>
    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("Не удалось определить пользователя.");

        return Guid.Parse(userIdClaim);
    }
}
