# API Overview

NfsSharp is split into small packages so applications can depend on the layer they need.

| Package | Main area |
| --- | --- |
| [`NfsSharp.Client`](https://www.nuget.org/packages/NfsSharp.Client) | High-level NFSv3 facade, direct NFSv3 client APIs, and experimental direct NFSv4 COMPOUND APIs. |
| [`NfsSharp.Protocol`](https://www.nuget.org/packages/NfsSharp.Protocol) | XDR primitives, ONC RPC helpers, NFSv3/NFSv4 models, status codes, and RPCSEC_GSS abstractions. |

The recommended entry point for applications is `NfsSharp.Client.NfsClient`, configured with `NfsClientBuilder`. Use `NfsV3Client` when you need direct mounted-client operations such as explicit handles, offset-based reads and writes, COMMIT, FSSTAT, FSINFO, PATHCONF, or ACCESS.

## Client layer

`NfsSharp.Client` contains:

| Type | Purpose |
| --- | --- |
| `NfsClientBuilder` | Fluent configuration for credentials, privileged source ports, timeouts, retry behavior, transfer sizes, directory caching, TCP options, logging, and RPCSEC_GSS hooks. |
| `NfsClient` | Stateful convenience facade for connect, export listing, mount, directory traversal, file I/O, metadata, links, mutations, and unmount. |
| `NfsV3Client` | Direct NFSv3 mounted client for export-relative and file-handle-oriented operations. |
| `NfsV4Client` | Experimental direct COMPOUND-oriented NFSv4.0, NFSv4.1, and NFSv4.2 surface. |

NFSv3 over TCP is the primary supported protocol. The NFSv4 surface is experimental and does not yet carry the same compatibility and integration guarantees.

## Protocol layer

`NfsSharp.Protocol` contains reusable protocol contracts and primitives, including:

- XDR encoding and decoding helpers.
- NFSv3 and NFSv4 model types.
- NFS status codes and exception mapping.
- AUTH_SYS options shared by client APIs.
- RPCSEC_GSS extension contracts for authentication, integrity, and privacy work.

Use the protocol package directly for protocol tooling, tests, or integrations that do not need the high-level client package.
