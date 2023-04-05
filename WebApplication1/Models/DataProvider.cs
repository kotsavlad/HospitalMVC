using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Text;

namespace Hospital.Models;

public class DataProvider
{
    private static List<Patient>? _patients;
    private static Dictionary<int, Patient>? _patientsDictionary;
    private static List<Doctor>? _doctors;
    private static Dictionary<int, Doctor>? _doctorsDictionary;
    private static List<Visit>? _visits;

    private const string DefaultDataDir = "./App_Data";
    private const string DoctorsFileName = "Doctors.csv";
    private const string PatientsFileName = "Patients.csv";
    private const string VisitsFileName = "Visits.csv";
    private static object _lock = new();

    public static List<Doctor> Doctors
    {
        get
        {
            if (_doctors is null)
            {
                lock (_lock)
                {
                    _doctors ??= ReadData<Doctor>(DoctorsFileName);
                }
            }

            return _doctors;
        }
    }
    //public static List<Doctor> Doctors => _doctors ??= ReadData<Doctor>(DoctorsFileName);

    // TODO: Synchronization of ReadData invocation like it done in Doctors
    public static List< Patient> Patients => _patients ??= ReadData<Patient>(PatientsFileName);

        public static Dictionary<int, Doctor> DoctorsDictionary =>
    _doctorsDictionary ??= Doctors.ToDictionary(d => d.Id, d => d);

    public static Dictionary<int, Patient> PatientsDictionary =>
        _patientsDictionary ??= Patients.ToDictionary(p => p.Id, p => p);

    public static List<Visit> Visits
    {
        get
        {
            if (_visits == null)
            {
                _visits = ReadData<Visit>(VisitsFileName);
                var doctorsSet = Doctors.Select(d => d.Id).ToHashSet();
                var patientsSet = Patients.Select(p => p.Id).ToHashSet();
                _visits = _visits.Where(v => doctorsSet.Contains(v.DoctorId) && patientsSet.Contains(v.PatientId))
                    .ToList();
            }

            return _visits;
        }
    }


    public static List<T> ReadData<T>(string fileName, string? dataDir = null, string separator = ";")
        where T : ICSVParser<T>
    {
        var items = new List<T>();
        dataDir ??= DefaultDataDir;

        int lineNumber = 0;
        var fullName = Path.Combine(dataDir, fileName);
        Trace.WriteLine($"{DateTime.Now:HH:mm:ss}: {fullName} load started");
        // Bug? Next indent does not work
        //Trace.Indent();
        try
        {
            foreach (var line in File.ReadAllLines(fullName))
            {
                lineNumber++;
                try
                {
                    var item = T.Parse(line, separator: separator);
                    items.Add(item);
                }
                catch (Exception)
                {
                    Trace.WriteLine($"{fullName}: inconsistent data in line #{lineNumber}");
                }
            }
        }
        catch (Exception e)
        {
            Trace.WriteLine($"{fullName}: exception {e.Message}");
        }
        finally
        {
            //Trace.Unindent();
            Trace.WriteLine($"{DateTime.Now:HH:mm:ss}: {fullName} load finished");
        }

        return items;
    }

    public static List<(string Name, int? Age)> MaxAges(int? year = null)
    {
        var list = new List<(string Name, int? Age)>();
        var ageDict = new Dictionary<int, int>();
        foreach (var visit in Visits)
        {
            if (year is null || visit.Date?.Year == year)
            {
                if (ageDict.ContainsKey(visit.DoctorId))
                {
                    ageDict[visit.DoctorId] =
                        Math.Max(ageDict[visit.DoctorId], GetAge(PatientsDictionary[visit.PatientId].BirthDate));
                }
                else
                {
                    ageDict[visit.DoctorId] = GetAge(PatientsDictionary[visit.PatientId].BirthDate);
                }

                ageDict[visit.DoctorId] = Math.Max(ageDict.GetValueOrDefault(visit.DoctorId),
                    GetAge(PatientsDictionary[visit.PatientId].BirthDate));
            }
        }


        foreach (var doctor in Doctors)
        {
            int age = ageDict.GetValueOrDefault(doctor.Id, -1);
            list.Add((doctor.Name, age >= 0 ? age : null));
        }

        return list;

        int GetAge(DateOnly date)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - date.Year;
            today.AddYears(-age);
            return today < date ? age - 1 : age;
        }
    }

    public static IList<string> VisitAllDoctors()
    {
        var list = new List<string>();
        HashSet<string> allSpecialities = new();
        foreach (var doctor in Doctors)
        {
            allSpecialities.Add(doctor.Speciality);
        }

        int specialityCount = allSpecialities.Count;

        Dictionary<int, HashSet<string>> dict = new();
        foreach (var visit in Visits)
        {
            if (!dict.ContainsKey(visit.PatientId))
                dict[visit.PatientId] = new HashSet<string>();
            dict[visit.PatientId].Add(DoctorsDictionary[visit.DoctorId].Speciality);
        }

        foreach (var (key, value) in dict)
        {
            if (value.Count == specialityCount)
            {
                list.Add(PatientsDictionary[key].Name);
            }
        }

        return list;
    }
}