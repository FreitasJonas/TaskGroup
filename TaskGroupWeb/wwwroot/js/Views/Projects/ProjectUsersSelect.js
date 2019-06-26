$('#btnAdd').click(function () {
    let userName = $('#usersSubscribe option:selected').text();
    let userId = $('#usersSubscribe option:selected').val();

    if (userName == '-- Selecione --') {
        return;
    }

    let userAlreadySelected = false;

    $('#usersSelected li').each(function (i) {

        let _userId = $(this).attr("data-userId");

        if (userId == _userId) {
            userAlreadySelected = true;
        }
    })

    if (!userAlreadySelected) {

        addUserSelected(userId, userName);
    }
    else {
        swal("", "Usuário já adicionado");
    }
})

$('#usersSelected').on('click', '.item-user', function (event) {
    event.stopPropagation();
    event.stopImmediatePropagation();
    $(this).parent().remove();
})

function addUserSelected(userId, userName) {
    $('#usersSelected').append(`<li data-userId="${userId}"> ${userName}  
                        <span class="item-user alert-danger"> &nbsp; x &nbsp; </span>
                    </li>`);
}