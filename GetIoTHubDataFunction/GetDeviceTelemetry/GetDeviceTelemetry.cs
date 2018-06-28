
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Azure.Documents.Client;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.Azure.Documents;
using System.Collections.Generic;
using Microsoft.Azure.Documents.Linq;

namespace GetDeviceTelemetry
{
    public static class GetDeviceTelemetry
    {

        //private static DocumentClient client;
        [FunctionName("GetDeviceTelemetry")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string deviceId = req.Query["deviceId"];

            if (String.IsNullOrEmpty(deviceId))
            {
                return new BadRequestObjectResult($"deviceId is missing from query parameters");
            }

            var device = await GetDocumentAsync(deviceId);
            return new OkObjectResult(device);
        }

        private static async Task<DeviceData> GetDocumentAsync(string deviceId)
        {
            var documentClientDetails = await DocumentDbClientFactory.GetDeviceClientAsync();

            // Query to get latest telemetry for specific device
            string query = "SELECT TOP 1 * FROM c WHERE c['device.id'] = '"+ deviceId +"' ORDER BY c['device.msg.created'] DESC";

            SqlQuerySpec sqlQuerySpec = new SqlQuerySpec(query);

            var documentQuery = documentClientDetails.DocumentClient.CreateDocumentQuery<DeviceData>(documentClientDetails.DocumentCollectionLink, sqlQuerySpec)
                                .AsDocumentQuery();

            var result = new List<DeviceData>();
            while (documentQuery.HasMoreResults)
            {
                result.AddRange(await documentQuery.ExecuteNextAsync<DeviceData>());
            }

            Console.WriteLine(result[0]);
            return result[0];

        }
    }
}
