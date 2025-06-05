using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {
        CloudStorageAccount StorageAccount { get; set; }
        CloudQueueClient Client { get; set; }
        CloudQueue Queue { get; set; }

        public override void Run()
        {
            while (true)
            {
                Thread.Sleep(10000);

                var message = Queue.GetMessage();
                if (message != null)
                {
                    Trace.WriteLine(message.AsString, "Message");

                    RoleInstance myIntEP = RoleEnvironment.Roles["WebRole1"].Instances[0];
                    string addressEP = myIntEP.InstanceEndpoints["UdpCheck"].IPEndpoint.ToString();
                    string IP = addressEP.Split(':').First();
                    int PORT = Int32.Parse(addressEP.Split(':').Last());
                    IPAddress targetIP = IPAddress.Parse(IP);
                    IPEndPoint ipEP = new IPEndPoint(targetIP, PORT);

                    using (UdpClient udpClient = new UdpClient())
                    {
                        XDocument resp = new XDocument(
                            new XElement("info",
                                new XElement("ip", this.GetCurrentIP()),
                                new XElement("dns", this.GetCurrentDNSName())
                            )
                        );
                        byte[] sendBytes = Encoding.ASCII.GetBytes(resp.ToString());
                        udpClient.Send(sendBytes, sendBytes.Length, ipEP);

                        Queue.DeleteMessage(message);
                    }
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;

            CloudStorageAccount StorageAccount = CloudStorageAccount.DevelopmentStorageAccount;

            Client = StorageAccount.CreateCloudQueueClient();
            Queue = Client.GetQueueReference("queueaddress");
            Queue.CreateIfNotExists();
            return base.OnStart();
        }

        public string GetCurrentIP()
        {
            IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress i in ips)
            {
                if (i.AddressFamily == AddressFamily.InterNetwork)
                    return i.ToString();
            }
            return "No IP address";
        }

        public string GetCurrentDNSName()
        {
            return Dns.GetHostName();
        }
    }
}