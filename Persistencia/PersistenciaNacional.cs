using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntidadesCompartidas;
using System.Data;
using System.Data.SqlClient;

namespace Persistencia
{
    internal class PersistenciaNacional : IPersistenciaNacional
    {
        //Singleton
        private static PersistenciaNacional _instancia = null;
        private PersistenciaNacional() { }
        public static PersistenciaNacional GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PersistenciaNacional();

            return _instancia;
        }

        //Operaciones
        public void Alta(Nacional pNacional)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand cmd = new SqlCommand("AltaNacionales", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CodInt", pNacional.CodInt);
            cmd.Parameters.AddWithValue("@FechaPub", pNacional.FechaPub);
            cmd.Parameters.AddWithValue("@Titulo", pNacional.Titulo);
            cmd.Parameters.AddWithValue("@Contenido", pNacional.Contenido);
            cmd.Parameters.AddWithValue("@Importancia", pNacional.Importancia);
            cmd.Parameters.AddWithValue("@NomUsu", pNacional.Empleado.NomUsu);
            cmd.Parameters.AddWithValue("@CodSec", pNacional.Seccion.CodInt);

            SqlParameter retorno = new SqlParameter();
            retorno.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retorno);

            SqlTransaction Transaction = null;

            try
            {
                cnn.Open();
                Transaction = cnn.BeginTransaction();

                cmd.Transaction = Transaction;
                cmd.ExecuteNonQuery();

                switch ((int)retorno.Value)
                {
                    case -1:
                        throw new Exception("No existe la Sección");
                    case -2:
                        throw new Exception("Ya existe la noticia.");
                    case -3:
                        throw new Exception("El empleado no existe.");
                    default:
                        break;
                }

                foreach (Periodista auxPeriodista in pNacional.Periodistas)
                {
                    PersistenciaEscriben.GetInstancia().Agregar(Transaction, pNacional.CodInt, auxPeriodista);
                }

                Transaction.Commit();
            }
            catch (Exception ex)
            {
                Transaction.Rollback();
                throw new Exception("DataBase: " + ex.Message);
            }
            finally
            { cnn.Close(); }
        }
        public void Modificar(Nacional pNacional)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand cmd = new SqlCommand("ModificarNacionales", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CodInt", pNacional.CodInt);
            cmd.Parameters.AddWithValue("@FechaPub", pNacional.FechaPub);
            cmd.Parameters.AddWithValue("@Titulo", pNacional.Titulo);
            cmd.Parameters.AddWithValue("@Contenido", pNacional.Contenido);
            cmd.Parameters.AddWithValue("@Importancia", pNacional.Importancia);
            cmd.Parameters.AddWithValue("@NomUsu", pNacional.Empleado.NomUsu);
            cmd.Parameters.AddWithValue("@CodSec", pNacional.Seccion.CodInt);

            SqlParameter retorno = new SqlParameter();
            retorno.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retorno);

            SqlTransaction Transaction = null;

            try
            {
                cnn.Open();
                Transaction = cnn.BeginTransaction();

                cmd.Transaction = Transaction;
                cmd.ExecuteNonQuery();

                switch ((int)retorno.Value)
                {
                    case -1:
                        throw new Exception("No existe la Sección");
                    case -2:
                        throw new Exception("No existe la noticia.");
                    case -3:
                        throw new Exception("El empleado no existe.");
                    default:
                        break;
                }

                PersistenciaEscriben.GetInstancia().Eliminar(Transaction, pNacional.CodInt);

                foreach(Periodista auxPeriodista in pNacional.Periodistas)
                {
                    PersistenciaEscriben.GetInstancia().Agregar(Transaction, pNacional.CodInt, auxPeriodista);
                }

                Transaction.Commit();
            }
            catch (Exception ex)
            {
                Transaction.Rollback();
                throw new Exception("DataBase: " + ex.Message);
            }
            finally
            { cnn.Close(); }
        }
        public Nacional Buscar(string pCodInt)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand cmd = new SqlCommand("BuscarNacional", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CodInt", pCodInt);

            Nacional auxNacional = null;

            try
            {
                cnn.Open();
                SqlDataReader lector = cmd.ExecuteReader();

                if (lector.HasRows)
                {
                    lector.Read();
                    DateTime fecha = (DateTime)lector["FechaPub"];
                    string titulo = (string)lector["Titulo"];
                    string contenido = (string)lector["Contenido"];
                    int importancia = (int)lector["Importancia"];

                    //Busco el empleado
                    string NomUsu = (string)lector["NomUsu"];
                    Empleado Empleado = PersistenciaEmpleado.GetInstancia().Buscar(NomUsu);

                    //Busco los periodistas de la noticia
                    List<int> list = PersistenciaEscriben.GetInstancia().List(pCodInt);
                    List<Periodista> ListPeriodistas = null;
                    foreach(int ci in list)
                    {
                        ListPeriodistas.Add(PersistenciaPeriodista.GetInstancia().BuscarTodos(ci));
                    }
                    //Busco la seccion a la que pertenece la noticia
                    Seccion Seccion = PersistenciaSeccion.GetInstancia().BuscarTodos(pCodInt);

                    auxNacional = new Nacional(pCodInt, fecha, titulo, contenido, importancia, ListPeriodistas, Empleado, Seccion);
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("DataBase: " + ex.Message);
            }
            finally
            { cnn.Close(); }

            return auxNacional;
        }
        public List<Nacional> ListarDefault()
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand cmd = new SqlCommand("ListarDefaultNacionales", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            List<Nacional> listNacional = null;

            try
            {
                cnn.Open();
                SqlDataReader lector = cmd.ExecuteReader();

                while (lector.HasRows)
                {
                    lector.Read();
                    string codint = (string)lector["CodInt"];
                    DateTime fecha = (DateTime)lector["FechaPub"];
                    string titulo = (string)lector["Titulo"];
                    string contenido = (string)lector["Contenido"];
                    int importancia = (int)lector["Importancia"];

                    //Busco el empleado
                    string NomUsu = (string)lector["NomUsu"];
                    Empleado Empleado = PersistenciaEmpleado.GetInstancia().Buscar(NomUsu);

                    //Busco los periodistas de la noticia
                    List<int> list = PersistenciaEscriben.GetInstancia().List(codint);
                    List<Periodista> ListPeriodistas = null;
                    foreach (int ci in list)
                    {
                        ListPeriodistas.Add(PersistenciaPeriodista.GetInstancia().BuscarTodos(ci));
                    }

                    //Busco la seccion a la que pertenece la noticia
                    string codsec = (string)lector["CodSec"];
                    Seccion Seccion = PersistenciaSeccion.GetInstancia().BuscarTodos(codsec);

                    listNacional.Add(new Nacional(codint, fecha, titulo, contenido, importancia, ListPeriodistas, Empleado, Seccion));
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("DataBase: " + ex.Message);
            }
            finally
            { cnn.Close(); }

            return listNacional;
        }
        public List<Nacional> ListarPublicados()
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand cmd = new SqlCommand("ListarPublicadosNacionales", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            List<Nacional> listNacional = null;

            try
            {
                cnn.Open();
                SqlDataReader lector = cmd.ExecuteReader();

                while (lector.HasRows)
                {
                    lector.Read();
                    string codint = (string)lector["CodInt"];
                    DateTime fecha = (DateTime)lector["FechaPub"];
                    string titulo = (string)lector["Titulo"];
                    string contenido = (string)lector["Contenido"];
                    int importancia = (int)lector["Importancia"];

                    //Busco el empleado
                    string NomUsu = (string)lector["NomUsu"];
                    Empleado Empleado = PersistenciaEmpleado.GetInstancia().Buscar(NomUsu);

                    //Busco los periodistas de la noticia
                    List<int> list = PersistenciaEscriben.GetInstancia().List(codint);
                    List<Periodista> ListPeriodistas = null;
                    foreach (int ci in list)
                    {
                        ListPeriodistas.Add(PersistenciaPeriodista.GetInstancia().BuscarTodos(ci));
                    }

                    //Busco la seccion a la que pertenece la noticia
                    string codsec = (string)lector["CodSec"];
                    Seccion Seccion = PersistenciaSeccion.GetInstancia().BuscarTodos(codsec);

                    listNacional.Add(new Nacional(codint, fecha, titulo, contenido, importancia, ListPeriodistas, Empleado, Seccion));
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("DataBase: " + ex.Message);
            }
            finally
            { cnn.Close(); }

            return listNacional;
        }
    }
}
