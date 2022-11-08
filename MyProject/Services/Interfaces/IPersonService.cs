using MyProject.Models;

namespace MyProject.Services.Interfaces;

public interface IPersonService
{
    public void CreatePerson(long id, string name, string firstSkill);
    public void AddSkill(long personId, string skill);
    public IEnumerable<PersonModel> GetPersonsWithSkill(string skill);
}
