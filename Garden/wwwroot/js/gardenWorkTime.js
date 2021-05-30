
//선택한 날짜의 요일 가져오기
function getDay() {
    let checkboxes = document.querySelectorAll('input[type="checkbox"]');

    checkboxes.forEach((checkbox) => {
        checkbox.checked = false;
    });

    const WEEKDAY = ['IsSun', 'IsMon', 'IsTue', 'IsWed', 'IsThr', 'IsFri', 'IsSat'];
    let dateValue = new Date(document.getElementById('TaskDate').value);
    console.log(dateValue);
    let dayText = document.getElementById(WEEKDAY[dateValue.getDay()] + "_thead").innerText;    
    document.getElementById(WEEKDAY[dateValue.getDay()]).checked =  true;
    document.getElementById(WEEKDAY[dateValue.getDay()]).onclick = function () {
        return false;
    };

    //document.getElementById(WEEKDAY[dateValue.getDay()]).checked = true;
    //document.getElementById(WEEKDAY[dateValue.getDay()]).setAttribute('onclick="return(false)"');
}


function removeValue(gardenUserId) {
    Swal.fire({
        title: '해당 정원관리사를 삭제 하시겠어요?',
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

            httpRequest.open('POST', '/GardenUsers/DeleteGardenUser', true);
            httpRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
            httpRequest.onload = function () {
                if (this.status === 200 || this.response == "true") {
                    location.reload();                  
                } else {
                    errorMessage();
                }
            };
            httpRequest.send('gardenUserId=' + gardenUserId);
        }
    });
}