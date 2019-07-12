using System.Threading.Tasks;

namespace MediaStorage.Common
{
    public interface IMailSender
    {
        Task Send(string to, string subject, string body);
    }
}