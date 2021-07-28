using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadesCompartidas
{
    public class Internacional : Noticia
    {
        #region Atributo
        private string pais;
        #endregion

        #region Propiedades
        public string Pais
        {
            //Se me acurre que podemos hacer un listado de paises donde el usuario elija el país ya existente. para evitar ingresos de paises erroneos.
            get { return pais; }
            set {
                string aux = value.ToLower().Trim();
                if (aux == null)
                    throw new Exception("Debe de introducir el país.");
                if (aux.Length <= 20 && aux.Length >= 4)
                    pais = aux;
                else
                    throw new Exception("El nombre del país no debe exceder los 20 caracteres y no debe ser menor a los 4 caracteres.");
                }
        }
        #endregion

        #region Constructor
        public Internacional(string pCodInt, DateTime pFechaPub, string pTitulo, string pContenido, int pImportancia, List<Periodista> pPeriodistas, Empleado pEmpleado, string pPais) :
            base(pCodInt, pFechaPub, pTitulo, pContenido, pImportancia, pPeriodistas, pEmpleado)
        { Pais = pPais; }
        #endregion

        #region Operaciones
        public override string ToString()
        {
            string frase = base.ToString();
                   frase += "País: " + Pais.Substring(0,1).ToUpper() + Pais.Substring(1) + ".";
            return frase;

        }
        #endregion
    }
}
