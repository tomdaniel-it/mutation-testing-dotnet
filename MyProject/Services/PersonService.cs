using MyProject.Models;
using MyProject.Repositories.Interfaces;
using MyProject.Services.Interfaces;

namespace MyProject.Services;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public void CreatePerson(long id, string name, string firstSkill)
    {
        if (name.Length > 50)
        {
            throw new ArgumentException("Name can't be longer than 50 characters.");
        }

        if (firstSkill.Length > 30)
        {
            throw new ArgumentException("Skill can't be longer than 30 characters.");
        }

        var person = new PersonModel
        {
            Id = id,
            Name = name,
            Skills = new List<SkillModel>
            {
                new SkillModel { Skill = firstSkill.ToLower() }
            }
        };

        _personRepository.Add(person);
    }

    public void AddSkill(long personId, string skill)
    {
        if (skill.Length > 30)
        {
            throw new ArgumentException("Skill can't be longer than 30 characters.");
        }

        var person = _personRepository.Get(personId);

        if (person == null)
        {
            throw new ArgumentException($"Person with id {personId} does not exist.");
        }

        if (person.Skills.Any(personSkill => personSkill.Skill == skill))
        {
            throw new ArgumentException("This person already has this skill.");
        }

        person.Skills = person.Skills.Append(new SkillModel { Skill = skill });
        _personRepository.Update(person);
    }

    public IEnumerable<PersonModel> GetPersonsWithSkill(string skill)
    {
        if (string.IsNullOrWhiteSpace(skill))
        {
            throw new ArgumentException("Skill can't be empty while doing a query.");
        }

        return _personRepository
            .GetAll()
            .Where(person => person.Skills?.Any(personSkill => personSkill.Skill == skill.ToLower()) ?? false);
    }
}
