using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace EntidadesCompartidas
{
    public class Periodista
    {
        #region Atributos
        private int ci;
        private string nombre;
        private string mail;
        #endregion

        #region Propiedades
        public int Ci
        {
            get { return ci; }
            set
            {
                if (value < 0)
                    throw new Exception("La cédula no puede ser un valor negativo.");
                if (value.ToString().Length != 8)
                    throw new Exception("La cédula debe de ser de 8 dígitos.");
                ci = value;
            }
        }

        public string Nombre
        {
            get { return nombre; }
            set
            {
                string aux = value.ToLower().Trim();
                if (aux == null)
                    throw new Exception("Debe de ingresar el nombre del periodista.");
                if (aux.Length > 50 || aux.Length < 3)
                    throw new Exception("El nombre debe de tener máximo 50 caracteres y mímino 3 caracteres.");
                nombre = aux;
            }
        }

        public string Mail
        {
            get { return mail; }
            set
            {
                string aux = value.Trim();
                var rgx = new Regex("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\\.[a-zA-Z0-9-]+)*$");
                if (rgx.IsMatch(aux))
                    mail = aux;
                else
                    throw new Exception("No es una direccion de correo válida.");
            }
        }
        #endregion

        #region Constructor
        public Periodista(int pCi, string pNombre, string pMail)
        {
            Ci = pCi;
            Nombre = pNombre;
            Mail = pMail;
        }
        #endregion

        #region Operaciones
        public override string ToString()
        {
            string frase = "Cédula: " + Ci.ToString() + ", ";
            frase += "Nombre: " + Nombre.Substring(0,1) + Nombre.Substring(1) + ", ";
            frase += "Mail: " + Mail.ToString() + ".";
            return frase;
        }
        #endregion
    }
}
