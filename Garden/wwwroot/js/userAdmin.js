//baseSubType - datatable
$('#userAdmin_dt').DataTable({
    'ajax': {
        'url': "/UserAdmins/GetUserList",
        'type': 'GET',
        'datatype': 'json'
    },
    'columns': [
        {
            'data': 'userName', 'className': 'text-left m-2',
            'render': function (data, type, row, meta) {
                return data;
            },
         
        },
        {
            'data': 'name', 'className': 'text-left m-2',
            'render': function (data, type, row, meta) {
                return data;
            },

        },
        {
            'data': 'userRole', 'className': 'text-left m-2',
            'render': function (data, type, row, meta) {
                return "<button class='btn btn-link p-0' type='button' onclick='openModal(\"UserAdmins\", \"Edit\", \""+row.id+"\")'>"+data+"</button>";        
            },
        },      
        {
            'data': 'gardenInfo', 'className': 'text-left m-2',
            'render': function (data, type, row, meta) {
                return data;
            }
        },
        {
            'data': 'isActive', 'clssName': 'm-2', 'orderable': false,
            'render': function (data, type, row, meta) {
                if (data == true) {
                    return '<input type="checkbox" value=' + row.id + ' onclick="changeActive(this)" style="width:25px; height:25px;" checked  />';
                } else {
                    return '<input type="checkbox" value=' + row.id + ' onclick="changeActive(this)" style="width:25px; height:25px;"  />';
                }

            }
        }
    ],
    'retrieve' : true,
    'responsive': true,
    'deferRender': true,
    'ordering': true,
    'info': true,
    'paging': true,
    'pageLength': 10,
    'searching': true,
    'processing': true,
});

//역할 수정
function roleChange(userId, roleId, obj) {
    let httpRequest = new XMLHttpRequest();
    if (!httpRequest) {
        errorMessage();
        return false;
    }

    httpRequest.open('POST', '/UserAdmins/RoleChange', true);
    httpRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    httpRequest.onload = function () {
        if (this.status === 200) {
            console.log(this.responseText);

            if (this.response == "false") {
                errorMessage();
            }
        }
    };
    httpRequest.send('userId=' + userId + '&roleId=' + roleId + '&isCheck=' + obj.checked);
}


//활성화/비활성화 변경
function changeActive(obj) {

    let httpRequest = new XMLHttpRequest();
    if (!httpRequest) {
        errorMessage();
        return false;
    }

    httpRequest.open('POST', '/UserAdmins/ChangeActive', true);
    httpRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    httpRequest.onload = function () {
        if (this.status === 200) {
            console.log(this.responseText);

            if (this.response == "false") {
                errorMessage();
            }
        } else {
            console.log("request에 문제가 있다.");
        }
    };
    httpRequest.send('id=' + obj.value + '&isCheck=' + obj.checked);
}