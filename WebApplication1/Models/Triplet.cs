using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hospital.Models;

public class Triplet
{
    public List<(string Name, int? Age)>? Pairs { get; set; }
    public List<SelectListItem>? Years { get; set; }
    public string? YearString { get; set; }
}