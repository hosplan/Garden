$('.gardenSpace_select2').select2();

let gardenSpace_option = document.getElementById('Garden_list');
gardenSpace_option = gardenSpace_option.options[gardenSpace_option.selectedIndex].value;

let updatePermission = '';
let deletePermission = '';
if (document.getElementById('permission_update').value == 'false') {
    updatePermission = 'disabled';
}

if (document.getElementById('permission_delete').value == 'false') {
    deletePermission = 'disabled';
}

//다른 정원 정보 불러오기
function GetOtherGarden() {
    let change_gardenSpace_option = document.getElementById('Garden_list');
    change_gardenSpace_option = change_gardenSpace_option.options[change_gardenSpace_option.selectedIndex].value;

    let url = '/GardenUsers/GetGardenUserList?id=' + change_gardenSpace_option + '';
    gardenUser_dataTable.ajax.url(url).load();
}

//baseSubType - datatable
var gardenUser_dataTable = $('#gardenUser_dt').DataTable({
    'ajax': {
        'url': "/GardenUsers/GetGardenUserList?id=" + gardenSpace_option+"",
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
                return '<button type="button" class="btn btn-link p-0"  onclick="openModal(\'GardenUsers\',\'Details\',\'' + row.id + '\')">' + data + '</button>';
            }
        },
        {
            'data': 'name', 'className': 'text-center m-2',
            'render': function (data, type, row, meta) {
                return '<button type="button" class="btn btn-link p-0"  onclick="openModal(\'GardenUsers\',\'Details\',\'' + row.id + '\')">' + data + '</button>';
            }
        },
        //{ 'data': 'activeDate', 'className': 'text-center m-2' },
        { 'data': 'activeDate', 'className': 'text-center m-2' },
        {
            'data': 'isActive', 'className': 'text-center m-2',
            'render': function (data, type, row, meta) {
                let isCheck = '';
                if (data == true) {
                    isCheck = 'checked';
                }
                return '<input type="checkbox" data-permission="update" style="width:25px; height:25px;" ' + updatePermission + ' onclick="changeIsActive(this, '+row.id+')" ' + isCheck + '/>';
            }
        },     
        {
            'data': 'id', 'clssName': 'm-2', 'orderable': false,
            'render': function (data, type, row, meta) {
                return '<button type="button" data-permission="delete" class="btn btn-link text-danger p-0 ml-3 float-right" ' + deletePermission + ' onclick="removeValue(' + data + ')"><i class="fas fa-trash"></i></button>' +
                       '<a data-permission="update" href="GardenUsers/Edit/?id='+data+'"; class="btn btn-link text-success p-0 ml-3 float-right"><i class="fas fa-brush"></i></a>';                  
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

//활성화시키기
function changeIsActive(obj, gardenUserId) {
    let isActive = obj.checked;
  
    let httpRequest = new XMLHttpRequest();
    if (!httpRequest) {
        errorMessage();
        return false;
    }

    httpRequest.open('POST', '/GardenUsers/ChangeIsActive', true);
    httpRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    httpRequest.onload = function () {
        console.log(this.status);
        if (this.status !== 200 && this.response != true) {
            errorMessage();
        }
    };
    httpRequest.send('isActive=' + isActive + '&gardenUserId=' + gardenUserId);
    
}

//유저 및 정원 관리사 동시 생성
function createOnlyGardenUser() {
    let gardenSpace_option = document.getElementById('Garden_list');
    gardenSpace_option = gardenSpace_option.options[gardenSpace_option.selectedIndex].value;
    window.location.href = "/GardenUsers/CreateForOnlyGardenUser?gardenSpaceId=" + gardenSpace_option + "";
}

//정원 유저 생성
function createGardenUser() {
    let gardenSpace_option = document.getElementById('Garden_list');
    gardenSpace_option = gardenSpace_option.options[gardenSpace_option.selectedIndex].value;
    openModal('GardenUsers', 'Create', gardenSpace_option); 
}

//정원 유저 
function removeValue(gardenUserId) {
    Swal.fire({
        title: '해당 정원관리사를 삭제 하시겠어요?',
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

            httpRequest.open('POST', '/GardenUsers/DeleteGardenUser', true);
            httpRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
            httpRequest.onload = function () {
                if (this.status === 200 || this.response == "true") {
                    location.reload();                  
                } else {
                    errorMessage();
                }
            };
            httpRequest.send('gardenUserId=' + gardenUserId);
        }
    });
}