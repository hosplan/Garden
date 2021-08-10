
let updatePermission = '';
let deletePermission = '';
if (document.getElementById('permission_update').value == 'false') {
    updatePermission = 'disabled';
}

if (document.getElementById('permission_delete').value == 'false') {
    deletePermission = 'disabled';
}

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
                return '<button type="button" id='+data+' onclick="getAttendUserWorkTime('+row.id+',\''+row.name+'\', \''+data+'\')" class="btn btn-link p-0">'+data+'</button>';
            }
        },
        {
            'data': 'name', 'className': 'text-center m-2',
            'render': function (data, type, row, meta) {
                return '<button type="button" onclick="getAttendUserWorkTime(' + row.id + ',\'' + row.name + '\',\'' + row.userName +'\')" class="btn btn-link p-0">' + data + '</button>';
            }
        },
        {
            'data': 'isRent', 'className': 'text-center m-2',
            'render': function (data, type, row, meta) {
                if (data == true) {
                    return '<input type="checkbox" style="width:25px; height:25px;" onclick="changeRentStatus(' + row.id + ',this)" checked />'
                } else {
                    return '<input type="checkbox" style="width:25px; height:25px;" onclick="changeRentStatus(' + row.id + ',this)" />'
                }
                
            }
        },
        {
            'data': 'description', 'className': 'text-center m-2',
            'render': function (data, type, row, meta) {
                return '<button type="button" class="btn btn-link p-0 ml-2" onclick="openModal()"><i class="fas fa-brush text-success"></i></button>';

            }
        },
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


function changeRentStatus(id, obj) {
    let httpRequest = new XMLHttpRequest();
    let isRent = obj.checked;
    if (!httpRequest) {
        errorMessage();
        return false;
    }

    httpRequest.open('POST', '/GardenUserTaskMaps/ChangeRentStatus', true);
    httpRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    httpRequest.onload = function () {
        if (this.response == "false") {
            errorMessage();
        } 
    };
    httpRequest.send('gardenUserTaskMapId=' + id + '&isRent=' + isRent);
}

function createWorkTimeTable(gardenUserTaskMapId, startMonth, endMonth) {
    let httpRequest = new XMLHttpRequest();
    if (!httpRequest) {
        errorMessage();
        return false;
    }

    httpRequest.open('POST', '/GardenUserTaskMaps/GetGardenUserWorkTime', true);
    httpRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    httpRequest.onload = function () {
        if (this.response == "\"empty\"") {
            showEmptyInfo(gardenUserTaskMapId);
        } else {
            showWorkTimeInfo(this.response, gardenUserTaskMapId);
        }
    };
    if (startMonth == 0 && endMonth == 0) {
        httpRequest.send('gardenUserTaskMapId=' + gardenUserTaskMapId);
    } else {
        httpRequest.send('gardenUserTaskMapId=' + gardenUserTaskMapId+'&startMonth='+startMonth+'&endMonth='+endMonth);
    }
    
}

//정원 업무 참여자의 업무시간 정보 가져오기
function getAttendUserWorkTime(gardenUserTaskMapId, name, userName, startMonth, endMonth) {
    document.getElementById('attendUserName').innerText = name;
    document.getElementById('attend_userName').value = userName;
    document.getElementById('gardenUserTaskMapId').value = gardenUserTaskMapId;
    createWorkTimeTable(gardenUserTaskMapId, 0, 0);
}

