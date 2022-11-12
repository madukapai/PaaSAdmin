/// 呼叫API的動作
function callAPI(strUrl, strMethod, strContent, onSuccess, onFail) {
    $.ajax({
        type: strMethod,
        url: strUrl,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: strContent,
        success: onSuccess,
        error: onFail,
        fail: onFail
    });
}

/// 寫入Cookies的動作
function setCookies(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    var domains = window.location.hostname.split(".");
    var i = 0;
    var domain = "";
    if (domains.length > 1) {
        domain = domains[0];
        for (i = 1; i < domains.length; i++)
            domain += "." + domains[i];
    }
    else
        domain = domains[0];

    document.cookie = cname + "=" + cvalue + ";" + expires + ";domain=" + domain + ";path=/";
}

/// 取得Cookies的動作
function getCookies(cname)
{
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) === ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) === 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function getUrlParamter(x) {
    var url = new URL(location.href);
    return url.searchParams.get(x);
}

function makeBlob(dataURL) {
    var BASE64_MARKER = ';base64,';
    if (dataURL.indexOf(BASE64_MARKER) === -1) {
        var parts = dataURL.split(',');
        var contentType = parts[0].split(':')[1];
        var raw = decodeURIComponent(parts[1]);
        return new Blob([raw], { type: contentType });
    }
    var parts = dataURL.split(BASE64_MARKER);
    var contentType = parts[0].split(':')[1];
    var raw = window.atob(parts[1]);
    var rawLength = raw.length;

    var uInt8Array = new Uint8Array(rawLength);

    for (var i = 0; i < rawLength; ++i) {
        uInt8Array[i] = raw.charCodeAt(i);
    }

    return new Blob([uInt8Array], { type: contentType });
}

/**
 * 顯示發生例外錯誤的訊息視窗
*/
function showException(ex) {
    var btns = [
        { "Text": "關閉", "NavigateUrl": "", "Event": "close", "IsPrimary": true },
    ];
    showMessage("系統發生錯誤", "目前系統發生錯誤，請稍後再試", btns, "Waraning");
}

function showExceptionAlert(ex) {
    alert("目前系統發生錯誤，請稍後再試");
    if (loading != null)
        loading.hide();
}

/**
 * 只顯示單一個關閉按鈕的訊息視窗
*/
function showCloseMessage(strTitle, strContent, status)
{
    var btns = [
        { "Text": "關閉", "NavigateUrl": "", "Event": "close", "IsPrimary": true },
    ];
    showMessage(strTitle, strContent, btns, status);
}

