using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntidadesCompartidas;
using Persistencia;

namespace Logica
{
    internal class LogicaSeccion : ILogicaSeccion
    {
        //Singleton
        private static LogicaSeccion _instancia = null;
        private LogicaSeccion() { }
        public static LogicaSeccion GetInstancia()
        {
            if (_instancia == null)
                _instancia = new LogicaSeccion();

            return _instancia;
        }

        //Operaciones
        public void Alta(Seccion pSeccion)
        {
            FabricaPersistencia.GetPersistenciaSeccion().Alta(pSeccion);
        }

        public void Baja(Seccion pSeccion)
        {
            FabricaPersistencia.GetPersistenciaSeccion().Baja(pSeccion);
        }

        public void Modificar(Seccion pSeccion)
        {
            FabricaPersistencia.GetPersistenciaSeccion().Modificar(pSeccion);
        }

        public Seccion Buscar(string pCodInt)
        {
            return (FabricaPersistencia.GetPersistenciaSeccion().Buscar(pCodInt));
        }

        public List<Seccion> Listar()
        {
            return (FabricaPersistencia.GetPersistenciaSeccion().Listar());
        }
    }
}
