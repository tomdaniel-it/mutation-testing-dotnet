using MyProject.Models;

namespace MyProject.Repositories.Interfaces
{
    public interface IPersonRepository
    {
        public void Add(PersonModel person);
        public void Update(PersonModel person);
        public void Delete(PersonModel person);
        public IEnumerable<PersonModel> GetAll();
        public PersonModel Get(long id);
    }
}
