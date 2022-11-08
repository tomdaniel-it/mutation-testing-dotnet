namespace MyProject.Models;

public class PersonModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<SkillModel> Skills { get; set; }
}
