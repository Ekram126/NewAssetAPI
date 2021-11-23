using Asset.ViewModels.UserVM;

namespace Asset.Domain
{
    public interface IEmailSender
    {
        void SendEmail(MessageVM message);
        void SendEmailString(string message);
    }
}
