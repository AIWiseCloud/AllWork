using AllWork.Model.Sys;
using System.Threading.Tasks;
using System;

namespace AllWork.Web.Auth
{
    public interface IAuthenticateService
    {
        Task<Tuple<bool,string>> IsAuthenticated(LoginRequestDTO request);

        string ParseToken(string token);
    }
}
