using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Persistence.Infrastructure;

public class PrivateResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(
        MemberInfo member, 
        MemberSerialization memberSerialization)
    {
        var jsonProperty = base.CreateProperty(member, memberSerialization);
        if (!jsonProperty.Writable)
        {
            var propertyInfo = member as PropertyInfo;
            if (propertyInfo is not null)
            {
                var hasPrivateSetter = propertyInfo.GetSetMethod(true) != null;
                jsonProperty.Writable = hasPrivateSetter;
            }
        }
        return jsonProperty;
    }
}