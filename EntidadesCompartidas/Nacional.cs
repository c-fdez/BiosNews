using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadesCompartidas
{
    public class Nacional : Noticia
    {
        #region Atributo
        Seccion seccion;
        #endregion

        #region Propiedades
        public Seccion Seccion
        {
            get { return seccion; }
            set
            {
                if (value != null)
                    seccion = value;
                else
                    throw new Exception("La noticia debe de pertenecer a una Seccion.");
            }
        }
        #endregion

        #region Constructor
        public Nacional(string pCodInt, DateTime pFechaPub, string pTitulo, string pContenido, int pImportancia, List<Periodista> pPeriodistas, Empleado pEmpleado, Seccion pSeccion) :
            base(pCodInt, pFechaPub, pTitulo, pContenido, pImportancia, pPeriodistas, pEmpleado)
        { Seccion = pSeccion; }
        #endregion

        #region Operaciones
        public override string ToString()
        {
            string frase = base.ToString();
                   frase += "Seccion: " + Seccion.ToString();
            return frase;
        }
        #endregion
    }
}
