#Getting Started with C# on Linux
This file is part of a multi-series workshop on learning C# on Linux available [here](../README.md).

First of all we want to get .NET Core installed on your environment.
For instructions see http://dotnet.github.io/getting-started/ and follow the instructions for
the platform of your choice.

## Getting started on RedHat Enterprise Linux, Fedora or CentOS

At the time of writing, the RPM packages for .NET Core are not publicly available. Therefore to install
with ```yum``` please do the following:

 1. Install the dependencies:
``` 
 sudo yum install –y libicu libuuid libcurl openssl libunwind
```
 1. Download this alpha build (which is known good at the time of writing):
```
 wget https://dotnetcli.blob.core.windows.net/dotnet/beta/Binaries/1.0.0.001088/dotnet-centos-x64.1.0.0.001088.tar.gz
```
 1. Unpack the build somewhere (e.g. /opt/dotnet)
```
 sudo mkdir /opt/dotnet
 sudo tar xf dotnet-centos-x64.1.0.0.001088.tar.gz -C /opt/dotnet
``` 
 1. Add `/opt/dotnet/bin` to your `PATH` and set `DOTNET_HOME` to `/opt/dotnet`. One way
    to do this is by adding the following to your `.bashrc` file.
```
 export PATH=$PATH:/opt/dotnet/bin
 export DOTNET_HOME=/opt/dotnet
```

## Getting started on Ubuntu & Debian based distributions

See http://dotnet.github.io/getting-started/ for the latest instructions, but essentially:

 1. Add the dotnet apt-get feed
```
sudo sh -c 'echo "deb [arch=amd64] http://apt-mo.trafficmanager.net/repos/dotnet/ trusty main" > /etc/apt/sources.list.d/dotnetdev.list'
sudo apt-key adv --keyserver apt-mo.trafficmanager.net --recv-keys 417A0893
sudo apt-get update
```
 1. Install .NET Core
```
sudo apt-get install dotnet-nightly
```

## Conclusion
That's it - you should have dotnet installed on your system. You should be able to 
type `dotnet --version` to verify. Next, lets create a new application 
and learn about the structure of a C# program.

---
 - Next: [Tutorial 2 - Hello C# World](../002-Hello-CSharp/)
 - Back to [Table of Contents](../README.md)
