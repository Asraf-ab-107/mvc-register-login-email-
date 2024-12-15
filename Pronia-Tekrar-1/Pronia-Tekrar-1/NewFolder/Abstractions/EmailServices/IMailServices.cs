using Pronia_Tekrar_1.Helpers.Email;

namespace Pronia_Tekrar_1.NewFolder.Abstractions.EmailServices
{
    public interface IMailServices
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
