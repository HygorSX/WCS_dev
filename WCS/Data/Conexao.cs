using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCS
{
    class Conexao
    {
        // vamos nos conectar ao SQL Server Express e à base de dados
        // locadora usando Windows Authentication
        private static string connString = @"Data Source = 192.168.222.243; Initial Catalog = wfs; User ID = sa; Password = sa;Connect Timeout = 30; Encrypt=true;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        // representa a conexão com o banco
        private static SqlConnection conn = null;

        // método que permite obter a conexão
        public static SqlConnection ObterConexao()
        {
            // vamos criar a conexão
            conn = new SqlConnection(connString);

            // a conexão foi feita com sucesso?
            try
            {
                // abre a conexão e a devolve ao chamador do método
                conn.Open();
            }
            catch (SqlException sqle)
            {
                conn = null;
                // ops! o que aconteceu?
                // uma boa idéia aqui é gravar a exceção em um arquivo de log
            }

            return conn;
        }

        public static void fecharConexao()
        {
            if (conn != null)
            {
                conn.Close();
            }
        }
    }
}

