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
    internal class PersistenciaEmpleado : IPersistenciaEmpleado
    {
        //Singleton
        private static PersistenciaEmpleado _instancia = null;
        private PersistenciaEmpleado() { }
        public static PersistenciaEmpleado GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PersistenciaEmpleado();

            return _instancia;
        }

        //Operaciones
        public void Alta(Empleado pEmpleado)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("AltaEmpleado", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@NomUsu", pEmpleado.NomUsu);
            cmd.Parameters.AddWithValue("@Pass", pEmpleado.Pass);

            SqlParameter retorno = new SqlParameter();
            retorno.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retorno);

            try
            {
                cnn.Open();
                cmd.ExecuteNonQuery();

                if ((int)retorno.Value == -1)
                    throw new Exception("Ya existe el empleado.");

            }
            catch (Exception ex)
            {
                throw new Exception("DataBase: " + ex.Message);
            }
            finally
            { cnn.Close(); }
        }
        public Empleado Buscar(string pNomUsu)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("BuscarEmpleado", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@NomUsu", pNomUsu);

            Empleado auxEmpleado = null;

            try
            {
                cnn.Open();
                SqlDataReader lector = cmd.ExecuteReader();

                if (lector.HasRows)
                {
                    lector.Read();
                    string pass = (string)lector["Pass"];

                    auxEmpleado = new Empleado(pNomUsu, pass);
                }
                lector.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("DataBase: " + ex.Message);
            }
            finally
            { cnn.Close(); }
            return auxEmpleado;
        }
        public Empleado Logueo(string pNomUsu, string pPass)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);
            SqlCommand cmd = new SqlCommand("Logueo", cnn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@NomUsu", pNomUsu);
            cmd.Parameters.AddWithValue("@Pass", pPass);

            Empleado auxEmpleado = null;

            try
            {
                cnn.Open();
                SqlDataReader lector = cmd.ExecuteReader();

                if (lector.Read())
                {
                    auxEmpleado = new Empleado(pNomUsu, pPass);
                }
                lector.Close();

            }
            catch (Exception ex)
            {
                throw new Exception("DataBase: " + ex.Message);
            }
            finally
            { cnn.Close(); }
            return auxEmpleado;
        }
    }
}
