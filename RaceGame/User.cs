using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Race
{
    public class User
    {
        public string Name { get; }
        public int Score { get; set; }
        public int Coins { get; set; }
        public DateTime Time { get; set; }

        public User(string name)
        {
            Name = name;
        }
    }
}
