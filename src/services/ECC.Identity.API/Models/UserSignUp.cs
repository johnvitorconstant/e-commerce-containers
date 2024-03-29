﻿using System.ComponentModel.DataAnnotations;

namespace ECC.Identity.API.Models;

public class UserSignUp
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
    public string Password { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [Compare("Password", ErrorMessage = "As senhas não conferem.")]
    public string PasswordConfirmation { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    public string Cpf { get; set; }


}