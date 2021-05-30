document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');
    var calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth'
    });
    calendar.render();

    //이달의 업무 갯수 가져오기
    let httpRequest = new XMLHttpRequest();
    if (!httpRequest) {
        errorMessage();
        return false;
    }

    httpRequest.open('POST', '/GardenSpaces/GetGardenSpaceOtherInfo', true);
    httpRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    httpRequest.onload = function () {
        if (this.status === 200) {
            console.log(this.response);
            document.getElementById('gardenSpace_task').innerText = this.response.replace(/\"/g,'');
        } else {
            errorMessage();
        }
    };
    httpRequest.send('gardenSpaceId=' + document.getElementById('gardenSpaceId').value);

});
