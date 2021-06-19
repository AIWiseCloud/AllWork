using AllWork.Model.Sys;

namespace AllWork.Web.Auth
{
    public interface IAuthenticateService
    {
        bool IsAuthenticated(LoginRequestDTO request, out string token);
    }
}
