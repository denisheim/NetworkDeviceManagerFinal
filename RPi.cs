namespace NetworkDeviceManager;

class RPi : NetworkDevice
{
    public RPi(string name, Subnet subnet, Zone zone, VLAN vlan) : base(name, subnet, zone, vlan) { }

    public override string GetDisplayInfo()
    {
        return $"{GetType().Name} - {Name}\n   IPv4: {IPv4Address}\n   IPv6: {IPv6Address}\n   Subnet: {Subnet}\n   VLAN: {VLAN}\n   Zone: {Zone}";
    }
}