/**
 * 顯示訊息文字方塊的內容
 * @param {string} strTitle 顯示在文字方塊中的抬頭訊息
 * @param {string} strContent 顯示在本文的內容，可以使用Html的標籤作為內文的本體
 * @param {Array} objButtonArray 設定顯示在按鈕列中的按鈕物件陣列，宣告方法如下[{"Text":"文字", "NavigateUrl":"超連結", "Event":"自訂的Click事件", "IsPrimary":true}, {"Text":"文字", "NavigateUrl":"", "Event":"close", "IsPrimary":false}]
 * @param {string} status 根據傳入Success、Fail、Waraning、Information，顯示不同icon、背景顏色
 * sample:
            var btns = [
                { "Text": "連結", "NavigateUrl": "http://www.google.com.tw", "Event": "", "IsPrimary": true },
                { "Text": "自訂事件", "NavigateUrl": "", "Event": "alert('this ok!');", "IsPrimary": false },
                { "Text": "關閉", "NavigateUrl": "", "Event": "close", "IsPrimary": false },
            ];
            showMessage('Title', 'Content', btns, 'Success');

*/
function showMessage(strTitle, strContent, objButtonArray, status) {
    if (status === undefined)
        status = 'Success';

    // 傳入狀態，可依據不同狀態變更背景顏色、icon
    var statusEnum = {
        Success: { name: "success", icon: "fa-check", backgroundColor: "#7fc6ab" },
        Fail: { name: "fail", icon: "fa-times", backgroundColor: "#ff7058" },
        Waraning: { name: "waraning", icon: "fa-exclamation", backgroundColor: "#ff7058" },
        Information: { name: "information", icon: "fa-exclamation", backgroundColor: "#b5b5b5" }
    };

    // 放入Title
    var divTitle = document.getElementById("divMessageBoxTitle");
    divTitle.innerHTML = "<h5 class='modal-title'>" + strTitle + "</h5>";
    // 放入本文
    var divBody = document.getElementById("divMessageBoxBody");
    divBody.innerHTML = strContent;
    // 放入Button
    var divButton = document.getElementById("divMessageBoxButton");
    // 多一層div 讓btn置中
    var divBut = document.createElement("div");
    // 上方圓形icon
    var divDocument = document.getElementById("divMessageBoxDialog");

    //var divCircle = document.createElement('div');
    //divCircle.className = 'modal-circle';
    //divCircle.setAttribute("style", `background-color: ${statusEnum[status].backgroundColor}`)
    //divCircle.innerHTML = `<i class="fas ${statusEnum[status].icon}"></i>`;
    //divDocument.appendChild(divCircle);

    var i = 0;
    divButton.innerHTML = "";
    for (i = 0; i < objButtonArray.length; i++) {
        var strText = objButtonArray[i].Text;
        var strNavigateUrl = objButtonArray[i].NavigateUrl;
        var strEvent = objButtonArray[i].Event;
        var blIsPrimary = objButtonArray[i].IsPrimary;

        var btn = document.createElement('button');
        // 放入顯示主要按鈕class
        if (blIsPrimary === true)
            btn.className = "btn";
        else
            btn.className = "btn";

        // 放入Event，如果Event是close，就出現關閉的動作
        if (strEvent === "close")
            btn.setAttribute("data-dismiss", "modal");
        else if (strEvent !== "") {
            strEvent = "$('#divMessageBox').modal('hide');sleep(300);" + strEvent;
            btn.setAttribute("onclick", strEvent);
        }

        // 放入NavigateUrl
        if (strNavigateUrl !== "")
            btn.setAttribute("onclick", "location.href='" + strNavigateUrl + "'");

        // 放入文字
        btn.innerHTML = strText;
        divButton.setAttribute("style", `background-color: ${statusEnum[status].backgroundColor}`);
        divBut.appendChild(btn);
        var newSpan = document.createElement('span');
        newSpan.innerHTML = "&nbsp;";
        divBut.appendChild(newSpan);
        divButton.appendChild(divBut);
    }

    $('#divMessageBox').modal({ keyboard: false, backdrop: false });
}

function showMobileMessage(strContent, strUrl) {
    var cont = document.createElement('p');
    cont.innerHTML = strContent;

    // 放入文字
    document.getElementById("divMobileMessageContent").innerHTML = "";
    document.getElementById("divMobileMessageContent").appendChild(cont);

    // 放入Button
    var btn = document.createElement('button');
    btn.innerHTML = "確定";

    if (strUrl != "" && strUrl != null)
        btn.setAttribute("onclick", "location.href='" + strUrl + "';");
    else
        btn.setAttribute("onclick", "document.getElementById('divMobileMessage').style.display = 'none';");

    document.getElementById("divMobileMessageContent").appendChild(btn);
    document.getElementById("divMobileMessage").style.display = "";
}

//DateTime Format 年/月/日 時:分:秒
function DateTimeFormat(date) {
    const isDate = new Date(date);
    return `${isDate.getFullYear()}/${padLeft((isDate.getMonth() + 1), 2, "0")}/${padLeft(isDate.getDate(), 2, "0")} ${padLeft(isDate.getHours(), 2, "0")}:${padLeft(isDate.getMinutes(), 2, "0")}:${padLeft(isDate.getSeconds(), 2, "0")}`;
}

function DateFormat(date, split) {
    const isDate = new Date(date);
    return isDate.getFullYear().toString() + split + padLeft((isDate.getMonth() + 1).toString(), 2, "0") + split + padLeft(isDate.getDate().toString(), 2, "0");
}

