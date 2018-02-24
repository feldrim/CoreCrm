using System.Threading.Tasks;

namespace Fiver.Security.AspIdentity.Core.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}