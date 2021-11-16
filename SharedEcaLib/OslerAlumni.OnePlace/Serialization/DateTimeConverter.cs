using Newtonsoft.Json;
using System;
using System.Linq;

namespace OslerAlumni.OnePlace.Serialization
{
    public class DateTimeConverter : JsonConverter
    {
        public static readonly Type[] _types =
        {
            typeof(DateTime),
            typeof(DateTime?)
        };

        public override bool CanRead
            => false;

        public override bool CanConvert(Type objectType)
        {
            return _types.Contains(objectType);
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException(
                "Unnecessary because CanRead is false. The type will skip the converter.");
        }

        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            // The conversion of MinValue to null should kick in only when converting to JSON for sending to Salesforce
            if (serializer.ContractResolver.GetType() == typeof(CustomContractResolver))
            {
                writer.WriteValue(value);

                return;
            }

            var dt = value as DateTime?;

            if (dt.HasValue)
            {
                if (dt.Value > DateTime.MinValue)
                {
                    // Converting to Universal Time is important, since OnePlace might be in a different time zone
                    writer.WriteValue(dt.Value.ToUniversalTime());
                }
                else
                {
                    // Treat MinValue as null - patching Salesforce with null works, 
                    // but can't use the null value, because of NullValueHandling setting (which is correctly set to Ignore).
                    // Changing NullValueHandling to Include would defeat the whole purpose of using PATCH instead of PUT
                    writer.WriteNull();
                }
            }
        }
    }
}
