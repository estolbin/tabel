using web_tabel.Domain;
using web_tabel.Services;
using web_tabel.Services.TimeShiftContext;

namespace web_table.Web.Services
{
    public class UserService
    {
        private readonly UnitOfWork _unitOfWork;

        public UserService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public async Task<AppUser> GetUser(string username) => 
            await _unitOfWork.UserRepository.SingleOrDefaultAsync(x => x.Name == username);

        public async Task<bool> IsUserInRole(string username, string role)
        {
            var user = await GetUser(username);
            var existsRole = await _unitOfWork.RoleRepository.SingleOrDefaultAsync(x => x.Name == role);
            return user != null && user.Role == existsRole;
        }
    }
}
