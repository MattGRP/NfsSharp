using NfsSharp.Client;
using NfsSharp.Protocol;

namespace NfsSharp.Tests;

public sealed class NfsV3IntegrationTests
{
    [NfsV3IntegrationFact]
    [Trait("Category", "Integration")]
    public async Task SmokeTest_ListsAndMountsExport()
    {
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        var options = new NfsClientOptions
        {
            UserId = NfsV3IntegrationEnvironment.UserId,
            GroupId = NfsV3IntegrationEnvironment.GroupId,
            UsePrivilegedSourcePort = false,
            CommandTimeout = TimeSpan.FromSeconds(10),
            MaxRetries = 0
        };

        var exports = await NfsV3Client.ListExportsAsync(
            NfsV3IntegrationEnvironment.Server,
            options,
            timeout.Token);

        Assert.Contains(
            exports,
            export => string.Equals(
                export.Path,
                NfsV3IntegrationEnvironment.ExportPath,
                StringComparison.Ordinal));

        await using var client = await NfsV3Client.ConnectAsync(
            NfsV3IntegrationEnvironment.Server,
            NfsV3IntegrationEnvironment.ExportPath,
            options,
            timeout.Token);

        var attributes = await client.GetAttributesAsync(
            client.RootHandle,
            timeout.Token);

        Assert.Equal(NfsType.Dir, attributes.Type);
        Assert.NotEmpty(client.RootHandle);

        await client.UnmountAsync(timeout.Token);
    }
}
