using System;
using System.Linq;
using System.Net.Http;
using Thinktecture.IdentityModel.Hawk.Client;
using Thinktecture.IdentityModel.Hawk.Core;
using Thinktecture.IdentityModel.Hawk.Core.Helpers;

namespace HawkClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string uri = "http://localhost:25532/api/values/";

            var credential = new Credential()
            {
                Id = "dh37fgj492je",
                Algorithm = SupportedAlgorithms.SHA256,
                User = "Steve",
                Key = Convert.FromBase64String("wBgvhp1lZTr4Tb6K6+5OQa1bL9fxK7j8wBsepjqVNiQ=")
            };

            // GET and POST using the Authorization header
            var options = new ClientOptions()
            {
                CredentialsCallback = () => credential,
                RequestPayloadHashabilityCallback = (r) => true,
                NormalizationCallback = (req) =>
                {
                    string name = "X-Request-Header-To-Protect";
                    return req.Headers.ContainsKey(name) ?
                                name + ":" + req.Headers[name].First() : null;
                }
            };

            var handler = new HawkValidationHandler(options);

            HttpClient client = HttpClientFactory.Create(handler);
            client.DefaultRequestHeaders.Add("X-Request-Header-To-Protect", "secret");

            var response = client.GetAsync(uri).Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            response = client.PostAsJsonAsync(uri, credential.User).Result;
            Console.WriteLine(response.Content.ReadAsStringAsync().Result);

            Console.Read();
        }
    }
}
