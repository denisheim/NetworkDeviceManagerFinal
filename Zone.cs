namespace NetworkDeviceManager;

class Zone : IDisplayable
{
    public string Name { get; }

    public Zone(string name)
    {
        Name = name;
    }

    public string GetDisplayInfo()
    {
        return Name;
    }
}