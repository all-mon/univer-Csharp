using System.ComponentModel.DataAnnotations;

public class Person
    {
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Password { get; set; }
    }
