namespace Hospital.Models;

public class Visit : ICSVParser<Visit>
{
    public int DoctorId { get; set; }
    public int PatientId { get; set; }
    public DateOnly? Date { get; set; }

    public static Visit Parse(string? s, string separator = ";")
    {
        DateOnly date = DateOnly.MinValue;
        var words = s?.Split(new[] { ';' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (words is null || words.Length is < 2 or > 3 || !int.TryParse(words[0], out var id1) ||
            !int.TryParse(words[1], out var id2) || words.Length == 3 && !DateOnly.TryParse(words[2], out date))
        {
            throw new FormatException("String cannot be parsed to the instance of Visit type");
        }

        return new Visit { DoctorId = id1, PatientId = id2, Date = words.Length == 3 ? date : null };
    }

    //public static bool TryParse(string? s, IFormatProvider? provider, out Visit? result) =>
    //    (result = Parse(s)) is not null;
}