using FluentValidation;
using TaskManager.Shared.DTOs;

namespace TaskManager.Shared.Validators;

public class AuthValidators : AbstractValidator<RegisterRequest>
{
    public AuthValidators()
    {
        // Для регистрации
        RuleFor(x => x.Username).NotEmpty().MinimumLength(3);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class TaskValidators : AbstractValidator<TaskItemCreateDto>
{
    public TaskValidators()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ProcessId).NotEmpty();
    }
}

public class ProcessValidators : AbstractValidator<ProcessCreateDto>
{
    public ProcessValidators()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
    }
}
