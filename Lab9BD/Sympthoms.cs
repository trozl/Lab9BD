using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lab9BD
{
   public class Sympthoms
    {
        private static int Sympthoms_id { get; set; }
        private String Sympthoms_name { get; set; }
        public Sympthoms( string sympthoms_name)
        {
            //Sympthoms_id = sympthoms_id;
            Sympthoms_name = sympthoms_name;
            Sympthoms_id++;
        }
    }
}
