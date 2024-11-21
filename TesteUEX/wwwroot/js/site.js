
function realizarLogin() {

    var url = 'http://localhost:5164/api/Usuarios/RealizarLogin';
    var json = {
        nome: document.getElementById("txtEmail").value,
        senha: document.getElementById("txtSenha").value
    }; 
    $.ajax({
        type: 'POST',
        url: url,
        data: JSON.stringify(json),
        contentType: 'application/json',
        success: function (dados) {
            if (dados == "Logado Com Sucesso") {
                // Storing the data:
                localStorage.setItem("login", document.getElementById("txtEmail").value);
                // Receiving the data:
                window.location.href = 'Cadastro';
            }
            if (dados == "Necessario fazer o cadastro do seu usuario ") {
                alert(dados)
            }
        }
    }); 
}
function carregarEndereco(teste, teste1) {

    document.getElementById("endereco").style.display = "inherit";
    var geocoder = new google.maps.Geocoder;
    var input = teste + ',' + teste1;
    var latlngStr = input.split(',', 2);
    var latlng = { lat: parseFloat(latlngStr[0]), lng: parseFloat(latlngStr[1]) };
    console.log(latlng);

    geocoder.geocode({ 'location': latlng }, function (results, status) {
        if (status === 'OK') {
            if (results[1]) {

                console.log(results[1].formatted_address);
                document.getElementById("endereco").innerHTML = results[1].formatted_address;

            } else {
                window.alert('No results found');
            }
        } else {
            window.alert('Geocoder failed due to: ' + status);
        }
    });
}

function carregarCidade(estado) {
    var selectCidade = document.getElementById("select_cidade");
    var selectEstado = document.getElementById("select_estado");
    var url = 'http://localhost:5164/api/Cidades/CarregarCidadePorEstado?id_estado=' + selectEstado.value;

    console.log(url)
    $.ajax({
        dataType: "json",
        type: 'POST',
        url: url,
        contentType: 'application/json',
        success: function (dados) {
            for (var i = 0; i < dados.length; i++) {
                var o = new Option(dados[i].nome, dados[i].id_cidade);
                selectCidade.appendChild(o);
            }
        }
    });
}

function carregarRuasSelecionadas() {
    var address = document.getElementById("Endereco").value;
    var selectRuas = document.getElementById("select_ruas");
    var geocoder = new google.maps.Geocoder;

    geocoder.geocode({ 'address': address }, function (results, status) {

        if (status == google.maps.GeocoderStatus.OK) {

            for (var i = 0; i < results.length; i++) {

                var o = new Option(results[i].formatted_address, JSON.stringify(results[i].geometry.location));
                selectRuas.appendChild(o);
            }
        }
    });
}

function selecionarRua() {

    var selectRuas2 = document.getElementById("select_ruas").value;
    let jsonObject = JSON.parse(selectRuas2);

    if (jsonObject.lng != null && jsonObject.lat != null) {

        var myCenter = new google.maps.LatLng(jsonObject.lat, jsonObject.lng);
        var mapCanvas = document.getElementById("map");
        var mapOptions = { center: myCenter, zoom: 15};
        var map = new google.maps.Map(mapCanvas, mapOptions);
        var marker = new google.maps.Marker({ position: myCenter });
        marker.setMap(map);
    }
}

function carregarSelectEstado(estado) {
    var url = 'http://localhost:5164/api/Estados';
    var selectEstado = document.getElementById("select_estado");

    $.ajax({
        dataType: "json",
        type: "GET",
        url: url,
        success: function (dados) {
            for (var i = 0; i < dados.length; i++) {
                var o = new Option(dados[i].sigla, dados[i].id_estado);
                selectEstado.appendChild(o);
            }
        }
    });
}

