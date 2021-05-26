﻿
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
                return '<button type="button" onclick="getAttendUserWorkTime('+row.id+',\''+row.name+'\')" class="btn btn-link p-0">'+data+'</button>';
            }
        },
        {
            'data': 'name', 'className': 'text-center m-2',
            'render': function (data, type, row, meta) {
                return '<button type="button" onclick="getAttendUserWorkTime('+row.id+',\''+row.name+'\')" class="btn btn-link p-0">' + data + '</button>';
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
function getAttendUserWorkTime(gardenUserTaskMapId, gardenUserName) {
    document.getElementById('attendUserName').innerText = "- ["+ gardenUserName+"]";

    let httpRequest = new XMLHttpRequest();
    if (!httpRequest) {
        errorMessage();
        return false;
    }

    httpRequest.open('POST', '/GardenUserTaskMaps/GetGardenUserWorkTime', true);
    httpRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    httpRequest.onload = function () {
        //if (this.status === 200 || this.response == "true") {
        //    location.reload();
             
        //}
        
        if (this.response == "\"empty\"") {
            showEmptyInfo(gardenUserTaskMapId);
        } else {
            showWorkTimeInfo(this.response);
        }
        
    };
    httpRequest.send('gardenUserTaskMapId=' + gardenUserTaskMapId);
}

//업무시간정보에 관한 동적테이블 생성
function makeWorkTimeTable(jsonArray) {

    let tbody = document.querySelector('#gardenWorktime_tb_tbody');
    while (tbody.hasChildNodes()) {
        tbody.removeChild(tbody.firstChild);
    }
    //전체 레슨 수
    let totalCount = 0;
    //완료한 레슨 수
    let completeCount = 0;
   
    jsonArray.forEach(json => {

        let newRow = tbody.insertRow(0);
        newRow.insertCell(0).innerText = json.taskDate;
        newRow.insertCell(1).innerText = json.startTime;
        newRow.insertCell(2).innerText = json.endTime;


        let isCheck = '';
        if (json.isComplete == true) {
            isCheck = 'checked';
            completeCount++;
        }

        newRow.insertCell(3).innerHTML = '<input type="checkbox" style="width:25px; height:25px;" ' + isCheck + ' />';
        newRow.insertCell(4).innerHTML = '<button type="button" data-value=' + json.id + '; class="p-0 btn btn-link btn-md float-right ml-3"><i class="fas fa-brush text-success"></i></button>' +
            '<button type="button" data-value=' + json.id + '; class="p-0 btn btn-link btn-md float-right"><i class="fas fa-trash text-danger"></button>';

        tbody.rows[0].cells[0].className = 'text-center m-2';
        tbody.rows[0].cells[1].className = 'text-center m-2';
        tbody.rows[0].cells[2].className = 'text-center m-2';
        tbody.rows[0].cells[3].className = 'text-center m-2';

        totalCount++;
    });


    updateWorktimeDashBoard(jsonArray[0].taskWeek, totalCount, completeCount);
}

//업무시간 대시보드정보 업데이트
function updateWorktimeDashBoard(taskWeek, totalCount, completeCount) {
    //완료한 횟수 정보
    document.getElementById('completeAndTotalCountInfo').innerHTML = '<span class="text-success">' + completeCount + '</span>' + ' / ' + totalCount;
    //달성률 정보
    let fullmentRate = (completeCount / totalCount) * 100;    
    document.getElementById('fullmentRateInfo').innerText = fullmentRate + "%";
    document.getElementById('fullmentRateInfo_progress').style.width = fullmentRate + "%";
    //등록주 정보
    document.getElementById('taskWeekInfo').innerText = taskWeek + " 주";
}

//업무시간 정보 나타내기
async function showWorkTimeInfo(gardenWorkTime_list) {
    document.getElementById('alert_empty_gardenWorkTime').style.display = 'none';
    document.getElementById('user_gardenWorkTime').style.display = 'block';

    await makeWorkTimeTable(JSON.parse(gardenWorkTime_list));             
}

// 생성된 업무 시간이 없을때 보여짐.
function showEmptyInfo(gardenUserTaskMapId) {
    let gardenSpaceId = document.getElementById('gardenSpaceId').value;
    document.getElementById('alert_empty_gardenWorkTime').style.display = 'block';
    document.getElementById('user_gardenWorkTime').style.display = 'none';
    document.getElementById('moveWorkTimeCreatePage').href = '/GardenWorkTimes/Create?gardenUserTaskMapId=' + gardenUserTaskMapId + '&gardenSpaceId=' + gardenSpaceId+'';
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