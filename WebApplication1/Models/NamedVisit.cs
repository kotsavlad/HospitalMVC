namespace Hospital.Models;

public record NamedVisit(string? DoctorName, string? PatientName, DateOnly? Date)
{ }