using FI.AtividadeEntrevista.DAL.Beneficiarios;
using System.Collections.Generic;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiarios
    {
        public long Incluir(DML.Beneficiarios Beneficiarios)
        {
            DaoBeneficiarios beneficiario = new DaoBeneficiarios();
            return beneficiario.Incluir(Beneficiarios);
        }

        public void Alterar(DML.Beneficiarios Beneficiarios)
        {
            DaoBeneficiarios beneficiario = new DaoBeneficiarios();
            beneficiario.Alterar(Beneficiarios);
        }

        public DML.Beneficiarios Consultar(long id)
        {
            DaoBeneficiarios beneficiario = new DaoBeneficiarios();
            return beneficiario.ConsultarBeneficiarios(id);
        }

        public void Excluir(long id)
        {
            DaoBeneficiarios beneficiario = new DaoBeneficiarios();
            beneficiario.Excluir(id);
        }

        public List<DML.Beneficiarios> Listar(long id)
        {
            DaoBeneficiarios beneficiario = new DaoBeneficiarios();
            return beneficiario.Listar(id);
        }

        public bool VerificarExistencia(string CPF, long id)
        {
            DaoBeneficiarios beneficiario = new DaoBeneficiarios();
            return beneficiario.VerificarExistencia(CPF, id);
        }
    }
}
