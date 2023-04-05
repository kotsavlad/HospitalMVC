namespace Hospital.Models;

public interface ICSVParser<out T>
{
    public static abstract T Parse(string s, string separator);
}