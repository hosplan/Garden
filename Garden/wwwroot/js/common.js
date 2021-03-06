//일반적으로 모달창 열기
function openModal(controllerName, actionName, id) {
    console.log('test');
    let url = '';
    if (id == "" || id == 0) {
        url = '/' + controllerName + '/' + actionName;
    } else {
        url = '/' + controllerName + '/' + actionName + '?id=' + id;
    }
    click_common_btn(url);
}


//데이터 테이블에서 모달창 열기
function datatable_openModal(obj) {
    click_common_btn(obj.value);
}

//데이터 테이블에서 모달창 열기 - param - string 추후 변경 예정
function datatable_openModal_p_string(obj) {
    let value = obj.getAttribute('data-value'); 
    let href = obj.getAttribute('data-url');

    href = href + "?id=" + value;
    click_common_btn(href);
}

//페이지 리로드
function reloadPage() {
    location.reload();
}

//에러 메세지 출력
function errorMessage() {
    Swal.fire({
        title: '문제가 발생하였습니다!',
        text: '잠시후에 다시 시도해주세요!',
        icon: 'error',
        confirmButtonText: '확인'
    });
}

function SelectingIdOpenModal(controllerName, actionName) {
    let gardenSpace_option = document.getElementById('Garden_list');
    gardenSpace_option = gardenSpace_option.options[gardenSpace_option.selectedIndex].value;
    openModal(controllerName, actionName, gardenSpace_option);
}

//숫자만 입력
function onlyNumber() {
    if ((event.keyCode < 48) || (event.keyCode > 57))
        event.returnValue = false;
}

//url 클릭
function click_common_btn(url) {
    console.log(url);
    document.getElementById('common_modal_btn').setAttribute('data-url', url);
    document.getElementById('common_modal_btn').click();
}