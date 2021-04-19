function GetBasySubTypeList(id) {
    //let httpRequest = new XMLHttpRequest();

    //if (!httpRequest) {
    //    console.log("xmlHttp 인스턴스를 만들 수 없습니다");
    //    return false;
    //}

    //httpRequest.open('POST', '/BaseTypes/GetBaseSubTypeList', true);
    //httpRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    //httpRequest.onload = function () {
    //    if (this.status === 200) {
    //        console.log(this.responseText);
    //    } else {
    //        console.log("request에 문제가 있다.");
    //    }
    //};
    //httpRequest.send('id=' + id);
    let url = '/BaseTypes/GetBaseSubTypeList?id=' + id + '';
    baseSubType_dataTable.ajax.url(url).load();
}
let gardenSpace_option = document.getElementById('Garden_list');
gardenSpace_option = gardenSpace_option.options[gardenSpace_option.selectedIndex].value;

function GetGardenIdAndOpenModal(token) {
   
    if (token == 1) {
        openModal('GardenTasks', 'Create', gardenSpace_option);
    }
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
                return '<button type="button" class="btn btn-link text-danger p-0 ml-3 float-right" value="/GardenTasks/Delete?id='+data+'" onclick="datatable_openModal(this)"><i class="fas fa-trash"></i></button>' +
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