
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Azure.Devices;
using System.Collections.Generic;
using System;

namespace GetIoTHubDataFunction
{
    public static class GetDeviceData
    {
        [FunctionName("GetDeviceData")]
        public static async System.Threading.Tasks.Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string connectionUri = "HostName=iothub-4c2eo.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=ErZTfptovM9WxDJwgcOb8OTQ5zIl4tq5adOjNSRixto=";
            RegistryManager registryManager;
            registryManager = RegistryManager.CreateFromConnectionString(connectionUri);
            var query = registryManager.CreateQuery("SELECT * FROM devices", 100);
            List<DeviceDetails> deviceDetails = new List<DeviceDetails>();

            while (query.HasMoreResults)
            {
                var page = await query.GetNextAsTwinAsync();
                foreach (var twin in page)
                {
                    DeviceDetails device = new DeviceDetails();
                    device.ConnectionState = (DeviceConnectionState)twin.ConnectionState;
                    device.DeviceId = twin.DeviceId;
                    device.ETag = twin.ETag;
                    device.Status = (DeviceStatus)twin.Status;
                    device.Version = (long)twin.Version;
                    device.StatusUpdatedTime = (System.DateTime)twin.StatusUpdatedTime;
                    device.Properties = twin.Properties;
                    device.LastActivityTime = (System.DateTime)twin.LastActivityTime;

                    deviceDetails.Add(device);
                    Console.WriteLine(twin.DeviceId.ToString());
                }
            }

            string sendData = JsonConvert.SerializeObject(deviceDetails);

            return sendData != null
                ? (ActionResult)new OkObjectResult($"{sendData}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
