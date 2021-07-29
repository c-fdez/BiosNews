using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntidadesCompartidas;

namespace Persistencia
{
    interface IPersistenciaInternacional
    {
        void Alta(Internacional pInternacional);
        void Modificar(Internacional pInternacional);
        Internacional Buscar(string pCodInt);
        List<Internacional> ListarDefault();
        List<Internacional> ListarPublicados();
    }
}
