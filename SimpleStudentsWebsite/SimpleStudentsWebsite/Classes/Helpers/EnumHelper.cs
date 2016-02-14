using System;

namespace SimpleStudentsWebsite.Classes.Helpers
{
    public class EnumHelper<T>
    {
        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}