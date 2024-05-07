using System.Net;

namespace NetworkDeviceManager;

class Subnet
{
    public IPAddress NetworkAddress { get; }
    public int PrefixLength { get; }

    public Subnet(string networkAddress, int prefixLength)
    {
        NetworkAddress = IPAddress.Parse(networkAddress);
        PrefixLength = prefixLength;
    }

    public override string ToString()
    {
        return $"{NetworkAddress}/{PrefixLength}";
    }
}