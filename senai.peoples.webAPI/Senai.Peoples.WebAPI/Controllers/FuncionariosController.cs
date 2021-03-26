using Microsoft.AspNetCore.Mvc;
using Senai.Peoples.WebAPI.Domains;
using Senai.Peoples.WebAPI.Interfaces;
using Senai.Peoples.WebAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Senai.Peoples.WebAPI.Controllers
{

    [Produces("application/json")]

    [Route("api/[controller]")]

    [ApiController]

    public class FuncionariosController : ControllerBase
    {
        private IFuncionarioRepository _funcionarioRepository { get; set; }

        public FuncionariosController()
        {

            _funcionarioRepository = new FuncionarioRepository();

        }


        
        // listar todos os funcionários
        [HttpGet]
        public IActionResult Get()
        {

            List<FuncionarioDomain> listaFuncionarios = _funcionarioRepository.ListarTodos();

            return Ok(listaFuncionarios);  // status 200

        }


        
        // buscar funcionário por id
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {

            FuncionarioDomain funcionarioBuscado = _funcionarioRepository.BuscarPorId(id);

            if (funcionarioBuscado == null)
            {

                return NotFound("nenhum funcionário encontrado, verifique os dados inseridos por favor");  // status 404

            }

            return Ok(funcionarioBuscado);  // aqui não precisa do "else", status 200

        }



        // cadastrar funcionário no banco de dados
        [HttpPost]
        public IActionResult Post(FuncionarioDomain novoFuncionario)
        {

            _funcionarioRepository.Cadastrar(novoFuncionario);

            return StatusCode(201);   // status "created" = 201

        }



        // deletar funcionário
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            _funcionarioRepository.Deletar(id);

            return StatusCode(204);   // status "no content" = 204

        }



        // atualizar registro de funcionário existente passando seu id pela url(recurso) da requisição no Postman 
        [HttpPut("{id}")]
        public IActionResult PutIdUrl(int id,FuncionarioDomain funcionarioAtualizado)
        {

            FuncionarioDomain funcionarioBuscado = _funcionarioRepository.BuscarPorId(id);

            if (funcionarioBuscado == null)
            {

                    return NotFound
                    (
                        new
                        {
                            mensagem = "funcionário não encontrado",
                            erro = true
                        }
                    );

            }


            try
            {

                _funcionarioRepository.AtualizarIdUrl(id, funcionarioAtualizado);

                return NoContent();   // status 204

            }
            
            
            catch (Exception codErro)
            {

                return BadRequest(codErro);   // status 400
                
            }

        }


    
    
    
    }
}
