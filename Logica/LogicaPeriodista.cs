using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntidadesCompartidas;
using Persistencia;

namespace Logica
{
    internal class LogicaPeriodista : ILogicaPeriodista
    {
        //Singleton
        private static LogicaPeriodista _instancia = null;
        private LogicaPeriodista() { }
        public static LogicaPeriodista GetInstancia()
        {
            if (_instancia == null)
                _instancia = new LogicaPeriodista();

            return _instancia;
        }

        //Operaciones
        public void Alta(Periodista pPeriodista)
        {
            FabricaPersistencia.GetPersistenciaPeriodista().Alta(pPeriodista);
        }

        public void Baja(Periodista pPeriodista)
        {
            FabricaPersistencia.GetPersistenciaPeriodista().Baja(pPeriodista);
        }

        public void Modificar(Periodista pPeriodista)
        {
            FabricaPersistencia.GetPersistenciaPeriodista().Modificar(pPeriodista);
        }

        public Periodista Buscar(int pCi)
        {
            return (FabricaPersistencia.GetPersistenciaPeriodista().Buscar(pCi));
        }

        public List<Periodista> Listar()
        {
            return (FabricaPersistencia.GetPersistenciaPeriodista().Listar());
        }
    }
}
