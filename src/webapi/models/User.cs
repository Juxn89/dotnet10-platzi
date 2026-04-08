namespace webapi.models;

public class User
{
  public int Id { get; set; }
  public string Name { get; set; } = default!;
  public string Email { get; set; } = default!;
  public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
