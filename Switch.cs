namespace NetworkDeviceManager;

class Switch : NetworkDevice, IConnectable
{
    public Switch(string name) : base(name, new Subnet("0.0.0.0", 0), new Zone("Undefined"), new VLAN("Default")) { }

    public new void ConnectTo(NetworkDevice device)
    {
        base.ConnectTo(device);
    }

    public override string GetDisplayInfo()
    {
        return $"{GetType().Name} - {Name}\n   IPv4: {IPv4Address}\n   IPv6: {IPv6Address}\n   VLAN: {VLAN}\n   Zone: {Zone}";
    }
}