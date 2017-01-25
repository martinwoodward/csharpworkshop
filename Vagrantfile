# coding: utf-8
Vagrant.configure(2) do |config|
  config.vm.box = "rhel-server-7"
  # Source: https://github.com/martinwoodward/csharpworkshop
  #
  # Getting started on RedHat Enterprise Linux (RHEL) with vagrant
  #
  # To get started:
  # Download the cdk rhel-server vagrant box from https://access.redhat.com/downloads/content/293/ver=1/rhel---7/1.0.1/x86_64/product-software (you will need a Red Hat account)
  #
  # Add the box to vagrant
  # vagrant box add --name "rhel-server-7" rhel-server-libvirt-7.1-3.x86_64.box
  #
  # At the time of writing, the RPM packages for .NET Core are not publicly available. Therefore to install with yum please do the following:

  # update to latest, this assumes you are using vagrant-registration to register: https://github.com/projectatomic/adb-vagrant-registration
  config.vm.provision 'shell', inline: "sudo yum -y update"

  #     Install the dependencies:

  config.vm.provision 'shell', inline: "sudo yum install -y libicu libuuid libcurl openssl libunwind wget || :"

  # Download this alpha build (which is known good at the time of writing):

  config.vm.provision 'shell', inline: "wget https://dotnetcli.blob.core.windows.net/dotnet/beta/Binaries/1.0.0.001088/dotnet-centos-x64.1.0.0.001088.tar.gz"

  # Unpack the build somewhere (e.g. /opt/dotnet)

  config.vm.provision 'shell', inline: "sudo mkdir /opt/dotnet || :"
  config.vm.provision 'shell', inline: "sudo tar xf dotnet-centos-x64.1.0.0.001088.tar.gz -C /opt/dotnet"

  # Add /opt/dotnet/bin to your PATH and set DOTNET_HOME to /opt/dotnet. One way to do this is by adding the following to your .bashrc file.

  config.vm.provision 'shell', inline: "echo export PATH=$PATH:/opt/dotnet/bin >> /home/vagrant/.bashrc"
  config.vm.provision 'shell', inline: "echo export DOTNET_HOME=/opt/dotnet >> /home/vagrant/.bashrc"

end
