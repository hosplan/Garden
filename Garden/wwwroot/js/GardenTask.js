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

var gardenSpaceId = document.getElementById('gardenSpaceId').value;
//baseSubType - datatable
var baseSubType_dataTable = $('#gardenTask_dt').DataTable({
    'ajax': {
        'url': "/GardenTasks/GetGardenTaskList?id=" + gardenSpaceId+"",
        'type': 'GET',
        'datatype': 'json'
    },
    'columns': [
        {
            'data': 'typeName', 'className': 'text-left m-2',
            'render': function (data) {
                return data;
            }
        },    
        {
            'data': 'name', 'className': 'text-left m-2',
            'render': function (data,type,row,meta) {
                return '<button class="btn btn-link btn-xs p-0" type="button" onclick="openDetail('+row.id+')">'+data+'</button>';
            }
        },
        { 'data': 'userCount', 'className': 'text-center m-2' },
        { 'data': 'todayTask', 'className': 'text-center m-2' },
        { 'data': 'isActive', 'className': 'text-center m-2' },     
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