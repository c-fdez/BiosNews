using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntidadesCompartidas;
using Persistencia;

namespace Logica
{
    internal class LogicaEmpleado : ILogicaEmpleado
    {
        //Singleton
        private static LogicaEmpleado _instancia = null;
        private LogicaEmpleado() { }
        public static LogicaEmpleado GetInstancia()
        {
            if (_instancia == null)
                _instancia = new LogicaEmpleado();

            return _instancia;
        }

        //Operaciones
        public void Alta(Empleado pEmpleado)
        {
            FabricaPersistencia.GetPersistenciaEmpleado().Alta(pEmpleado);
        }

        public Empleado Logueo(string pNombre, string pContrasena)
        {
            return (FabricaPersistencia.GetPersistenciaEmpleado().Logueo(pNombre, pContrasena));
        }
    }
}
