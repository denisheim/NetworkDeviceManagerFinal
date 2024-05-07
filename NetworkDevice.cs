using System.Net;

namespace NetworkDeviceManager;

abstract class NetworkDevice : IConnectable, IDisplayable
    {
        public string Name { get; }
        public IPAddress IPv4Address { get; }
        public IPAddress IPv6Address { get; }
        public Subnet Subnet { get; }
        public VLAN VLAN { get; }
        public Zone Zone { get; }

        protected List<NetworkDevice> Connections { get; }

        protected NetworkDevice(string name, Subnet subnet, Zone zone, VLAN vlan)
        {
            Name = name;
            IPv4Address = GenerateUniqueIPv4Address(subnet);
            IPv6Address = GenerateUniqueIPv6Address();
            Subnet = subnet;
            Zone = zone;
            VLAN = vlan;
            Connections = new List<NetworkDevice>();
        }

        public IEnumerable<NetworkDevice> GetConnections()
        {
            return Connections;
        }

        public void DisconnectFrom(NetworkDevice device)
        {
            Connections.Remove(device);
            device.Connections.Remove(this);
        }

        public void RemoveConnection(NetworkDevice device)
        {
            Connections.Remove(device);
        }

        public void ConnectTo(NetworkDevice device)
        {
            Connections.Add(device);
            device.Connections.Add(this);
        }

        private IPAddress GenerateUniqueIPv4Address(Subnet subnet)
        {
            Random rand = new Random();
            byte[] networkBytes = subnet.NetworkAddress.GetAddressBytes();
            int hostBits = 32 - subnet.PrefixLength;

            for (int i = 0; i < Math.Min(hostBits, networkBytes.Length); i++)
            {
                networkBytes[i] |= (byte)(rand.Next(0, 256) & (1 << (7 - i)));
            }

            return new IPAddress(networkBytes);
        }

        private IPAddress GenerateUniqueIPv6Address()
        {
            byte[] ipv6Bytes = new byte[16];
            Random rand = new Random();
            rand.NextBytes(ipv6Bytes);

            ipv6Bytes[0] = 0xFD;

            return new IPAddress(ipv6Bytes, 0);
        }

        public abstract string GetDisplayInfo();
    }