using MagazineManager.Models;
using MagazineManager.Server.Controllers.CustomExceptions;
using MagazineManager.Server.Controllers.Helpers;
using MagazineManager.Server.Data.Repositories.Abstraction;
using MagazineManager.Server.Data.Repositories.Implementation;
using System.Text.RegularExpressions;

namespace MagazineManager.Server.Controllers.BLL
{
    internal class ApplicationUserBLL
    {
        ApplicationUserRepository _repo;

        internal ApplicationUserBLL(BaseRepository<ApplicationUser> repository)
        {
            _repo = (ApplicationUserRepository)repository;
        }

        internal ApplicationUser? Login(ApplicationUser applicationUser)
        {
            var storedApplicationUsers = _repo.GetByName(applicationUser.UserName);

            var applicationUserCount = storedApplicationUsers?.Count();
            if (storedApplicationUsers == null || applicationUserCount == 0)
            {
                throw new UserNotFoundException();
            }

            if (applicationUserCount > 1)
            {
                throw new ArgumentException();
            }

            var matchUser = storedApplicationUsers.FirstOrDefault();

            var pwdA = applicationUser.Pwd;
            var pwdB = Encryption.Decrypt(matchUser.Pwd);

            if (pwdA == pwdB)
            {
                HidePassword(matchUser);
                return matchUser;
            }

            return null;
        }

        internal void Save(ApplicationUser model)
        {
            var storedApplicationUser = _repo.Get(model.Id);

            if (storedApplicationUser == null)
                Post(model);
            else
                Put(model);
        }

        private void Post(ApplicationUser model)
        {
            Validate(ref model);
            _repo.Post(model);
        }

        private void Put(ApplicationUser model)
        {
            Validate(ref model);
            _repo.Put(model);
        }

        internal void Delete(int id)
        {
            _repo.Delete(id);
        }

        internal ApplicationUser Get(int id)
        {
            var storedApplicationUser = _repo.Get(id);
            HidePassword(storedApplicationUser);
            return storedApplicationUser;
        }

        internal IEnumerable<ApplicationUser> GetAll()
        {
            var storedApplicationUsers = _repo.GetAll();
            HidePasswords(storedApplicationUsers);
            return storedApplicationUsers;
        }

        internal IEnumerable<ApplicationUser> GetByName(string name)
        {
            var storedApplicationUsers = _repo.GetByName(name);
            HidePasswords(storedApplicationUsers);
            return storedApplicationUsers;
        }

        #region Validations
        ///Erase passwords from collection to avoid return in the payload
        private static void HidePasswords(IEnumerable<ApplicationUser>? storedApplicationUsers)
        {
            if (storedApplicationUsers != null)
                foreach (var item in storedApplicationUsers)
                    HidePassword(item);
        }

        ///Erase passwords from object application user to avoid return in the payload
        private static void HidePassword(ApplicationUser? storedApplicationUser)
        {
            if (storedApplicationUser != null)
                storedApplicationUser.Pwd = null; //Security work around: Not nullable, but null necessary to return in the payload
        }

        private void Validate(ref ApplicationUser model)
        {
            if (!ApplicationUserRoles.RolesArray.Contains(model.Role.ToLower()))
                throw new ApplicationUserRoleDoesntMatchException();

            if (!IsValidPassword(model.Pwd))
                throw new ApplicationUserPasswordInvalidPatternException();
            
            if (!ValidateApplicationUser(model))
                throw new ApplicationUserAlreadyExistsException();

            model.Pwd = Encryption.Encrypt(model.Pwd);
        }

        private bool ValidateApplicationUser(ApplicationUser model)
        {
            var storedApplicationUser = _repo.GetByName(model.UserName);

            if (storedApplicationUser != null && storedApplicationUser.Count() > 0)
            {
                foreach (var item in storedApplicationUser)
                {
                    if (item.Id != model.Id)
                        return false;
                }
            }
            return true;
        }

        private static bool IsValidPassword(string pwdText)
        {
            Regex regex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
            Match match = regex.Match(pwdText);
            return match.Success;
        }
        #endregion
    }
}
