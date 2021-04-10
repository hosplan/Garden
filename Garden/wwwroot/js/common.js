//일반적으로 모달창 열기
function openModal(controllerName, actionName, id) {
    let url = '';
    if (id == 0) {
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

//url 클릭
function click_common_btn(url) {
    document.getElementById('common_modal_btn').setAttribute('data-url', url);
    document.getElementById('common_modal_btn').click();
}