function searchWorkTime() {
    let startMonth_value = document.getElementById('startMonth').value;
    let endMonth_value = document.getElementById('endMonth').value;
    let gardenUserTaskMapId = document.getElementById('gardenUserTaskMapId').value;

    createWorkTimeTable(gardenUserTaskMapId, startMonth_value, endMonth_value);
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
        //업무날짜
        newRow.insertCell(0).innerText = json.taskDate;
        //시작시간
        newRow.insertCell(1).innerText = json.startTime;
        //종료 시간
        newRow.insertCell(2).innerText = json.endTime;

        let isCheck = '';
        if (json.isComplete == true) {
            isCheck = 'checked';
            completeCount++;
        }
        //완료여부
        newRow.insertCell(3).innerHTML = '<input type="checkbox" onclick="completeWorkTime('+json.id+',this);" style="width:25px; height:25px;" ' + isCheck + ' />';
        //관리버튼(수정,삭제)
        newRow.insertCell(4).innerHTML = '<button type="button" class="p-0 btn btn-link btn-md float-right ml-3" onclick="updateWorkTime('+json.id+',this)"><i class="fas fa-brush text-success"></i></button>' +
            '<button type="button" class="p-0 btn btn-link btn-md float-right" onclick="removeWorkTime('+json.id+')"><i class="fas fa-trash text-danger"></button>';

        //클래스 주입
        tbody.rows[0].cells[0].className = 'text-center m-2';
        tbody.rows[0].cells[1].className = 'text-center m-2';
        tbody.rows[0].cells[2].className = 'text-center m-2';
        tbody.rows[0].cells[3].className = 'text-center m-2';

        totalCount++;
    });
    updateWorktimeDashBoard(totalCount, completeCount);
}

//업무시간 수정창 열기
function updateWorkTime(gardenWorkTimeId, obj) {
    //제이쿼리 사용
    let tr = $(obj).parent().parent();
    $(obj).parent().empty();
    tr.children().eq(0).html('<input type="date" class="form-control" />');
    tr.children().eq(1).html('<input type="time" class="form-control" />');
    tr.children().eq(2).html('<input type="time" class="form-control" />');
    tr.children().eq(3).empty();
    tr.children().eq(4).html('<button type="button" class="btn btn-secondary btn-md ml-3 float-right" onclick="cancleWorkTimeUpdate()">취소</button>' +
                             '<button type="button" class="btn btn-primary btn-md float-right" onclick="saveWorkTimeUpdate(' + gardenWorkTimeId + ', this)">저장</button>');
    //openModal('GardenWorkTimes','Edit',gardenWorkTimeId);
}
//업무시간 수정 완료
function saveWorkTimeUpdate(gardenWorkTimeId, obj) {
    let tr = $(obj).parent().parent();
    let update_taskDate = tr.children().eq(0).children().val();
    let update_startDate = tr.children().eq(1).children().val();
    let update_endDate = tr.children().eq(2).children().val();

    let httpRequest = new XMLHttpRequest();
    if (!httpRequest) {
        errorMessage();
        return false;
    }

    httpRequest.open('POST', '/GardenWorkTimes/UpdateWorkTime', true);
    httpRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    httpRequest.onload = function () {
        if (this.status === 200 || this.response == "true") {
            let userName = document.getElementById('attend_userName').value;
            //제이쿼리 강제로 버튼 클릭 이벤트주입
            $('#' + userName).trigger('click');
        } else {
            errorMessage();
        }
    };
    httpRequest.send('gardenWorkTimeId=' + gardenWorkTimeId + '&taskDate=' + update_taskDate+'&startTime='+update_startDate+'&endTime='+update_endDate);

}
//업무시간 수정 취소
function cancleWorkTimeUpdate() {
    let userName = document.getElementById('attend_userName').value;
    //제이쿼리 강제로 버튼 클릭 이벤트주입
    $('#' + userName).trigger('click');
}

//업무시간 삭제
function removeWorkTime(gardenWorkTimeId) {
    Swal.fire({
        title: '해당 업무를 삭제 하시겠어요?',
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

            httpRequest.open('POST', '/GardenWorkTimes/DeleteWorkTime', true);
            httpRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
            httpRequest.onload = function () {
                if (this.status === 200 || this.response == "true") {
                    let userName = document.getElementById('attend_userName').value;
                    //제이쿼리 강제로 버튼 클릭 이벤트주입
                    $('#' + userName).trigger('click');
                } else {
                    errorMessage();
                }
            };
            httpRequest.send('gardenWorkTimeId=' + gardenWorkTimeId);
        }
    });
}

