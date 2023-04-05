namespace Hospital.Models;

public class Patient : ICSVParser<Patient>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }

    public static Patient Parse(string? s, string separator = ";")
    {
        var words = s?.Split(new[] { separator }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (words is null || words.Length != 3 || !int.TryParse(words[0], out var id) ||
            !DateOnly.TryParse(words[2], out var date))
            throw new FormatException("String cannot be parsed to the instance of Patient type");
        return new Patient { Id = id, Name = words[1], BirthDate = date };
    }

    //public static bool TryParse(string? s, IFormatProvider? provider, out Patient? result) =>
    //    (result = Parse(s)) is not null;
}