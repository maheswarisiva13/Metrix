using Metrix.Application.DTOs.Admin;
using System;
using System.Collections.Generic;
using System.Text;

namespace Metrix.Application.Interfaces.Services
{
    public interface IAdminService
    {
        Task<string> CreateAdminAsync(RegisterAdminRequestDto dto);
    }
}
