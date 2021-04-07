function GetBasySubTypeList(id) {
    console.log(id);
    let httpRequest = new XMLHttpRequest();

    if (!httpRequest) {
        console.log("xmlHttp 인스턴스를 만들 수 없습니다");
        return false;
    }

    httpRequest.open('POST', '/BaseTypes/GetBaseSubTypeList');
    httpRequest.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    httpRequest.send('id=' +id);
}