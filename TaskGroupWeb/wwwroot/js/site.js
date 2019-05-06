const SUCESSO = 0;
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
