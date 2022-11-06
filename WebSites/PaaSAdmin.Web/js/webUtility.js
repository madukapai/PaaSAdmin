window.onload = function () {
    // 載入上方以及下方的Template檔案
    // $("#page-header").load('/template-header.html');
    // $("#page-footer").load('/template-footer.html');
    if (document.getElementById("page-menu") != null)
        $("#page-menu").load('/template-menu.html', funBindMenu);

    if (document.getElementById("page-title") != null)
        $("#page-title").load('/template-title.html', funBindMember);

    funLoadMessageWindows();

    /// 放入選單項目的功能
    function funBindMenu() {
        // 載入選單
        var menu = $("#divMenu");
        // menu.empty();
        var strContent = "";

        // 放入Brand、Dashboard
        strContent += `<h6 class="main-sidebar__nav-title">Dashboards</h6>
        <ul class="nav nav--no-borders flex-column">
            <li class="nav-item">
                <a class="nav-link" href="/main.html">
                    <i class="material-icons">&#xE917;</i>
                    <span>Home</span>
                </a>
            </li>
        </ul>`;

        var data = JSON.parse(localStorage.getItem("Menu"));
        data = data.ListSystem;

        for (var r = 0; r < data.length; r++) {
            strContent += `<h6 class="main-sidebar__nav-title">${data[r].SystemName}</h6>
                              <ul class="nav nav--no-borders flex-column">`;
            // 放入第二層選單
            for (var s = 0; s < data[r].ListFunc.length; s++) {
                strContent += `<li class="nav-item dropdown">
                <a class="nav-link dropdown-toggle " data-toggle="dropdown" href="#" role="button" aria-haspopup="true" aria-expanded="true">
                    <i class="${data[r].ListFunc[s].IconName}"></i>
                    <span>${data[r].ListFunc[s].FuncName}</span>
                </a>
                <div class="dropdown-menu dropdown-menu-small">`;

                for (var t = 0; t < data[r].ListFunc[s].ListFunc.length; t++) {
                    if (data[r].ListFunc[s].ListFunc[t].DbConfig == "")
                        strContent += `<a class="dropdown-item " href="/${data[r].ListFunc[s].ListFunc[t].NavigateUrl}"><i class="${data[r].ListFunc[s].ListFunc[t].IconName}"></i>&nbsp;${data[r].ListFunc[s].ListFunc[t].FuncName}</a>`;
                    else
                        strContent += `<a class="dropdown-item " href="/Page/Auto/List.html?FuncId=${data[r].ListFunc[s].ListFunc[t].FuncId}"><i class="${data[r].ListFunc[s].ListFunc[t].IconName}"></i>&nbsp;${data[r].ListFunc[s].ListFunc[t].FuncName}</a>`;
                }

                strContent += `</div></li>`;
            }

            strContent += `</ul>`;
        }

        menu.append(strContent);
        $("#spanMenuTitle").html(localStorage.getItem("SystemName"));
    }

    /// 放入會員資料的動作
    function funBindMember() {
        var data = JSON.parse(localStorage.getItem("UserInfo"));
        // 放入會員上方的Title資料
        $("#spanName").html(data.UserName);

        /*
        // 放入大頭照
        if (data.imageUrl != "")
            $("#imgPerson").attr("src", data.imageUrl);

        // 載入站內未讀訊息的數量與訊息
        funGetUnreadCount();
        */

        // 延遲載入Javascript檔案
        funLoadLazyScriptLoad();
    }

    /// 延遲載入Javascript檔案的動作
    function funLoadLazyScriptLoad() {
        // shards-dashboards的延遲載入
        var script = document.createElement('script');
        script.src = "/scripts/shards-dashboards.1.3.1.min.js";
        document.body.appendChild(script);

        // 顯示內容框架
        $("#divContent").show();
    }

    function funLoadMessageWindows() {
        // Do anotherthing...
        var divModal = document.createElement('div');
        divModal.tabIndex = -1;
        divModal.id = "divMessageBox";
        divModal.role = "dialog";
        divModal.setAttribute("aria-labelledby", "exampleModalLabel");
        divModal.setAttribute("aria-hidden", "true");
        divModal.className = "modal fade";

        var divDocument = document.createElement('div');
        divDocument.className = "modal-dialog";
        divDocument.id = "divMessageBoxDialog";
        divDocument.role = "document";
        divModal.appendChild(divDocument);

        var divContent = document.createElement('div');
        divContent.className = "modal-content";
        divDocument.appendChild(divContent);

        // 加入Title
        var divTitle = document.createElement('div');
        divTitle.id = "divMessageBoxTitle";
        divTitle.className = "modal-header";
        divContent.appendChild(divTitle);

        // 加入內容
        var divBody = document.createElement('div');
        divBody.id = "divMessageBoxBody";
        divBody.className = "modal-body";
        divContent.appendChild(divBody);

        // 加入Button
        var divButton = document.createElement('div');
        divButton.id = "divMessageBoxButton";
        divButton.className = "modal-footer";
        divContent.appendChild(divButton);
        document.body.appendChild(divModal);

        // 加入行動裝置用的訊息
        var divMobileMessage = document.createElement('div');
        divMobileMessage.style.display = "none";
        divMobileMessage.id = "divMobileMessage";
        divMobileMessage.className = "dialog-bg black";

        var divMobileMessageContent = document.createElement('div');
        divMobileMessageContent.id = "divMobileMessageContent";
        divMobileMessageContent.className = "alert";
        divMobileMessage.appendChild(divMobileMessageContent);
        document.body.appendChild(divMobileMessage);
    }

    // 變更Title
    document.title = localStorage.getItem("SystemName");

    // 啟用讀取站內信數量的動作
    funGetUnreadCount();
    setInterval(funGetUnreadCount, 3000);
};

