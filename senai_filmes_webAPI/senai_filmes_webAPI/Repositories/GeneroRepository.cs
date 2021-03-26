
using senai_filmes_webAPI.Domains;
using senai_filmes_webAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace senai_filmes_webAPI.Repositories
{

    /// <summary>
    /// Classe responsável pelo repositório dos gêneros
    /// </summary>
    public class GeneroRepository : IGeneroRepository
    {
        
        /// <summary>
        /// string de conexão com o banco de dados e recebe os parâmetros:
        /// 
        /// data source = nome do servidor
        /// 
        /// initial catalog = nome do banco de dados
        /// 
        /// user id=sa e pwd=senai@132 --> autenticação com logon e senha do usuário do SQL Server
        /// 
        /// integrated security = true --> autenticação do usuário do sistema (Windows)
        /// </summary>
        private string stringConexao = "data source=DESKTOP-7SJR3UU\\SQLEXPRESS; initial catalog=Filmes; user id=sa; pwd=senai@132";

        
        

        /// <summary>
        /// atualiza um gênero existente passando seu id pelo corpo da requisição ( raw body em formato JSON no Postman)
        /// </summary>
        /// <param name="genero"> objeto genero com as novas informações </param>
        public void AtualizarIdCorpo(GeneroDomain genero)
        {
            // declara a SqlConnection con passando a string de conexão como parâmetro
            using (SqlConnection con = new SqlConnection(stringConexao))
            {

                // declara a query a ser executada
                string queryUpdateIdBody = "UPDATE FROM Generos SET Nome = @Nome WHERE idGenero = @ID";

                // declara o cmd passando a query e conexão como parâmetros
                using (SqlCommand cmd = new SqlCommand(queryUpdateIdBody, con))
                {

                    // passa os valores para os parâmetros
                    cmd.Parameters.AddWithValue("@ID", genero.idGenero); // ordem da query aqui não importa
                    cmd.Parameters.AddWithValue("@Nome", genero.nome);

                    // abre a conexão com o banco de dados
                    con.Open();

                    // executa o comando
                    cmd.ExecuteNonQuery();

                }

            }
        }

        
        

        /// <summary>
        /// atualiza um gênero passando o id pelo recurso (URL)
        /// </summary>
        /// <param name="id"> id do gênero que será atualizado </param>
        /// <param name="genero"> objeto nomeado gênero com as novas informações </param>
        public void AtualizarIdUrl(int id, GeneroDomain genero)
        {
            // declara a SqlConnection con passando a string de conexão como parâmetro
            using (SqlConnection con = new SqlConnection(stringConexao))
            {
                
                // declara a query a ser executada
                string queryUpdateIdUrl = "UPDATE Generos SET Nome = @Nome WHERE idGenero = @ID";

                // declara o cmd passando a query e conexão como parâmetros
                using (SqlCommand cmd = new SqlCommand(queryUpdateIdUrl,con))
                {

                    // passa os valores para os parâmetros
                    cmd.Parameters.AddWithValue("@ID",id); // ordem da query aqui não importa
                    cmd.Parameters.AddWithValue("@Nome",genero.nome);

                    // abre a conexão com o banco de dados
                    con.Open();

                    // executa o comando
                    cmd.ExecuteNonQuery();

                }

            }

        }

        

        
        /// <summary>
        /// busca um gênero através do seu id
        /// </summary>
        /// <param name="id"> id do gênero que será buscado </param>
        /// <returns> um gênero buscado ou null caso nao seja encontrado </returns>
        public GeneroDomain BuscarPorId(int id)
        {

            using (SqlConnection con = new SqlConnection(stringConexao))
            {
                
                // declara a query a ser executada
                string querySelectById = "SELECT idGenero, Nome  FROM Generos WHERE idGenero = @ID ";
                                                  //[0]    //[1]
                
                // abre conexão com o banco de dados
                con.Open();

                // declara o reader rdr para receber os valores do banco de dados
                SqlDataReader rdr;


                // declara o SqlCommand cmd da biblioteca System.Data.SqlClient passando a query e conexão con como parâmetros
                using (SqlCommand cmd = new SqlCommand(querySelectById,con))
                {

                    // passa o valor para o parâmetro @ID
                    cmd.Parameters.AddWithValue("@ID", id);

                    // executa a query e armazena os dados no rdr
                    rdr = cmd.ExecuteReader();

                    // verifica se o resultado da query retornou algum registro
                    if (rdr.Read() )
                    {
                        // instancia o objeto
                        GeneroDomain generoBuscado = new GeneroDomain
                        {
                            // atribue a propriedade idGenero o valor da coluna idGenero da tabela do banco de dados
                            idGenero = Convert.ToInt32(rdr["idGenero"]),

                            // atribue a propriedade nome o valor da coluna Nome da tabela do banco de dados
                            nome = rdr["Nome"].ToString()


                        };

                        
                        // retorna o gênero buscado com os dados obtidos
                        return generoBuscado;
                    
                    }

                    return null;
                
                }


            }

            


        }
        
        
        
        
        
        /// <summary>
        /// cadastra um novo gênero
        /// </summary>
        /// <param name="novoGenero"> Objeto da classe GeneroDomain com as informações que serão cadastradas</param>
        public void Cadastrar(GeneroDomain novoGenero)
        {
            // declara a conexão con passando stringConexao como parâmetro
            using (SqlConnection con = new SqlConnection(stringConexao))
            {
                
                // não usar com concatenação pois causa efeito "Joana D'Arc" e permite "SQL Injection"
                //Ex: string queryInsert = "INSERT INTO Generos(Nome) VALUES ('" + novoGenero.nome + "')";

                // declara a query a ser executada                      // pode passar mais de uma variável
                string queryInsert = "INSERT INTO Generos(Nome) VALUES (@Nome)";
                
                
                // declara SqlCommand nomeada cmd(command) passando a query que será executada e a conexão como parâmetros 
                using (SqlCommand cmd = new SqlCommand(queryInsert,con))
                {
                    
                    // passa o valor para o parâmetro @Nome
                    cmd.Parameters.AddWithValue("@Nome",novoGenero.nome);

                    // abre a conexão com o banco de dados
                    con.Open();

                    // executa a query
                    cmd.ExecuteNonQuery();

                }
            
            }

        }

        
        
        
        /// <summary>
        /// Deleta um gênero através do seu id
        /// </summary>
        /// <param name="id"> id do gênero que será deletado </param>
        public void Deletar(int id)
        {
            // declara o objeto con to tipo SqlConnection passando a string de conexão como parâmetro
            using (SqlConnection con = new SqlConnection(stringConexao) )
            {
                
                // declara a query a ser executada passando o parâmetro @id
                string queryDelete = "DELETE FROM Generos WHERE idGenero = @ID ";

                // declara o objeto do tipo SqlCommand cmd passando a query que ainda será executada e a conexão con como parâmetros
                using (SqlCommand cmd = new SqlCommand(queryDelete,con))
                {

                    // define o valor do id recebido no método como o valor do parâmetro @ID
                    cmd.Parameters.AddWithValue("@ID",id);

                    
                    // abre a conexão com o banco de dados
                    con.Open();


                    // executa o comando
                    cmd.ExecuteNonQuery();

                }

            }
        
        }
        
        
        
        
        
        /// <summary>
        /// Lista todos os gêneros 
        /// </summary>
        /// <returns> Lista com todos os gêneros </returns>
        public List<GeneroDomain> ListarTodos()
        {
            // lista criada onde serão armazenados os gêneros (objetos da classe GeneroDomain)
            List<GeneroDomain> listaGeneros = new List<GeneroDomain>();

            // declara a variável con do tipo SqlConnection passando a string de conexão como parâmetro
            using (SqlConnection con = new SqlConnection(stringConexao))
            {
                // declara a instrução do SQL Server Management Studio (DQL) a ser executada e definindo na ordem para posterior leitura do DQL pelo backend:
                // [0] = idGenero
                // [1] = Nome
                string querySelectAll = "SELECT idGenero, Nome FROM Generos";

                // abre a conexão com o banco de dados SQL Server (botão "conectar" na parte de autenticação autenticação do SQL Server Management Studio)
                con.Open();

                // declara SqlDataReader nomeado rdr para percorrer a tabela Generos no SQL Server (rdr = reader)
                SqlDataReader rdr;

                // declara o SqlCommand chamado cmd passando a query e a conexão como parâmetros (cmd = command)
                using (SqlCommand cmd = new SqlCommand(querySelectAll,con))
                {
                    // executa a query e armazena os dados no rdr
                    rdr = cmd.ExecuteReader();

                    // enquanto houver registros para serem lidos no rdr, o laço se repete
                    while (rdr.Read())
                    {
                        // instancia um objeto genero do tipo GeneroDomain
                        GeneroDomain genero = new GeneroDomain()
                        {
                            // atribue para a propriedade idGenero o valor da primeira coluna da tabela do banco de dados conforme ordem definida na querySelectAll
                            idGenero = Convert.ToInt32(rdr[0]),

                            // atribue para a propriedade nome o valor da segunda coluna da tabela do banco de dados conforme ordem definida na querySelectAll
                            nome = rdr[1].ToString()
                         };

                        listaGeneros.Add(genero);
                    
                    }

                }


            } // fim do using

            return listaGeneros;
        }
    
    
    
    
    
    }
}
