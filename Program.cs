using CRI_Client;
using System;
using Newtonsoft;
using MQTTnet.Client;
using MQTTnet;
using MQTTnet.Protocol;

namespace robot_be_hcm;

class Program
{
    static async Task Main(string[] args)
    {
        #region robot

        HardwareProtocolClient itf = new();
        string ipAddress = "192.168.3.11";
        // robot task running:
        // connectCRI
        itf.SetIPAddress(ipAddress);
        if (!itf.GetConnectionStatus())
        {
            itf.Connect();
            Console.WriteLine("Interface: Connecting...");
        }
        else
        {
            Console.WriteLine("Interface: already connected");
        }
        // check connection status
        bool connectionStatus = itf.GetConnectionStatus();
        if (connectionStatus)
        {
            // try to ping robot
            bool statusPing = itf.Ping();
            if (statusPing)
            {
                // start robot control
                bool isStartRobotControl = itf.StartRobotControl();
                if (isStartRobotControl)
                {
                    itf.SendCommand(string.Format("CMD Move Joint {0} {1} {2} {3} {4} {5} 0 0 0 {6}", 5.0, 0.0, 5.7, 2.8, 16.4, 0.0, 10));
                    itf.SendCommand("CMD Move Stop");
                    itf.SendCommand(string.Format("CMD Move Joint {0} {1} {2} {3} {4} {5} 0 0 0 {6}", 10.0, 2.0, 5.7, 2.8, 13.4, 0.0, 10));
                    itf.SendCommand("CMD Move Stop");
                    itf.SendCommand(string.Format("CMD Move Joint {0} {1} {2} {3} {4} {5} 0 0 0 {6}", 7.0, 0.0, 8.7, 5.8, 16.4, 0.0, 15));
                    itf.SendCommand("CMD Move Stop");
                    itf.SendCommand(string.Format("CMD Move Joint {0} {1} {2} {3} {4} {5} 0 0 0 {6}", 10.0, 5.0, 2.7, 2.8, 13.4, 0.0, 10));
                    itf.SendCommand("CMD Move Stop");
                }
            }
        }
        else
        {
            // disconnect
            itf.Disconnect();
            // stopCRI
            itf.StopCRIClient();
        }
        #endregion

        // #region mqtt
        // MQTTInterface mqttInterface = new();
        // string topic = "/robot/mqtt";

        // (IMqttClient client, MqttClientOptions options) = mqttInterface.InitMQTT(brokerUri: "10.0.2.15", port: 1887);
        // bool connStatus = mqttInterface.MQTTConnection(client, options);
        // Console.WriteLine(connStatus);
        // while (connStatus)
        // {
        //     await mqttInterface.SubsHandler(client, options, topic);
        //     await Task.Delay(1000);
        // }

        // // await mqttInterface.MQTTDisconnect(client, topic);
        // await client.UnsubscribeAsync(topic);
        // await client.DisconnectAsync();
        // #endregion
    }
}
