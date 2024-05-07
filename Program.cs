namespace NetworkDeviceManager
{
    interface IConnectable
    {
        void ConnectTo(NetworkDevice device);
        void DisconnectFrom(NetworkDevice device);
    }

    interface IDisplayable
    {
        string GetDisplayInfo();
    }

    class Program
    {
        static void Main()
        {
            Network network = new Network();

            Subnet subnet1 = new Subnet("192.168.1.0", 24);
            Subnet subnet2 = new Subnet("192.168.2.0", 24);
            Subnet subnet3 = new Subnet("10.0.0.0", 24);

            VLAN vlan1 = new VLAN("VLAN1");
            VLAN vlan2 = new VLAN("VLAN2");

            Zone zone1 = new Zone("Zone1");
            Zone zone2 = new Zone("Zone2");

            network.AddSubnet(subnet1);
            network.AddSubnet(subnet2);
            network.AddSubnet(subnet3);

            network.AddVLAN(vlan1);
            network.AddVLAN(vlan2);

            network.AddZone(zone1);
            network.AddZone(zone2);

            PC pc1 = new PC("PC1", subnet1, zone1, vlan1);
            PC pc2 = new PC("PC2", subnet1, zone1, vlan1);
            RPi rpi1 = new RPi("RPi1", subnet2, zone2, vlan2);
            Switch switch1 = new Switch("Switch1");
            Router router1 = new Router("Router1", subnet3, zone1);

            network.AddDevice(pc1);
            network.AddDevice(pc2);
            network.AddDevice(rpi1);
            network.AddDevice(switch1);
            network.AddDevice(router1);

            network.ConnectDevices(pc1, switch1);
            network.ConnectDevices(pc2, switch1);
            network.ConnectDevices(switch1, router1);
            network.ConnectDevices(rpi1, router1);

            network.DisplayGraph();
        }
    }
}
