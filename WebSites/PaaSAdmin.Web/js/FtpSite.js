function FtpSites() {
    // #region // ListFtpSites
    this.ListFtpSites = function () {
        var data = {
            "Product": $("#Product").val(),
            "IP": $("#UserName").val(),
            "Domain": $("#PhysicalPath").val(),
            "Sort": "UserName",
            "SortExpress": "Asc",
            "PageSize": 100,
            "PageIndex": 1,
        };

        var strUrl = funParseParamUrl(constListFtpSitesUrl, data);
        callAPI(strUrl, "GET", "", ListFtpSitesSuccess, showException);
    }
    function ListFtpSitesSuccess(data) {

        if (data.isSuccess) {
            var tbData = $("#tbData");
            tbData.empty();
            for (var i = 0; i < data.body.length; i++) {
                tbData.append(`<tr>
                <td>${data.body[i].Product}</td>
                <td>${data.body[i].UserName}</td>
                <td>${data.body[i].PhysicalPath}</td>
                <td>
                    <div class="btn-group btn-group-sm" System="group" aria-label="Table row actions">
                        <button type="button" class="btn btn-white" onclick="javascript:location.href='FtpSitesEdit.html?PaaSFtpSitesId=${data.body[i].PaaSFtpSitesId}';">
                            <i class="fas fa-pen"></i>
                        </button>
                        <button type="button" class="btn btn-white" onclick="javascript:new FtpSites().DeleteFtpSites('${data.body[i].PaaSFtpSitesId}');">
                            <i class="fas fa-trash"></i>
                        </button>
                    </div>
                </td>
            </tr>`);
            }

            $("#divData").show();
        }
        else {
            funListDataFail();
        }
    }
    // #endregion

    // #region // CreateFtpSites
    this.CreateFtpSites = function () {
        var data = {
            "Product": $("#Product").val(),
            "UserName": $("#UserName").val(),
            "UserPassword": $("#UserPassword").val(),
            "PhysicalPath": $("#PhysicalPath").val(),
        }

        callAPI(constCreateFtpSitesUrl, "POST", JSON.stringify(data), CreateFtpSitesSuccess, showException);
    }
    function CreateFtpSitesSuccess(data) {
        if (data.isSuccess == true) {
            var btns = [{ "Text": "確定", "NavigateUrl": "FtpSitesList.html", "Event": "", "IsPrimary": true }];
            funCreateDataSuccess(btns);
        }
        else {
            if (data.message == "FtpAccountIsExits")
                showCloseMessage("發生錯誤", "相同的目錄已存在", "Fail");
            else
                funCreateDataFail();
        }
    }
    // #endregion

    // #region // GetFtpSites
    this.GetFtpSites = function () {
        var strUrl = constGetFtpSitesUrl + "?query.PaaSFtpSitesId=" + getUrlParamter("PaaSFtpSitesId");
        callAPI(strUrl, "GET", "", funGetFtpSitesSuccess, showException);
    }

    function funGetFtpSitesSuccess(data) {
        if (data.isSuccess) {
            $("#Product").val(data.body.Product);
            $("#UserName").val(data.body.UserName);
            $("#UserPassword").val(data.body.UserPassword);
            $("#PhysicalPath").val(data.body.PhysicalPath);
        }
        else {
            funGetDataFail();
        }
    }
    // #endregion

    // #region // DeleteFtpSites
    this.DeleteFtpSites = function (strIISFtpSitesId) {
        var btns = [
            { "Text": "確定", "NavigateUrl": "", "Event": "new FtpSites().DeleteFtpSitesExecute('" + strIISFtpSitesId + "')", "IsPrimary": false },
            { "Text": "取消", "NavigateUrl": "", "Event": "close", "IsPrimary": true },
        ];
        showMessage("確定刪除?", "確定刪除資料?", btns, "Information");
    }

    this.DeleteFtpSitesExecute = function (strIISFtpSitesId) {
        var data = {
            "PaaSFtpSitesId": [strIISFtpSitesId]
        };
        callAPI(constDeleteFtpSitesUrl, "DELETE", JSON.stringify(data), funDeleteFtpSitesSuccess, showException);
    }

    function funDeleteFtpSitesSuccess(data) {
        if (data.isSuccess) {
            var btns = [{ "Text": "確定", "NavigateUrl": "", "Event": "new FtpSites().ListFtpSites();", "IsPrimary": true }];
            funDeleteDataSuccess(btns);
        }
        else {
            funDeleteDataFail();
        }
    }
    // #endregion
}