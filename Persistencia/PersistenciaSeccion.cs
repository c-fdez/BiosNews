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
    internal class PersistenciaSeccion : IPersistenciaSeccion
    {
        //Singleton
        private static PersistenciaSeccion _instancia = null;
        private PersistenciaSeccion() { }
        public static PersistenciaSeccion GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PersistenciaSeccion();

            return _instancia;
        }

        //Operaciones
        public void Alta(Seccion pSeccion)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("AltaSeccion ", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CodInt ", pSeccion.CodInt);
            cmd.Parameters.AddWithValue("@Nombre", pSeccion.Nombre);

            SqlParameter retorno = new SqlParameter();
            retorno.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retorno);

            try
            {
                cnn.Open();
                cmd.ExecuteNonQuery();

                if ((int)retorno.Value == -1)
                    throw new Exception("Ya existe la Sección.");

            }
            catch (Exception ex)
            {
                throw new Exception("DataBase: " + ex.Message);
            }
            finally
            { cnn.Close(); }
        }
        public void Baja(Seccion pSeccion)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("BajaSeccion", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CodInt", pSeccion.CodInt);

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
                        throw new Exception("No se pudo eliminar la Seccion.");
                    case -1:
                        throw new Exception("No existe la Sección.");
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
        public void Modificar(Seccion pSeccion)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("ModificarSeccion", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CodInt ", pSeccion.CodInt);
            cmd.Parameters.AddWithValue("@Nombre", pSeccion.Nombre);

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
                        throw new Exception("No se pudo modificar la Sección.");
                    case -1:
                        throw new Exception("No existe la Sección.");
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
        public Seccion Buscar(string pCodInt)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("BuscarSeccionActivo", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CodInt ", pCodInt);

            Seccion auxSeccion = null;

            try
            {
                cnn.Open();
                SqlDataReader lector = cmd.ExecuteReader();

                if (lector.Read())
                {
                    string nombre = (string)lector["Nombre"];

                    auxSeccion = new Seccion(pCodInt, nombre);
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("DataBase: " + ex.Message);
            }
            finally
            { cnn.Close(); }
            return auxSeccion;
        }
        internal Seccion BuscarTodos(string pCodInt)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("BuscarSeccion", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CodInt ", pCodInt);

            Seccion auxSeccion = null;

            try
            {
                cnn.Open();
                SqlDataReader lector = cmd.ExecuteReader();

                if (lector.Read())
                {
                    string nombre = (string)lector["Nombre"];

                    auxSeccion = new Seccion(pCodInt, nombre);
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("DataBase: " + ex.Message);
            }
            finally
            { cnn.Close(); }
            return auxSeccion;
        }
        public List<Seccion> Listar()
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("ListarSecciones", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            List<Seccion> listSecciones = null;

            try
            {
                cnn.Open();
                SqlDataReader lector = cmd.ExecuteReader();

                while (lector.Read())
                {
                    string codint = (string)lector["CodInt"];
                    string nombre = (string)lector["Nombre"];

                    listSecciones.Add(new Seccion(codint, nombre));
                }
                lector.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("DataBase: " + ex.Message);
            }
            finally
            { cnn.Close(); }
            return listSecciones;
        }

    }
}
