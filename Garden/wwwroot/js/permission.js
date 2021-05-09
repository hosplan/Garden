function getControllerAndActionName() {
    let httpRequest = new XMLHttpRequest();
    let pathName = window.location.pathname;
    if (!httpRequest) {
        console.log("컨트롤러와 페이지 이름을 가져오기위한 인스턴스를 만들 수 없습니다.");
        return false;
    }

    httpRequest.open('POST', '/Home/GetControllerAndActionName', true);
    httpRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    httpRequest.onload = function () {
        if (this.status === 200) {
            let jsonValue = JSON.parse(this.response);
            console.log(jsonValue);

            for (let i = 0; i < jsonValue.permission.length; i++) {
                if (jsonValue.permission[i].isRead == true) {
                    document.getElementById('read_'+jsonValue.permission[i].name).checked = true;
                }
                if (jsonValue.permission[i].isCreate == true) {
                    document.getElementById('create_'+jsonValue.permission[i].name).checked = true;
                }
                if (jsonValue.permission[i].isUpdate == true) {
                    document.getElementById('update_'+jsonValue.permission[i].name).checked = true;
                }
                if (jsonValue.permission[i].isDelete == true) {
                    document.getElementById('delete_'+jsonValue.permission[i].name).checked = true;
                }
            }

            document.getElementById('currentController').value = jsonValue.controller;
            document.getElementById('currentAction').value = jsonValue.action;

        }
    };
    httpRequest.send('pathName=' + pathName);
}


function changePermission(id, actionName, obj) {
    let controllerPath = document.getElementById('currentController').value;
    let actionPath = document.getElementById('currentAction').value;
    let isCheck = obj.checked;

    let httpRequest = new XMLHttpRequest();
    if (!httpRequest) {
        console.log("xmlHttp 인스턴스를 만들 수 없습니다");
        return false;
    }

    httpRequest.open('POST', '/UserAdmins/ChangePermission', true);
    httpRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    httpRequest.onload = function () {
        if (this.status === 200) {
            console.log(this.responseText);
        } else {
            console.log("request에 문제가 있다.");
        }
    };
    httpRequest.send('id=' + id + '&action=' + actionName + '&controllerPath=' + controllerPath + '&actionPath=' + actionPath + '&isCheck=' + isCheck);
}

window.onload = function () {
    permissionCheck();
}


//baseSubType - datatable
$('#userAdmin_dt').DataTable({
    'ajax': {
        'url': "/UserAdmins/GetUserList",
        'type': 'GET',
        'datatype': 'json'
    },
    'columns': [
        {
            'data': 'Name', 'className': 'text-left m-2',
        },
        //{
        //    'data': 'Role', 'className': 'text-left m-2',
        //    'render': function (data) {
        //        return data;
        //    }
        //},
        { 'data': 'GardenInfo', 'className': 'text-left m-2' },
        {
            'data': 'IsActive', 'clssName': 'm-2', 'orderable': false,
            'render': function (data, type, row, meta) {
                if (data == true) {
                    return '<input type="checkbox" value=' + row.Id + ' style="width:25px; height:25px;" checked  />';
                } else {
                    return '<input type="checkbox" value=' + row.Id + ' style="width:25px; height:25px;"  />';
                }

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
