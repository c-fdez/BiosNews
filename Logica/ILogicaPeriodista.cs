using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntidadesCompartidas;

namespace Logica
{
    public interface ILogicaPeriodista
    {
        void Alta(Periodista pPeriodista);

        void Baja(Periodista pPeriodista);

        void Modificar(Periodista pPeriodista);

        Periodista Buscar(int pCi);

        List<Periodista> Listar();
    }
}
