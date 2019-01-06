using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace MazeSolution.MazeServerConnection
{
    public class MazeConnection : IMazeServerConnection
    {
        private const string ServiceUrl = "http://localhost:3000";

        public void Move(DirectionEnum direction)
        {
            ExecuteMethod("/move", "application/json", "POST", direction.ToString());
        }

        public void Reset()
        {
            ExecuteMethod("/reset", "application/json", "POST");
        }

        public AvailableDirections GetDirections() =>
            JsonConvert.DeserializeObject<AvailableDirections>(
                ExecuteMethod("/directions", "application/json", "GET"));

        public PositionResponse GetPosition() =>
            JsonConvert.DeserializeObject<PositionResponse>(
                ExecuteMethod("/position", "application/json", "GET"));

        public StatusResponse GetStatus() =>
            JsonConvert.DeserializeObject<StatusResponse>(
                ExecuteMethod("/state", "application/json", "GET"));

        private static string ExecuteMethod(string method, string contentType, string httpRequestMethod, string param = null)
        {
            var httpRequest = (HttpWebRequest) WebRequest.Create(new Uri(ServiceUrl + method));
            httpRequest.Accept = contentType;
            httpRequest.ContentType = contentType;
            httpRequest.Method = httpRequestMethod;

            if (!string.IsNullOrEmpty(param))
            {
                var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(param));

                using (var stream = httpRequest.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Close();
                }
            }

            using (var httpResponse = (HttpWebResponse) httpRequest.GetResponse())
            {
                if (httpResponse.StatusCode != HttpStatusCode.OK) throw new Exception();
                
                using (var stream = httpResponse.GetResponseStream())
                {
                    return new StreamReader(stream ?? throw new NullReferenceException()).ReadToEnd();
                }
            }
        }
    }
}