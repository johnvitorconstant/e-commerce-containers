using ECC.Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace ECC.Client.API.Application.Commands
{
    public class ClientCommandHandler :CommandHandler ,IRequestHandler<RegisterClientCommand, ValidationResult>
    {
        public async Task<ValidationResult> Handle(RegisterClientCommand message, 
            CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;

            var client = new Models.Client(message.Id, message.Name, message.Email, message.Cpf);
            //Validar negocio

            //Persistir no banco
            if (true) //se já existir cliente
            {
                AddError("Este CPF já está em uso");
                return ValidationResult;
            }


            return message.ValidationResult;
        }
    }
}
