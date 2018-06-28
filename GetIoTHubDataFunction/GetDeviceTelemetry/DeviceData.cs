using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GetDeviceTelemetry
{
    public class DeviceData
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "doc.schemaversion")]
        public int Docschemaversion { get; set; }

        [JsonProperty(PropertyName = "doc.schema")]
        public object Docschema { get; set; }

        [JsonProperty(PropertyName = "device.id")]
        public object DeviceId { get; set; }

        [JsonProperty(PropertyName = "device.msg.schema")]
        public object DeviceMsgSchema { get; set; }

        [JsonProperty(PropertyName = "data.schema")]
        public object DataSchema { get; set; }

        [JsonProperty(PropertyName = "device.msg.created")]
        public long DeviceMsgCreated { get; set; }

        [JsonProperty(PropertyName = "device.msg.received")]
        public long DeviceMsgReceived { get; set; }

        public Data Data { get; set; }
        public string _rid { get; set; }
        public string _self { get; set; }
        public string _etag { get; set; }
        public string _attachments { get; set; }
        public int _ts { get; set; }
    }

    public class Data
    {
        public float Temperature { get; set; }
        public string Temperature_unit { get; set; }
        public float Pressure { get; set; }
        public string Pressure_unit { get; set; }
        public float Speed { get; set; }
        public string Speed_unit { get; set; }
        public int Vibration { get; set; }
        public string Vibration_unit { get; set; }
        public float Humidity { get; set; }
        public string Humidity_unit { get; set; }
        public int FuelLevel { get; set; }
        public string FuelLevel_unit { get; set; }
        public float Coolant { get; set; }
        public string Coolant_unit { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int PartitionId { get; set; }
    }
}
