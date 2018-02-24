using System.Threading.Tasks;

namespace CoreCrm.Core.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}