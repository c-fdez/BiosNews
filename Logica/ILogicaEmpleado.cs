using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntidadesCompartidas;

namespace Logica
{
    public interface ILogicaEmpleado
    {
        void Alta(Empleado pEmpleado);

        Empleado Logueo(string pNombre, string pContrasena);
    }
}
