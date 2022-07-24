using ECC.Client.API.Models;
using ECC.Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace ECC.Client.API.Application.Commands
{
    public class ClientCommandHandler : CommandHandler, IRequestHandler<RegisterClientCommand, ValidationResult>
    {
        private readonly IClientRepository _clientRepository;

        public ClientCommandHandler(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }


        public async Task<ValidationResult> Handle(RegisterClientCommand message,
            CancellationToken cancellationToken)

        {
            if (!message.IsValid()) return message.ValidationResult;

            var client = new Models.Client(message.Id, message.Name, message.Email, message.Cpf);

            var clientExists = await _clientRepository.FindByCpf(client.Cpf.Number);
            
      
            if (clientExists != null) //se já existir cliente
            {
                AddError("Este CPF já está em uso");
                return ValidationResult;
            }

            _clientRepository.Add(client);

            return await PersistData(_clientRepository.UnityOfWork);
        }
    }
}
