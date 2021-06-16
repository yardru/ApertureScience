using System;

namespace ApertureScience
{
    class Common
    {
        static void Main(string[] args)
        {
            var photos = new string[] { "a.jpg", "b.jpg", "c.jpg" };
            var emp = new Employee("xxx@yyy.com", "drowssap", "GLaDOS", "", Employee.Roles.ADMIN, "911", "a.jpg|.jpg|c.jpg");
            var emp1 = new Employee("1@yyy.com", "drowssap", "emp", "1");
            photos[0] = "d.jpg";
            Console.WriteLine(emp);
            Console.WriteLine(emp.PhotoNames);
            Console.WriteLine(emp1);
            Console.WriteLine(emp1.PhotoNames);
        }
    }
}
