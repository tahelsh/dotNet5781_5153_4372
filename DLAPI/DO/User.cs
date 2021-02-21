using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class User
    {
        public  string UserName { get; set; }//unique username
        public string Name { get; set; }//name
        public int Passcode { get; set; }//passcode
        public bool AdminAccess { get; set; }//admon access
        public bool IsDeleted { get; set; }
    }
}
