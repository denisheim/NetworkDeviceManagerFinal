namespace NetworkDeviceManager;

class VLAN : IDisplayable
{
    public string Name { get; }

    public VLAN(string name)
    {
        Name = name;
    }

    public string GetDisplayInfo()
    {
        return Name;
    }
}