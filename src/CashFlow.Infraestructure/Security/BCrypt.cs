using CashFlow.Domain.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;
// Coloco em infraestrutura por ser algo relacionado a segurança.
// É feito a abstração porque eu preciso de uma forma de: caso eu troque de biblioteca eu posso trocar em apenas um lugar.
// Preciso encriptar a senha para poder garantir um maior valor de segurança.
namespace CashFlow.Infraestructure.Security;
internal class BCrypt : IPasswordEncripter
{
    public string Encrypt(string password)
    {
        string passwordHash = BC.HashPassword(password);

        return passwordHash;
    }
}
