﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ECC.Core.DomainObjects
{
    public class Email
    {
        public const int EnderecoMaxLength = 254;
        public const int EnderecoMinLength = 5;
        public string Address { get; private set; }

        //Construtor do EntityFramework
        protected Email() { }

        public Email(string address)
        {
            if (!Validar(address)) throw new DomainException("E-mail inválido");
            Address = address;
        }

        public static bool Validar(string email)
        {
            var regexEmail = new Regex(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
            return regexEmail.IsMatch(email);
        }
    }
}
