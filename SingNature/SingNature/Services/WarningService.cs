using SingNature.Data;
using SingNature.Models;
using System.Linq;
using System.Threading.Tasks;

public class WarningService
{
    private readonly WarningDAO _warningDAO;

    public WarningService()
    {
    }

    public WarningService(WarningDAO warningDAO)
    {
        _warningDAO = warningDAO;
    }

    public Warning GetLatestWarning()
{
    var warnings = _warningDAO.GetAllWarnings();
    
    Console.WriteLine($"Total warnings retrieved: {warnings.Count}");

    if (!warnings.Any())
    {
        Console.WriteLine("No warnings found in the database.");
        return null;
    }

    var latestWarning = warnings.OrderByDescending(w => w.WarningId).FirstOrDefault();
    
    if (latestWarning == null)
    {
        Console.WriteLine("No latest warning found after sorting.");
    }
    else
    {
        Console.WriteLine($"Latest Warning ID: {latestWarning.WarningId}");
    }

    return latestWarning;
}
}
