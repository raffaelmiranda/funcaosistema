
$(document).ready(function () {
    document.getElementById('Cpf').addEventListener('input', function (e) {
        let value = e.target.value.replace(/\D/g, '');
        value = value.replace(/(\d{3})(\d)/, '$1.$2');
        value = value.replace(/(\d{3})(\d)/, '$1.$2');
        value = value.replace(/(\d{3})(\d{1,2})$/, '$1-$2');
        e.target.value = value;
    });

    document.getElementById('CPFBeneficiario').addEventListener('input', function (e) {
        let value = e.target.value.replace(/\D/g, '');
        value = value.replace(/(\d{3})(\d)/, '$1.$2');
        value = value.replace(/(\d{3})(\d)/, '$1.$2');
        value = value.replace(/(\d{3})(\d{1,2})$/, '$1-$2');
        e.target.value = value;
    });

    preencherCampos(obj);

    //if (obj) {
    //    $('#formCadastro #Nome').val(obj.Nome);
    //    $('#formCadastro #CEP').val(obj.CEP);
    //    $('#formCadastro #Email').val(obj.Email);
    //    $('#formCadastro #Sobrenome').val(obj.Sobrenome);
    //    $('#formCadastro #Nacionalidade').val(obj.Nacionalidade);
    //    $('#formCadastro #Estado').val(obj.Estado);
    //    $('#formCadastro #Cidade').val(obj.Cidade);
    //    $('#formCadastro #Logradouro').val(obj.Logradouro);
    //    $('#formCadastro #Telefone').val(obj.Telefone);
    //    $('#formCadastro #Cpf').val(obj.Cpf);
    //}

    $('#formCadastro').submit(function (e) {
        e.preventDefault();

        var clienteData = {
            "Nome": $("#Nome").val(),
            "CEP": $("#CEP").val(),
            "Email": $("#Email").val(),
            "Sobrenome": $("#Sobrenome").val(),
            "Nacionalidade": $("#Nacionalidade").val(),
            "Estado": $("#Estado").val(),
            "Cidade": $("#Cidade").val(),
            "Logradouro": $("#Logradouro").val(),
            "Telefone": $("#Telefone").val(),
            "Cpf": $("#Cpf").val(),
            "Beneficiarios": []
        };

        var beneficiarios = document.getElementById("listaBeneficiarios");

        if (beneficiarios.rows.length > 0) {

            for (var i = 0; i < beneficiarios.rows.length; i++) {
                var row = beneficiarios.rows[i];

                var nome = row.cells[1].innerText;
                var cpf = row.cells[0].innerText;

                var beneficiario = {
                    "Nome": nome,
                    "CPF": cpf
                };

                clienteData.Beneficiarios.push(beneficiario);
            }
        }

        $.ajax({
            url: urlPost,
            method: "POST",
            data: clienteData,
            error:
            function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
            success:
            function (r) {
                ModalDialog("Sucesso!", r)
                $("#formCadastro")[0].reset();                                
                window.location.href = urlRetorno;
            }
        });
    })
    
})

function preencherCampos(obj) {
    document.getElementById('Nome').value = obj.Nome;
    document.getElementById('Sobrenome').value = obj.Sobrenome;
    document.getElementById('Cpf').value = obj.Cpf;
    document.getElementById('Nacionalidade').value = obj.Nacionalidade;
    document.getElementById('CEP').value = obj.CEP;
    document.getElementById('Estado').value = obj.Estado;
    document.getElementById('Cidade').value = obj.Cidade;
    document.getElementById('Logradouro').value = obj.Logradouro;
    document.getElementById('Email').value = obj.Email;
    document.getElementById('Telefone').value = obj.Telefone;

    var listaBeneficiarios = document.getElementById("listaBeneficiarios");

    listaBeneficiarios.innerHTML = '';

    for (var i = 0; i < obj.Beneficiarios.length; i++) {
        var beneficiario = obj.Beneficiarios[i];
        var cpf = beneficiario.CPF;
        var nome = beneficiario.Nome;

        var newRow = document.createElement("tr");

        newRow.innerHTML = `
            <td>${cpf}</td>
            <td>${nome}</td>
            <td style="text-align:right">
                <button class="btn btn-info btn-sm" onclick="">Alterar</button>
                <button class="btn btn-danger btn-sm" onclick="excluirBeneficiario(this)">Excluir</button>
            </td>
        `;

        listaBeneficiarios.appendChild(newRow);
    }
}

function openModal() {
    $('#modalBeneficiario').modal('show');
}

function incluirBeneficiario() {
   
    var cpf = document.getElementById("CPFBeneficiario").value.trim();
    var nome = document.getElementById("NomeBeneficiario").value.trim();


    if (cpf === '' || nome === '') {
        alert('Por favor, preencha todos os campos antes de incluir um beneficiário.');
        return;
    }

    var listaBeneficiarios = document.getElementById("listaBeneficiarios");
    for (var i = 0; i < listaBeneficiarios.rows.length; i++) {
        var cpfExistente = listaBeneficiarios.rows[i].cells[0].innerText;
        if (cpfExistente === cpf) {
            alert('O CPF informado já existe na lista.');
            return;
        }
    }

    var newRow = document.createElement("tr");

    newRow.innerHTML = `
            <td>${cpf}</td>
            <td>${nome}</td>
            <td style="text-align:right">
                <button class="btn btn-info btn-sm" onclick="">Alterar</button>
                <button class="btn btn-danger btn-sm" onclick="excluirBeneficiario(this)">Excluir</button>
            </td>
        `;

    document.getElementById("listaBeneficiarios").appendChild(newRow);


    document.getElementById("CPFBeneficiario").value = "";
    document.getElementById("NomeBeneficiario").value = "";
}
function excluirBeneficiario(btnExcluir) {
    // Obter a linha correspondente ao botão "Excluir" clicado
    var row = btnExcluir.closest('tr');

    // Remover a linha da tabela listaBeneficiarios
    row.remove();
}
function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
}
