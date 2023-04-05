using System.ComponentModel.DataAnnotations;

namespace Hospital.Models;

public class Doctor : ICSVParser<Doctor>
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required] public string Speciality { get; set; } = string.Empty;

    public static Doctor Parse(string s, string separator=";")
    {
        var words = s.Split(new[] { separator }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (words.Length != 3 || !int.TryParse(words[0], out var id))
            throw new FormatException("String cannot be parsed to the instance of Doctor type");
        return new Doctor { Id = id, Name = words[1], Speciality = words[2] };
    }

    public override string ToString() => $"Doctor(Id = {Id}, Name = {Name}, Speciality = {Speciality})";

    //public static bool TryParse(string? s, IFormatProvider? _, out Doctor? result)
    //{
    //    try
    //    {
    //        if (s is null)
    //            throw new NullReferenceException();
    //        result = Parse(s);
    //    }
    //    catch (Exception)
    //    {
    //        result = null;
    //        return false;
    //    }

    //    return true;
    //}
}