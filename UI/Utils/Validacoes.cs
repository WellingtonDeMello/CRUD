using System.Text.RegularExpressions;

namespace UI.Utils
{
    public static class Validacoes
    {
        public static bool CampoVazio(string texto)
        {
            return string.IsNullOrWhiteSpace(texto);
        }

        public static bool EmailValido(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public static bool ApenasNumeros(string texto)
        {
            return Regex.IsMatch(texto, @"^\d+$");
        }

        public static bool TelefoneValido(string telefone)
        {
            return ApenasNumeros(telefone) && telefone.Length >= 10 && telefone.Length <= 11;
        }

        public static bool CnpjValido(string cnpj)
        {
            return ApenasNumeros(cnpj) && cnpj.Length == 14;
        }
    }
}