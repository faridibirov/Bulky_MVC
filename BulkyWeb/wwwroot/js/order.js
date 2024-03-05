var dataTable;

var languageStrings = {
    'en': {
        'languageUrl': "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/English.json",
        'orderStatus' : 'orderStatusEN'

    },
    'ru': {
        'languageUrl': "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Russian.json",
        'orderStatus': 'orderStatusRU'
    }
};

var languageUrl = languageStrings[cultureInfo].languageUrl;

var orderStatusProperty = languageStrings[cultureInfo].orderStatus;

$(document).ready(function () {
   

    var url = window.location.search;

    if (url.includes("inprocess")) {
        loadDataTable("inprocess");
    }
    else {
        if (url.includes("completed")) {
            loadDataTable("completed");
        }
        else {
            if (url.includes("paymentpending")) {
                loadDataTable("paymentpending");
            }
            else {
                if (url.includes("approved")) {
                    loadDataTable("approved");
                }
                else {
                    loadDataTable("all");

                }
            }
        }
    }
});

function loadDataTable(status) {
    if ($.fn.DataTable.isDataTable('#tblData')) {
        $('#tblData').DataTable().destroy();
    }

    dataTable = $('#tblData').DataTable({
        /*"language": {url: languageUrl},*/
        "ajax": { url: '/admin/order/getall?status=' + status },
        "columns": [
            { data: 'id', "width": "5%" },
            { data: 'name', "width": "25%" },
            { data: 'phoneNumber', "width": "20%" },
            { data: 'applicationUser.email', "width": "20%" },
            { data: orderStatusProperty, "width": "15%" },
            { data: 'orderTotal', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/admin/order/details?orderId=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i></a>               
                    </div>`
                },
                "width": "25%"
            }
        ]
    });
}




