using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistencia
{
    internal class Conexion
    {
        private static string cnn = "Data Source=.; Initial Catalog = BiosNews; Integrated Security = true";

        internal static string Cnn
        {
            get { return cnn; }
        }
    }
}
