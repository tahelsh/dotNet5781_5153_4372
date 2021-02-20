using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class User
    {
        public string UserName { get; set; }//unique username
        public bool AdminAccess { get; set; }//if the user is admin or not
        public string Name { get; set; }//name
        public int Passcode { get; set; }//passcode

    }
}
