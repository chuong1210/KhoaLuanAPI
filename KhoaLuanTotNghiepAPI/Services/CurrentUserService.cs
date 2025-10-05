using System.Security.Claims;
using Application.Interfaces;
using Application.Interfaces.Identity;
using KhoaLuanTotNghiepAPI.Constants;
namespace KhoaLuanTotNghiepAPI.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

        //public string? UserProfileId
        //{
        //    get
        //    {
        //        string userIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(CONSTANT_CLAIM_TYPES.Uid);
        //        if (userIdString != null && int.TryParse(userIdString, out int userId))
        //        {
        //            return userIdString;
        //        }
        //        return null;
        //    }
        //}

        public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public string UserProfileId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("UserProfileId");

        public string Username => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public bool IsInRole(string role)
        {
            return _httpContextAccessor.HttpContext?.User?.IsInRole(role) ?? false;
        }

        public List<string> GetRoles()
        {
            return _httpContextAccessor.HttpContext?.User?
                .FindAll(ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList() ?? new List<string>();
        }
   
     
        //public string? Type => _httpContextAccessor.HttpContext?.User?.FindFirstValue(CONSTANT_CLAIM_TYPES.Type);

        //public int? StaffId => int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(CONSTANT_CLAIM_TYPES.Staff));

    }
}
