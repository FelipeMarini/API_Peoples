using Senai.Peoples.WebAPI.Domains;
using Senai.Peoples.WebAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Senai.Peoples.WebAPI.Repositories
{
    public class FuncionarioRepository : IFuncionarioRepository
    {

        private string stringConexao = " data source=DESKTOP-7SJR3UU\\SQLEXPRESS; initial catalog=T_Peoples; user id=sa; pwd=senai@132 ";  // escrever certinho


        public void AtualizarIdUrl(int id, FuncionarioDomain funcionario)
        {

            using (SqlConnection con = new SqlConnection(stringConexao))
            {

                string queryUpdateIdUrl = "UPDATE Funcionarios SET nome = @Nome, sobrenome = @Sobrenome WHERE idFuncionario = @ID";

                using (SqlCommand cmd = new SqlCommand(queryUpdateIdUrl,con))
                {

                    cmd.Parameters.AddWithValue("@ID",id);
                    cmd.Parameters.AddWithValue("@Nome",funcionario.nome);
                    cmd.Parameters.AddWithValue("@Sobrenome",funcionario.sobrenome);

                    con.Open();

                    cmd.ExecuteNonQuery();

                }

            }

        }

        
        public FuncionarioDomain BuscarPorId(int id)
        {

            using (SqlConnection con = new SqlConnection(stringConexao))
            {

                string querySelectById = "SELECT idFuncionario,nome,sobrenome FROM Funcionarios WHERE idFuncionario = @ID";

                con.Open();

                SqlDataReader rdr;


                using (SqlCommand cmd = new SqlCommand(querySelectById,con))
                {
                    cmd.Parameters.AddWithValue("@ID", id);

                    rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {

                        FuncionarioDomain funcionarioBuscado = new FuncionarioDomain
                        {

                            idFuncionario = Convert.ToInt32(rdr["idFuncionario"]),

                            nome = rdr["nome"].ToString(),

                            sobrenome = rdr["sobrenome"].ToString()

                        };

                        return funcionarioBuscado;

                    }

                    return null;

                }

            }

        }

        
        
        public void Cadastrar(FuncionarioDomain novoFuncionario)
        {

            using (SqlConnection con = new SqlConnection(stringConexao))
            {

                string queryInsert = "INSERT INTO Funcionarios(nome,sobrenome) VALUES(@nome,@sobrenome)";


                using (SqlCommand cmd = new SqlCommand(queryInsert,con))
                {

                    cmd.Parameters.AddWithValue("@nome",novoFuncionario.nome);
                    cmd.Parameters.AddWithValue("@sobrenome",novoFuncionario.sobrenome);

                    con.Open();

                    cmd.ExecuteNonQuery();

                }

            }

        }

        
        
        public void Deletar(int id)
        {

            using (SqlConnection con = new SqlConnection(stringConexao))
            {

                string queryDelete = "DELETE FROM Funcionarios WHERE idFuncionario = @ID";


                using (SqlCommand cmd = new SqlCommand(queryDelete,con))
                {

                    cmd.Parameters.AddWithValue("@ID",id);

                    con.Open();

                    cmd.ExecuteNonQuery();

                }

            }

        }

        
        
        public List<FuncionarioDomain> ListarTodos()
        {

            List<FuncionarioDomain> listaFuncionarios = new List<FuncionarioDomain>();

            using (SqlConnection con = new SqlConnection(stringConexao))
            {

                string querySelectAll = "SELECT idFuncionario,nome,sobrenome FROM Funcionarios";

                con.Open();

                SqlDataReader rdr;


                using (SqlCommand cmd = new SqlCommand(querySelectAll,con))
                {

                    rdr = cmd.ExecuteReader();

                    
                    while (rdr.Read())
                    {

                        FuncionarioDomain funcionario = new FuncionarioDomain
                        {

                            idFuncionario = Convert.ToInt32(rdr["idFuncionario"]),

                            nome = rdr["nome"].ToString(),

                            sobrenome = rdr["sobrenome"].ToString()
                        
                        };

                        listaFuncionarios.Add(funcionario);

                    }

                }

            }

            return listaFuncionarios;

        }
    
    
    
    
    }
}
