using System.Web.Script.Serialization;

namespace XRates.Classes
{
    public class GlobalHelper
    {
        public static string Json(object toSerialize)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer() { MaxJsonLength = int.MaxValue };
            return jsonSerializer.Serialize(toSerialize).Trim('"');
        }

        public static dynamic DeserializeJson(string content)
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer() { MaxJsonLength = int.MaxValue };
            return jsonSerializer.DeserializeObject(content);
        }
    }
}