function TimeFormat(date, split) {
    const isDate = new Date(date);
    return padLeft(isDate.getHours().toString(), 2, "0") + split + padLeft(isDate.getMinutes().toString(), 2, "0");
}


function GetMobileOperatingSystem() {
    var userAgent = navigator.userAgent || navigator.vendor || window.opera;

    // Windows Phone must come first because its UA also contains "Android"
    if (/windows phone/i.test(userAgent)) {
        return 2;
    }

    if (/android/i.test(userAgent)) {
        return 1;
    }

    if (/iPad|iPhone|iPod/.test(userAgent) && !window.MSStream) {
        return 0;
    }

    return 3;
}

function addDays(date, days) {
    var result = new Date(date);
    result.setDate(result.getDate() + days);
    return result.getFullYear() + "-" + (result.getMonth() + 1) + "-" + result.getDate();
}

function GetWeekOfYear(date) {
    var d = new Date(+date);
    d.setHours(0, 0, 0);
    d.setDate(d.getDate() + 4 - (d.getDay() || 7));
    return Math.ceil((((d - new Date(d.getFullYear(), 0, 1)) / 8.64e7) + 1) / 7);
}

function GetStartDateOfWeek(w, y) {
    var simple = new Date(y, 0, 1 + (w - 1) * 7);
    var dow = simple.getDay();
    var ISOweekStart = simple;
    if (dow <= 4)
        ISOweekStart.setDate(simple.getDate() - simple.getDay() + 1);
    else
        ISOweekStart.setDate(simple.getDate() + 8 - simple.getDay());
    return ISOweekStart;
}

function padLeft(str, lenght, chr) {
    if (str.toString().length >= lenght)
        return str;
    else
        return padLeft(chr + str, lenght, chr);
}

function padRight(str, lenght, chr) {
    if (str.toString().length >= lenght)
        return str;
    else
        return padRight(str + chr, lenght, chr);
}

function MobileLoading() {
    var $newdiv1 = $(`<div class="dialog-bg" id="divLoading">
            <div class="loader" style="margin-top:10px;">
                <svg class="circle-loader progress">
                    <circle cx="20" cy="20" r="12">
                </svg>
            </div>
        </div>`);
    $("body").append($newdiv1);

    this.show = function () {
        $("#divLoading").show();
    };

    this.hide = function () {
        $("#divLoading").hide();
    };
}

function GetUserInfo() {
    return JSON.parse(localStorage.getItem("UserInfo"));
}

function BindSystemName(obj) {
    obj.html(localStorage.getItem("SystemName"));
}

function sleep(milliseconds) {
    var start = new Date().getTime();
    for (var i = 0; i < 1e7; i++) {
        if ((new Date().getTime() - start) > milliseconds) {
            break;
        }
    }
}

function GetRandomColorRgba() {
    var o = Math.round, r = Math.random, s = 255;
    return 'rgba(' + o(r() * s) + ',' + o(r() * s) + ',' + o(r() * s) + ',' + r().toFixed(1) + ')';
}

function funChangeCycleType(objCycleType, objCycleCount, intDefaultValue) {
    var strCycleType = $("#" + objCycleType).val();
    var intCount = 0;

    if (strCycleType == "minute")
        intCount = 60;
    if (strCycleType == "hour")
        intCount = 24;
    if (strCycleType == "day")
        intCount = 30;
    if (strCycleType == "week")
        intCount = 4;
    if (strCycleType == "month")
        intCount = 12;

    $("#" + objCycleCount + " option").remove();
    for (var i = 1; i <= intCount; i++) {
        if (intDefaultValue == i)
            $("#" + objCycleCount).append($("<option selected></option>").attr("value", i).text(i));
        else
            $("#" + objCycleCount).append($("<option></option>").attr("value", i).text(i));
    }
}

function funGetRoleName(strRoleName) {
    switch (strRoleName) {
        case "universeadmin": strRoleName = "全域管理員"; break;
        case "admin": strRoleName = "系統管理員"; break;
        case "tester": strRoleName = "程式開發管理員"; break;
    }
    return strRoleName;
}