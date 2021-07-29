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
    internal class PersistenciaInternacional : IPersistenciaInternacional
    {
        //Singleton
        private static PersistenciaInternacional _instancia = null;
        private PersistenciaInternacional() { }
        public static PersistenciaInternacional GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PersistenciaInternacional();

            return _instancia;
        }

        //Operaciones
        public void Alta(Internacional pInternacional)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand cmd = new SqlCommand("AltaInternacionales", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CodInt", pInternacional.CodInt);
            cmd.Parameters.AddWithValue("@FechaPub", pInternacional.FechaPub);
            cmd.Parameters.AddWithValue("@Titulo", pInternacional.Titulo);
            cmd.Parameters.AddWithValue("@Contenido", pInternacional.Contenido);
            cmd.Parameters.AddWithValue("@Importancia", pInternacional.Importancia);
            cmd.Parameters.AddWithValue("@NomUsu", pInternacional.Empleado.NomUsu);
            cmd.Parameters.AddWithValue("@Pais", pInternacional.Pais);

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
                        throw new Exception("Ya existe la noticia.");
                    case -2:
                        throw new Exception("El empleado no existe.");
                    default:
                        break;
                }

                foreach (Periodista auxPeriodista in pInternacional.Periodistas)
                {
                    PersistenciaEscriben.GetInstancia().Agregar(Transaction, pInternacional.CodInt, auxPeriodista);
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
        public void Modificar(Internacional pInternacional)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand cmd = new SqlCommand("ModificarInternacionales", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CodInt", pInternacional.CodInt);
            cmd.Parameters.AddWithValue("@FechaPub", pInternacional.FechaPub);
            cmd.Parameters.AddWithValue("@Titulo", pInternacional.Titulo);
            cmd.Parameters.AddWithValue("@Contenido", pInternacional.Contenido);
            cmd.Parameters.AddWithValue("@Importancia", pInternacional.Importancia);
            cmd.Parameters.AddWithValue("@NomUsu", pInternacional.Empleado.NomUsu);
            cmd.Parameters.AddWithValue("@Pais", pInternacional.Pais);

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
                        throw new Exception("No existe la noticia.");
                    case -2:
                        throw new Exception("El empleado no existe.");
                    default:
                        break;
                }

                PersistenciaEscriben.GetInstancia().Eliminar(Transaction, pInternacional.CodInt);

                foreach (Periodista auxPeriodista in pInternacional.Periodistas)
                {
                    PersistenciaEscriben.GetInstancia().Agregar(Transaction, pInternacional.CodInt, auxPeriodista);
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
        public Internacional Buscar(string pCodInt)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand cmd = new SqlCommand("BuscarInternacional", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CodInt", pCodInt);

            Internacional auxInternacional = null;

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
                    string pais = (string)lector["Pais"];

                    //Busco el empleado
                    string NomUsu = (string)lector["NomUsu"];
                    Empleado Empleado = PersistenciaEmpleado.GetInstancia().Buscar(NomUsu);

                    //Busco los periodistas de la noticia
                    List<int> list = PersistenciaEscriben.GetInstancia().List(pCodInt);
                    List<Periodista> ListPeriodistas = null;
                    foreach (int ci in list)
                    {
                        ListPeriodistas.Add(PersistenciaPeriodista.GetInstancia().BuscarTodos(ci));
                    }

                    auxInternacional = new Internacional(pCodInt, fecha, titulo, contenido, importancia, ListPeriodistas, Empleado, pais);
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("DataBase: " + ex.Message);
            }
            finally
            { cnn.Close(); }

            return auxInternacional;
        }
        public List<Internacional> ListarDefault()
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand cmd = new SqlCommand("ListarDefaultInternacionales", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            List<Internacional> listInteracional = null;

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
                    string pais = (string)lector["Pais"];

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

                    listInteracional.Add(new Internacional(codint, fecha, titulo, contenido, importancia, ListPeriodistas, Empleado, pais));
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("DataBase: " + ex.Message);
            }
            finally
            { cnn.Close(); }

            return listInteracional;
        }
        public List<Internacional> ListarPublicados()
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand cmd = new SqlCommand("ListarPublicadosInternacionales", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            List<Internacional> listInternacional = null;

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
                    string pais = (string)lector["Pais"];

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

                    listInternacional.Add(new Internacional(codint, fecha, titulo, contenido, importancia, ListPeriodistas, Empleado, pais));
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("DataBase: " + ex.Message);
            }
            finally
            { cnn.Close(); }

            return listInternacional;
        }

    }
}
