using ECC.Client.API.Models;
using ECC.Core.DomainObjects;
using ECC.Core.Messages;
using FluentValidation;

namespace ECC.Client.API.Application.Commands
{
    public class RegisterClientCommand : Command
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Cpf { get; private set; }

        public RegisterClientCommand(Guid id, string name, string email, string cpf)
        {
            AggregateId = id;
            Id = id;
            Name = name;
            Email = email;
            Cpf = cpf;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterClientValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class RegisterClientValidation : AbstractValidator<RegisterClientCommand>
    {
        public RegisterClientValidation()
        {

            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do cliente inválido");

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("O nome do cliente não foi informado");

            RuleFor(c => c.Cpf)
                .Must(HasValidCpf)
                .WithMessage("O CPF informado não é válido.");

            RuleFor(c => c.Email)
                .Must(HasValidEmail)
                .WithMessage("O e-mail informado não é válido.");
        }

        protected static bool HasValidCpf(string cpf)
        {
            return Cpf.Validate(cpf);
        }
        protected static bool HasValidEmail(string email)
        {
            return Email.Validate(email);
        }
    }
}
