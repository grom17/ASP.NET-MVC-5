using System.ComponentModel;

namespace SimpleStudentsWebsite.Classes.Attributes
{
    public class CustomDisplayNameAttribute : DisplayNameAttribute
    {
        public CustomDisplayNameAttribute(string messageId)
            : base(GetMessage(messageId))
        {
        }

        private static string GetMessage(string messageId)
        {
            return Messages.GetMessage(messageId);
        }
    }
}