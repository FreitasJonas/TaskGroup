﻿const SUCESSO = 0;
const FALHA = 1;

function submitAssync(url, data, sucesso, falha) {

    $.ajax({
        method: "POST",
        url: url,
        data: data,
        success: function (response) {
            if (response.status == SUCESSO) {
                sucesso(response);
            }
            else {
                falha(response);
            }
        },
        error: function (error) {
            let response = { message: "Ocorreu um erro ao realizar a requisição [" + error + "]" };
            falha(response);
        }        
    })
}

function submitAssyncGet(url, data, sucesso, falha) {

    $.ajax({
        method: "GET",
        url: url,
        data: data,
        success: function (response) {
            if (response.status == SUCESSO) {
                sucesso(response);
            }
            else if (response.status == FALHA) {
                falha(response);
            }
        }
    })
}

function getDatePickerConfiguration() {
    return {
        dateFormat: 'dd/mm/yy',
        dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        nextText: 'Próximo',
        prevText: 'Anterior'
    };
}

function LoadingShow() {
    $(".modal-loading").show();
}

function LoadingHide() {
    $(".modal-loading").hide();
}

function ActiveProjectMenu(projectId) {

    console.log(projectId)

    let items = $('.project-item');
    for (let i = 0; i < items.length; i++) {

        $(items[i]).removeClass('active');
    }

    $('.project_' + projectId).addClass('active');
}

function ConfirmDefault(mensagem, acao, callback) {

    swal({
        title: "",
        text: mensagem,
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: acao,
        cancelButtonText: "Cancelar"
    },
        function () {

            callback();
        });

    $('.sa-button-container .cancel').focus();
}