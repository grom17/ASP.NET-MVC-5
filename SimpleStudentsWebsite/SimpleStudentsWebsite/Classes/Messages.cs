using System;
using System.Collections.Generic;
using System.Web;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using System.Xml;

public class Messages
{
    public class Message
    {
        [XmlAttribute("name")]
        public string name { get; set; }
        public string value { get; set; }
    }

    [XmlElement("Message")]
    public List<Message> MessageList { get; set; }

    private static DateTime LastWriteTime = DateTime.MinValue;

    protected static readonly string Filename = "Messages.xml";

    protected static Messages mInstance = null;

    public static Messages Instance
    {
        get { return GetInstance(); }
    }

    public static Messages GetInstance()
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string fn = HttpContext.Current.Request.MapPath("~/App_Data/") + Filename;
        if (mInstance == null || (File.Exists(fn) && File.GetLastWriteTime(fn) != LastWriteTime))
        {

            XmlSerializer serializer = new XmlSerializer(typeof(Messages));
            Stream stream = null;

            try
            {
                // check App_Data folder
                if (File.Exists(fn))
                {
                    stream = new FileStream(fn, FileMode.Open, FileAccess.Read);
                }
                // if nothing found previously do load default values from executing assembly
                if (stream == null)
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    string FileNS = assembly.FullName.Split(new[] { ',' })[0];
                    stream = assembly.GetManifestResourceStream(FileNS + "." + Filename);
                }

                if (stream == null)
                {
                    throw new Exception(Filename + " not found");
                }
                XmlTextReader reader = new XmlTextReader(stream);
                try
                {
                    mInstance = (Messages)serializer.Deserialize(reader);
                }
                finally
                {
                    reader.Close();
                }
                LastWriteTime = File.GetLastWriteTime(fn);
            }
            finally
            {
                try { if (stream != null) stream.Close(); }
                catch { }
            }
        }
        return mInstance;
    }
    public static string GetMessage(string id)
    {
        return Instance[id];
    }

    public static string FormatMessage(string Message, params object[] args)
    {
        return string.Format(Message, args);
    }

    public string this[string Id]
    {
        get
        {
            if (MessageList == null || MessageList.Count == 0)
                return "ERROR! Messages not loaded";

            var res = MessageList.Find(e => e.name == Id);
            
            if (res != null)
            {
                if (res.value != null)
                    return res.value;
            }
            return string.Format("ERROR! Message {0} not found", Id);
        }
    }

    #region Common
    public static string ErrorInvalidField { get { return Instance["ErrorInvalidField"]; } }
    #endregion

    #region Login
    public static string AuthenticatingPleaseWait { get { return Instance["AuthenticatingPleaseWait"]; } }
    #endregion
}