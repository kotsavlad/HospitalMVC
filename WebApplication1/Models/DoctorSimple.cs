using System.ComponentModel.DataAnnotations;

namespace Hospital.Models;

public class DoctorSimple
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public string? Speciality { get; set; }


    public override string ToString() => $"DoctorSimple(Id = {Id}, Name = {Name}, Speciality = {Speciality})";

}