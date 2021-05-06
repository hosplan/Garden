function GetBasySubTypeList(id) {
    let url = '/BaseTypes/GetBaseSubTypeList?id=' + id + '';
    baseSubType_dataTable.ajax.url(url).load();
}


//baseSubType - datatable
var baseSubType_dataTable = $('#baseSubType_dt').DataTable({
    'ajax': {
        'url': "/BaseTypes/GetBaseSubTypeList",
        'type': 'GET',
        'datatype': 'json'
    },
    'columns': [
        {
            'data': 'name', 'className': 'text-left m-2',
            'render': function (data) {
                return '<span class="font-weight-bold text-info">'+data+'</span>';
            }
        },
        {
            'data': 'description', 'className': 'text-left m-2',
            'render': function (data) {
                return data;
            }
        },    
        {
            'data': 'id', 'clssName': 'm-2', 'orderable': false,
            'render': function (data, type, full, meta) {
                let update_btn = '';
                let delete_btn = '';
                if (document.getElementById('permission_update').value == 'true') {
                    update_btn = '<button type="button" class="btn btn-link text-success p-0 float-right" data-permission="update" value="/BaseSubTypes/Edit?id=' + data + '" onclick="datatable_openModal(this)"><i class="fas fa-brush"></i></button>';
                }
                if (document.getElementById('permission_delete').value == 'true') {
                    delete_btn = '<button type="button" class="btn btn-link text-danger p-0 ml-3 float-right" data-permission="delete" value="/BaseSubTypes/Delete?id=' + data + '" onclick="datatable_openModal(this)"><i class="fas fa-trash"></i></button>';
                }
                return delete_btn + update_btn;
                    
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