var _intMessageCount = 0;
function funGetUnreadCount() {
    var userId = getCookies("UserId");
    var strUrl = constMailBoxUnreadCountUrl + "?query.UserId=" + userId;
    callAPI(strUrl, "GET", "", funGetUnreadCountSuccess, showException);
}

function funGetUnreadCountSuccess(data) {
    if (data.IsSuccess) {
        if (data.Body != "0") {
            if (_intMessageCount != parseInt(data.Body)) {
                $("#spanMailCount").html(data.Body);
                funListUnreadMail();
                _intMessageCount = parseInt(data.Body);
            }
        }
    }
    else {
        showCloseMessage("發生錯誤", "取得信箱資料失敗，請稍後再試", "Fail");
    }
}

function funListUnreadMail() {
    var userId = getCookies("UserId");
    var strUrl = constListMailBoxUrl + "?query.IsRead=false&query.UserId=" + userId;
    callAPI(strUrl, "GET", "", funListUnreadMailSuccess, showException);
}

function funListUnreadMailSuccess(data) {
    if (data.IsSuccess) {
        var divMessage = $("#divTitleMessage");
        divMessage.empty();

        divMessage.append(`<a class="dropdown-item notification__category text-center" onclick="javascript:funMarkAllIsReadMail();"> 全部清除<i class="far fa-trash-alt"></i> </a>`);

        for (var i = 0; i < data.Body.length; i++) {
            divMessage.append(`<a style="position:relative;" class="dropdown-item">
                        <div class="notification__content">
                            <span class="notification__category">${data.Body[i].MessageContent}</span>
                            <p>${DateFormat(data.Body[i].SendDateTime, "-")}</p>
                        </div>
                        <div style="position:absolute;right : 5px; top: 5px">
                            <i class="far fa-trash-alt notification__category" onclick="javascript:funMarkIsReadMail('${data.Body[i].MessageId}');"></i>
                        </div>
                    </a>`);
        }
    }
    else {
        showCloseMessage("發生錯誤", "讀取信箱資料失敗，請稍後再試", "Fail");
    }
}

function funMarkIsReadMail(messageId) {
    var data = {
        "UserId": getCookies("UserId"),
        "AccessToken": getCookies("AccessToken"),
        "MessageId": messageId,
    };
    callAPI(constMarkIsReadMailBoxUrl, "PUT", JSON.stringify(data), function () { $("#spanMailCount").empty(); $("#divTitleMessage").empty(); funGetUnreadCount(); }, null);
}

function funMarkAllIsReadMail() {
    var data = {
        "UserId": getCookies("UserId"),
        "AccessToken": getCookies("AccessToken"),
    };
    callAPI(constMarkAllIsReadMailBoxUrl, "PUT", JSON.stringify(data), function () { $("#spanMailCount").empty(); $("#divTitleMessage").empty(); $("#divTitleMessage").append(`<a class="dropdown-item notification__all text-center" href="#"> 沒有新訊息 </a>`); }, null);
}