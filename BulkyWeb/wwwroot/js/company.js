var dataTable;

var languageStrings = {
    'en': {
        'languageUrl': "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/English.json",
        'deleteConfirmationTitle': 'Are you sure?',
        'deleteConfirmationText': 'You won\'t be able to revert this!',
        'deleteConfirmationButtonText': 'Yes, delete it!',
        'editButtonText': 'Edit',
        'deleteButtonText': 'Delete',
        deleteCancelButtonText: 'Cancel',
        'successMessage': 'Operation successful!'
    },
    'ru': {
        'languageUrl': "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Russian.json",
        'deleteConfirmationTitle': 'Вы уверены?',
        'deleteConfirmationText': 'Вы не сможете отменить это действие!',
        'deleteConfirmationButtonText': 'Да, удалить!',
        'editButtonText': 'Редактировать',
        'deleteButtonText': 'Удалить',
        deleteCancelButtonText: 'Отмена',
        'successMessage': 'Операция выполнена успешно!'
    }
};


$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    var languageUrl = languageStrings[cultureInfo].languageUrl;

    if ($.fn.DataTable.isDataTable('#tblData')) {
        $('#tblData').DataTable().destroy();
    }

    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/company/getall' },
        "language": {
            "url": languageUrl
        },
        "columns": [
            { data: 'name', "width":"25%" },
            { data: 'streetAddress', "width": "15%" },
            { data: 'city', "width": "10%" },
            { data: 'state', "width": "15%" },
            { data: 'phoneNumber', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/admin/company/upsert?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> ${languageStrings[cultureInfo].editButtonText}</a>               
                     <a onClick=Delete('/admin/company/delete/${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> ${languageStrings[cultureInfo].deleteButtonText}</a>
                    </div>`
                },
                "width": "25%"
            }
        ]
    } );
}

function Delete(url) {
    Swal.fire({
        title: languageStrings[cultureInfo].deleteConfirmationTitle,
        text: languageStrings[cultureInfo].deleteConfirmationText,
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: languageStrings[cultureInfo].deleteConfirmationButtonText,
        cancelButtonText: languageStrings[cultureInfo].deleteCancelButtonText
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
           
        }
    })
}



