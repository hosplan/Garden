
let updatePermission = '';
let deletePermission = '';
if (document.getElementById('permission_update').value == 'false') {
    updatePermission = 'disabled';
}

if (document.getElementById('permission_delete').value == 'false') {
    deletePermission = 'disabled';
}

//baseSubType - datatable
var gardenAttendUser_dataTable = $('#gardenAttendUser_dt').DataTable({
    'ajax': {
        'url': "/GardenUserTaskMaps/GetAttendUserList?id=" + document.getElementById('GardenTaskId').value + "",
        'type': 'GET',
        'datatype': 'json'
    },
    'columns': [
        {
            'data': 'roleType', 'className': 'text-center m-2',
            'render': function (data,type,row,meta) {
                return '<button type="button" class="btn btn-link p-0" data-permission="update" ' + updatePermission+' onclick="openModal(\'GardenUsers\',\'EditForGardenUserRole\',\''+row.id+'\')"><span class="badge badge-primary shadow p-1">'+data+'</span></button>';
            }
        },
        {
            'data': 'userName', 'className': 'text-center m-2',
            'render': function (data, type, row, meta) {
                return '<button type="button" onclick="getAttendUserWorkTime('+row.id+')" class="btn btn-link p-0">'+data+'</button>';
            }
        },
        {
            'data': 'name', 'className': 'text-center m-2',
            'render': function (data, type, row, meta) {
                return '<button type="button" onclick="getAttendUserWorkTime('+row.id+')" class="btn btn-link p-0">' + data + '</button>';
            }
        },
        { 'data': 'regDate', 'className': 'text-center m-2' },   
        {
            'data': 'id', 'clssName': 'm-2', 'orderable': false,
            'render': function (data, type, row, meta) {
                return '<button type="button" data-permission="delete" class="btn btn-link text-danger p-0 ml-3 float-right" '+deletePermission+' onclick="removeValue('+data+')"><i class="fas fa-trash"></i></button>';                  
            }
        }
    ],
    'responsive': true,
    'deferRender': true,
    'ordering': true,
    'info': true,
    'paging': true,
    'pageLength': 10,
    'searching': true,
    'processing': true,
});

//정원 업무 참여자의 업무시간 정보 가져오기
function getAttendUserWorkTime(gardenUserTaskMapId) {
    let httpRequest = new XMLHttpRequest();
    if (!httpRequest) {
        errorMessage();
        return false;
    }

    httpRequest.open('POST', '/GardenUserTaskMaps/GetGardenUserWorkTime', true);
    httpRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    httpRequest.onload = function () {
        if (this.status === 200 || this.response == "true") {
            location.reload();
        }
        else if (this.response == "empty") {
            console.log("zz");
            showEmptyInfo();
        }
        else {
            errorMessage()
        }
    };
    httpRequest.send('gardenUserTaskMapId=' + gardenUserTaskMapId);
}

function showEmptyInfo() {
    document.getElementById('alert_empty_gardenWorkTime').style.display = 'block';
    document.getElementById('user_gardenWorkTime').style.display = 'none';
}

function createGardenUser() {
    let gardenSpace_option = document.getElementById('Garden_list');
    gardenSpace_option = gardenSpace_option.options[gardenSpace_option.selectedIndex].value;
    openModal('GardenUsers', 'Create', gardenSpace_option); 
}

function removeValue(gardenUserTaskMapId) {
    Swal.fire({
        title: '해당 업무참여자를 삭제 하시겠어요?',
        text : '관련된 업무가 모두 삭제됩니다!',
        icon: 'warning',
        confirmButtonText: '삭제',
        showCancelButton: true,
        cancelButtonText : '취소',        
    }).then(function (result) {
        if (result.value) {
            let httpRequest = new XMLHttpRequest();
            if (!httpRequest) {
                errorMessage();
                return false;
            }

            httpRequest.open('POST', '/GardenUserTaskMaps/DeleteAttendUser', true);
            httpRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
            httpRequest.onload = function () {
                if (this.status === 200 || this.response == "true") {
                    location.reload();                  
                } else {
                    errorMessage();
                }
            };
            httpRequest.send('gardenUserTaskMapId=' + gardenUserTaskMapId);
        }
    });
}