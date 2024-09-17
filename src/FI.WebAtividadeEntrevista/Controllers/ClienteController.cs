using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using FI.WebAtividadeEntrevista.Models;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
            BoBeneficiarios boBeneficiarios = new BoBeneficiarios();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else if(bo.VerificarExistencia(model.Cpf))
            {
                Response.StatusCode = 400;
                return Json($"O CPF {model.Cpf} já está cadastrado");
            }
            else
            {
                
                model.Id = bo.Incluir(new Cliente()
                {                    
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    Cpf = model.Cpf,
                });

                foreach (var beneficiario in model.Beneficiarios)
                {
                    if (boBeneficiarios.VerificarExistencia(beneficiario.CPF, beneficiario.Id))
                        return Json("Beneficiário já cadastrado com esse CPF");
                    else
                    {
                        beneficiario.Id = boBeneficiarios.Incluir(new Beneficiarios()
                        {
                            Nome = beneficiario.Nome,
                            CPF = beneficiario.CPF,
                            IdCliente = model.Id
                        });
                    }
                }

                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente bo = new BoCliente();
            BoBeneficiarios boBeneficiarios = new BoBeneficiarios();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            //if (bo.VerificarExistencia(model.Cpf))
            //{
            //    Response.StatusCode = 400;
            //    return Json($"O CPF {model.Cpf} já está cadastrado");
            //}
            else
            {
                bo.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone,
                    Cpf = model.Cpf,
                });

                foreach (var beneficiario in model.Beneficiarios)
                {
                    if (beneficiario.Id == 0)
                    {
                        if (boBeneficiarios.VerificarExistencia(beneficiario.CPF, model.Id))
                            return Json("Beneficiário já cadastrado com esse CPF");
                        else
                        {
                            beneficiario.Id = boBeneficiarios.Incluir(new Beneficiarios()
                            {
                                Nome = beneficiario.Nome,
                                CPF = beneficiario.CPF,
                                IdCliente = model.Id
                            });
                        }
                    }
                    else
                    {
                        boBeneficiarios.Alterar(new Beneficiarios()
                        {
                            Id = beneficiario.Id,
                            Nome = beneficiario.Nome,
                            CPF = beneficiario.CPF
                        });
                    }
                }

                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);

            BoBeneficiarios boBeneficiarios = new BoBeneficiarios();
            List<Beneficiarios> beneficiarios = boBeneficiarios.Listar(id);
            List<BeneficiarioModel> beneficiariosModel = new List<BeneficiarioModel>();
            foreach (var beneficiario in beneficiarios)
            {
                BeneficiarioModel beneficiarioModel = new BeneficiarioModel
                {
                    Id = beneficiario.Id,
                    Nome = beneficiario.Nome,
                    CPF = beneficiario.CPF,
                    IdCliente = cliente.Id,
                };
                beneficiariosModel.Add(beneficiarioModel);
            }

            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    Cpf = cliente.Cpf,
                    Beneficiarios = beneficiariosModel
                };

            
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}