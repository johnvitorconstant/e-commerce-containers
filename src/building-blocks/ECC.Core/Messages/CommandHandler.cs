using FluentValidation.Results;

namespace ECC.Core.Messages;

public abstract class CommandHandler
{
    protected ValidationResult ValidationResult;


    protected CommandHandler()
    {
        ValidationResult = new ValidationResult();
    }

    protected void AddError(string message)
    {
        ValidationResult.Errors.Add(new ValidationFailure(string.Empty, message));
    }


}