using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace EntidadesCompartidas
{
    public class Empleado
    {
        #region Atributo
        private string nomUsu;
        private string pass;
        #endregion

        #region Propiedades
        public string NomUsu
        {
            get { return nomUsu; }
            set
            {
                string aux = value.ToLower().Trim();
                if (aux == null)
                    throw new Exception("Debe ingresar el nombre del empleado.");
                if (aux.Length == 10)
                    nomUsu = aux;
                else
                    throw new Exception("El nombre el empleado debe contener 10 caracteres.");
            }
        }

        public string Pass
        {
            get { return pass; }
            set
            {
                string aux = value.ToLower().Trim();
                if (aux == null)
                    throw new Exception("Debe de introducir el password.");
                var rgx = new Regex("[a-zA-Z]{4}[0-9]{3}");
                if (rgx.IsMatch(aux))
                    pass = aux;
                 else
                    throw new Exception("El password debe contener 4 letras seguidos de 3 números.");
            }
        }
        #endregion

        #region Constructor
        public Empleado(string pNomUsu, string pPass)
        {
            NomUsu = pNomUsu;
            Pass = pPass;
        }
        #endregion
        
        #region Operaciones
        public override string ToString()
        {
            string frase = "Nombre de usuario: " + NomUsu.Substring(0, 1).ToUpper() + NomUsu.Substring(1) + ".";
            return frase;
        }
        #endregion
    }
}
