using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoelScottFitness.Services.Services
{
    public interface IEmailService
    {
        bool SendEmail(string subject, string content, IEnumerable<string> receivers);

        Task<bool> SendEmailAsync(string subject, string content, IEnumerable<string> receivers);
    }
}
