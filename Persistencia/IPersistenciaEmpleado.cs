using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntidadesCompartidas;

namespace Persistencia
{
    interface IPersistenciaEmpleado
    {
        void Alta(Empleado pEmpleado);
        Empleado Buscar(string pNomUsu);
        Empleado Logueo(string pNomUsu, string pPass);
    }
}
