//일반적으로 모달창 열기
function openModal(controllerName, actionName, id) {
    let url = '';
    if (id == "" || id == 0) {
        url = '/' + controllerName + '/' + actionName;
    } else {
        url = '/' + controllerName + '/' + actionName + '?id=' + id;
    }
    click_common_btn(url);
}

function openModalForDataTable(obj, id) {
    let tt = obj.querySelector("[data-url]");
    console.log(tt);
}

//데이터 테이블에서 모달창 열기
function datatable_openModal(obj) {
    click_common_btn(obj.value);
}

function reloadPage() {
    location.reload();
}

function errorMessage() {
    Swal.fire({
        title: '문제가 발생하였습니다!',
        text: '잠시후에 다시 시도해주세요!',
        icon: 'error',
        confirmButtonText: '확인'
    });
}

//선택한 정원에 해당하는 모달창 open
function GetGardenIdAndOpenModal(token) {
    let gardenSpace_option = document.getElementById('Garden_list');
    gardenSpace_option = gardenSpace_option.options[gardenSpace_option.selectedIndex].value;

    if (token == 1) {
        console.log("gardenUser: " + gardenSpace_option);
        openModal('GardenUsers', 'Create', gardenSpace_option);
    }
    else if (token == 2) {
        openModal('GardenTasks', 'Create', gardenSpace_option);
    }
}

//url 클릭
function click_common_btn(url) {
    document.getElementById('common_modal_btn').setAttribute('data-url', url);
    document.getElementById('common_modal_btn').click();
}