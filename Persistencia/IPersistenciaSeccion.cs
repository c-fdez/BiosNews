using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntidadesCompartidas;

namespace Persistencia
{
    interface IPersistenciaSeccion
    {
        void Alta(Seccion pSeccion);
        void Baja(Seccion pSeccion);
        void Modificar(Seccion pSeccion);
        Seccion Buscar(string pCodInt);
        List<Seccion> Listar();
    }
}
