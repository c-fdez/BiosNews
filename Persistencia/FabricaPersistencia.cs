using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Persistencia
{
    public class FabricaPersistencia
    {
        public static IPersistenciaNacional GetPersistenciaNacional()
        {
            return (PersistenciaNacional.GetInstancia());
        }

        public static IPersistenciaInternacional GetPersistenciaInternacional()
        {
            return (PersistenciaInternacional.GetInstancia());
        }

        public static IPersistenciaPeriodista GetPersistenciaPeriodista()
        {
            return (PersistenciaPeriodista.GetInstancia());
        }

        public static IPersistenciaEmpleado GetPersistenciaEmpleado()
        {
            return (PersistenciaEmpleado.GetInstancia());
        }

        public static IPersistenciaSeccion GetPersistenciaSeccion()
        {
            return (PersistenciaSeccion.GetInstancia());
        }
    }
}
