using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntidadesCompartidas;

namespace Persistencia
{
    interface IPersistenciaNacional
    {
        void Alta(Nacional pNacional);
        void Modificar(Nacional pNacional);
        Nacional Buscar(string pCodInt);
        List<Nacional> ListarDefault();
        List<Nacional> ListarPublicados();
    }
}
