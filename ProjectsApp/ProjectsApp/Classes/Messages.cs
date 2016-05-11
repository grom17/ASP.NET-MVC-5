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
        string fn = (HttpContext.Current != null) && (HttpContext.Current.Request.MapPath("~/App_Data/") != null) ?
            HttpContext.Current.Request.MapPath("~/App_Data/") + Filename :
            baseDir.Substring(0, baseDir.IndexOf("ProjectsApp") + 11) + "\\ProjectsApp\\App_Data\\" + Filename;
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
    public static string ApplicationName { get { return Instance["ApplicationName"]; } }
    public static string HomePage { get { return Instance["HomePage"]; } }
    public static string BackBtn { get { return Instance["BackBtn"]; } }
    #endregion

    #region Staff
    public static string StaffTitle { get { return Instance["StaffTitle"]; } }

    public static string LoadingNewEmployeeCard { get { return Instance["LoadingNewEmployeeCard"]; } }
    public static string LoadingEmployeesList { get { return Instance["LoadingEmployeesList"]; } }
    public static string LoadingEmployeeDetails { get { return Instance["LoadingEmployeeDetails"]; } }

    public static string UpdatingEmployeeDetails { get { return Instance["UpdatingEmployeeDetails"]; } }
    public static string CreatingNewEmployee { get { return Instance["CreatingNewEmployee"]; } }
    public static string DeletingEmployee { get { return Instance["DeletingEmployee"]; } }

    public static string FirstName { get { return Instance["FirstName"]; } }
    public static string Patronymic { get { return Instance["Patronymic"]; } }
    public static string LastName { get { return Instance["LastName"]; } }
    public static string Email { get { return Instance["Email"]; } }

    public static string CreateEmployeeBtn { get { return Instance["CreateEmployeeBtn"]; } }
    public static string DeleteEmployeeBtn { get { return Instance["DeleteEmployeeBtn"]; } }
    public static string UpdateEmployeeBtn { get { return Instance["UpdateEmployeeBtn"]; } }

    public static string EmployeeDetailsUpdatedSuccessfully { get { return Instance["EmployeeDetailsUpdatedSuccessfully"]; } }
    public static string EmployeeDeletedSuccessfully { get { return Instance["EmployeeDeletedSuccessfully"]; } }
    public static string EmployeeCreatedSuccessfully { get { return Instance["EmployeeCreatedSuccessfully"]; } }

    public static string EmployeeNotExists { get { return Instance["EmployeeNotExists"]; } }
    #endregion

    #region Projects
    public static string ProjectsTitle { get { return Instance["ProjectsTitle"]; } }

    public static string LoadingNewProjectCard { get { return Instance["LoadingNewProjectCard"]; } }
    public static string LoadingProjectsList { get { return Instance["LoadingProjectsList"]; } }
    public static string LoadingProjectDetails { get { return Instance["LoadingProjectDetails"]; } }

    public static string UpdatingProjectDetails { get { return Instance["UpdatingProjectDetails"]; } }
    public static string CreatingNewProject { get { return Instance["CreatingNewProject"]; } }
    public static string DeletingProject { get { return Instance["DeletingProject"]; } }

    public static string ClientCompanyName { get { return Instance["ClientCompanyName"]; } }
    public static string ExecutiveCompanyName { get { return Instance["ExecutiveCompanyName"]; } }
    public static string StartDate { get { return Instance["StartDate"]; } }
    public static string EndDate { get { return Instance["EndDate"]; } }
    public static string Priority { get { return Instance["Priority"]; } }
    public static string Comment { get { return Instance["Comment"]; } }
    public static string ProjectManagerName { get { return Instance["ProjectManagerName"]; } }

    public static string CreateProjectBtn { get { return Instance["CreateProjectBtn"]; } }
    public static string DeleteProjectBtn { get { return Instance["DeleteProjectBtn"]; } }
    public static string UpdateProjectBtn { get { return Instance["UpdateProjectBtn"]; } }

    public static string ProjectDetailsUpdatedSuccessfully { get { return Instance["ProjectDetailsUpdatedSuccessfully"]; } }
    public static string ProjectDeletedSuccessfully { get { return Instance["ProjectDeletedSuccessfully"]; } }
    public static string ProjectCreatedSuccessfully { get { return Instance["ProjectCreatedSuccessfully"]; } }

    public static string ProjectNotExists { get { return Instance["ProjectNotExists"]; } }
    #endregion
}