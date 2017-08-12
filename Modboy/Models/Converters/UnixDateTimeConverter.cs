// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <UnixDateTimeConverter.cs>
//  Created By: Alexey Golub
//  Date: 18/08/2016
// ------------------------------------------------------------------ 

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Modboy.Models.Converters
{
    public class UnixDateTimeConverter : DateTimeConverterBase
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(((DateTime) value - Epoch).TotalSeconds + "");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) { return null; }
            return Epoch.AddSeconds((long) reader.Value);
        }
    }
}