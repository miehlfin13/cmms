using Microsoft.SqlServer.Dac;

namespace Synith.Test;
public class Dacpac
{
    private readonly string _connectionString;
    private readonly string _dacpacPath;

    public Dacpac(string connectionString, string dacpacPath)
    {
        _connectionString = connectionString;
        _dacpacPath = dacpacPath;
    }

    public void PublishDatabase()
    {
        DacDeployOptions dacOptions = new() { BlockOnPossibleDataLoss = false };

        using DacPackage dacpac = DacPackage.Load(_dacpacPath);

        DacServices dacService = new(_connectionString);
        dacService.Deploy(dacpac, "master", true, dacOptions);
    }
}
