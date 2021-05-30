$('.gardenSpace_select2').select2();
let gardenSpace_option = document.getElementById('Garden_list');
gardenSpace_option = gardenSpace_option.options[gardenSpace_option.selectedIndex].value;

//다른 정원 정보 불러오기
function GetOtherGarden() {
    let change_gardenSpace_option = document.getElementById('Garden_list');
    change_gardenSpace_option = change_gardenSpace_option.options[change_gardenSpace_option.selectedIndex].value;

    let url = '/GardenTasks/GetGardenTaskList?id=' + change_gardenSpace_option + '';
    baseSubType_dataTable.ajax.url(url).load();
}

//정원 업무삭제하기
function removeGardenTask(gardenTaskId) {
    Swal.fire({
        title: '해당 정원 업무를 삭제 하시겠어요?',
        text : '관련된 모든 정보가 삭제 됩니다.',
        icon: 'warning',
        confirmButtonText: '삭제',
        showCancelButton: true,
        cancelButtonText: '취소',
    }).then(function (result) {
        if (result.value) {
            let httpRequest = new XMLHttpRequest();
            if (!httpRequest) {
                errorMessage();
                return false;
            }

            httpRequest.open('POST', '/GardenTasks/DeleteGardenTask', true);
            httpRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
            httpRequest.onload = function () {
                if (this.status === 200 || this.response == "true") {
                    location.href = "/GardenTasks/Index";
                } else {
                    errorMessage();
                }
            };
            httpRequest.send('gardenTaskId=' + gardenTaskId);
        }
    });
}


//baseSubType - datatable
var baseSubType_dataTable = $('#gardenTask_dt').DataTable({
    'ajax': {
        'url': "/GardenTasks/GetGardenTaskList?id=" + gardenSpace_option+"",
        'type': 'GET',
        'datatype': 'json'
    },
    'columns': [
        {
            'data': 'typeName', 'className': 'text-center m-2',
            'render': function (data) {
                return '<span class="badge badge-primary p-1">'+data+'</span>';
            }
        },    
        {
            'data': 'name', 'className': 'text-left m-2',
            'render': function (data,type,row,meta) {
                return '<a href="/GardenTasks/Details?id='+row.id+'">'+data+'</a>';
            }
        },
        { 'data': 'userCount', 'className': 'text-center m-2' },
        { 'data': 'todayTask', 'className': 'text-center m-2' },
        {
            'data': 'isActive', 'className': 'text-center m-2',
            'render': function (data, type, row, meta) {
                let bg_color = "";
                if (data == true) {
                    bg_color = "text-primary";
                } else {
                    bg_color = "text-info";
                }
                return '<span class=' + bg_color + '><i class="fas fa-flag"></i></span>';
            }
        },     
        {
            'data': 'id', 'clssName': 'm-2', 'orderable': false,
            'render': function (data, type, row, meta) {
                return '<button type="button" class="btn btn-link text-danger p-0 ml-3 float-right" onclick="removeGardenTask('+data+')"><i class="fas fa-trash"></i></button>' +
                    '<button type="button" class="btn btn-link text-success p-0 float-right" value="/GardenTasks/Edit?id='+data+'" onclick="datatable_openModal(this)"><i class="fas fa-brush"></i></button>';
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