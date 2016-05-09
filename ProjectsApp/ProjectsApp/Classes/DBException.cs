using System;
namespace SimpleStudentsWebsite.Classes.Exceptions
{
    public class DBException : Exception
    {
        public DBException(string method, string message): base(System.Diagnostics.Debugger.IsAttached ? method + message : message)
        {
        }
    }
}