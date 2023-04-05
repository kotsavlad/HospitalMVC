using System.Diagnostics;

namespace Hospital.Models;

public class UserProvider
{
    private const string DefaultFileName = "users.txt";
    private const string DefaultDataDir = "./App_Data";
    private static string _fullName = string.Empty;
    private static readonly object _lock = new();

    private static readonly Dictionary<string, User> _users = new();

    static UserProvider()
    {
        ReadUsers();
    }

    public static void ClearUsers() => _users.Clear();

    public static bool IsAuthorizedUser(string username, string password) =>
        _users.ContainsKey(username) && _users[username].Password == password;

    public static bool HasAccount(string userName) => _users.ContainsKey(userName);

    public static void ReadUsers(string? dataDir = null, string? fileName = null, bool append = true)
    {
        dataDir ??= DefaultDataDir;
        fileName ??= DefaultFileName;
        _fullName = Path.Combine(dataDir, fileName);
        var userList = DataProvider.ReadData<User>(DefaultFileName, dataDir, separator: "\t");
        if (!append)
        {
            ClearUsers();
            Trace.WriteLine($"{DateTime.Now.TimeOfDay}: user list cleared");
        }

        // Bug? Next indent does not work
        //Trace.Indent();
        int count = 0;
        lock (_lock)
        {
            for (var index = 0; index < userList.Count; index++)
            {
                if (!_users.TryAdd(userList[index].Name, userList[index]))
                {
                    Trace.WriteLine(
                        $"{_fullName}: repeated occurrence of {userList[index].Name} is ignored");
                }
                else
                {
                    count++;
                }
            }
        }

        //Trace.Unindent();
        Trace.WriteLine($"{DateTime.Now:HH:mm:ss}: {count} users loaded");
    }

    public static bool TryAddUser(User user)
    {
        var result = false;
        if (!_users.ContainsKey(user.Name))
        {
            try
            {
                lock (_lock)
                {
                    using var writer = new StreamWriter(_fullName, append: true);
                    _users.Add(user.Name, user);
                    writer.Write($"\n{user}");
                }

                result = true;
                Trace.WriteLine($"{DateTime.Now:HH:mm:ss}: {user.Name} is saved in {_fullName}");
            }
            catch (IOException e)
            {
                Trace.WriteLine(
                    $"{DateTime.Now.TimeOfDay}: {user.Name} saving in {_fullName} failed with exception: {e.Message}");
            }
        }

        return result;
    }

    public static bool TrySaveUsers(string? dataDir = null, string? fileName = null)
    {
        dataDir ??= DefaultDataDir;
        fileName ??= DefaultFileName;
        var fullName = Path.Combine(dataDir, fileName);
        bool result = false;
        try
        {
            lock (_lock)
            {
                Trace.WriteLine($"{DateTime.Now:HH:mm:ss}: users save started in {fullName}");
                using var writer = new StreamWriter(fullName, append: true);
                foreach (var user in _users)
                {
                    writer.WriteLine(user);
                }
            }

            result = true;
            Trace.WriteLine($"{DateTime.Now:HH:mm:ss}: users save finished. {_users.Count} users saved in {fullName}");
        }
        catch (Exception e)
        {
            Trace.WriteLine($"Users saving in {fullName} failed: exception {e.Message}");
        }

        return result;
    }
}