function buscarCepPorRua() {

    var rua = document.getElementById("Endereco").value;
    var selectBairro = document.getElementById("select_bairro");
    var uf = $("#select_estado option:selected").text();
    var cidade = $("#select_cidade option:selected").text();
    const url = `https://viacep.com.br/ws/${uf}/${cidade}/${rua}/json/`;

    $.ajax({
        dataType: "json",
        type: "GET",
        url: url,
        success: function (dados) {
            for (var i = 0; i < dados.length; i++) {
                var o = new Option(dados[i].bairro, dados[i].cep);
                document.getElementById("Endereco").value = dados[i].logradouro;
                selectBairro.appendChild(o);
            }
        }
    });
}


function selecionaBairro() {
    var selectBairro = document.getElementById("select_bairro").value; 
    document.getElementById("CEP").value = selectBairro;
}

function carregarListaContato() {
    
    var url = 'http://localhost:5164/api/Usuarios';

    columns = [];


    $.ajax({
        dataType: "json",
        type: "GET",
        url: url,
        success: function (data) {

            var rowData = data[0];   

            Object.keys(rowData).forEach(function (key, index) {
                    columns.push({
                        data: key,
                        title: key,
                    });
            }); 
            
            var table = $('#example').DataTable({
                data: data,
                "processing": true,
                    language: {
                        url: 'https://cdn.datatables.net/plug-ins/1.11.1/i18n/pt_br.json'
                }, 
                order: [[2, 'desc']],
                columns: [
                    { data: 'id_usuario' ,title:'id_usuario' },
                    { data: 'nome' ,title:'nome' },
                    { data: 'cpf' ,title:'cpf' },
                    { data: 'cep', title:'cep', },
                    { data: 'rua' ,title:'rua' },
                    { data: 'numero' ,title:'numero' },
                    { data: 'bairro', title:'bairro', },
                    { data: 'cidade' ,title:'cidade' },
                    { data: 'estado', title: 'estado' },
                    { data: 'telefone' ,title:'telefone'  }, 
                    {
                        data: null,
                        defaultContent: '<button>Excluir!</button>',
                        targets: -1
                    }
                ],
            });


            table.on('click', 'button', function (e) {
                let data = table.row(e.target.closest('tr')).data();
                console.log(data)
                confirmarExclusao(data);
            });
            
        }
    });
}

function confirmarExclusao(data) {
    let text = "Deseja realmente excluir o cadastro de:\n" + data.nome + "";

    var url = 'http://localhost:5164/api/Usuarios/';
    let person = prompt(text,"Digite a sua senha");
    if (person != null) {
        $.ajax({
            dataType: "json",
            type: "DELETE",
            url: url + data.id_usuario,
            success: function (dados) {
                alert(dados);
            }
        });
        document.getElementById("demo").innerHTML =  person ;
    }
}
function cadastrarUsuario() {

    var url = 'http://localhost:5164/api/Usuarios';

    var rua = $("#select_bairro option:selected").text();
    console.log(rua)
    var json = {
        nome: document.getElementById("Nome").value,
        CPF: document.getElementById("CPF").value,
        Cep: document.getElementById("CEP").value,
        Rua: rua,
        Numero: document.getElementById("Numero").value,
        Bairro: document.getElementById("bairro").value,
        Cidade: document.getElementById("select_cidade").value,
        Estado: document.getElementById("select_estado").value,
        Telefone: document.getElementById("telefone").value
    };

  

    $.ajax({
        type: 'POST',
        url: url,
        data: JSON.stringify(json),
        contentType: 'application/json',
        success: function (dados) {
            if (dados == "Cadastro feito com o sucesso") {
                alert(dados);
                setTimeout(function () {
                    window.location.reload(); // you can pass true to reload function to ignore the client cache and reload from the server
                }, 2000);
            }
        }
    });

}


function abrirModal(detalhes) {
    var id = $(detalhes).attr("data-bs-id");
    var url = $(detalhes).attr("data-bs-url");
     console.log(url)
    if (id != undefined) {
        $("#customModal").load(url + "/" + id, function () {
            $("#customModal").modal('show');
        });

    } else {
        console.log(url);
        $("#customModal").load(url, function () {
            $("#customModal").modal('show');
        });
    }

}