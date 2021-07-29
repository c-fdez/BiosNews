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
    internal class PersistenciaPeriodista : IPersistenciaPeriodista
    {
        //Singleton
        private static PersistenciaPeriodista _instancia = null;
        private PersistenciaPeriodista() { }
        public static PersistenciaPeriodista GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PersistenciaPeriodista();

            return _instancia;
        }

        //Operaciones
        public void Alta(Periodista pPeriodista)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand cmd = new SqlCommand("AltaPeriodista", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CI", pPeriodista.Ci);
            cmd.Parameters.AddWithValue("@Nombre", pPeriodista.Nombre);
            cmd.Parameters.AddWithValue("@Mail", pPeriodista.Mail);

            SqlParameter retorno = new SqlParameter();
            retorno.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retorno);

            try
            {
                cnn.Open();
                cmd.ExecuteNonQuery();

                if ((int)retorno.Value == -1)
                    throw new Exception("Ya existe el periodista.");
            }
            catch (Exception ex)
            {
                throw new Exception("DataBase: " + ex.Message);
            }
            finally
            { cnn.Close(); }
        }
        public void Baja(Periodista pPeriodista)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand cmd = new SqlCommand("BajaPeriodista", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CI", pPeriodista.Ci);

            SqlParameter retorno = new SqlParameter();
            retorno.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retorno);

            try
            {
                cnn.Open();
                cmd.ExecuteNonQuery();

                switch ((int)retorno.Value)
                {
                    case 0:
                        throw new Exception("No se pudo eliminar el periodista.");
                    case -1:
                        throw new Exception("No existe el periodista.");
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DataBase: " + ex.Message);
            }
            finally
            { cnn.Close(); }
        }
        public void Modificar(Periodista pPeriodista)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand cmd = new SqlCommand("ModificarPeriodista", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CI", pPeriodista.Ci);
            cmd.Parameters.AddWithValue("@Nombre", pPeriodista.Nombre);
            cmd.Parameters.AddWithValue("@Mail", pPeriodista.Mail);

            SqlParameter retorno = new SqlParameter();
            retorno.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retorno);

            try
            {
                cnn.Open();
                cmd.ExecuteNonQuery();

                switch ((int)retorno.Value)
                {
                    case 0:
                        throw new Exception("No se pudo modificar el periodista.");
                    case -1:
                        throw new Exception("No existe el periodista.");
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DataBase: " + ex.Message);
            }
            finally
            { cnn.Close(); }
        }
        public Periodista Buscar(int pCI)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand cmd = new SqlCommand("BuscarPeriodistaActivo", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CI", pCI);

            Periodista auxPeriodista = null;

            try
            {
                cnn.Open();
                SqlDataReader lector = cmd.ExecuteReader();

                if (lector.HasRows)
                {
                    lector.Read();
                    string nombre = (string)lector["Nombre"];
                    string mail = (string)lector["Mail"];

                    auxPeriodista = new Periodista(pCI, nombre, mail);
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("DataBase: " + ex.Message);
            }
            finally
            { cnn.Close(); }
            return auxPeriodista;
        }
        internal Periodista BuscarTodos(int pCI)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand cmd = new SqlCommand("BuscarPeriodista", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CI", pCI);

            Periodista auxPeriodista = null;

            try
            {
                cnn.Open();
                SqlDataReader lector = cmd.ExecuteReader();

                if (lector.HasRows)
                {
                    lector.Read();
                    string nombre = (string)lector["Nombre"];
                    string mail = (string)lector["Mail"];

                    auxPeriodista = new Periodista(pCI, nombre, mail);
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("DataBase: " + ex.Message);
            }
            finally
            { cnn.Close(); }
            return auxPeriodista;
        }
        public List<Periodista> Listar()
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand cmd = new SqlCommand("ListarPeriodistas ", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            List<Periodista> listPeriodistas = null;

            try
            {
                cnn.Open();
                SqlDataReader lector = cmd.ExecuteReader();

                while (lector.HasRows)
                {
                    lector.Read();
                    int ci = (int)lector["CI"];
                    string nombre = (string)lector["Nombre"];
                    string mail = (string)lector["Mail"];

                    listPeriodistas.Add(new Periodista(ci, nombre, mail));
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("DataBase: " + ex.Message);
            }
            finally
            { cnn.Close(); }
            return listPeriodistas;
        }
    }
}
