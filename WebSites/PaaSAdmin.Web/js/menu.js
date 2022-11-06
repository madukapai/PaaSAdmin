function funListMenu(url)
{
    // 取得Id與Token
    var userId = getCookies("UserId");
    var accessToken = "";

    // 呼叫取得選單的Api
    var strUrl = constGetMenuUrl + "?query.UserId=" + userId + "&query.AccessToken=" + accessToken;
    callAPI(strUrl, "GET", "", function (data) { funListMenuSuccess(data, url); }, showException);
}

function funListMenuSuccess(data, url)
{
    if (data.IsSuccess == true) {
        // 寫入選單項目到LocalStorage
        localStorage.setItem("Menu", JSON.stringify(data.Body));
        // 轉頁到主頁
        location.href = url;
    }
    else
    {
        var btns = [
            { "Text": "確定", "NavigateUrl": "", "Event": "parent.location.href = 'login.html';", "IsPrimary": true },
        ];
        showMessage("查無資料", "無法取得您的選單資料，請重新登入系統", btns, "Fail");
    }
}

function funGetSystem() {
    var OSName = "Other";
    if (window.navigator.userAgent.indexOf("Mac") != -1) OSName = "iOS";
    if (window.navigator.userAgent.indexOf("Android") != -1) OSName = "Android";

    return OSName;
}