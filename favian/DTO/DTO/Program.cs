using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
    public class info
    {
        private string Id;
        private string Password;
        private string Name;

        public string id { get => Id; set => Id = value; }
        public string password { get => Password; set => Password = value; }
        public string name { get => Name; set => Name = value; }

        public info(string _Id,string _Password,string _Name)
        {
            Id = _Id;
            Password = _Password;
            Name = _Name;
        }
    }
}
