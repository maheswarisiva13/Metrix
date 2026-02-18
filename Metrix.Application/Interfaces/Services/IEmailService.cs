using System;
using System.Collections.Generic;
using System.Text;

namespace Metrix.Application.Interfaces.Services
{
    

    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body);
    }

}
