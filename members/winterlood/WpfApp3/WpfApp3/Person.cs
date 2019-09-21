using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    class Person
    {
        
        public Person()
        {

        }
        public string Name { get => Name; set => Name = value; }
        public int Age { get => Age; set => Age = value; }

        public Person(string name, int age)
        {
            this.Name = name;
            this.Age = age;
        }
 
        public static List<Person> GetPerson()
        {
            List<Person> p = new List<Person>();
            p.Add(new Person("lee", 23));
            p.Add(new Person("ha", 23));
            p.Add(new Person("shin", 23));
            return p;
        }
    }
}
