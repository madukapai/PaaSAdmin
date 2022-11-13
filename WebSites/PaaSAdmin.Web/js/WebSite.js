function WebSites() {
    // #region // ListWebSites
    this.ListWebSites = function () {
        var data = {
            "WebSiteName": $("#WebSiteName").val(),
            "Product": $("#Product").val(),
            "Port": $("#Port").val(),
            "IP": $("#IP").val(),
            "Domain": $("#Domain").val(),
            "Sort": "WebSiteName",
            "SortExpress": "Asc",
            "PageSize": 100,
            "PageIndex": 1,
        };

        var strUrl = funParseParamUrl(constListWebSitesUrl, data);
        callAPI(strUrl, "GET", "", ListWebSitesSuccess, showException);
    }
    function ListWebSitesSuccess(data) {

        if (data.isSuccess) {
            var tbData = $("#tbData");
            tbData.empty();
            for (var i = 0; i < data.body.length; i++) {
                tbData.append(`<tr>
                <td>${data.body[i].WebSiteName}</td>
                <td>${data.body[i].Product}</td>
                <td>${data.body[i].IP}</td>
                <td>${data.body[i].Port}</td>
                <td>${data.body[i].Domain}</td>
                <td>
                    <div class="btn-group btn-group-sm" System="group" aria-label="Table row actions">
                        <button type="button" class="btn btn-white" onclick="javascript:location.href='WebSitesEdit.html?IISWebSitesId=${data.body[i].IISWebSitesId}';">
                            <i class="fas fa-pen"></i>
                        </button>
                        <button type="button" class="btn btn-white" onclick="javascript:new WebSites().DeleteWebSites('${data.body[i].IISWebSitesId}');">
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

    // #region // CreateWebSites
    this.CreateWebSites = function () {
        var data = {
            "WebSiteName": $("#WebSiteName").val(),
            "Product": $("#Product").val(),
            "Port": $("#Port").val(),
            "IP": $("#IP").val(),
            "Domain": $("#Domain").val(),
            "UserName": $("#UserName").val(),
            "UserPassword": $("#UserPassword").val(),
            "PhysicalPath": $("#PhysicalPath").val(),
            "MaxMemoryGB": $("#MaxMemoryGB").val(),
            "MaxInstance": $("#MaxInstance").val(),
            "RecycleMinutes": $("#RecycleMinutes").val(),
            "RuntimeVersion": $("#RuntimeVersion").val(),
            "IsEnable32Bit": $("#IsEnable32Bit").val(),
        }

        callAPI(constCreateWebSitesUrl, "POST", JSON.stringify(data), CreateWebSitesSuccess, showException);
    }
    function CreateWebSitesSuccess(data) {
        if (data.isSuccess == true) {
            var btns = [{ "Text": "確定", "NavigateUrl": "WebSitesList.html", "Event": "", "IsPrimary": true }];
            funCreateDataSuccess(btns);
        }
        else {
            funCreateDataFail();
        }
    }
    // #endregion

    // #region // GetWebSites
    this.GetWebSites = function () {
        var strUrl = constGetWebSitesUrl + "?query.IISWebSitesId=" + getUrlParamter("IISWebSitesId");
        callAPI(strUrl, "GET", "", funGetWebSitesSuccess, showException);
    }

    function funGetWebSitesSuccess(data) {
        if (data.isSuccess) {
            $("#WebSiteName").val(data.body.WebSiteName);
            $("#Product").val(data.body.Product);
            $("#Port").val(data.body.Port);
            $("#IP").val(data.body.IP);
            $("#Domain").val(data.body.Domain);
            $("#UserName").val(data.body.UserName);
            $("#UserPassword").val(data.body.UserPassword);
            $("#PhysicalPath").val(data.body.PhysicalPath);
            $("#MaxMemoryGB").val(data.body.MaxMemoryGB);
            $("#MaxInstance").val(data.body.MaxInstance);
            $("#RecycleMinutes").val(data.body.RecycleMinutes);
            $("#RuntimeVersion").val(data.body.RuntimeVersion);
            $("#IsEnable32Bit").val(data.body.IsEnable32Bit.toString());
        }
        else {
            funGetDataFail();
        }
    }
    // #endregion

    // #region // UpdateWebSites
    this.UpdateWebSites = function () {
        var data = {
            "IISWebSitesId": getUrlParamter("IISWebSitesId"),
            "WebSiteName": $("#WebSiteName").val(),
            "Product": $("#Product").val(),
            "Port": $("#Port").val(),
            "IP": $("#IP").val(),
            "Domain": $("#Domain").val(),
            "UserName": $("#UserName").val(),
            "UserPassword": $("#UserPassword").val(),
            "PhysicalPath": $("#PhysicalPath").val(),
            "MaxMemoryGB": $("#MaxMemoryGB").val(),
            "MaxInstance": $("#MaxInstance").val(),
            "RecycleMinutes": $("#RecycleMinutes").val(),
            "RuntimeVersion": $("#RuntimeVersion").val(),
            "IsEnable32Bit": $("#IsEnable32Bit").val(),
        }

        callAPI(constUpdateWebSitesUrl, "PUT", JSON.stringify(data), funUpdateWebSitesSuccess, showException);
    }

    function funUpdateWebSitesSuccess(data) {
        if (data.isSuccess) {
            var btns = [{ "Text": "確定", "NavigateUrl": "WebSitesList.html", "Event": "", "IsPrimary": true }];
            funUpdateDataSuccess(btns);
        }
        else {
            funUpdateDataFail();
        }
    }
    // #endregion

    // #region // DeleteWebSites
    this.DeleteWebSites = function (strIISWebSitesId) {
        var btns = [
            { "Text": "確定", "NavigateUrl": "", "Event": "new WebSites().DeleteWebSitesExecute('" + strIISWebSitesId + "')", "IsPrimary": false },
            { "Text": "取消", "NavigateUrl": "", "Event": "close", "IsPrimary": true },
        ];
        showMessage("確定刪除?", "確定刪除資料?", btns, "Information");
    }

    this.DeleteWebSitesExecute = function (strIISWebSitesId) {
        var data = {
            "IISWebSitesId": [strIISWebSitesId]
        };
        callAPI(constDeleteWebSitesUrl, "DELETE", JSON.stringify(data), funDeleteWebSitesSuccess, showException);
    }

    function funDeleteWebSitesSuccess(data) {
        if (data.isSuccess) {
            var btns = [{ "Text": "確定", "NavigateUrl": "", "Event": "new WebSites().ListWebSites();", "IsPrimary": true }];
            funDeleteDataSuccess(btns);
        }
        else {
            funDeleteDataFail();
        }
    }
    // #endregion
}