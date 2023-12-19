//#define DEFAULT_DATA_TYPE_DOUBLE
using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text;
using System.IO;
using System.Xml;
using System.Runtime.Serialization.Json;






#if DEFAULT_DATA_TYPE_DOUBLE
using vcompo_t = System.Double;
using vector3_t = OpenTK.Mathematics.Vector3d;
using vector4_t = OpenTK.Mathematics.Vector4d;
using matrix4_t = OpenTK.Mathematics.Matrix4d;
#else
using vcompo_t = System.Single;
using vector3_t = OpenTK.Mathematics.Vector3;
using vector4_t = OpenTK.Mathematics.Vector4;
using matrix4_t = OpenTK.Mathematics.Matrix4;
#endif


namespace Plotter.Serializer;

public static class JsonElementExtends
{
    public static bool GetBool(this JsonElement jo, string key, bool defaultValue)
    {
        JsonElement prop;

        if (!jo.TryGetProperty(key, out prop))
        {
            return defaultValue;
        }

        return prop.GetBoolean();
    }

    public static vcompo_t GetDouble(this JsonElement jo, string key, vcompo_t defaultValue)
    {
        JsonElement prop;

        if (!jo.TryGetProperty(key, out prop))
        {
            return defaultValue;
        }

        return (vcompo_t)prop.GetDouble();
    }
    public static string GetString(this JsonElement jo, string key, string defaultValue)
    {
        JsonElement prop;

        if (!jo.TryGetProperty(key, out prop))
        {
            return defaultValue;
        }

        return prop.GetString();
    }

    public static T GetEnum<T>(this JsonElement jo, string key, T defaultValue)
    {
        JsonElement prop;

        if (!jo.TryGetProperty(key, out prop))
        {
            return defaultValue;
        }

        int num = prop.GetInt32();

        try
        {
            return (T)Enum.ToObject(typeof(T), num);
        }
        catch (ArgumentException)
        {
            return defaultValue;
        }
    }
}

public static class JsonObjExtends
{
    public static string ToIndentedString(this JsonObject jo)
    {
        return convertToIndentedJson(jo.ToJsonString());
    }


    public static string convertToIndentedJson(string json)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(json);
        using (MemoryStream stream = new MemoryStream())
        using (XmlDictionaryWriter writer = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, true, true))
        using (XmlDictionaryReader reader = JsonReaderWriterFactory.CreateJsonReader(buffer, XmlDictionaryReaderQuotas.Max))
        {
            writer.WriteNode(reader, true);
            writer.Flush();
            return Encoding.UTF8.GetString(stream.ToArray());
        }
    }
}