//업무시간 완료처리
function completeWorkTime(gardenWorkTimeId, obj) {
    let httpRequest = new XMLHttpRequest();
    if (!httpRequest) {
        errorMessage();
        return false;
    }

    httpRequest.open('POST', '/GardenWorkTimes/CompleteWorkTime', true);
    httpRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    httpRequest.onload = function () {
        if (this.status === 200) {
            let userName = document.getElementById('attend_userName').value;
            //제이쿼리 강제로 버튼 클릭 이벤트주입
            $('#' + userName).trigger('click');
        } else {
            errorMessage();
        }
    };
    httpRequest.send('gardenWorkTimeId=' + gardenWorkTimeId + '&isComplete=' + obj.checked);
}

//업무시간 대시보드정보 업데이트
function updateWorktimeDashBoard(totalCount, completeCount) {
    //완료한 횟수 정보
    document.getElementById('completeAndTotalCountInfo').innerHTML = '<span class="text-success">' + completeCount + '</span>' + ' / ' + totalCount;
    //달성률 정보
    let fullmentRate = (completeCount / totalCount) * 100;    
    document.getElementById('fullmentRateInfo').innerText = fullmentRate + "%";
    document.getElementById('fullmentRateInfo_progress').style.width = fullmentRate + "%";
    //등록주 정보
    //document.getElementById('taskWeekInfo').innerText = taskWeek + " 주";
}

//업무시간 정보 나타내기
async function showWorkTimeInfo(gardenWorkTime_list, gardenUserTaskMapId) {
    document.getElementById('alert_empty_gardenWorkTime').style.display = 'none';
    document.getElementById('user_gardenWorkTime').style.display = 'block';
    let gardenSpaceId = document.getElementById('gardenSpaceId').value;
    document.getElementById("moveWorkTimeCreatePage").href = '/GardenWorkTimes/Create?gardenUserTaskMapId=' + gardenUserTaskMapId + '&gardenSpaceId=' + gardenSpaceId + '';
    await makeWorkTimeTable(JSON.parse(gardenWorkTime_list));             
}

//생성된 업무 시간이 없을때 보여짐.
function showEmptyInfo(gardenUserTaskMapId) {
    let gardenSpaceId = document.getElementById('gardenSpaceId').value;
    document.getElementById('alert_empty_gardenWorkTime').style.display = 'block';
    document.getElementById('user_gardenWorkTime').style.display = 'none';
    document.getElementById("moveWorkTimeCreatePage_btn").href = '/GardenWorkTimes/Create?gardenUserTaskMapId=' + gardenUserTaskMapId + '&gardenSpaceId=' + gardenSpaceId+'';
}

//업무참여자 추가
function createGardenUser() {
    let gardenSpace_option = document.getElementById('Garden_list');
    gardenSpace_option = gardenSpace_option.options[gardenSpace_option.selectedIndex].value;
    openModal('GardenUsers', 'Create', gardenSpace_option); 
}

//업무 참여자 삭제
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

//찾아보기 열기
function openSearchWorkTime() {
    document.getElementById('attend_search_workTime').style.display = 'block';
}

//찾아보기 닫기
function closeSearchWorkTime() {
    document.getElementById('attend_search_workTime').style.display = 'none';
}

//월 까지 부분의 option 값 생성
function makeEndMonthOption(obj) {
    let startMonth_value = obj.value;
    let endMonth_selectbox = document.getElementById('endMonth');
    //이전 endMonth option 값 지우기
    for (i = endMonth_selectbox.options.length -1 ; i >= 0; i--) {
        endMonth_selectbox.options[i] = null;
    }
    //endMonth option 값 넣기
    for (startMonth_value; startMonth_value < 13; startMonth_value++) {
        let option = document.createElement('option');
        option.innerText = startMonth_value;
        endMonth_selectbox.append(option);
    }
}