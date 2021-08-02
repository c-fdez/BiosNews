using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntidadesCompartidas;
using Persistencia;

namespace Logica
{
    internal class LogicaNoticia : ILogicaNoticia
    {
        //Singleton
        private static LogicaNoticia _instancia = null;
        private LogicaNoticia() { }
        public static LogicaNoticia GetInstancia()
        {
            if (_instancia == null)
                _instancia = new LogicaNoticia();

            return _instancia;
        }

        //Operaciones
        public void Alta(Noticia pNoticia)
        {
            if (pNoticia != null)
            {
                if (pNoticia is Nacional)
                    FabricaPersistencia.GetPersistenciaNacional().Alta((Nacional)pNoticia);
                else if (pNoticia is Internacional)
                    FabricaPersistencia.GetPersistenciaInternacional().Alta((Internacional)pNoticia);
                else
                    throw new Exception("La noticia no posee el formato correcto.");
            }
            else
                throw new Exception("Debe ingresar la noticia.");
        }

        public void Modificar(Noticia pNoticia)
        {
            if (pNoticia != null)
            {
                if (pNoticia is Nacional)
                    FabricaPersistencia.GetPersistenciaNacional().Modificar((Nacional)pNoticia);
                else if (pNoticia is Internacional)
                    FabricaPersistencia.GetPersistenciaInternacional().Modificar((Internacional)pNoticia);
                else
                    throw new Exception("La noticia no posee el formato correcto.");
            }
            else
                throw new Exception("Debe ingresar la noticia.");
        }
    }
}
