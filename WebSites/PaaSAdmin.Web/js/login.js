function funLogin()
{
    var username = document.getElementById("username").value;
    var password = document.getElementById("password").value;
    this.funExecLogin(username, password);
}

function funExecLogin(strUsername, strPassword)
{
    var objLogin = { "Username": strUsername, "Password": strPassword };
    // 執行登入的動作
    callAPI(constMemberLoginUrl, "POST", JSON.stringify(objLogin), funLoginSuccess, showException);
}

function funLoginSuccess(data) {
    if (data.IsSuccess) {
        setCookies("UserId", data.Body.UserId, 7);

        // 寫入localStorage
        localStorage.setItem("UserInfo", JSON.stringify(data.Body));

        // 載入System與Menu選項
        funListMenu("main.html");
    }
    else {
        alert("登入帳號與密碼錯誤，請重新嘗試");
    }
}

function funGetSystemNameToStorage() {
    var strUrl = `${constGetConfigUrl}?query.ConfigName=SystemName`;
    callAPI(strUrl, "GET", "",
        function (x) {
            var strSystemName = (x.IsSuccess) ? x.Body.ConfigValue : "";
            localStorage.setItem("SystemName", strSystemName);

            $("#spanSystemName").html(strSystemName);
            document.title = strSystemName;
        },
        showException);
}