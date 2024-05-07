using System.Text;

namespace NetworkDeviceManager;

class Network
    {
        private List<Subnet> subnets;
        private List<VLAN> vlans;
        private List<Zone> zones;
        private List<NetworkDevice> devices;

        public Network()
        {
            subnets = new List<Subnet>();
            vlans = new List<VLAN>();
            zones = new List<Zone>();
            devices = new List<NetworkDevice>();
        }

        public bool ValidateNetwork()
        {
            // Validate devices are connected to existing subnets, VLANs, and zones
            foreach (var device in devices)
            {
                if (device.VLAN == null) continue;

                if (!subnets.Contains(device.Subnet))
                {
                    Console.WriteLine($"Error: Device {device.Name} is not connected to a valid subnet.");
                    return false;
                }
                
                if (!vlans.Contains(device.VLAN))
                {
                    Console.WriteLine($"Error: Device {device.Name} is not connected to a valid VLAN.");
                    return false;
                }

                if (!zones.Contains(device.Zone))
                {
                    Console.WriteLine($"Error: Device {device.Name} is not connected to a valid zone.");
                    return false;
                }

                // Check for unique device names
                if (devices.Count(d => d.Name == device.Name) > 1)
                {
                    Console.WriteLine($"Error: Duplicate device name found - {device.Name}.");
                    return false;
                }

                // Check for self-connection
                if (device.GetConnections().Contains(device))
                {
                    Console.WriteLine($"Error: Device {device.Name} is connected to itself.");
                    return false;
                }

                // Check for devices connected to multiple VLANs
                if (device is IConnectable connectableDevice && device.VLAN != null)
                {
                    var connectedDevicesInSameVLAN = device.GetConnections()
                        .Where(d => d.VLAN == device.VLAN)
                        .ToList();

                    if (connectedDevicesInSameVLAN.Count > 1)
                    {
                        Console.WriteLine($"Error: Device {device.Name} is connected to multiple devices in VLAN {device.VLAN}.");
                        return false;
                    }
                }
            }

            Console.WriteLine("Network configuration is valid.");
            return true;
        }

        public NetworkDevice GetDeviceByName(string deviceName)
        {
            return devices.Find(d => d.Name == deviceName);
        }

        public List<NetworkDevice> GetDevicesInSubnet(Subnet subnet)
        {
            return devices.FindAll(d => d.Subnet == subnet);
        }

        public List<NetworkDevice> GetDevicesInVLAN(VLAN vlan)
        {
            return devices.FindAll(d => d.VLAN == vlan);
        }

        public List<NetworkDevice> GetDevicesInZone(Zone zone)
        {
            return devices.FindAll(d => d.Zone == zone);
        }




        public void RemoveDevice(NetworkDevice device)
        {
            devices.Remove(device);

            // Remove the device from connections of other devices
            foreach (var otherDevice in devices)
            {
                otherDevice.RemoveConnection(device);
            }
        }

        public void DisconnectDevices(NetworkDevice device1, NetworkDevice device2)
        {
            device1.DisconnectFrom(device2);
            device2.DisconnectFrom(device1);
        }

        public void DisplayDeviceInfo(string deviceName)
        {
            NetworkDevice device = devices.Find(d => d.Name.Equals(deviceName, StringComparison.OrdinalIgnoreCase));

            if (device != null)
            {
                Console.WriteLine($"Detailed Information for Device {device.Name}:\n{device.GetDisplayInfo()}\n");
            }
            else
            {
                Console.WriteLine($"Device {deviceName} not found.\n");
            }
        }

        public void DisplayDevicesInSubnet(Subnet subnet)
        {
            List<NetworkDevice> devicesInSubnet = devices.Where(d => d.Subnet == subnet).ToList();

            if (devicesInSubnet.Any())
            {
                Console.WriteLine($"Devices in Subnet {subnet}:");

                foreach (var device in devicesInSubnet)
                {
                    Console.WriteLine(device.GetDisplayInfo());
                }

                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"No devices found in Subnet {subnet}.\n");
            }
        }

        public void DisplayDevicesInVLAN(VLAN vlan)
        {
            List<NetworkDevice> devicesInVLAN = devices.Where(d => d.VLAN == vlan).ToList();

            if (devicesInVLAN.Any())
            {
                Console.WriteLine($"Devices in VLAN {vlan}:");

                foreach (var device in devicesInVLAN)
                {
                    Console.WriteLine(device.GetDisplayInfo());
                }

                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"No devices found in VLAN {vlan}.\n");
            }
        }

        public void DisplayDevicesInZone(Zone zone)
        {
            List<NetworkDevice> devicesInZone = devices.Where(d => d.Zone == zone).ToList();

            if (devicesInZone.Any())
            {
                Console.WriteLine($"Devices in Zone {zone}:");

                foreach (var device in devicesInZone)
                {
                    Console.WriteLine(device.GetDisplayInfo());
                }

                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"No devices found in Zone {zone}.\n");
            }
        }


        public void AddSubnet(Subnet subnet)
        {
            subnets.Add(subnet);
        }

        public void AddVLAN(VLAN vlan)
        {
            vlans.Add(vlan);
        }

        public void AddZone(Zone zone)
        {
            zones.Add(zone);
        }

        public void AddDevice(NetworkDevice device)
        {
            devices.Add(device);
        }

        public void ConnectDevices(NetworkDevice device1, NetworkDevice device2)
        {
            device1.ConnectTo(device2);
        }

        static string GetDeviceListInfo(List<NetworkDevice> devices)
        {
            StringBuilder info = new StringBuilder();
            foreach (var device in devices)
            {
                info.AppendLine(device.GetDisplayInfo());
            }
            return info.ToString();
        }

        public void DisplayGraph()
        {
            Console.WriteLine("Network Graph:");

            foreach (var subnet in subnets)
            {
                DisplayDevices("Subnet", subnet, devices.FindAll(d => d.Subnet == subnet));
            }

            foreach (var vlan in vlans)
            {
                DisplayDevices("VLAN", vlan, devices.FindAll(d => d.VLAN == vlan));
            }

            foreach (var zone in zones)
            {
                DisplayDevices("Zone", zone, devices.FindAll(d => d.Zone == zone));
            }

            Console.WriteLine("Connections:");

            foreach (var device in devices)
            {
                Console.WriteLine($"{device.Name} is connected to:");

                foreach (var connectedDevice in device.GetConnections())
                {
                    Console.WriteLine($"   {connectedDevice.Name}");
                }

                Console.WriteLine();
            }
        }

        private void DisplayDevices(string category, object key, List<NetworkDevice> devices)
        {
            Console.WriteLine($"{category} {key} Devices:");
            foreach (var device in devices)
            {
                if (device is IDisplayable displayableDevice)
                {
                    Console.WriteLine(displayableDevice.GetDisplayInfo());
                }
                else
                {
                    Console.WriteLine(device);
                }
            }
            Console.WriteLine();
        }
    }