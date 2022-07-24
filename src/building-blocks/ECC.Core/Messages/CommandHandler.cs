using ECC.Core.Data;
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

    protected async Task<ValidationResult> PersistData(IUnityOfWork unityOfWork)
    {
        if(!await unityOfWork.Commit()) AddError("Houve erro ao persistir os dados");
        
        return ValidationResult;
    }


}