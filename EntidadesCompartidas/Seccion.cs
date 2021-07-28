using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace EntidadesCompartidas
{
    public class Seccion
    {
        #region Atributo
        private string codInt;
        private string nombre;
        #endregion

        #region Propiedades
        public string CodInt
        {
            get { return codInt; }
            set
            {
                string aux = value.ToLower().Trim();
                if (aux == null)
                    throw new Exception("Debe de ingresar el código de la sección.");
                Regex reg = new Regex("[a-z]");
                bool a = reg.IsMatch(aux);
                if (a == false)
                    throw new Exception("El código de la sección no puede contener números ni símbolos");
                if (aux.Length == 5)
                    codInt = aux;
                else
                    throw new Exception("El código de la sección debe contener 5 caracteres.");
            }
        }
        public string Nombre
        {
            get { return nombre; }
            set
            {
                string aux = value.ToLower().Trim();
                if (aux == null)
                    throw new Exception("Debe de ingresar el nombre de la sección.");
                if (aux.Length <= 20)
                    nombre = aux;
                else
                    throw new Exception("El nombre de la sección no debe ser mayor a 20 caracteres.");
            }
        }
        #endregion

        #region Constructor
        public Seccion(string pCodInt, string pNombre)
        {
            CodInt = pCodInt;
            Nombre = pNombre;
        }
        #endregion

        #region Operaciones
        public override string ToString()
        {
            string frase = "Código: " + CodInt.ToString() + ", ";
            frase += "Nombre: " + Nombre.Substring(0,1).ToUpper() + Nombre.Substring(1) + ".";
            return frase;
        }
        #endregion
    }
}
