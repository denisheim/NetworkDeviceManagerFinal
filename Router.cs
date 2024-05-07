namespace NetworkDeviceManager;

class Router : NetworkDevice, IConnectable
{
    public Router(string name, Subnet subnet, Zone zone) : base(name, subnet, zone, new VLAN("Default")) { }

    public new void ConnectTo(NetworkDevice device)
    {
        base.ConnectTo(device);
    }

    public override string GetDisplayInfo()
    {
        return $"{GetType().Name} - {Name}\n   IPv4: {IPv4Address}\n   IPv6: {IPv6Address}\n   Subnet: {Subnet}\n   Zone: {Zone}";
    }
}