using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Hospital.Models
{
    public class User : ICSVParser<User>
    {
        private static readonly Regex _pattern = new(@"\t");
        
        [RegularExpression(@"^\S{2,40}$")]
        public string Name { get; set; } = string.Empty;
        
        [RegularExpression(@"^\S{5,40}$")]
        public string Password { get; set; } = string.Empty;

        public static User Parse(string s, string separator)
        {
            var words = _pattern.Split(s);
            if (words.Length != 2)
            {
                throw new FormatException($"Error in the user parsing from the line {s}");
            }
            return new User { Name = words[0], Password = words[1] };
        }

        public override string ToString() => $"{Name}\t{Password}";
    }
}