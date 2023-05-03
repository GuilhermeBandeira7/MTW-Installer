
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace UtilsCore
{
    public class Decoder
    {
        const int cpuMessageSize = 6;
        const string cpuMessageIndentifier = "TE01";
        const int ebMessageSize = 8;
        const string ebMessageIndentifier = "DO03";
        const int acMessageSize = 3;
        const string acMessageIndentifier = "AC03";

        public static object DecodeMessage(string message)
        {
            try
            {
                object response = new object();
                message = message.Substring(0, message.IndexOf('}') + 1);
                if (message.Contains("TYPE"))
                {
                    JObject data = JObject.Parse(message);
                    if (data["TYPE"].ToString() == "AC")
                    {
                        response = JsonConvert.DeserializeObject<AC>(message);
                        ((AC)response).TYPE = "AC2";
                    }
                    else if (data["TYPE"].ToString() == "EB")
                    {
                        response = JsonConvert.DeserializeObject<EB>(message);
                        ((EB)response).TYPE = "EB2";
                    }
                    else if (data["TYPE"].ToString() == "CPU")
                    {
                        response = JsonConvert.DeserializeObject<CPU>(message);
                        ((CPU)response).TYPE = "CPU2";
                    }
                }
                else
                {
                    if (message.Split(',').ToList().Count == cpuMessageSize)
                    {
                        if (message.Contains(cpuMessageIndentifier))
                        {
                            response = JsonConvert.DeserializeObject<CPU>(message);
                            ((CPU)response).TYPE = "CPU";
                        }

                    }

                    if (message.Split(',').ToList().Count == ebMessageSize)
                    {
                        if (message.Contains(ebMessageIndentifier))
                        {
                            response = JsonConvert.DeserializeObject<EB>(message);
                            ((EB)response).TYPE = "EB";
                        }

                    }

                    if (message.Split(',').ToList().Count == acMessageSize)
                    {
                        if (message.Contains(acMessageIndentifier))
                        {
                            response = JsonConvert.DeserializeObject<AC>(message);
                            ((AC)response).TYPE = "AC";
                        }

                    }
                }

                return response;
            }
            catch
            {
                return null;
            }
            
        }
    }
}
