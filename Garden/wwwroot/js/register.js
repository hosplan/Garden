function checkEmail() {
    let httpRequest = new XMLHttpRequest();
    let register_id = document.getElementById('register_id').value;
    if (!httpRequest) {
        console.log("xmlHttp 인스턴스를 만들 수 없습니다");
        return false;
    }

    httpRequest.open('POST', '/Home/CheckEmail', true);
    httpRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    httpRequest.onload = function () {
        if (this.status === 200) {
            //console.log(this.responseText);
            if (this.responseText == "true") {
                registerHideIcon('register_id_icon', 'register_id_checkbox');
            } else {
                Swal.fire({
                    title: '해당 아이디는 등록되어 있습니다!',
                    icon: 'error',
                    confirmButtonText: '확인'
                });
            }
        } else {
            console.log("request에 문제가 있다.");
        }
    };
    httpRequest.send('register_id=' + register_id);
}

function registerHideIcon(hide_info, show_info) {
    document.getElementById(hide_info).style.display = 'none';
    document.getElementById(show_info).style.display = 'block';
    document.getElementById(show_info).checked = true;
    document.getElementById(show_info).disabled = true;

    //4개의 항목창 모두 충족했으면 계정 생성하기 버튼 보여줌
    let register_name_value = document.getElementById('register_name_checkbox').checked;
    let register_email_value = document.getElementById('register_email_checkbox').checked;
    let register_password_value = document.getElementById('register_password_checkbox').checked;
    let register_password_check_value = document.getElementById('register_password_check_checkbox').checked;
    
    if (register_name_value && register_email_value && register_password_value && register_password_check_value)
    {
        document.getElementById('create_account_btn').style.display = 'block';
    }
}

function registerShowIcon(show_info, hide_info) {
    document.getElementById(hide_info).style.display = 'none';
    document.getElementById(show_info).style.display = 'block';

    document.getElementById(hide_info).checked = false;
    document.getElementById(hide_info).disabled = true;
}

function checkPassword() {
    let reg = /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$/;
    let password = document.getElementById('register_password').value;

    if (false === reg.test(password)) {
        console.log("f");
        document.getElementById('password_validation_warning').innerHTML = "비밀번호는 8자 이상이어야 하며, 숫자/대문자/소문자/특수문자를 모두 포함해야 합니다.";
    } else {
        console.log("t");
        $('#register_password_check').attr('disabled', false);
    }
}
//비밀번호 , 비밀번호 체크값 동일 확인
function checkPasswordValue() {
    let reg = /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$/;
    let password = document.getElementById('register_password').value;

    if (false === reg.test(password)) {
        document.getElementById('password_validation_warning').innerHTML = "비밀번호는 8자 이상이어야 하며, 숫자/대문자/소문자/특수문자를 모두 포함해야 합니다.";
    } else {
        $('#register_password_check').attr('disabled', false);
        //let password = document.getElementById('register_password').value;
        let password_check = document.getElementById('register_password_check').value;
        document.getElementById('password_validation_warning').innerHTML = "";
        if (password === password_check) {
            registerHideIcon('register_password_icon', 'register_password_checkbox');
            registerHideIcon('register_password_check_icon', 'register_password_check_checkbox');
        } else {
            registerShowIcon('register_password_icon', 'register_password_checkbox');
            registerShowIcon('register_password_check_icon', 'register_password_check_checkbox');
        }
    }   
}

//비밀번호 길이 체크
function checkPasswordLength() {
    let password = document.getElementById('register_password').value;
    if (password.length < 6) {
        alert("비밀번호는 6자 이상입니다");
    } else {
        checkPasswordValue();
    }
}

