// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <Command.cs>
//  Created By: Alexey Golub
//  Date: 13/02/2016
// ------------------------------------------------------------------ 

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Modboy.Models.API
{
    public class Command
    {
        [JsonProperty("Command"), JsonConverter(typeof(StringEnumConverter))]
        public CommandType Type { get; set; }

        [JsonProperty("ErrorMessage")]
        public string ErrorMessage { get; set; }

        [JsonProperty("Parameters")]
        public JObject Parameters { get; set; }

        [JsonConstructor]
        public Command(CommandType type, string errorMessage, JObject parameters)
        {
            Type = type;
            Parameters = parameters;
            ErrorMessage = errorMessage;
        }

        public Command(CommandType type, JObject parameters)
        {
            Type = type;
            Parameters = parameters;
            ErrorMessage = null;
        }

        public Command(CommandType type, Dictionary<string, object> parameters)
        {
            Type = type;
            Parameters = JObject.FromObject(parameters);
        }

        /// <summary>
        /// Gets the parameter by name or default if it doesn't exist
        /// </summary>
        public T Parameter<T>(string name)
        {
            try
            {
                var value = Parameters.GetValue(name, StringComparison.InvariantCultureIgnoreCase);
                if (value == null || value.HasValues) return default(T);
                return value.Value<T>();
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Gets the parameter array by name or null if it doesn't exist
        /// </summary>
        public T[] ParameterArray<T>(string name)
        {
            try
            {
                var value = Parameters.GetValue(name, StringComparison.InvariantCultureIgnoreCase);
                if (value == null || !value.HasValues) return null;
                return value.Values<T>().ToArray();
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the parameter by name as a string
        /// </summary>
        public string Parameter(string name)
        {
            return Parameter<string>(name);
        }

        /// <summary>
        /// Gets the parameter array by name as a string array
        /// </summary>
        public string[] ParameterArray(string name)
        {
            return ParameterArray<string>(name);
        }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}