
//이달의 업무 갯수 가져오기  
let gardenSpaceId = document.getElementById('gardenSpaceId').value;
var calendarEl = document.getElementById('calendar');
var calendar = new FullCalendar.Calendar(calendarEl, {
    initialView: 'dayGridMonth',
    headerToolbar: {
        left: 'prev,next today',
        center: 'title',
        right : 'dayGridMonth,timeGridWeek,listWeek'
    },
    navLinks: true,
    eventLimit: true,
    events: {
        url: '/GardenSpaces/GetGardenTaskList?gardenSpaceId=' + gardenSpaceId + ''        
    }
});
calendar.getEventById("test123");
calendar.render();


document.addEventListener('DOMContentLoaded', function () {
    document.querySelector("[aria-label='prev']").addEventListener('click', function () {
        console.log(calendar.Month);
    });

    currentMonthWorkTimeCount();
});

function currentMonthWorkTimeCount() {
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
    httpRequest.send('gardenSpaceId=' + gardenSpaceId);
}

