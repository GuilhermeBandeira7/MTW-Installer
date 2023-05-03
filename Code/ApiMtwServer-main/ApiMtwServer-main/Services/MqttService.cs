using EntityMtwServer.Entities;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using UtilsCore;

namespace ApiMtwServer.Services
{
    public class MqttService
    {
        MqttClient client;
        string clientId;
        string serverIp = "localhost";

        public MqttService()
        {
            client = new MqttClient(serverIp);
            client.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            client.ConnectionClosed += reconnect;
            clientId = Guid.NewGuid().ToString();
            client.Connect(clientId);
        }

        private void reconnect(object sender, EventArgs e)
        {
            CancellationTokenSource source = new CancellationTokenSource();
            Task T = Task.Run(() => TryReconnectAsync(source.Token));
            T.Wait();
        }

        private async Task TryReconnectAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("DISCONNECTED");
            var connected = client.IsConnected;
            while (!connected && !cancellationToken.IsCancellationRequested)
            {
                try
                {
                    Console.WriteLine("Trying to reconnect");
                    client.Connect(clientId);
                }
                catch (Exception exe)
                {
                    Console.WriteLine("FAILED:" + exe.Message);
                    Console.WriteLine("Trying again in 5 seconds");
                }
                connected = client.IsConnected;
                await Task.Delay(5000, cancellationToken);
            }

            Console.WriteLine("CONNECTED");
        }

        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {





        }

        public void SendTelemetryAlarm(Origin origin)
        {

            if (origin.Telemetry != null)
            {
                foreach (string input in origin.Telemetry.VirtualInputs.Split(';'))
                {
                    if(input != String.Empty)
                    {
                        VARIABLES var = GetVariableByInput(input);
                        MessageModel activateMessage = MessageController.Instance.CreateMessage(
                             new Dictionary<VARIABLES, List<byte>>() { { var, GetValueByVariableState(var, true) } },
                             GetBoardByInput(input),
                             MESSAGE_TYPE.KEEP_ALIVE);

                        MessageModel desactivateMessage = MessageController.Instance.CreateMessage(
                            new Dictionary<VARIABLES, List<byte>>() { { var, GetValueByVariableState(var, false) } },
                            GetBoardByInput(input),
                            MESSAGE_TYPE.KEEP_ALIVE);

                        string mqttTopicMessage = origin.Telemetry.SerialNumber + "/" + GetSlotByInput(input);
                        client.Publish(mqttTopicMessage, MessageController.Instance.EncodeMessage(activateMessage), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                        Thread.Sleep(5000);
                        client.Publish(mqttTopicMessage, MessageController.Instance.EncodeMessage(desactivateMessage), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                        Thread.Sleep(5000);
                    }
           
                }
            }
        }

        public BOARDS GetBoardByInput(string input)
        {
            if (input == "IN1" || input == "IN11" || input == "IN12")
                return BOARDS.CPU;
            else
                return BOARDS.EB;
        }

        public int GetSlotByInput(string input)
        {
            if (input == "IN1" || input == "IN11" || input == "IN12")
                return 0;
            else if (input == "IN2" || input == "IN3" || input == "IN4" || input == "IN13" || input == "IN14")
                return 2;
            else if (input == "IN5" || input == "IN6" || input == "IN7" || input == "IN15" || input == "IN16")
                return 3;
            else if (input == "IN8" || input == "IN9" || input == "IN10" || input == "IN17" || input == "IN18")
                return 4;

            return 0;
        }

        public VARIABLES GetVariableByInput(string input)
        {
            switch (input)
            {
                case "IN1":
                case "IN2":
                case "IN5":
                case "IN8":
                    return VARIABLES.DI01;

                case "IN3":
                case "IN6":
                case "IN9":
                    return VARIABLES.DI02;

                case "IN4":
                case "IN7":
                case "IN10":
                    return VARIABLES.DI03;

                case "IN11":
                case "IN13":
                case "IN15":
                case "IN17":
                    return VARIABLES.DC01;

                case "IN12":
                case "IN14":
                case "IN16":
                case "IN18":
                    return VARIABLES.DC02;

            }

            return VARIABLES.DI01;
        }

        public List<byte> GetValueByVariableState(VARIABLES var, bool state)
        {
            if (var == VARIABLES.DI01 || var == VARIABLES.DI02 || var == VARIABLES.DI03)
            {
                if(state)   
                    return new List<byte>() { 0x01};
                else
                    return new List<byte>() { 0x00 };
            }
            else if(var == VARIABLES.DC01 || var == VARIABLES.DC02)
            {
                if (state)
                    return new List<byte>() { 0x00, 0x10 };
                else
                    return new List<byte>() { 0x00, 0x00};
            }

            return new List<byte> { 0 };
        }
    }
}
