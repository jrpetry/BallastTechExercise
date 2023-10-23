using MagazineManager.Models;
using MagazineManager.Server.Controllers.CustomExceptions;
using MagazineManager.Server.Data.Repositories.Abstraction;
using MagazineManager.Server.Data.Repositories.Implementation;

namespace MagazineManager.Server.Controllers.BLL
{
    internal class MagazineBLL
    {
        MagazineRepository _repo;
        internal MagazineBLL(BaseRepository<Magazine> repository)
        {
            _repo = (MagazineRepository)repository;
        }

        internal Magazine Get(int id)
        {
            var storedMagazine = _repo.Get(id);
            return storedMagazine;
        }

        internal IEnumerable<Magazine> GetByName(string name)
        {
            var storedMagazines = _repo.GetByName(name);
            return storedMagazines;
        }

        internal IEnumerable<Magazine> GetByApplicationUserId(int applicationUserId)
        {
            var storedMagazines = _repo.GetByApplicationUserId(applicationUserId);
            return storedMagazines;
        }

        internal IEnumerable<Magazine> GetAll()
        {
            var storedMagazines = _repo.GetAll();
            return storedMagazines;
        }

        internal void Save(Magazine model)
        {
            var storedMagazine = _repo.Get(model.Id);

            if (storedMagazine == null)
                Post(model);
            else
                Put(model);
        }

        private void Post(Magazine model)
        {
            Validate(ref model);
            _repo.Post(model);
        }

        private void Put(Magazine model)
        {
            Validate(ref model);
            _repo.Put(model);
        }

        internal void Delete(int id)
        {
            _repo.Delete(id);
        }

        #region Validations

        private void Validate(ref Magazine model)
        {
            if (model.ReleaseDate < DateTime.MinValue || model.ReleaseDate > DateTime.Now)
                throw new MagazineInvalidReleaseDateException();

            if (model.ApplicationUserId == 0)
                throw new MagazineInvalidApplicationUserException();

            var storedMagazines = _repo.GetByName(model.Name);
            if (storedMagazines != null && storedMagazines.Count() > 0)
            {
                int id = model.Id;
                foreach (var item in storedMagazines.Where(o => o.Id != id))
                {
                    if (item.ApplicationUserId == model.ApplicationUserId)
                        throw new MagazineAlreadyExistsForApplicationUserException();
                }
            }
        }
        #endregion
    }
}
