using Moq;
using MyProject.Models;
using MyProject.Repositories.Interfaces;
using MyProject.Services;

namespace MyProject.Test.Services;

public class PersonServiceTest
{
    private const long BOB_ID = 1;
    private const long ALICE_ID = 2;
    private const string BOB_NAME = "Bob";
    private const string ALICE_NAME = "Alice";
    private const string MUTATION_TESTING_SKILL = "mutation-testing";
    private const string STRYKER_SKILL = "stryker";
    private const string UNIT_TESTING_SKILL = "unit-testing";

    private readonly Mock<IPersonRepository> _personRepositoryMock;

    public PersonServiceTest()
    {
        _personRepositoryMock = new Mock<IPersonRepository>();

        var bob = new PersonModel
        {
            Id = BOB_ID,
            Name = BOB_NAME,
            Skills = new List<SkillModel>
            {
                new SkillModel { Skill = MUTATION_TESTING_SKILL }
            }
        };
        var alice = new PersonModel
        {
            Id = ALICE_ID,
            Name = ALICE_NAME,
            Skills = new List<SkillModel>
            {
                new SkillModel { Skill = UNIT_TESTING_SKILL }
            }
        };

        _personRepositoryMock
            .Setup(el => el.Get(BOB_ID))
            .Returns(bob);
        _personRepositoryMock
            .Setup(el => el.Get(ALICE_ID))
            .Returns(alice);
        _personRepositoryMock
            .Setup(el => el.GetAll())
            .Returns(new List<PersonModel>() { alice, bob });
    }

    #region CreatePerson
    [Fact]
    public void CreatePerson_WithNameAndFirstSkill_CreatesPerson()
    {
        var service = CreatePersonService();

        service.CreatePerson(BOB_ID, BOB_NAME, MUTATION_TESTING_SKILL);

        _personRepositoryMock.Verify(el => el.Add(It.Is<PersonModel>(person =>
            person.Id == BOB_ID &&
            person.Name == BOB_NAME &&
            person.Skills.Single().Skill == MUTATION_TESTING_SKILL)));
    }
    #endregion

    #region AddSkill
    [Fact]
    public void AddSkill_ExistingPersonAndSkill_AddsSkill()
    {
        var service = CreatePersonService();

        service.AddSkill(BOB_ID, STRYKER_SKILL);

        _personRepositoryMock.Verify(el => el.Update(It.Is<PersonModel>(person =>
            person.Id == BOB_ID &&
            person.Name == BOB_NAME &&
            person.Skills.Any(personSkill => personSkill.Skill == STRYKER_SKILL) &&
            person.Skills.Count() == 2)));
    }

    [Fact]
    public void AddSkill_ExistingPersonWithExistingSkill_ThrowsException()
    {
        var service = CreatePersonService();

        var ex = Assert.Throws<ArgumentException>(() => service.AddSkill(BOB_ID, MUTATION_TESTING_SKILL));
        Assert.Equal("This person already has this skill.", ex.Message);
    }

    [Fact]
    public void AddSkill_PersonDoesNotExist_ThrowsException()
    {
        var service = CreatePersonService();

        var ex = Assert.Throws<ArgumentException>(() => service.AddSkill(100, STRYKER_SKILL));
        Assert.Equal("Person with id 100 does not exist.", ex.Message);
    }

    [Fact]
    public void AddSkill_SkillTooLong_ThrowsException()
    {
        var service = CreatePersonService();

        Assert.ThrowsAny<Exception>(() => service.AddSkill(1, "this_skill_name_is_way_too_long_to_be_readable"));
    }
    #endregion

    #region GetPersonsWithSkill
    [Fact]
    public void GetPersonsWithSkill_OnePersonFound_ReturnsPerson()
    {
        var service = CreatePersonService();

        var result = service.GetPersonsWithSkill(UNIT_TESTING_SKILL);

        Assert.Equal(ALICE_ID, result.First().Id);
    }

    [Fact]
    public void GetPersonsWithSkill_SkillIsNull_ThrowsException()
    {
        var service = CreatePersonService();

        var ex = Assert.Throws<ArgumentException>(() => service.GetPersonsWithSkill(null));
        Assert.Equal("Skill can't be empty while doing a query.", ex.Message);
    }
    #endregion

    private PersonService CreatePersonService()
    {
        return new PersonService(_personRepositoryMock.Object);
    }
}
