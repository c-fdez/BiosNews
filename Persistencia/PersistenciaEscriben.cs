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
    internal class PersistenciaEscriben
    {
        //Singleton
        private static PersistenciaEscriben _instancia = null;
        private PersistenciaEscriben() { }
        public static PersistenciaEscriben GetInstancia()
        {
            if (_instancia == null)
                _instancia = new PersistenciaEscriben();

            return _instancia;
        }

        //Operaciones
        internal void Agregar(SqlTransaction pTransaction, string pCodInt, Periodista pPeriodista)
        {
            SqlCommand cmd = new SqlCommand("AltaEscriben", pTransaction.Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodInt", pCodInt);
            cmd.Parameters.AddWithValue("@CI", pPeriodista.Ci);

            SqlParameter retorno = new SqlParameter();
            retorno.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retorno);

            try
            {
                cmd.Transaction = pTransaction;
                cmd.ExecuteNonQuery();

                switch ((int)retorno.Value)
                {
                    case 0:
                        throw new Exception("No se pudo agregar el periodista a la noticia.");
                    case -1:
                        throw new Exception("No existe la noticia.");
                    case -2:
                        throw new Exception("No existe el periodista.");
                    case -3:
                        throw new Exception("Este periodista ya está asignado para esta noticia.");
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DataBase: " + ex.Message);
            }
        }
        internal void Eliminar(SqlTransaction pTransaction, string pCodInt)
        {
            //En este eliminar paso una Transaccion por parametro porque esta dentro de la operacion modificar noticia que tiene una transaccion.
            //NO lo hago porque este eliminar necesite en si una transaccion.
            SqlCommand cmd = new SqlCommand("BajaEscriben", pTransaction.Connection);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodInt", pCodInt);

            SqlParameter retorno = new SqlParameter();
            retorno.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(retorno);

            try
            {
                cmd.Transaction = pTransaction;
                cmd.ExecuteNonQuery();

                //esto no se si es necesario
                switch ((int)retorno.Value)
                 {
                     case 0:
                         throw new Exception("No se pudo eliminar el periodista de la noticia.");
                     case -1:
                         throw new Exception("La noticia no tiene periodistas asociados.");
                     default:
                         break;
                 }
            }
            catch (Exception ex)
            {
                throw new Exception("DataBase: " + ex.Message);
            }
        }
        internal List<int> List(string pCodInt)
        {
            SqlConnection cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand cmd = new SqlCommand("ListarEscriben", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodInt", pCodInt);

            List<int> list = null;

            try
            {
                cnn.Open();
                SqlDataReader lector = cmd.ExecuteReader();

                while (lector.HasRows)
                {
                    lector.Read();
                    list.Add((int)lector["CI"]);
                }

                lector.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("DataBase: " + ex.Message);
            }
            finally
            {
                cnn.Close();
            }
            return list;
        }
    }
}
