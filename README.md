# crossroads-net &mdash; Crossroads I/O Bindings for .NET and Mono

This project aims to provide the full functionality of the underlying Crossroads I/O API to .NET projects.

Bundled libxs version: **1.0.1 (stable)**  

## Getting Started

The quickest way to get started with crossroads-net is by using the [NuGet package][crossroads-net-nuget]. The NuGet packages include a copy of the native libxs.dll, which is required to use crossroads-net.

You may also build crossroads-net directly from the source. See the Development Environment Setup instructions below for more detail.

To get an idea of how to use crossroads-net, have a look at the following example.

### Example server

```c#
using System;
using System.Text;
using System.Threading;
using CrossroadsIO;

namespace ServerExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Context, server socket
            using (Context context = Context.Create())
            using (Socket server = context.CreateSocket(SocketType.REP))
            {
                socket.Bind("tcp://*:5555");
                
                while (true)
                {
                    // Wait for next request from client
                    string message = server.Receive(Encoding.Unicode);
                    Console.WriteLine("Received request: {0}", message);

                    // Do Some 'work'
                    Thread.Sleep(1000);

                    // Send reply back to client
                    server.Send("World", Encoding.Unicode);
                }
            }
        }
    }
}
```

### Example client

```c#
using System;
using System.Text;
using CrossroadsIO;

namespace ClientExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Context and client socket
            using (Context context = Context.Create())
            using (Socket client = context.CreateSocket(SocketType.REQ))
            {
                client.Connect("tcp://localhost:5555");

                string request = "Hello";
                for (int requestNum = 0; requestNum < 10; requestNum++)
                {
                    Console.WriteLine("Sending request {0}...", requestNum);
                    client.Send(request, Encoding.Unicode);

                    string reply = client.Receive(Encoding.Unicode);
                    Console.WriteLine("Received reply {0}: {1}", requestNum, reply);
                }
            }
        }
    }
}
```

Crossroads I/O is a fork of [ZeroMQ][zeromq], so more C# examples can be found in the [ZeroMQ Guide][zmq-guide] or in the [examples repository][zmq-example-repo]. Tutorials and API documentation specific to crossroads-net are on the way.

## Development Environment

On Windows/.NET, crossroads-net is developed with Visual Studio 2010. Mono development is done with MonoDevelop 2.8+.

### Windows/.NET

crossroads-net depends on `libxs.dll`, which will be retrieved automatically via NuGet. If you require a specific version of libxs, you can compile it from the [libxs sources][libxs].

#### crossroads-net

1. Clone the source.
2. Run `build.cmd` to build the project and run the test suite.
3. The resulting binaries will be available in `/build`.

#### Alternate libxs (optional)

If you want to use a custom build of `libxs.dll`, perform the following steps:

1. Delete or rename the `src/CrossroadsIO/packages.config` file. This will prevent the NuGet package from being retrieved.
2. Remove any folders matching `src/packages/libxs-*` that may have been downloaded previously.
3. Copy the 32-bit and 64-bit (if applicable) build of `libxs.dll` to `lib/x86` and `lib/x64`, respectively.

Note that PGM-related tests will fail if a non-PGM build of libxs is used.

### Mono

**NOTE**: Mono 2.10.7+ is required **for development only**, as the NuGet scripts and executables require this version to be present.
If you choose to install dependencies manually, you may use any version of Mono 2.6+.

#### Mono 2.10.7+ configuration

NuGet relies on several certificates to be registered with Mono. The following is an example terminal session (on Ubuntu) for setting this up correctly.
This assumes you have already installed Mono 2.10.7 or higher.

```shell
$ mozroots --import --sync

$ certmgr -ssl https://go.microsoft.com
$ certmgr -ssl https://nugetgallery.blob.core.windows.net
$ certmgr -ssl https://nuget.org
```

This should result in a working Mono setup for use with NuGet.

#### libxs

Either clone the [libxs repository][libxs] or [download the sources][xs-dl], and then follow the build/install instructions for your platform.
Use the `--with-pgm` option if possible.

#### crossroads-net

1. Clone the source.
2. Run `nuget.sh`, which downloads any dependent packages (e.g., Machine.Specifications for acceptance tests).
3. Run `make` to build the project.
4. The resulting binaries will be available in `/build`.

**NOTE**: The combination of P/Invoke, MSpec, and Mono currently has issues, so the test suite does not automatically run.  
**NOTE**: `crossroads-net` only supports x86 builds on Mono at this time

## Issues

Issues should be logged on the [GitHub issue tracker][issues] for this project.

When reporting issues, please include the following information if possible:

* Version of crossroads-net and/or how it was obtained (compiled from source, NuGet package)
* Version of libxs being used
* Runtime environment (.NET/Mono and associated version)
* Operating system and platform (Win7/64-bit, Linux/32-bit)
* Code snippet demonstrating the failure

## Contributing

Pull requests and patches are always appreciated! To speed up the merge process, please follow the guidelines below when making a pull request:

* Create a new branch in your fork for the changes you intend to make. Working directly in master can often lead to unintended additions to the pull request later on.
* When appropriate, add to the AcceptanceTests project to cover any new functionality or defect fixes.
* Ensure all previous tests continue to pass (with exceptions for PGM tests)
* Follow the code style used in the rest of the project. ReSharper and StyleCop configurations have been included in the source tree.

Pull requests will still be accepted if some of these guidelines are not followed: changes will just take longer to merge, as the missing pieces will need to be filled in.

## License

This project is released under the [LGPL][lgpl] license, as is the native libxs library. See LICENSE for more details as well as the [Crossroads I/O Licensing][xs-license] page.

[crossroads-net-nuget]: http://packages.nuget.org/Packages/crossroads-net
[libxs]: https://github.com/crossroads-io/libxs
[zmq-guide]: http://zguide.zeromq.org/page:all
[zmq-example-repo]: https://github.com/imatix/zguide/tree/master/examples/C%23
[xs-dl]: http://www.crossroads.io/download
[xs-license]: http://www.crossroads.io/dev:legal
[zeromq]: http://www.zeromq.org/
[issues]: https://github.com/jgoz/crossroads-net/issues
[lgpl]: http://www.gnu.org/licenses/lgpl.html
