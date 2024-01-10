
using System.Net;
using System.Net.NetworkInformation;

namespace NetWorking1
{
    class Program
    {
static void Main(string[] args)
        {
            //lop URI
            //string url = "https://xuanthulab.net/lap-trinh/csharp/?page=3#acff";
            //var uri = new Uri(url);
            //var uritype = typeof(Uri);
            //uritype.GetProperties().ToList().ForEach(property =>
            //{
            //    Console.WriteLine($"{property.Name,15} {property.GetValue(uri)}");
            //});
            //Console.WriteLine($"Segments: {string.Join(",", uri.Segments)}");

            //lop tinh DNS
            //var hostname = Dns.GetHostName();
            //Console.WriteLine(hostname);
            var url = "https://www.google.com/";
            var uri = new Uri(url);
            Console.WriteLine(uri.Host);
           var ipHostEntry = Dns.GetHostEntry(uri.Host);
            Console.WriteLine(ipHostEntry.HostName);
            ipHostEntry.AddressList.ToList().ForEach(ip => Console.WriteLine(ip));

            //lop pig

            var ping = new Ping();
            var pingReply = ping.Send("www.google.com");
            Console.WriteLine(pingReply.Status);
            if (pingReply.Status == IPStatus.Success)
            {
                Console.WriteLine(pingReply.RoundtripTime);
                Console.WriteLine(pingReply.Address);
            }
        }
    }
}
