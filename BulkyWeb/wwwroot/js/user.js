var dataTable;

var languageStrings = {
    'en': {
        'languageUrl': "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/English.json",
        'lockButtonText': 'Locked',
        'unlockButtonText': 'Unlocked',
        'permissionButtonText': 'Permission',
    },
    'ru': {
        'languageUrl': "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Russian.json",
        'lockButtonText': 'Заблокирован',
        'unlockButtonText': 'Разблокирован',
        'permissionButtonText': 'Разрешение',
       
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
        "ajax": { url: '/admin/user/getall' },
        "language": {
            "url": languageUrl
        },
        "columns": [
            { data: 'name', "width":"15%" },
            { data: 'email', "width": "15%" },
            { data: 'phoneNumber', "width": "10%" },
            { data: 'company.name', "width": "15%" },
            { data: 'role', "width": "10%" },
            {
                data: {id:"id", lockoutEnd: "lockoutEnd"},
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();

                    if (lockout > today) {
                        return `
                        <div class="text-center">
                          <a onclick=LockUnlock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; ">
                                    <i class="bi bi-lock-fill"></i>  ${languageStrings[cultureInfo].lockButtonText}
                                </a> 
                            <a href="/admin/user/RoleManagement?id=${data.id}" class="btn btn-danger text-white" style="cursor:pointer; ";>
                            <i class="bi bi-unlock-fill"></i> ${languageStrings[cultureInfo].permissionButtonText}
                            </a>
                        </div>
                    `
                    }

                    else {
                        return `
                        <div class="text-center">
                           <a onclick=LockUnlock('${data.id}') class="btn btn-success text-white" style="cursor:pointer; ">
                                    <i class="bi bi-unlock-fill"></i>  ${languageStrings[cultureInfo].unlockButtonText}
                                </a>
                            <a  href="/admin/user/RoleManagement?id=${data.id}" class="btn btn-danger text-white" style="cursor:pointer;";>
                            <i class="bi bi-unlock-fill"></i> ${languageStrings[cultureInfo].permissionButtonText}
                            </a>
                        </div>
                        `
                    }
                }, 
               
            }
        ]
    } );
}


function LockUnlock(id) {
    $.ajax({
        type: "POST",
        url: '/Admin/User/LockUnlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
        }
    });
}