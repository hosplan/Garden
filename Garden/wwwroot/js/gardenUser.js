﻿$('.select2').select2();

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
        { 'data': 'userName', 'className':'text-center m-2' },
        { 'data': 'name', 'className': 'text-center m-2' },
        { 'data': 'regDate', 'className': 'text-center m-2' },
        {
            'data': 'isActive', 'className': 'text-center m-2',
            'render': function (data, type, row, meta) {
                let isCheck = '';
                if (data == true) {
                    isCheck = 'checked';
                }
                return '<input type="checkbox" data-permission="update" style="width:25px; height:25px;" ' + updatePermission + ' onclick="changeIsActive()" ' + isCheck + '/>';
            }
        },     
        {
            'data': 'id', 'clssName': 'm-2', 'orderable': false,
            'render': function (data, type, row, meta) {
                return '<button type="button" data-permission="delete" class="btn btn-link text-danger p-0 ml-3 float-right" '+deletePermission+' onclick="removeValue(\''+data+'\')"><i class="fas fa-trash"></i></button>';                  
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