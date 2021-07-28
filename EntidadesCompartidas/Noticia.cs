using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntidadesCompartidas
{
    public abstract class Noticia
    {
        #region Atributo
        private string codInt;
        private DateTime fechaPub;
        private string titulo;
        private string contenido;
        private int importancia;

        // Asociación
        private List<Periodista> periodistas;
        private Empleado empleado;
        #endregion

        #region Propiedades
        public string CodInt
        {
            get { return codInt; }
            set
            {
                //Este Código Interno es NO autogenerado, lo ingresa el propio Empleado.
                string aux = value.ToLower().Trim();
                if (aux == null)
                    throw new Exception("Debe de ingresar un código para la noticia.");
                if (aux.Length > 20 || aux.Length < 5)
                    throw new Exception("El código debe de tener máximo 20 caracteres y mímino 5 caracteres.");
                codInt = aux;
            }
        }

        public DateTime FechaPub
        {
            get { return fechaPub; }
            set { fechaPub = value; }
        }

        public string Titulo

        {
            get { return titulo; }
            set {
                string aux = value.ToLower().Trim();
                if (aux == null)
                    throw new Exception("Debe de ingresar un título para la noticia.");
                if (aux.Length >= 50 || aux.Length < 5)
                    throw new Exception("El título debe de tener máximo 50 caracteres y mímino 5 caracteres.");
                titulo = aux; }
        }

        public string Contenido
        {
            get { return contenido; }
            set {
                string aux = value.ToLower().Trim();
                if (aux == null)
                    throw new Exception("Por favor ingrese el contenido de la noticia.");
                if (aux.Length > 1000 || aux.Length < 5)
                    throw new Exception("El contenido de la noticia no debe superar los 1000 caracteres y tener mímino 5 caracteres.");
                contenido = aux; }
        }

        public int Importancia
        {
            get { return importancia; }
            set
            {
                if (value >= 1 && value <= 5)
                    importancia = value;
                else
                    throw new Exception("El ranking de importancia solo acepta valores de 1 al 5.");
            }
        }
        public List<Periodista> Periodistas
        {
            get { return periodistas; }
            set
            {
                if (value != null && value.Count() >= 1)
                    periodistas = value;
                else
                    throw new Exception("La noticia debe contener al menos un periodista.");
            }
        }

        public Empleado Empleado
        {
            get { return empleado; }
            set {
                if (value != null)
                    empleado = value;
                else
                    throw new Exception("La noticia no posee el datos del Empleado que la modificó o generó.");
            }
        }
        #endregion

        #region Constructor
        public Noticia(string pCodInt, DateTime pFechaPub, string pTitulo, string pContenido, int pImportancia, List<Periodista> pPeriodistas, Empleado pEmpleado)
        {
            CodInt = pCodInt;
            FechaPub = pFechaPub;
            Titulo = pTitulo;
            Contenido = pContenido;
            Importancia = pImportancia;
            Empleado = pEmpleado;
            Periodistas = pPeriodistas;
        }
        #endregion

        #region Operaciones
        public override string ToString()
        {
            string frase = "Codigo Interno: " + CodInt.ToString() + ", ";
                   frase += "Fecha de Publicación: " + FechaPub.ToString() + ", ";
                   frase += "Título: " + Titulo.ToString() + ", ";
                   frase += "Contenido: " + Contenido.ToString() + ", ";
                   frase += "Importancia: " + Importancia.ToString() + ". ";
                   frase += "Empleado: " + Empleado.ToString() + ". ";
            switch(this.Periodistas.Count())
            {
                case 0:
                    frase += "La noticia no tiene periodistas asignados. ";
                    break;
                case 1:
                    frase += "Periodista: " + this.Periodistas[0].ToString();
                    break;
                default:
                    frase += "Periodistas: ";
                    foreach (Periodista aux in this.Periodistas)
                    {
                        frase += aux.ToString();
                        frase += "/ ";
                    }
                    break;
            }            
            return frase;
        }
        #endregion
    }
}
