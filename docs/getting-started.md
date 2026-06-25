# Getting Started

NfsSharp provides managed .NET APIs for NFSv3 export discovery, mounting, directory traversal, file I/O, metadata, and file-system capability queries without invoking native NFS command-line tools.

## Install

```powershell
dotnet add package NfsSharp.Client
```

Use the protocol package directly when you only need XDR, ONC RPC, NFS model types, status codes, or RPCSEC_GSS abstractions:

```powershell
dotnet add package NfsSharp.Protocol
```

## Connect and list exports

```csharp
using NfsSharp.Client;

await using var client = new NfsClientBuilder()
    .WithCredentials(userId: 1000, groupId: 1000)
    .WithPrivilegedSourcePort(false)
    .Build();

await client.ConnectAsync("nfs.example.internal");

var exports = await client.GetExportedDevicesAsync();
foreach (var export in exports)
{
    Console.WriteLine(export.Path);
}
```

`WithPrivilegedSourcePort(false)` is useful for development environments and containerized test servers. Some production NFS servers require privileged source ports, which may require elevated process permissions.

## Mount an export and read a file

```csharp
using NfsSharp.Client;

await using var client = new NfsClientBuilder()
    .WithCredentials(userId: 1000, groupId: 1000)
    .WithPrivilegedSourcePort(false)
    .Build();

await client.ConnectAsync("nfs.example.internal");
await client.MountDeviceAsync("/srv/data");

var entries = await client.GetItemListAsync(".");
foreach (var entry in entries)
{
    Console.WriteLine(entry.Name);
}

await using var destination = File.Create("download.bin");
await client.ReadAsync("backups/latest.bin", destination);

await client.UnMountDeviceAsync();
```

## Use the direct NFSv3 client

Applications that need direct mounted-client APIs can use `NfsV3Client`:

```csharp
using NfsSharp.Client;
using NfsSharp.Protocol;

var options = new NfsClientOptions
{
    UserId = 1000,
    GroupId = 1000,
    UsePrivilegedSourcePort = false,
    CommandTimeout = TimeSpan.FromSeconds(30)
};

await using var client = await NfsV3Client.ConnectAsync(
    "nfs.example.internal",
    "/srv/data",
    options,
    CancellationToken.None);

var attributes = await client.GetAttributesAsync("documents/report.pdf", CancellationToken.None);
Console.WriteLine($"{attributes.Size} bytes");
```

## Verify compatibility

NFS behavior depends on the server implementation, export policy, identity mapping, firewall, rpcbind, and mountd configuration. Review the [NFS compatibility matrix](nfs-compatibility.md) before relying on a behavior in production.
