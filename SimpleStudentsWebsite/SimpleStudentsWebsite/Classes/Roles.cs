using System;

namespace SimpleStudentsWebsite.Classes
{
    [Flags]
    public enum Roles
    {
        Guest = 0,
        Student = 0x01,
        Teacher = 0x02,
        Dean = 0x04,
    }
}