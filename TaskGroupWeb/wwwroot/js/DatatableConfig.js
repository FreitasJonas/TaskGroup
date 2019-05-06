function getDataTableConfig() {

    return {
        paging: true,
        searching: true,
        info: true,
        ordering: true,
        "iDisplayLength": 25,
        "dom": '<fl<t>ip>',
        "language": {
            "searchPlaceholder": 'Pesquisar...',
            "search": "_INPUT_",
            "lengthMenu": "_MENU_ ",
            "zeroRecords": "Nenhum registro encontrado",
            "info": "Exibindo página _PAGE_ de _PAGES_ de um total de _MAX_ registros",
            "infoEmpty": "Nenhum registro encontrado",
            "infoFiltered": "(filtrado de _MAX_ registros)",
            "paginate": {
                "first": "Primeiro",
                "last": "Último",
                "next": "Próximo",
                "previous": "Anterior"
            }
        },
        columnDefs: [
            {
                targets: 0,
                visible: false
            }],
        "order": []
    }
}

function Redirect(url) {
    window.location.href = url;
}