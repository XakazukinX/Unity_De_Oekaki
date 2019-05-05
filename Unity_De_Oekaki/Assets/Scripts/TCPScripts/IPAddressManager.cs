using System.Net.NetworkInformation;
using System.Net.Sockets;

public class IPAddressManager
{
    public static string GetIP(ADDRESSFAMILYTYPE addressfamilyType)
    {
        if (addressfamilyType == ADDRESSFAMILYTYPE.IPv6 && !Socket.OSSupportsIPv6)
        {
            return null;
        }

        string ret = "";
        
        //ローカルのネットワークを全部拾ってくる。
        NetworkInterface[] interfaces = new NetworkInterface[NetworkInterface.GetAllNetworkInterfaces().Length];
        interfaces = NetworkInterface.GetAllNetworkInterfaces();
        
        for (var i = 0; i < interfaces.Length; i++)
        {
            var item = interfaces[i];
            
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            NetworkInterfaceType _type1 = NetworkInterfaceType.Wireless80211;
            NetworkInterfaceType _type2 = NetworkInterfaceType.Ethernet;

            if ((item.NetworkInterfaceType == _type1 || item.NetworkInterfaceType == _type2) &&
                item.OperationalStatus == OperationalStatus.Up)
#endif
            {
                for (var index = 0; index < item.GetIPProperties().UnicastAddresses.Count; index++)
                {
                    UnicastIPAddressInformation ip = item.GetIPProperties().UnicastAddresses[index];
                    //IPv4
                    if (addressfamilyType == ADDRESSFAMILYTYPE.IPv4)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ret = ip.Address.ToString();
                        }
                    }

                    //IPv6
                    else if (addressfamilyType == ADDRESSFAMILYTYPE.IPv6)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetworkV6)
                        {
                            ret = ip.Address.ToString();
                        }
                    }
                }
            }
        }

        return ret;
    }
}

public enum ADDRESSFAMILYTYPE
{
    IPv4, IPv6
}