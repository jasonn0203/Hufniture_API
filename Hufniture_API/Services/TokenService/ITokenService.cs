using Hufniture_API.Data;

namespace Hufniture_API.Services.TokenService
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(AppUser user);
    }

}
