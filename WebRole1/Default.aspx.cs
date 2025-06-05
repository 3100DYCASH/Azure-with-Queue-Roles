using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Xml.Linq;

namespace WebRole1
{
    public partial class Default : System.Web.UI.Page
    {
        CloudStorageAccount StorageAccount { get; set; }
        CloudQueueClient Client { get; set; }
        CloudQueue Queue { get; set; }
        CloudQueueMessage Message { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            StorageAccount = CloudStorageAccount.Parse(
                RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString"));

            Client = StorageAccount.CreateCloudQueueClient();
            Queue = Client.GetQueueReference("queueaddress");
            Queue.CreateIfNotExists();
        }

        protected void GetInfo_Click(object sender, EventArgs e)
        {
            Message = new CloudQueueMessage("Give me info!");
            Queue.AddMessage(Message);

            RoleInstance myIntEP = RoleEnvironment.Roles["WorkerRole1"].Instances[0];
            string addressEP = myIntEP.InstanceEndpoints["MyIntEndpoint"].IPEndpoint.ToString();
            string IP = addressEP.Split(':').First();
            int PORT = Int32.Parse(addressEP.Split(':').Last());
            IPAddress targetIP = IPAddress.Parse(IP);
            IPEndPoint ipEP = new IPEndPoint(targetIP, PORT);

            RoleInstance myIntEP2 = RoleEnvironment.Roles["WebRole1"].Instances[0];
            string addressEP2 = myIntEP2.InstanceEndpoints["UdpCheck"].IPEndpoint.ToString();
            int PORT2 = Int32.Parse(addressEP2.Split(':').Last());
            string GetData = null;

            using (UdpClient socket = new UdpClient(PORT2))
            {
                try
                {
                    byte[] recData = socket.Receive(ref ipEP);
                    GetData = Encoding.ASCII.GetString(recData);
                    XDocument resp = XDocument.Parse(GetData);
                    this.IP_Label.Text = resp.Root.Element("ip").Value;
                    this.DNS_Label.Text = resp.Root.Element("dns").Value;
                }
                catch
                {
                    this.IP_Label.Text = "Error";
                    this.DNS_Label.Text = "Request failed";
                }
            }
        }
    }
}