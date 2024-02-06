using System.Text;
using Microsoft.VisualBasic;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;

public class MQTTInterface
{
    #region setup
    public (IMqttClient, MqttClientOptions) InitMQTT(string brokerUri, int port)
    {
        var mqttFactory = new MqttFactory();
        IMqttClient client = mqttFactory.CreateMqttClient();
        var options = new MqttClientOptionsBuilder()
        .WithClientId(Guid.NewGuid().ToString())
        .WithTcpServer(brokerUri, port)
        .WithCleanSession()
        .Build();

        return (client, options);
    }
    #endregion

    #region connect
    public bool MQTTConnection(IMqttClient client, MqttClientOptions options)
    {
        client.ConnectAsync(options).Wait();
        if (client.IsConnected)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region subscribe
    public async Task SubsHandler(IMqttClient mqttClient, MqttClientOptions mqttClientOptions, string topic)
    {
        // Console.WriteLine("Connected to MQTT broker successfully.");
        await mqttClient.SubscribeAsync(topic);
        // Callback function when a message is received
        mqttClient.ApplicationMessageReceivedAsync += e =>
        {
            Console.WriteLine($"Received message => {Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment)}");
            return Task.CompletedTask;
        };
    }
    #endregion

    #region publish
    public async Task PubHandler(IMqttClient mqttClient, MqttQualityOfServiceLevel qosLevel, string topic, string payload)
    {

        var message = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .WithQualityOfServiceLevel(qosLevel)
            .Build();

        await mqttClient.PublishAsync(message);
    }
    #endregion

    #region unsub-disconnect
    public async Task MQTTDisconnect(IMqttClient client, string topic)
    {
        // Unsubscribe and disconnect
        await client.UnsubscribeAsync(topic);
        await client.DisconnectAsync();
    }
    #endregion
}