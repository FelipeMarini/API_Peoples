using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using senai_filmes_webAPI.Domains;
using senai_filmes_webAPI.Interfaces;
using senai_filmes_webAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Controller responsável pelos endpoints(pontos onde uma requisição é feita através de uma url = recurso) referentes aos gêneros
/// </summary>
namespace senai_filmes_webAPI.Controllers
{
    
    // define que o tipo de resposta da API será no formato json
    [Produces("application/json")]

    // define que a rota de uma requisição será no formato dominio/api/nomeController
    // ex: http://localhost:5000/api/generos (tanto faz generos ou Generos)
    [Route("api/[controller]")]

    // define que é um controlador de API
    [ApiController]
    
    public class GenerosController : ControllerBase
    {
        /// <summary>
        /// objeto _generoRepository que irá receber todos os métodos definidos na interface IGeneroRepository
        /// </summary>
        private IGeneroRepository _generoRepository { get; set; }
        
        
        
        /// <summary>
        /// método construtor que instancia o objeto _generoRepository para trazer os métodos de GeneroRepository
        /// </summary>
        public GenerosController()
        {
            _generoRepository = new GeneroRepository();
        }

        
        
        /// <summary>
        /// lista todos os gêneros
        /// </summary>
        /// <returns> Uma lista de gêneros e um status code </returns>
        [HttpGet]
        public IActionResult Get()
        {
            // cria uma lista chamado listaGeneros para receber os dados
            List<GeneroDomain> listaGeneros = _generoRepository.ListarTodos();

            // retorna o status code 200(ok) com a lista de gêneros no formato json
            return Ok(listaGeneros);
        }



        /// <summary>
        /// busca um gênero através do seu id
        /// </summary>
        /// <param name="id"> id do gênero que será buscado </param>
        /// <returns> um gênero buscado ou Not Found(404) caso nenhum gênero seja encontrado </returns>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {

            // cria objeto que irá receber os dados do gênero buscado no banco de dados
            GeneroDomain generoBuscado = _generoRepository.BuscarPorId(id);

            
            
            //  = ---> atribuição       == ---> comparação
            
            // verifica se nenhum gênero foi encontrado (null)
            if (generoBuscado == null)
            {
                // retorna StatusCode 404 com mensagem se não for encontrado o gênero
                return NotFound("Nenhum gênero encontrado ");

            }

            // retorna StatusCode 200 retorna o gênero buscado caso seja encontrado o gênero
            return Ok(generoBuscado);

        }

        
        
        
        /// <summary>
        /// dadastra um novo gênero
        /// </summary>
        /// <returns> um status code 201 (created) </returns>
        [HttpPost]
        public IActionResult Post(GeneroDomain novoGenero)
        {
            // faz a chamada para o método Cadastrar() do GeneroRepository
            _generoRepository.Cadastrar(novoGenero);

            // retorna o status code
            return StatusCode(201);

        }


        
        /// <summary>
        /// atualiza um gênero existente passando o seu id pela URL(recurso) da requisição
        /// </summary>
        /// <param name="id"> id do gênero que será atualizado </param>
        /// <param name="generoAtualizado"> objeto generoAtualizado com as novas informações </param>
        /// <returns> um status code </returns>
        /// Ex: http://localhost:5000/api/generos/4
        [HttpPut("{id}")] // atualiza todos os campos ([HttpPatch] atualiza apenas um campo da tabela)
        public IActionResult PutIdUrl(int id, GeneroDomain generoAtualizado)
        {

            // cria objeto generoBuscado que irá receber o gênero alvo de atualização do banco de dados
            GeneroDomain generoBuscado = _generoRepository.BuscarPorId(id);

            // caso o gênero não seja encontrado na busca, é retornado NotFound(404) com mensagem personalizada e um bool para apresentar que houve erro
            if (generoBuscado == null)
            {
                return NotFound      // poderia ser:"return StatusCode(404);"
                    (

                        new
                        {
                            mensagem = "Gênero não encontrado",
                            erro = true
                        }

                    );    
            }

            
            // tenta atualizar o registro (gênero)
            try  // estrutura para tratamento de erros e não parar a aplicação se houver erro (comandos dentro de "catch" são acionados)
            {

                // faz a chamada para o método AtualizarIdUrl() 
                _generoRepository.AtualizarIdUrl(id,generoAtualizado);

                // retorna status code 204(No Content)
                return NoContent();                       // ---> ou "return StatusCode(204);"
            }
            
            // caso ocorra erro
            catch (Exception codErro)
            {
                
                // retorna status code 400 (Bad Request) e o código do erro (codErro)
                return BadRequest(codErro);              // ---> ou "return StatusCode(400);"
            
            }


        }


        /// <summary>
        /// Atualiza um gênero existente passando seu id pelo corpo da requisição (raw body em formato JSON na requisição do Postman)
        /// </summary>
        /// <param name="generoAtualizado"> objeto generoAtualizado com as novas informações </param>
        /// <returns> um status code </returns>
        [HttpPut]
        public IActionResult PutIdBody(GeneroDomain generoAtualizado)
        {

            // cria objeto generoBuscado que irá receber o gênero alvo de atualização do banco de dados
            GeneroDomain generoBuscado = _generoRepository.BuscarPorId(generoAtualizado.idGenero);

            // verificar se o gênero foi encontrado
            if (generoBuscado != null)
            {
                
                // tenta atualizar o registro
                try
                {
                    // faz a chamada para o método AtualizarIdCorpo()
                    _generoRepository.AtualizarIdCorpo(generoAtualizado);

                    // retorna o status code 204
                    return NoContent();
                }
                
                // caso ocorra erro:
                catch (Exception codErro)
                {

                    return BadRequest(codErro);
                   
                }
            
            }


            // caso não seja encontrado o gênero, retorna status code 404 (Not Found) com uma mensagem personalizada
            return NotFound
                (
                    new
                    {
                        mensagem = "Gênero não encontrado"
                    }
                );

        }

        
        
        /// <summary>
        /// deleta um gênero existente
        /// </summary>
        /// <param name="id"> id do gênero que será deletado </param>
        /// <returns> um status code 204 (no content) </returns>
        
        //altera a rota  ex: http://localhost:5000/api/generos/id
        [HttpDelete("{id}")]

        public IActionResult Delete(int id)
        {

            // faz a chamada para o método Deletar()
            _generoRepository.Deletar(id);

            // retorna o status code
            return StatusCode(204);

        }


    
    }

}
