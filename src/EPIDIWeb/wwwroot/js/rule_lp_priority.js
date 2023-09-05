var tempInstument = [];
var countRuleIBInstrument = 0;
var countIBRuleDetails = 0;
function AddNewLPPrioJs(count = 0, RuleInstrumentId = 0) {
    $("#RuleLP_" + count).append(`<div class="row" style="MARGIN-LEFT: 9PX;">
        <div class="col-2" id="divLPId_${count}_${countLPRuleDetails}">
            <input id="objRuleLpPriorityViewModel_Id_${count}_${countLPRuleDetails}" type="hidden">
            <input id="objRuleLpPriorityViewModel_RuleId_${count}_${countLPRuleDetails}" type="hidden">
            <input id="objRuleLpPriorityViewModel_RuleInstrumentId_${count}_${countLPRuleDetails}" type="hidden" value="${RuleInstrumentId}">
            <div class="form-group">
                <label>LP Rule</label>
                    <select id="objRuleLpPriorityViewModel_LPId_${count}_${countLPRuleDetails}" class="form-control">
                    </select>
            </div>
        </div>
        <div class="col-1" id="divPriority_${count}_${countLPRuleDetails}">
            <div class="form-group">
                <label>Priority</label>
                <input class="form-control" id="objRuleLpPriorityViewModel_Priority_${count}_${countLPRuleDetails}" placeholder="Priority" type="number">
            </div>
        </div>
        <div class="col-2" id="divMarkUpValuey_${count}">
            <div class="form-group">
                <label>MarkUp Value</label>
                <input class="form-control" id="objRuleLpPriorityViewModel_MarkUpValue_${count}_${countLPRuleDetails}" placeholder="MarkUpValue" type="number">
            </div>
        </div>
       <div class="col-2">
            <div class="form-group">
                <label>MBId</label>
                <input class="form-control" id="objRuleLpPriorityViewModel_MbId_${count}_${countLPRuleDetails}" placeholder="MBId" type="number">
            </div>
        </div>
        <div class="col-2">
            <div class="form-group">
                <label>Mask</label>
                <input class="form-control" id="objRuleLpPriorityViewModel_Mask_${count}_${countLPRuleDetails}" placeholder="Mask" type="number">
            </div>
        </div>
        <div class="col-2">
            <div class="form-group">
                <label>Timeout <span>(millisecond)</span></label>
                <input class="form-control" id="objRuleLpPriorityViewModel_Timeout_${count}_${countLPRuleDetails}" placeholder="Timeout" type="number">
            </div>
        </div>
        <div class= "col-1"> 
            <div class= "form-group">
                <label></label>
                <a href = "javascript:void(0)" value = "Delete" onclick = "DeleteLpRule(0,${RuleInstrumentId})" style = "display:block;width:100%;height:calc(2.25rem + 2px);padding:0.375rem 0.75rem;"> <i class="fa fa-trash" style="font-size: large;margin-top: 8px;" aria-hidden="true"></i></a>
            </div>
        </div>
        </div>
    `);
    BindLpRule(count + "_" + countLPRuleDetails);
    countLPRuleDetails = countLPRuleDetails + 1;
    //if (response != "") {
    //    $("#objRuleLpPriorityViewModel_Priority_" + countLPRuleDetails).val(response.priority);
    //    $("#objRuleLpPriorityViewModel_MarkUpValue_" + countLPRuleDetails).val(response.markUpValue);
    //    $("#objRuleLpPriorityViewModel_MbId_" + countLPRuleDetails).val(response.mbid);
    //    $("#objRuleLpPriorityViewModel_Mask_" + countLPRuleDetails).val(response.mask);
    //    $("#objRuleLpPriorityViewModel_Timeout_" + countLPRuleDetails).val(response.timeout);

    //    $("#objRuleLpPriorityViewModel_Id_" + countLPRuleDetails).val(response.id);
    //    $("#objRuleLpPriorityViewModel_RuleId_" + countLPRuleDetails).val(response.ruleId);
    //    $("#objRuleLpPriorityViewModel_RuleInstrumentId_" + countLPRuleDetails).val(response.ruleInstrumentId);
    //}
}

function AddNeInstrumentRule_Js(response = "") {
    $("#DivInstrument").append(`<div class="row">
    <div class="col-2">
        <div class="form-group">
        <input id="objRuleInstrument_Id_${countRuleInstrument}" type="hidden">
        <input id="objRuleInstrumen_RuleId_${countRuleInstrument}" type="hidden">
            <label>Instruments</label>
            <select class="form-control" id="instrumentid_${countRuleInstrument}" style="border:1px solid;">                               
            </select>
        </div>
            </div >
            <div class="col-2">
                <div class="form-group">
                    <label>Day</label>
                    <input class="form-control" id="Day_${countRuleInstrument}" name="Day">
                </div>
            </div>
            <div class="col-2">
                <div class="form-group">
                    <label>Start Time</label>
                    <input class="form-control" id="StartTime_${countRuleInstrument}" type="time" style="width:95%">
                </div>
            </div>
            <div class="col-2">
                <div class="form-group">
                    <label>End Time</label>
                    <input class="form-control" id="EndTime_${countRuleInstrument}" type="time" style="width:95%">
                </div>
            </div>
            <div class="col-2">
                <div class="form-group">
                    <label>Max Spread</label>
                    <input class="form-control valid" id="MaxSpread_${countRuleInstrument}" placeholder="Max Spread" type="number">
                </div>
            </div>
            <div class="col-2">
                <div class="form-group">
                    <label></label>
                    <a href="javascript:void(0)" value="Delete" onclick="DeleteInstrument(${countRuleInstrument})" style=" display: block; width: 100%; height: calc(2.25rem + 2px); padding: 0.375rem 0.75rem;" ><i class="fa fa-trash" style="font-size: large;margin-top: 8px;" aria-hidden="true"></i></a>
                </div>
            </div>
        </div >
        
`);
    BindIntruments(countRuleInstrument, response.instrumentId);
    if (response != "") {
        $("#instrumentid_" + countRuleInstrument).val(response.instrumentId);
        $("#Day_" + countRuleInstrument).val(response.day);
        var _startTime = moment(response.startTime).format("HH:mm:ss");
        $("#StartTime_" + countRuleInstrument).val(_startTime);
        var _endTime = moment(response.endTime).format("HH:mm:ss");
        $("#EndTime_" + countRuleInstrument).val(_endTime);
        $("#MaxSpread_" + countRuleInstrument).val(response.maxSpread);
        $("#objRuleInstrument_Id_" + countRuleInstrument).val(response.id);
        $("#objRuleInstrumen_RuleId_" + countRuleInstrument).val(response.ruleId);
    }
    countRuleInstrument = countRuleInstrument + 1;
}
function BindLpRule(count, defaultVal = "") {
    $.ajax({
        async: true,
        type: "POST",
        url: '/QuoteMarkUpRule/GetMarketLPDropDown',
        dataType: 'json',
        data: {},
        success: function (response) {
            $("#objRuleLpPriorityViewModel_LPId_" + count).append(`<option value="">--Select--</option>`)
            for (var i = 0; i < response.length; i++) {
                $("#objRuleLpPriorityViewModel_LPId_" + count).append(`<option value="${response[i].value}">${response[i].text}</option>`)
            }
            if (defaultVal != "") {
                $("#objRuleLpPriorityViewModel_LPId_" + count).val(defaultVal);
            }
        },
        error: function (errorTemp) {
            console.log('error', errorTemp);
        }
    })
}
var countRuleInstrument = 0;
var countLPRuleDetails = 0;
function GetInstrumentWithRuleLpInEdit(response) {
    var strHTML = "";
    strHTML = `<div class="row">`;
    strHTML += `<div class="col-2">`;
    strHTML += `<div class="form-group">`;
    strHTML += `<input id="objRuleInstrument_Id_${countRuleInstrument}" type="hidden">`;
    strHTML += `<input id="objRuleInstrumen_RuleId_${countRuleInstrument}" type="hidden">`;
    strHTML += `<label>Instruments</label>`;
    strHTML += `<select class="form-control" id="instrumentid_${countRuleInstrument}" style="border:1px solid;">                               `;
    strHTML += `</select>`;
    strHTML += `</div>`;
    strHTML += `</div >`;
    strHTML += `<div class="col-2">`;
    strHTML += `<div class="form-group">`;
    strHTML += `<label>Day</label>`;
    strHTML += `<input class="form-control" id="Day_${countRuleInstrument}" name="Day">`;
    strHTML += `</div>`;
    strHTML += `</div>`;
    strHTML += `<div class="col-2">`;
    strHTML += `<div class="form-group">`;
    strHTML += `<label>Start Time</label>`;
    strHTML += `<input class="form-control" id="StartTime_${countRuleInstrument}" type="time" style="width:95%">`;
    strHTML += `</div>`;
    strHTML += `</div>`;
    strHTML += `<div class="col-2">`;
    strHTML += `<div class="form-group">`;
    strHTML += `<label>End Time</label>`;
    strHTML += `<input class="form-control" id="EndTime_${countRuleInstrument}" type="time" style="width:95%">`;
    strHTML += `</div>`;
    strHTML += `</div>`;
    strHTML += `<div class="col-2">`;
    strHTML += `<div class="form-group">`;
    strHTML += `<label>Max Spread</label>`;
    strHTML += `<input class="form-control valid" id="MaxSpread_${countRuleInstrument}" placeholder="Max Spread" type="number">`;
    strHTML += `</div>`;
    strHTML += `</div>`;
    strHTML += `<div class="col-2">`;
    strHTML += `<div class="form-group">`;
    strHTML += `<label></label>`;
    strHTML += `<a href="javascript:void(0)" value="Delete" onclick="DeleteInstrument(${countDetails})" style=" display: block; width: 100%; height: calc(2.25rem + 2px); padding: 0.375rem 0.75rem;" ><i class="fa fa-trash" style="font-size: large;margin-top: 8px;" aria-hidden="true"></i></a>`;
    strHTML += `</div>`;
    strHTML += `</div>`;
    strHTML += `</div>`;
    strHTML += `<div class="row" style="border: 1px solid #17A2BA;">`;
    strHTML += `<div class="col-12">`;
    strHTML += `<div class="card card-warning">`;
    strHTML += `<div class="card-header">`;
    strHTML += `<h3 class="card-title w-100">`;
    strHTML += `<a class="d-block w-100" data-toggle="collapse" href="#collapse1_${countRuleInstrument}" aria-expanded="false">`;
    strHTML += `LP Priority`;
    strHTML += `</a>`;
    strHTML += `</h3>`;
    strHTML += `</div>`;
    strHTML += `<div id="collapse1_${countRuleInstrument}" class="collapse">`;
    strHTML += `<div class="row">`;
    strHTML += `<div class="col-12">`;
    strHTML += `<div class="row" id="RuleLP_${countRuleInstrument}" style="background-color: lightgray;margin: 5PX;">`;
    for (var i = 0; i < response.ruleLpPriorityViewModelList.length; i++) {
        strHTML += `<div class="row" style="MARGIN-LEFT: 9PX;">`;
        strHTML += `<div class="col-2" id="divLPId_${countRuleInstrument}_${countLPRuleDetails}">`;
        strHTML += `<input id="objRuleLpPriorityViewModel_Id_${countRuleInstrument}_${countLPRuleDetails}" type="hidden" value="${response.ruleLpPriorityViewModelList[i].id}">`;
        strHTML += `<input id="objRuleLpPriorityViewModel_RuleId_${countRuleInstrument}_${i}" type="hidden" value="${response.ruleLpPriorityViewModelList[i].ruleId}">`;
        strHTML += `<input id="objRuleLpPriorityViewModel_RuleInstrumentId_${countRuleInstrument}_${countLPRuleDetails}" type="hidden" value="${response.ruleLpPriorityViewModelList[i].ruleInstrumentId}">`;
        strHTML += `<div class="form-group">`;
        strHTML += `<label>LP </label>`;
        strHTML += `<select id="objRuleLpPriorityViewModel_LPId_${countRuleInstrument}_${countLPRuleDetails}" class="form-control">`;
        strHTML += `</select>`;
        strHTML += `</div>`;
        strHTML += `</div>`;
        strHTML += `<div class="col-1" id="divPriority_${countRuleInstrument}_${countLPRuleDetails}">`;
        strHTML += `<div class="form-group">`;
        strHTML += `<label>Priority</label>`;
        strHTML += `<input class="form-control" id="objRuleLpPriorityViewModel_Priority_${countRuleInstrument}_${countLPRuleDetails}" placeholder="Priority" type="number" value="${response.ruleLpPriorityViewModelList[i].priority}">`;
        strHTML += `</div>`;
        strHTML += `</div>`;
        strHTML += `<div class="col-2" id="divMarkUpValuey_${countRuleInstrument}_${countLPRuleDetails}">`;
        strHTML += `<div class="form-group">`;
        strHTML += `<label>MarkUp Value</label>`;
        strHTML += `<input class="form-control" id="objRuleLpPriorityViewModel_MarkUpValue_${countRuleInstrument}_${countLPRuleDetails}" placeholder="MarkUpValue" type="number" value="${response.ruleLpPriorityViewModelList[i].markUpValue}">`;
        strHTML += `</div>`;
        strHTML += `</div>`;
        strHTML += `<div class="col-2">`;
        strHTML += `<div class="form-group">`;
        strHTML += `<label>MBId</label>`;
        strHTML += `<input class="form-control" id="objRuleLpPriorityViewModel_MbId_${countRuleInstrument}_${countLPRuleDetails}" placeholder="MBId" type="number" value="${response.ruleLpPriorityViewModelList[i].mbid}">`;
        strHTML += `</div>`;
        strHTML += `</div>`;
        strHTML += `<div class="col-2">`;
        strHTML += `<div class="form-group">`;
        strHTML += `<label>Mask</label>`;
        strHTML += `<input class="form-control" id="objRuleLpPriorityViewModel_Mask_${countRuleInstrument}_${countLPRuleDetails}" placeholder="Mask" type="number" value="${response.ruleLpPriorityViewModelList[i].mask}">`;
        strHTML += `</div>`;
        strHTML += `</div>`;
        strHTML += `<div class="col-2">`;
        strHTML += `<div class="form-group">`;
        strHTML += `<label>Timeout</label>`;
        strHTML += `<input class="form-control" id="objRuleLpPriorityViewModel_Timeout_${countRuleInstrument}_${countLPRuleDetails}" placeholder="Timeout" type="number" value="${response.ruleLpPriorityViewModelList[i].timeout}">`;
        strHTML += `</div>`;
        strHTML += `</div>`;
        strHTML += `<div class="col-1">`;
        strHTML += `<div class="form-group">`;
        strHTML += `<label></label>`;
        strHTML += `<a href="javascript:void(0)" value="Delete" onclick="DeleteLpRule(${response.ruleLpPriorityViewModelList[i].id},${response.ruleLpPriorityViewModelList[i].ruleId})" style=" display: block; width: 100%; height: calc(2.25rem + 2px); padding: 0.375rem 0.75rem;" ><i class="fa fa-trash" style="font-size: large;margin-top: 8px;" aria-hidden="true"></i></a>`;
        strHTML += `</div>`;
        strHTML += `</div>`;
        strHTML += `</div>`;

        BindLpRule(countRuleInstrument + "_" + countLPRuleDetails, response.ruleLpPriorityViewModelList[i].lpId);
        countLPRuleDetails = countLPRuleDetails + 1;
    }
    strHTML += `</div>`;
    strHTML += `<div class="row col-12 mt-1" style="float: right;">`;
    strHTML += `<div class="form-group">`;
    strHTML += `<button type="button" onclick="AddNewLPPrio(${countRuleInstrument},${response.id});" id="addnewLPPrio_${countRuleInstrument}" class="btn btn-primary">Add LP Priority</button>`;
    strHTML += `</div>`;
    strHTML += `</div>`;
    strHTML += `</div>`;
    strHTML += `</div>`;
    strHTML += `</div>`;
    strHTML += `</div>`;
    strHTML += `</div > `;
    strHTML += `</div >`;
    $("#DivInstrument").append(strHTML);
    BindIntruments(countRuleInstrument, response.instrumentId);
    if (response != "") {
        $("#instrumentid_" + countRuleInstrument).val(response.instrumentId);
        $("#Day_" + countRuleInstrument).val(response.day);
        var _startTime = moment(response.startTime).format("HH:mm:ss");
        $("#StartTime_" + countRuleInstrument).val(_startTime);
        var _endTime = moment(response.endTime).format("HH:mm:ss");
        $("#EndTime_" + countRuleInstrument).val(_endTime);
        $("#MaxSpread_" + countRuleInstrument).val(response.maxSpread);
        $("#objRuleInstrument_Id_" + countRuleInstrument).val(response.id);
        $("#objRuleInstrumen_RuleId_" + countRuleInstrument).val(response.ruleId);
    }
    //for (var i = 0; i < response.ruleLpPriorityViewModelList.length; i++) {
    //    if (response.ruleLpPriorityViewModelList != "") {
    //        $("#objRuleLpPriorityViewModel_Priority_" + countRuleInstrument + "_" + i).val(response.ruleLpPriorityViewModelList[i].priority);
    //        $("#objRuleLpPriorityViewModel_MarkUpValue_" + countRuleInstrument + "_" + i).val(response.ruleLpPriorityViewModelList[i].markUpValue);
    //        $("#objRuleLpPriorityViewModel_MbId_" + countRuleInstrument + "_" + i).val(response.ruleLpPriorityViewModelList[i].mbid);
    //        $("#objRuleLpPriorityViewModel_Mask_" + countRuleInstrument + "_" + i).val(response.ruleLpPriorityViewModelList[i].mask);
    //        $("#objRuleLpPriorityViewModel_Timeout_" + countRuleInstrument + "_" + i).val(response.ruleLpPriorityViewModelList[i].timeout);

    //        $("#objRuleLpPriorityViewModel_Id_" + countRuleInstrument + "_" + i).val(response.ruleLpPriorityViewModelList[i].id);
    //        $("#objRuleLpPriorityViewModel_RuleId_" + countRuleInstrument + "_" + i).val(response.ruleLpPriorityViewModelList[i].ruleId);
    //        $("#objRuleLpPriorityViewModel_RuleInstrumentId_" + countRuleInstrument + "_" + i).val(response.ruleLpPriorityViewModelList[i].ruleInstrumentId);
    //    }
    //}
    countRuleInstrument = countRuleInstrument + 1;
}
var countRuleInstrument = 1;
var countLPRuleDetails = 0;
function GetInstrumentWithRuleLpInEditTable(response) {
    countRuleInstrument = countRuleInstrument + 1;

    var strHTML = "";
    if (countRuleInstrument == "0") {
        strHTML += `<tr>`;
        strHTML += `<th>Instruments</th>`;
        strHTML += `<th>Day</th>`;
        strHTML += `<th>Start Time</th>`;
        strHTML += `<th>End Time</th>`;
        strHTML += `<th>Max Spread <span style="font-size: 12px;font-style: italic;">(decimal)</span></th>`;
        strHTML += `<th>LP</th>`;
        strHTML += `<th>Priority</th>`;
        strHTML += `<th>MBId <span style="font-size: 12px;font-style: italic;">(decimal)</span></th>`;
        strHTML += `<th>Mask <span style="font-size: 12px;font-style: italic;">(decimal)</span></th>`;
        strHTML += `<th>Timeout <span style="font-size: 12px;font-style: italic;">(millisecond)</span></th>`;
        strHTML += `<th>Action</th>`;
        strHTML += `</tr>`;
    }
    strHTML += `<tr>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group" style="width:130px;">`;
    strHTML += `<input id="objRuleInstrument_Id_${countRuleInstrument}" type="hidden">`;
    strHTML += `<input id="objRuleInstrumen_RuleId_${countRuleInstrument}" type="hidden">`;

    if (response == null || response == 'undefined') {
        //strHTML += `<input id="instrumentid_${countRuleInstrument}" type="hidden">`;
        //strHTML += `<input class="form-control" value="" type="text" disabled="disabled">`;
        strHTML += `<select class="form-control" id="instrumentid_${countRuleInstrument}" style="border:1px solid;">`;
        strHTML += `</select>`;
    }
    else {
        strHTML += `<input id="instrumentid_${countRuleInstrument}" value="${response.instrumentId}" type="hidden">`;
        strHTML += `<input class="form-control" id="instrumentName_${countRuleInstrument}" value="${response.instrumentName}" type="text" disabled="disabled">`;
    }
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group" style="width:100px;">`;
    strHTML += `<input class="form-control" id="Day_${countRuleInstrument}" name="Day">`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group">`;
    strHTML += `<input class="form-control" id="StartTime_${countRuleInstrument}" type="time" step="1" min="00:00" max="23:59" style="width:95%">`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group">`;
    strHTML += `<input class="form-control" id="EndTime_${countRuleInstrument}" type="time" step="1" min="00:00" max="23:59" style="width:95%"> `;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group" style="width:150px;">`;
    strHTML += `<input class="form-control valid" id="MaxSpread_${countRuleInstrument}" placeholder="Max Spread" type="number">`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<input id="objRuleLpPriorityViewModel_Id_${countRuleInstrument}" type="hidden">`;
    strHTML += `<input id="objRuleLpPriorityViewModel_RuleId_${countRuleInstrument}" type="hidden">`;
    strHTML += `<input id="objRuleLpPriorityViewModel_RuleInstrumentId_${countRuleInstrument}" type="hidden">`;
    strHTML += `<div class="form-group" style="width:115px;">`;
    strHTML += `<select id="objRuleLpPriorityViewModel_LPId_${countRuleInstrument}" class="form-control">`;
    strHTML += `</select>`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group" style="width:80px;">`;
    strHTML += `<input class="form-control" id="objRuleLpPriorityViewModel_Priority_${countRuleInstrument}" placeholder="Priority" type="number">`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group" style="width:150px;">`;
    strHTML += `<input class="form-control" id="objRuleLpPriorityViewModel_MbId_${countRuleInstrument}" placeholder="MBId" type="number">`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group" style="width:150px;">`;
    strHTML += `<input class="form-control" id="objRuleLpPriorityViewModel_Mask_${countRuleInstrument}" placeholder="Mask" type="number">`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group" style="width:120px;">`;
    strHTML += `<input class="form-control" id="objRuleLpPriorityViewModel_Timeout_${countRuleInstrument}" placeholder="Timeout" type="number">`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group">`;
    strHTML += `<a href="javascript:void(0)" value="Delete" onclick="DeleteLpRule($(this))" style=" display: block; width: 100%; height: calc(2.25rem + 2px); padding: 0.375rem 0.75rem;">`;
    strHTML += `<i class="fa fa-trash" style="font-size: large;margin-top: 8px;" aria-hidden="true"></i>`;
    strHTML += `</a>`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `</tr>`;

    $("#tblInstrument").append(strHTML);


    if (response != undefined) {

        //BindIntruments(countRuleInstrument, response.instrumentId);
        BindLpRule(countRuleInstrument, response.lpId);
        $("#instrumentid_" + countRuleInstrument).val(response.instrumentId);
        $("#Day_" + countRuleInstrument).val(response.day);
        var _startTime = moment(response.startTime).format("HH:mm:ss");
        //$("#DateApproved").val(moment(response.Item.DateApprovedStr).format("YYYY-MM-DD"))
        $("#StartTime_" + countRuleInstrument).val(response.startTime.toString('HH:mm ss'));
        //var _endTime = moment(response.endTime).format("HH:mm:ss");
        $("#EndTime_" + countRuleInstrument).val(response.endTime);
        $("#MaxSpread_" + countRuleInstrument).val(response.maxSpread);
        $("#objRuleInstrument_Id_" + countRuleInstrument).val(response.id);
        $("#objRuleInstrumen_RuleId_" + countRuleInstrument).val(response.ruleId);

        $("#objRuleLpPriorityViewModel_Priority_" + countRuleInstrument).val(response.priority);
        //$("#objRuleLpPriorityViewModel_MarkUpValue_" + countRuleInstrument).val(response.markUpValue);
        $("#objRuleLpPriorityViewModel_MbId_" + countRuleInstrument).val(response.mbid);
        $("#objRuleLpPriorityViewModel_Mask_" + countRuleInstrument).val(response.mask);
        $("#objRuleLpPriorityViewModel_Timeout_" + countRuleInstrument).val(response.timeout);

        $("#objRuleLpPriorityViewModel_Id_" + countRuleInstrument).val(response.tblLP_ID);
        $("#objRuleLpPriorityViewModel_RuleId_" + countRuleInstrument).val(response.ruleId);
        $("#objRuleLpPriorityViewModel_RuleInstrumentId_" + countRuleInstrument).val(response.ruleInstrumentId);
    }
    else {
        console.log("countRuleInstrument Get-" + countRuleInstrument);
        BindIntruments(countRuleInstrument);
        BindLpRule(countRuleInstrument);
    }
}
function GetLeverageInEditTable(response) {
    debugger
    var rows = $('#tblInstrument').find('tbody tr');
    var lastTR = $("tr:last");
    var tdHtml = lastTR.find("td:first").html();

    countRuleInstrument = countRuleInstrument + 1;
    //countRuleInstrument = $("#countRuleInstrument").val();
    console.log("countRuleInstrument -" + countRuleInstrument);
    var strHTML = "";
    //if (countRuleInstrument == "0") {
    //    strHTML += `<tr>`;
    //    strHTML += `<th>#</th>`;
    //    strHTML += `<th>Instruments</th>`;
    //    strHTML += `<th>Symbol Group</th>`;
    //    strHTML += `<th>Day</th>`;
    //    strHTML += `<th>Start Time</th>`;
    //    strHTML += `<th>End Time</th>`;
    //    strHTML += `<th>Priority</th>`;
    //    strHTML += `<th>Band From</th>`;
    //    strHTML += `<th>Band To</th>`;
    //    strHTML += `<th>Leverage</th>`;
    //    strHTML += `<th>New Position</th>`;
    //    strHTML += `<th>Legal Entity </th>`;
    //    strHTML += `<th>Action</th>`;
    //    strHTML += `</tr>`;
    //}
    strHTML += `<tr>`;
    //strHTML += `<td>${countRuleInstrument}`;
    //strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group">`;
    strHTML += `<input id="objRuleInstrument_Id_${countRuleInstrument}" type="hidden">`;
    strHTML += `<input id="objRuleInstrumen_RuleId_${countRuleInstrument}" type="hidden">`;
    var instruments = JSON.parse($('#InstrumentsList').val());
    strHTML += `<select class="form-control" id="instrumentid_${countRuleInstrument}" style="border:1px solid; onchange="GetSymbol(0);" ">`;
    $.each(instruments, function (i, j) {
        //strHTML += `<input id="instrumentid_${countRuleInstrument}" type="hidden">`;
        //strHTML += `<input class="form-control" value="" type="text" disabled="disabled">`;
        strHTML += '<option value="' + instruments[i].id + '">' + instruments[i].name + '</Option>';
    });
    strHTML += `</select>`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td style="width:130px;">`;
    strHTML += `<div class="form-group">`;
    var symbols = JSON.parse($('#SymbolList').val());
    strHTML += `<select class="form-control" id="symbolgroup_${countRuleInstrument}" name="Symbols">`;
    $.each(symbols, function (i, j) {
        if (symbols[i].Group != undefined && symbols[i].Group != null && symbols[i].Group != '') {
            strHTML += '<option value="' + symbols[i].Id + '">' + symbols[i].Group + '</Option>';
        }
    });
    //strHTML += `<input id="symbolgroup${countRuleInstrument}" type="hidden">`;
    //strHTML += `<input class="form-control" value="" type="text" disabled="disabled">`;
    strHTML += `</select>`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group">`;
    strHTML += `<input class="form-control" id="day_${countRuleInstrument}" >`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group">`;
    strHTML += `<input class="form-control" id="StartTime_${countRuleInstrument}" type="time" step="1" min="00:00" max="23:59" style="width:95%"> `;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group">`;
    strHTML += `<input class="form-control" id="EndTime_${countRuleInstrument}" type="time" step="1" min="00:00" max="23:59" style="width:95%"> `;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td style="width:100px;">`;
    strHTML += `<div class="form-group">`;
    strHTML += `<input class="form-control valid" id="Priority_${countRuleInstrument}" placeholder="Max Spread" type="number">`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td style="width:100px;">`;
    strHTML += `<input id="objRuleLpPriorityViewModel_Id_${countRuleInstrument}" type="hidden">`;
    strHTML += `<input id="objRuleLpPriorityViewModel_RuleId_${countRuleInstrument}" type="hidden">`;
    strHTML += `<input id="objRuleLpPriorityViewModel_RuleInstrumentId_${countRuleInstrument}" type="hidden">`;
    strHTML += `<div class="form-group">`;
    strHTML += `<input class="form-control valid" id="BandFrom_${countRuleInstrument}" placeholder="from" type="number">`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td style="width:100px;">`;
    strHTML += `<div class="form-group">`;
    strHTML += `<input class="form-control valid" id="BandTo_${countRuleInstrument}" placeholder="To" type="number">`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td style="width:80px;">`;
    strHTML += `<div class="form-group">`;
    strHTML += `<input class="form-control" id="LeverageAmount_${countRuleInstrument}" placeholder="Leverage" type="number">`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td style="width:80px;">`;
    strHTML += `<div class="form-group">`;
    strHTML += `<select class="form-control" id="NewPosition_${countRuleInstrument}" style="border:1px solid;">`;
    strHTML += '<option>Yes</option>';

    strHTML += '<option>No</option>';
    strHTML += `</select>`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td style="width:100px;">`;
    strHTML += `<div class="form-group" style="width:300px;">`;
    var legalentity = JSON.parse($('#LegalEntityList').val());
    console.log("legalentity : " + legalentity);
    strHTML += `<select class="form-control" id="LegalEntity_${countRuleInstrument}" name="Day">`;
    $.each(legalentity, function (i, j) {
        if (legalentity[i].Name != undefined && legalentity[i].Name != null && legalentity[i].Name != '') {
            strHTML += '<option value="' + legalentity[i].Id + '">' + legalentity[i].Name + '</Option>';
        }
    });
    strHTML += `</select>`;
    strHTML += `</div>`;
    strHTML += `</td>`;

    strHTML += `<td>`;
    strHTML += `<div class="form-group">`;
    strHTML += `<a href="javascript:void(0)" value="Delete" onclick="DeleteLeverageRule($(this))" style=" display: block; width: 100%; height: calc(2.25rem + 2px); padding: 0.375rem 0.75rem;">`;
    strHTML += `<i class="fa fa-trash" style="font-size: large;margin-top: 8px;" aria-hidden="true"></i>`;
    strHTML += `</a>`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `</tr>`;

    $("#tblInstrument").append(strHTML);


    if (response != undefined) {

        //BindIntruments(countRuleInstrument, response.instrumentId);
        BindLpRule(countRuleInstrument, response.lpId);
        $("#instrumentid_" + countRuleInstrument).val(response.instrumentId);
        $("#Day_" + countRuleInstrument).val(response.day);
        var _startTime = moment(response.startTime).format("HH:mm:ss");
        //$("#DateApproved").val(moment(response.Item.DateApprovedStr).format("YYYY-MM-DD"))
        $("#StartTime_" + countRuleInstrument).val(response.startTime.toString('HH:mm ss'));
        //var _endTime = moment(response.endTime).format("HH:mm:ss");
        $("#EndTime_" + countRuleInstrument).val(response.endTime);
        $("#MaxSpread_" + countRuleInstrument).val(response.maxSpread);
        $("#objRuleInstrument_Id_" + countRuleInstrument).val(response.id);
        $("#objRuleInstrumen_RuleId_" + countRuleInstrument).val(response.ruleId);

        $("#objRuleLpPriorityViewModel_Priority_" + countRuleInstrument).val(response.priority);
        //$("#objRuleLpPriorityViewModel_MarkUpValue_" + countRuleInstrument).val(response.markUpValue);
        $("#objRuleLpPriorityViewModel_MbId_" + countRuleInstrument).val(response.mbid);
        $("#objRuleLpPriorityViewModel_Mask_" + countRuleInstrument).val(response.mask);
        $("#objRuleLpPriorityViewModel_Timeout_" + countRuleInstrument).val(response.timeout);

        $("#objRuleLpPriorityViewModel_Id_" + countRuleInstrument).val(response.tblLP_ID);
        $("#objRuleLpPriorityViewModel_RuleId_" + countRuleInstrument).val(response.ruleId);
        $("#objRuleLpPriorityViewModel_RuleInstrumentId_" + countRuleInstrument).val(response.ruleInstrumentId);
    }
    else {
        //BindIntruments(countRuleInstrument);
        BindLpRule(countRuleInstrument);
    }
    countRuleInstrument = countRuleInstrument + 1;
}

function BindMasterIntruments(count, defaultVal = "") {
    console.log("Instru: " + tempInstument);
    if (tempInstument.length > 0) {
        for (var i = 0; i < tempInstument.length; i++) {
            $("#instrumentid_" + count).append(`<option value = "${tempInstument[i].value}">${tempInstument[i].text}</option>`)
        }
        if (defaultVal != "") {
            $("#instrumentid_" + count).val(defaultVal);
        }
    }
    else {
        $.ajax({
            async: true,
            type: "POST",
            url: '\GetInstruments',
            dataType: 'json',
            data: {},
            success: function (response) {
                tempInstument = [];
                $("#instrumentid_" + count).append(`<option value = "" > --Select--</option>`)
                for (var i = 0; i < response.length; i++) {
                    $("#instrumentid_" + count).append(`<option value = "${response[i].value}"> ${response[i].text}</option>`)
                    var obj =
                    {
                        value: response[i].value,
                        text: response[i].text,
                    }
                    tempInstument.push(obj);
                }
                if (defaultVal != "") {
                    $("#instrumentid_" + count).val(defaultVal);
                }
            },
            error: function (errorTemp) {
                console.log('error', errorTemp);
            }
        })
    }
}
function SaveIBInstrument() {
    //alert();

    IsValid = true;
    var IBInstrument = []; 0
    //var total_cnt = document.getElementById("myTable").rows.length;
    for (var i = 0; i <= countRuleIBInstrument; i++) {
        if (($("#instrumentid_" + i).val() != "") && ($("#instrumentid_" + i).val() != null)) {
            var obj = {
                InstrumentalName: $("#instrumentid_" + i + " :selected").text(),
                //MarkUpValue: $("#objRuleLpPriorityViewModel_MarkUpValue_" + i).val(),
                SellSwapPer: $("#SellSwapPer_" + i).val(),
                Id: $("#objRuleInstrument_Id_" + i).val() == "" ? 0 : $("#objRuleInstrument_Id_" + i).val(),
                MasterInstrumentalId: $("#instrumentid_" + i).val(),
                BuySwapPer1000: $("#BuySwapPer1000_" + i).val(),
                BuySwapPer: $("#BuySwapPer_" + i).val(),
                MarkUpPer: $("#MarkUpPer_" + i).val(),
                MarkupPer1000: $("#MarkupPer1000_" + i).val(),
                SellSwapPer1000: $("#SellSwapPer1000_" + i).val(),
                MasterInstrumental: $("#MasterInstrumental_" + i + " option:selected").text(),
            }
            IBInstrument.push(obj);
        }
    }
    $("#instrumentData").val(JSON.stringify(IBInstrument));
}


function GetIBInstrument(response) {
    console.log("countRuleInstrument :" + countRuleInstrument);
    var strHTML = "";
    if (countRuleInstrument == "0") {
        strHTML += `<tr>`;
        strHTML += `<th>#</th>`;
        strHTML += `<th>MasterInstrumental</th>`;
       strHTML += `<th>Symbol Group Name</th>`;
        strHTML += `<th>MarkupPer1000</th>`;
        strHTML += `<th>MarkUpPer</th>`;
        strHTML += `<th>BuySwapPer</th>`;
        strHTML += `<th>BuySwapPer1000</th>`;
        strHTML += `<th>SellSwapPer1000</th>`;
        strHTML += `<th>SellSwapPer</th>`;
        strHTML += `<th>Action</th>`;
        strHTML += `</tr>`;
    }
    strHTML += `<tr id="objRuleInstrument_TR_${countRuleInstrument}">`;
    strHTML += `<td>${countRuleInstrument + 1}`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group" style="width:130px;">`;
    strHTML += `<input id="objRuleInstrument_Id_${countRuleInstrument}" type="hidden">`;
    strHTML += `<input id="objRuleInstrumen_RuleId_${countRuleInstrument}" type="hidden">`;

    if (response == null || response == 'undefined') {
        //strHTML += `<input id="instrumentid_${countRuleInstrument}" type="hidden">`;
        //strHTML += `<input class="form-control" value="" type="text" disabled="disabled">`;
        strHTML += `<select class="form-control" id="instrumentid_${countRuleInstrument}" style="border:1px solid; width:170px;">`;
        strHTML += `</select>`;
    }
    else {
        strHTML += `<input id="instrumentid_${countRuleInstrument}" value="${response.instrumentId}" type="hidden">`;
        strHTML += `<input class="form-control" id="MasterInstrumental_${countRuleInstrument}" value="${response.instrumentName}" type="text">`;
    }
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group">`;
    strHTML += `<input class="form-control" id="SymbolGroup_${countRuleInstrument}" name="SymbolGroup" style="width:170px;">`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group">`;
    strHTML += `<input class="form-control" id="MarkupPer1000_${countRuleInstrument}" style="width:105px">`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group">`;
    strHTML += `<input class="form-control" id="MarkUpPer_${countRuleInstrument}" style="width:100px"> `;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group">`;
    strHTML += `<input class="form-control valid" id="BuySwapPer_${countRuleInstrument}" style="width:80px;"> `;
    strHTML += `</div>`;
    strHTML += `</td>`;
    //strHTML += `<td>`;
    //strHTML += `<input id="objRuleLpPriorityViewModel_Id_${countRuleInstrument}" type="hidden">`;
    //strHTML += `<input id="objRuleLpPriorityViewModel_RuleId_${countRuleInstrument}" type="hidden">`;
    //strHTML += `<input id="objRuleLpPriorityViewModel_RuleInstrumentId_${countRuleInstrument}" type="hidden">`;
    //strHTML += `<div class="form-group" style="width:115px;">`;
    //strHTML += `<select id="objRuleLpPriorityViewModel_LPId_${countRuleInstrument}" class="form-control">`;
    //strHTML += `</select>`;
    //strHTML += `</div>`;
    //strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group" style="width:80px;">`;
    strHTML += `<input class="form-control" id="BuySwapPer1000_${countRuleInstrument}">`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group" style="width:50px;">`;
    strHTML += `<input class="form-control" id="SellSwapPer1000_${countRuleInstrument}">`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group" style="width:50px;">`;
    strHTML += `<input class="form-control" id="SellSwapPer_${countRuleInstrument}">`;
    strHTML += `</div>`;
    strHTML += `</td>`;

    strHTML += `<td>`;
    strHTML += `<div class="form-group">`;
    strHTML += `<a href="javascript:void(0)" value="Delete" onclick="DeleteLpRule(0,0,${countRuleInstrument});" style=" display: block; width: 100%; height: calc(2.25rem + 2px); padding: 0.375rem 0.75rem;">`;
    strHTML += `<i class="fa fa-trash" style="font-size: large;margin-top: 8px;" aria-hidden="true"></i>`;
    strHTML += `</a>`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `</tr>`;


    $("#tblInstrument").append(strHTML);


    if (response != undefined) {

        //BindIntruments(countRuleInstrument, response.instrumentId);
        BindFiledNameDynamic(countRuleInstrument, response.lpId);
        $("#instrumentid_" + countRuleInstrument).val(response.instrumentid);
        $("#MasterInstrumental_" + countRuleInstrument).val(response.MasterInstrumentName);
        $("#SymbolGroup_" + countRuleInstrument).val(response.symbolGroupName);
        $("#MarkupPer1000_" + countRuleInstrument).val(response.markUpPer1000);
        $("#MarkUpPer_" + countRuleInstrument).val(response.markUpPer);
        $("#BuySwapPer1000_" + countRuleInstrument).val(response.buySwapPer1000);
        $("#BuySwapPer_" + countRuleInstrument).val(response.buySwapPer);
        $("#SellSwapPer1000_" + countRuleInstrument).val(response.sellSwapPer1000);
        $("#SellSwapPer_" + countRuleInstrument).val(response.sellSwapPer);
    }
    else {
        console.log("countRuleInstrument Get-" + countRuleInstrument);
        BindMasterIntruments(countRuleInstrument);
        BindFiledNameDynamic(countRuleInstrument);
    }
    countRuleInstrument = countRuleInstrument + 1;
}
//function DeleteLpRule(element) {
//    alert()
//    $(element).closest('tr').remove();
//}


function GetRuleInstrumentByRuleId(ruleId, lpFilter = 0, instrumentsFilter = 0, dayFilter = 0) {
    debugger
    //$("#Loader").show();
    //$.ajax({
    //    async: true,
    //    type: "GET",
    //    url: 'Get_InstrumentLpPriorityByRuleId',
    //    dataType: 'json',
    //    data: { Rule_Id: ruleId, lp_filter: lpFilter, instrument_filter: instrumentsFilter, day_filter: dayFilter },
    //    success: function (response) {
    //        $("#tblInstrument").empty();
    //        countRuleInstrument = 0;
    //        for (var i = 0; i < response.length; i++) {
    //            ////Instrument design
    //            GetInstrumentWithRuleLpInEditTable(response[i]);
    //        }
    //        $("#Loader").hide();
    //    },
    //    error: function (errorTemp) {
    //        console.log('error', errorTemp);
    //    }
    //})

    $('#tblInstrument').DataTable(

        {
            ajax: {
                url: "/QuoteMarkUpRule/Get_InstrumentLpPriorityByRuleId",
                type: "POST",
                data: { Rule_Id: ruleId, lp_filter: lpFilter, instrument_filter: instrumentsFilter, day_filter: dayFilter }
            },
            destroy: true,
            stateSave: true,
            autoWidth: false,
            paging: true,
            pageLength: 10,
            processing: true,
            serverSide: true,
            columns: [

                {
                    "title": "Instruments", "data": "instruments",
                    "render": function (data, type, row) {
                        if (countRuleInstrument > 10) {
                            countRuleInstrument = 1;
                        }
                        
                        var html = "";
                        html += "<input id='objRuleLpPriorityViewModel_RuleInstrumentId_" + countRuleInstrument + "' value=" + row.ruleInstrumentId + " type='hidden'>"
                        html += "<input id='objRuleInstrument_Id_" + countRuleInstrument + "' value=" + row.id + " type='hidden'>"
                        html += "<input id='instrumentid_" + countRuleInstrument + "' value=" + row.instrumentId + " type='hidden'>"
                        html += "<input class='form-control' id='instrumentName_" + countRuleInstrument + "'  value='" + row.instrumentName + "' type='text' readonly='readonly'>";
                        return html;
                    }
                    , "orderable": false, "width": "2%"
                },
                
                {
                    "title": "Day", "data": "day",
                    "render": function (data, type, row) {

                        var html = "";
                        html += "<input class='form-control' id='Day_" + countRuleInstrument + "' value=" + row.day + ">";
                        return html;
                    }
                    , "orderable": false, "width": "2%"
                },
                {
                    "title": "Start Time", "data": "startTime",
                    "render": function (data, type, row) {

                        var html = "";
                        html += "<input class='form-control' id='StartTime_" + countRuleInstrument + "' value=" + row.startTime.toString('HH:mm ss') + ">";
                        return html;
                    }
                    , "orderable": false, "width": "2%"
                },
                {
                    "title": "End time", "data": "endTime",
                    "render": function (data, type, row) {

                        var html = "";
                        html += "<input class='form-control' id='EndTime_" + countRuleInstrument + "' value=" + row.endTime + ">";
                        return html;
                    }
                    , "orderable": false, "width": "2%"
                },
                {
                    "title": "Max Spread", "data": "maxSpread",
                    "render": function (data, type, row) {

                        var html = "";
                        html += "<input class='form-control' id='MaxSpread_" + countRuleInstrument + "' value=" + row.maxSpread + ">";
                        return html;
                    }
                    , "orderable": false, "width": "2%"
                },
                {
                    "title": "LP", "data": "lp",
                    "render": function (data, type, row) {

                        var html = "";
                        html += "<input id='objRuleLpPriorityViewModel_Id_" + countRuleInstrument + "' value=" + row.tblLP_ID + " type='hidden'>";
                        html += "<input class='form-control' id='objRuleLpPriorityViewModel_LPId_" + countRuleInstrument + "' value=" + row.tblLP_ID + ">";
                        return html;
                    }
                    , "orderable": false, "width": "2%"
                },
                {
                    "title": "Priority", "data": "priority",
                    "render": function (data, type, row) {

                        var html = "";
                        html += "<input class='form-control' id='objRuleLpPriorityViewModel_Priority_" + countRuleInstrument + "' value=" + row.priority + ">";
                        return html;
                    }
                    , "orderable": false, "width": "2%"
                },
                {
                    "title": "MBId", "data": "mbid",
                    "render": function (data, type, row) {

                        var html = "";
                        html += "<input class='form-control' id='objRuleLpPriorityViewModel_MbId_" + countRuleInstrument + "' value=" + row.mbid + ">";
                        return html;
                    }
                    , "orderable": false, "width": "2%"
                },
                {
                    "title": "Mask", "data": "mask",
                    "render": function (data, type, row) {

                        var html = "";
                        html += "<input class='form-control' id='objRuleLpPriorityViewModel_Mask_" + countRuleInstrument + "' value=" + row.mask + ">";
                        return html;
                    }
                    , "orderable": false, "width": "2%"
                },
                {
                    "title": "Timeout", "data": "timeout",
                    "render": function (data, type, row) {

                        var html = "";
                        html += "<input class='form-control' id='objRuleLpPriorityViewModel_Timeout_" + countRuleInstrument + "' value=" + row.timeout + ">";
                        return html;
                    }
                    , "orderable": false, "width": "2%"
                },
                {
                    "title": "Action", "data": "daysToApply",
                    "render": function (data, type, row) {

                        var html = "";
                        html += "<div class='form-group'>";
                        html += "<a href='javascript:void(0)' value='Delete' onclick='DeleteQuotemarkUpRuleInstrument(" + row.id + "," + row.ruleId + "," + row.tblLP_ID + ")' style='display:block;width:100%;height: calc(2.25rem + 2px); padding: 0.375rem 0.75rem;'>";
                        html += "<i class='fa fa-trash' style='font-size: large;margin-top: 8px;' aria-hidden='true'></i>";
                        html += "</a>";
                        html += "</div>";

                        if (countRuleInstrument < 10) {
                            countRuleInstrument++;
                        }
                        console.log("countRuleInstrument :" + countRuleInstrument);
                        return html;
                        
                    }
                    , "orderable": false, "width": "2%"
                },
            ],
            scrollX: true,
            scrollCollapse: true,
            scrollY: '500px',

            // responsive: true,
            "lengthMenu": [
                [5, 10, 15, 20, 1000],
                [5, 10, 15, 20, 1000] // change per page values here
            ],
        }
    );
    console.log("countRuleInstrument :" + countRuleInstrument);
}
//By niti

function DeleteQuotemarkUpRuleInstrument(id, ruleId, tblLP_ID) {
    ruleId = $('#Id').val();
    Swal.fire({
        title: "Are you sure ?",
        text: "You will be deleting this rule!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
        showCancelButton: true,
        showCloseButton: true,
        focusConfirm: false,
        focusCancelButton: true,

    })
        .then((willdelete) => {
            if (willdelete.value == true) {
                $.ajax({
                    async: true,
                    type: "POST",
                    url: 'DeleteQuotemarkUpRuleInstrument',
                    dataType: 'json',
                    data: { Id: id, RuleId: ruleId, tLP_ID: tblLP_ID },
                    success: function (res) {
                        debugger;
                        if (res.code == 200) {
                            //$("#frmModel").modal('hide');
                            //$("#rulesDetailsData").resetForm();
                            NotifyMsg('success', res.message);
                            window.location.href = "/QuoteMarkUpRule/Edit?Id=" + ruleId;

                        } else {
                            NotifyMsg('error', res.message);
                        }
                    },
                    error: function (errorTemp) {
                        NotifyMsg('error', errorTemp);
                        console.log('error', errorTemp);
                    }
                })
            }
        });
}

function GetIBCommitionEditTable(response) {

    var strHTML = "";
    if (countIbCommition == "0") {
        strHTML += `<tr>`;
        strHTML += `<th>#</th>`;
        strHTML += `<th>MasterInstrumental</th>`;
        strHTML += `<th>Symbol Group Name</th>`;
        strHTML += `<th>MarkupPer1000</th>`;
        strHTML += `<th>MarkUpPer</th>`;
        strHTML += `<th>BuySwapPer</th>`;
        strHTML += `<th>BuySwapPer1000</th>`;
        strHTML += `<th>SellSwapPer1000</th>`;
        strHTML += `<th>SellSwapPer</th>`;
        strHTML += `<th>Action</th>`;
        strHTML += `</tr>`;
    }
    strHTML += `<tr>`;
    strHTML += `<td>${countIbCommition + 1}`;
    strHTML += `</td>`;
    //strHTML += `<td>`;
    //strHTML += `<div class="form-group" style="width:130px;">`;
    strHTML += `<input id="objRuleInstrument_Id_${countIbCommition}" type="hidden">`;
    strHTML += `<input id="objRuleInstrumen_RuleId_${countIbCommition}" type="hidden">`;

    ////if (response == null || response == 'undefined') {
    ////    //strHTML += `<input id="instrumentid_${countIbCommition}" type="hidden">`;
    ////    //strHTML += `<input class="form-control" value="" type="text" disabled="disabled">`;
    ////    strHTML += `<select class="form-control" id="Masterinstrumentid_${countIbCommition}" style="border:1px solid;">`;
    ////    strHTML += `</select>`;
    ////}
    ////else {
    ////    strHTML += `<input id="instrumentid_${countIbCommition}" value="${response.instrumentId}" type="hidden">`;
    ////    strHTML += `<input class="form-control" id="instrumentName_${countIbCommition}" value="${response.instrumentName}" type="text" disabled="disabled">`;
    ////}
    //strHTML += `</div>`;
    //strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group">`;
    strHTML += `<input class="form-control" id="MasterInstrumental_${countIbCommition}" name="MasterInstrumental" style="width:170px;" readonly="readonly">`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group">`;
    strHTML += `<input class="form-control" id="SymbolGroup_${countIbCommition}" name="SymbolGroup" style="width:170px;" readonly="readonly">`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group">`;
    strHTML += `<input class="form-control" id="MarkupPer1000_${countIbCommition}" style="width:100px" readonly="readonly">`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group">`;
    strHTML += `<input class="form-control" id="MarkUpPer_${countIbCommition}" style="width:100px" readonly="readonly"> `;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group">`;
    strHTML += `<input class="form-control valid" id="BuySwapPer1000_${countIbCommition}" style="width:70px;" readonly="readonly"> `;
    strHTML += `</div>`;
    strHTML += `</td>`;
    //strHTML += `<td>`;
    //strHTML += `<input id="objRuleLpPriorityViewModel_Id_${countIbCommition}" type="hidden">`;
    //strHTML += `<input id="objRuleLpPriorityViewModel_RuleId_${countIbCommition}" type="hidden">`;
    //strHTML += `<input id="objRuleLpPriorityViewModel_RuleInstrumentId_${countIbCommition}" type="hidden">`;
    //strHTML += `<div class="form-group" style="width:115px;">`;
    //strHTML += `<select id="objRuleLpPriorityViewModel_LPId_${countIbCommition}" class="form-control">`;
    //strHTML += `</select>`;
    //strHTML += `</div>`;
    //strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group" style="width:80px;">`;
    strHTML += `<input class="form-control" id="BuySwapPer_${countIbCommition}" readonly="readonly">`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group" style="width:50px;">`;
    strHTML += `<input class="form-control" id="SellSwapPer1000_${countIbCommition}" readonly="readonly">`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group" style="width:50px;">`;
    strHTML += `<input class="form-control" id="SellSwapPer_${countIbCommition}" readonly="readonly">`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    //strHTML += `<td>`;
    //strHTML += `<div class="form-group" style="width:120px;">`;
    //strHTML += `<input class="form-control" id="objRuleLpPriorityViewModel_Timeout_${countIbCommition}" placeholder="Timeout" type="number">`;
    //strHTML += `</div>`;
    //strHTML += `</td>`;
    strHTML += `<td>`;
    strHTML += `<div class="form-group">`;
    strHTML += '<a class="btn btn-success edt-rcd mr-1" href="javascript:void(0);"  data-uri="/IB/EditIBRuleInstrumentById?IBId=' + response.ibid + '&MasterInstrumentalID=' + response.masterInstrumentalId + '"><i class="fa fa-edit"></i></a>';
    strHTML += `<a href="javascript:void(0)" value="Delete" onclick="DeleteLpRule(${response.ibid},${response.masterInstrumentalId})" style=" display: block; width: 100%; height: calc(2.25rem + 2px); padding: 0.375rem 0.75rem;">`;
    strHTML += `<i class="fa fa-trash" style="font-size: large;margin-top: 8px;" aria-hidden="true"></i>`;
    strHTML += `</a>`;
    strHTML += `</div>`;
    strHTML += `</td>`;
    strHTML += `</tr>`;

    $("#tblInstrument").append(strHTML);


    if (response != undefined) {

        //BindIntruments(countIbCommition, response.instrumentId);
        //BindLpRule(countIbCommition, response.lpId);

        $("#objRuleInstrument_Id_" + countIbCommition).val(response.ibid);
        $("#objRuleInstrumen_RuleId_" + countIbCommition).val(response.masterInstrumentalId);

        $("#MasterInstrumental_" + countIbCommition).val(response.instrumentalName);
        $("#SymbolGroup_" + countIbCommition).val(response.symbolGroupName);
        $("#MarkupPer1000_" + countIbCommition).val(response.markUpPer1000);
        //var _startTime = moment(response.startTime).format("HH:mm:ss");
        //$("#DateApproved").val(moment(response.Item.DateApprovedStr).format("YYYY-MM-DD"))
        $("#MarkUpPer_" + countIbCommition).val(response.markUpPer);
        //var _endTime = moment(response.endTime).format("HH:mm:ss");
        $("#BuySwapPer1000_" + countIbCommition).val(response.buySwapPer1000);
        $("#BuySwapPer_" + countIbCommition).val(response.buySwapPer);
        //$("#objRuleInstrument_Id_" + countIbCommition).val(response.id);
        //$("#objRuleInstrumen_RuleId_" + countIbCommition).val(response.ruleId);

        $("#SellSwapPer1000_" + countIbCommition).val(response.sellSwapPer1000);
        //$("#objRuleLpPriorityViewModel_MarkUpValue_" + countIbCommition).val(response.markUpValue);
        $("#SellSwapPer_" + countIbCommition).val(response.sellSwapPer);
        //$("#objRuleLpPriorityViewModel_Mask_" + countIbCommition).val(response.mask);
        //$("#objRuleLpPriorityViewModel_Timeout_" + countIbCommition).val(response.timeout);

        //$("#objRuleLpPriorityViewModel_Id_" + countIbCommition).val(response.tblLP_ID);
        //$("#objRuleLpPriorityViewModel_RuleId_" + countIbCommition).val(response.ruleId);
        //$("#objRuleLpPriorityViewModel_RuleInstrumentId_" + countIbCommition).val(response.ruleInstrumentId);
    }
    else {
        console.log("countIbCommition Get-" + countIbCommition);
        //BindIntruments(countIbCommition);
        BindLpRule(countIbCommition);
    }
    countIbCommition = countIbCommition + 1;
}

function GetIBCommitionRuleId(mainId) {

    $.ajax({
        async: true,
        type: "GET",
        url: '/IB/Get_IBCommitionId',
        dataType: 'json',
        data: { Rule_Id: mainId },
        success: function (response) {
            $("#tblInstrument").empty();
            countIbCommition = 0;
            for (var i = 0; i < response.length; i++) {
                ////Instrument design
                GetIBCommitionEditTable(response[i]);
            }
        },
        error: function (errorTemp) {
            console.log('error', errorTemp);
        }
    })
}
//By niti end
function GetLpRuleByRuleId(RuleId) {
    $.ajax({
        async: true,
        type: "GET",
        url: '/RuleLpPriority/Get_ByRuleId',
        dataType: 'json',
        data: { Rule_Id: RuleId },
        success: function (response) {
            for (var i = 0; i < response.length; i++) {
                ////Instrument design
                //AddNewLPPrioJs(response[i]);
                //BindIntruments(countRuleInstrument, response[i].instrumentId);

                //$("#Day_" + countRuleInstrument).val(response[i].day);
                //var _startTime = moment(response[i].startTime).format("HH:mm:ss");
                //$("#StartTime_" + countRuleInstrument).val(_startTime);
                //var _endTime = moment(response[i].endTime).format("HH:mm:ss");
                //$("#EndTime_" + countRuleInstrument).val(_endTime);
                //$("#MaxSpread_" + countRuleInstrument).val(response[i].maxSpread);
                //countRuleInstrument = countRuleInstrument + 1;
            }
        },
        error: function (errorTemp) {
            console.log('error', errorTemp);
        }
    })
}
function BindIntruments(count, defaultVal = "") {
    $.ajax({
        async: true,
        type: "POST",
        url: '\GetInstrumentsDropDown',
        dataType: 'json',
        data: {},
        success: function (response) {
            $("#instrumentid_" + count).append(`<option value = "" > --Select--</option>`)
            for (var i = 0; i < response.length; i++) {
                $("#instrumentid_" + count).append(`<option value = "${response[i].value}"> ${response[i].text}</option>`)
            }
            if (defaultVal != "") {
                $("#instrumentid_" + count).val(defaultVal);
            }
        },
        error: function (errorTemp) {
            console.log('error', errorTemp);
        }
    })
}
function SaveRulesCondiDtl() {
    debugger;
    IsValid = true;
    var rulesDetails = [];
    for (var i = 0; i <= countDetails; i++) {
        if (
            ($("#FieldName_" + i).val() != "")
            &&
            ($("#FieldName_" + i).val() != null)
        ) {
            var obj = {
                Id: $("#objRuleConditions_Dtl_Id_" + i).val() == "" ? 0 : $("#objRuleConditions_Dtl_Id_" + i).val(),
                RuleConditionsId: $("#objRuleConditions_Id_" + i).val() == "" ? 0 : $("#objRuleConditions_Id_" + i).val(),
                FieldName: $("#FieldName_" + i).val(),
                Opration: $("#Opration_" + i).val(),
                OprationValue: $("#OprationValue_" + i).val(),
            }
            rulesDetails.push(obj);
        }
    }
    $("#rulesCondtionData").val(JSON.stringify(rulesDetails));

}
function SaveInstrumentDataDtl() {

    IsValid = true;
    var rulesInstrument = [];
    //var total_cnt = document.getElementById("myTable").rows.length;
    for (var i = 0; i <= countRuleInstrument; i++) {
        if (($("#instrumentid_" + i).val() != "") && ($("#instrumentid_" + i).val() != null)) {
            var obj = {
                tblLP_ID: $("#objRuleLpPriorityViewModel_Id_" + i).val() == "" ? 0 : $("#objRuleLpPriorityViewModel_Id_" + i).val(),
                RuleId: $("#Id").val(),
                RuleInstrumentId: $("#objRuleLpPriorityViewModel_RuleInstrumentId_" + i).val() == "" ? 0 : $("#objRuleLpPriorityViewModel_RuleInstrumentId_" + i).val(),
                LPId: $("#objRuleLpPriorityViewModel_LPId_" + i).val(),
                Priority: $("#objRuleLpPriorityViewModel_Priority_" + i).val(),
                //MarkUpValue: $("#objRuleLpPriorityViewModel_MarkUpValue_" + i).val(),
                MbId: $("#objRuleLpPriorityViewModel_MbId_" + i).val(),
                Mask: $("#objRuleLpPriorityViewModel_Mask_" + i).val(),
                Timeout: $("#objRuleLpPriorityViewModel_Timeout_" + i).val(),
                Id: $("#objRuleInstrument_Id_" + i).val() == "" ? 0 : $("#objRuleInstrument_Id_" + i).val(),
                InstrumentId: $("#instrumentid_" + i).val(),
                Day: $("#Day_" + i).val(),
                StartTime: $("#StartTime_" + i).val(),
                EndTime: $("#EndTime_" + i).val(),
                MaxSpread: $("#MaxSpread_" + i).val(),
                InstrumentName: $("#instrumentName_" + i).val(),
                LPName: $("#objRuleLpPriorityViewModel_LPId_" + i + " option:selected").text(),
            }
            rulesInstrument.push(obj);
        }
    }
    $("#instrumentData").val(JSON.stringify(rulesInstrument));
}
function SaveIB() {
    SaveIBInstrument();
    //var model = {};
    //model.Name = $('#Name').val();
    //model.BDMId = $('#BDMId').val();
    //model.LegalEntityId = $('#LegalEntityId').val();
    //model.ParentIBId = $('#ParentIBId').val();
    //model.CountryId = $('#CountryId').val();
    //model.ParentCommissionPercentageRebate = parseFloat($('#ParentCommissionPercentageRebate').val());
    /* model.File = $('#INSUpload')[0].files;*/

    var formData = new FormData();
    formData.append("Id", $("#Id").val());
    formData.append("Name", $("#Name").val());
    formData.append("BDMId", $("#BDMId").val());
    formData.append("LegalEntityId", $("#LegalEntityId").val());
    formData.append("ParentIBId", $("#ParentIBId").val());
    formData.append("CountryId", $("#CountryId").val());
    formData.append("ParentCommissionPercentageRebate", $("#ParentCommissionPercentageRebate").val());
    formData.append("FileUpload", $('#INSUpload')[0].files);
    formData.append("objIBCommissionMarkUpSettingInstrumentWise", $("#instrumentData").val());
    debugger;
    $.ajax({
        type: "POST",
        url: '\SaveIB_new',
        data: formData,
        contentType: false,
        processData: false,
        async: false,
        success: function (res) {

            if (res.statusCode == 200) {
                //$("#frmModel").modal('hide');
                //$("#rulesDetailsData").resetForm();
                $("#Loader").hide();
                NotifyMsg('success', res.message);
                window.location.href = "/IB/Edit?Id=" + $("#Id").val();

            } else {
                NotifyMsg('error', res.message);
            }
        },
        error: function (errorTemp) {
            NotifyMsg('error', errorTemp);
            console.log('error', errorTemp);
        }
    })
}
function SaveQuoteMarkUpRule() {
    SaveRulesCondiDtl();
    SaveInstrumentDataDtl();
    var formData = new FormData();
    formData.append("Id", $("#Id").val());
    formData.append("Name", $("#Name").val());
    formData.append("AutoupdateLPPriorityUp", $("#AutoupdateLPPriorityUp").val());
    formData.append("Priority", $("#Priority").val());
    formData.append("objRuleConditionViewModelstr", $("#rulesCondtionData").val());
    formData.append("objRuleInstrumentViewModelstr", $("#instrumentData").val());
    var RuleId = $("#Id").val();
    $.ajax({
        type: "POST",
        url: 'SaveQuoteMarkUpRule',
        data: formData,
        contentType: false,
        processData: false,
        success: function (res) {

            if (res.statusCode == 200) {
                //$("#frmModel").modal('hide');
                //$("#rulesDetailsData").resetForm();
                //$("#Loader").hide();
                NotifyMsg('success', res.message);
                window.location.href = "/QuoteMarkUpRule/Edit?Id=" + RuleId;

            } else {
                NotifyMsg('error', res.message);
            }
        },
        error: function (errorTemp) {
            NotifyMsg('error', errorTemp);
            console.log('error', errorTemp);
        }
    })
}
function DeleteRule(RowNumber) {
    Swal.fire({
        title: "Are you sure ?",
        text: "You will be deleting this rule!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
        showCancelButton: true,
        showCloseButton: true,
        focusConfirm: false,
        focusCancelButton: true,

    })
        .then((willdelete) => {
            if (willdelete.value == true) {
                var Conditions_dtl_Id = $("#objRuleConditions_Dtl_Id_" + RowNumber).val()
                var RuleId = $("#objRuleConditions_Id_" + RowNumber).val()
                $.ajax({
                    async: true,
                    type: "POST",
                    url: 'DeleteRuleConditionById',
                    dataType: 'json',
                    data: { Id: Conditions_dtl_Id, RuleConditionId: RuleId },
                    success: function (res) {
                        if (res.statusCode == 200) {
                            //$("#frmModel").modal('hide');
                            //$("#rulesDetailsData").resetForm();
                            NotifyMsg('success', res.message);
                            window.location.href = "/QuoteMarkUpRule/index";

                        } else {
                            NotifyMsg('error', res.message);
                        }
                    },
                    error: function (errorTemp) {
                        NotifyMsg('error', errorTemp);
                        console.log('error', errorTemp);
                    }
                })
            }
        });
}
function DeleteLeverageRule(obj) {
    var object = obj;

    Swal.fire({
        title: "Are you sure ?",
        text: "You will be deleting this rule!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
        showCancelButton: true,
        showCloseButton: true,
        focusConfirm: false,
        focusCancelButton: true,

    }).then((willdelete) => {
        if (willdelete.value == true) {
            debugger
            $(object).parent().parent().parent().remove();
        }
    });
}
function DeleteLpRule(ibid, masterInstrumentalID, cntRuleIBInst = 0) {


    Swal.fire({
        title: "Are you sure ?",
        text: "You will be deleting this rule!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
        showCancelButton: true,
        showCloseButton: true,
        focusConfirm: false,
        focusCancelButton: true,

    })
        .then((willdelete) => {
            if (willdelete.value == true) {
                debugger;
                if (cntRuleIBInst > 0) {
                    document.getElementById("objRuleInstrument_TR_" + cntRuleIBInst).remove();
                }
                else {
                    $.ajax({
                        async: true,
                        type: "POST",
                        url: 'DeleteIBRuleInstrument',
                        dataType: 'json',
                        data: { IBId: ibid, MasterInstrumentalID: masterInstrumentalID },
                        success: function (res) {

                            if (res.code == 200) {
                                NotifyMsg('success', res.message);

                                /*window.location.href = "/IB/Edit?Id=" + ibid;*/
                                location.reload();

                            } else {
                                NotifyMsg('error', res.message);
                            }
                        },
                        error: function (errorTemp) {
                            NotifyMsg('error', errorTemp);
                            console.log('error', errorTemp);
                        }
                    })
                }
            }
        });
}


function UpdateLpRule(ibid, masterInstrumentalID) {
    Swal.fire({
        title: "Are you sure ?",
        text: "You will be updeting this rule!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
        showCancelButton: true,
        showCloseButton: true,
        focusConfirm: false,
        focusCancelButton: true,

    })
        .then((willdelete) => {
            if (willdelete.value == true) {
                $.ajax({
                    async: true,
                    type: "GET",
                    url: 'EditIBRuleInstrumentById',
                    dataType: 'json',
                    data: { IBId: ibid, MasterInstrumentalID: masterInstrumentalID },
                    success: function (res) {

                        if (res.code == 200) {
                            NotifyMsg('success', res.message);

                            window.location.href = "/IB/Edit?Id=" + ibid;

                        } else {
                            NotifyMsg('error', res.message);
                        }
                    },
                    error: function (errorTemp) {
                        NotifyMsg('error', errorTemp);
                        console.log('error', errorTemp);
                    }
                })
            }
        });
}
function btnSearchClick() {

    var lpFilter = $("#objRuleLpPriorityViewModel_LPId_filter").val();
    var instrumentFilter = $("#instrumentid_filter").val();
    var day = $("#day_filter").val();
    var mainId = $('#Id').val();
    GetRuleInstrumentByRuleId(mainId, lpFilter, instrumentFilter, day);
}

function ExportQuoteMarkUpRule() {

    updateProgress(0);
    var table1 = $('#tblInstrument tr').length;

    //$("#ProcessModal").modal({ backdrop: 'static', keyboard: false });
    //$("#ProcessModal").focus();
    $("#ProcessModal").modal('show');

    var totalCnt = table1;//.fnSettings().fnRecordsTotal()
    var counter = totalCnt / 100;
    counter = Math.round(counter);
    var intervalId = window.setInterval(function () {
        if (counter < 90) {
            updateProgress(counter);
            counter = counter + 7;
        }
    }, 100);

    SaveInstrumentDataDtl();
    var formData = new FormData();
    formData.append("Id", $("#Id").val());
    formData.append("objRuleInstrumentViewModelstr", $("#instrumentData").val());
    $.ajax({
        type: "POST",
        url: 'ExportQuoteMarkUpRule',
        data: formData,
        contentType: false,
        processData: false,
        success: function (res) {

            $("#ProcessModal").modal("hide");
            updateProgress(0);
            clearInterval(intervalId);
            completeHandler();

            if (res.statusCode != "400") {
                window.location = '/QuoteMarkUpRule/Download?Filename=' + res.fileName;
            }
            NotifyMsg('success', res.message);


        },
        error: function (errorTemp) {
            NotifyMsg('error', errorTemp);
            console.log('error', errorTemp);
        }
    })
}
function updateProgress(count) {
    $('.bar').css('width', count + '%');
    $('.percent').text(count + '%');
}
function completeHandler() {
    $('.bar').width("100%");
    $('.percent').html("100%");
    $("#ProcessModal").modal('hide');
}

function SaveMarginRule() {

    SaveRulesCondiDtl();
    //CheckEvent();
    var formData = new FormData();
    formData.append("MarginRuleId", $("#MarginRuleId").val());
    formData.append("RuleName", $("#RuleName").val());
    formData.append("StopOutPercent", $("#StopOutPercent").val());
    formData.append("Priority", $("#Priority").val());
    formData.append("MarginCallPercent", $("#MarginCallPercent").val());
    formData.append("LeveltotheStopOut", $("#LeveltotheStopOut").val());
    formData.append("IsPartialCloseoutonStopOut", $("#IsPartialCloseoutonStopOut").val());
    formData.append("objRuleConditionViewModelstr", $("#rulesCondtionData").val());
    $.ajax({
        type: "POST",
        url: 'SaveMarginRule',
        data: formData,
        contentType: false,
        processData: false,
        success: function (res) {

            if (res.statusCode == 200) {
                //$("#frmModel").modal('hide');
                //$("#rulesDetailsData").resetForm();
                NotifyMsg('success', res.message);
                window.location.href = "/MarginRule/index";

            } else {
                NotifyMsg('error', res.message);
            }
        },
        error: function (errorTemp) {
            NotifyMsg('error', errorTemp);
            console.log('error', errorTemp);
        }
    })
}

function GetIntrumentFilterDropdownByRuleId(ruleId) {

    $.ajax({
        async: true,
        type: "GET",
        url: 'Get_InstrumentByRuleId',
        dataType: 'json',
        data: { Rule_Id: ruleId },
        success: function (response) {
            $("#instrumentid_filter").append(`<option value = "" > --Select--</option>`)
            for (var i = 0; i < response.length; i++) {
                $("#instrumentid_filter").append(`<option value = "${response[i].value}"> ${response[i].text}</option>`)
            }
        },
        error: function (errorTemp) {
            console.log('error', errorTemp);
        }
    })
}
function DeleteRule1(RowNumber) {
    Swal.fire({
        title: "Are you sure ?",
        text: "You will be deleting this rule!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
        showCancelButton: true,
        showCloseButton: true,
        focusConfirm: false,
        focusCancelButton: true,

    })
        .then((willdelete) => {
            if (willdelete.value == true) {
                var Conditions_dtl_Id = $("#objRuleConditions_Dtl_Id_" + RowNumber).val()
                var RuleId = $("#objRuleConditions_Id_" + RowNumber).val()
                $.ajax({
                    async: true,
                    type: "POST",
                    //url: "@Url.Action("DeleteLeverageRuleConditionById","LevarageRuleImport")",
                    url: '/LevarageRuleImport/DeleteLeverageRuleConditionById',
                    dataType: 'json',
                    data: { Id: Conditions_dtl_Id, RuleConditionId: RuleId },
                    success: function (res) {

                        if (res.code == 200) {
                            NotifyMsg('success', res.message);
                            window.location.href = "/LevarageRuleImport/index";

                        } else {
                            NotifyMsg('error', res.message);
                        }
                    },
                    error: function (errorTemp) {
                        NotifyMsg('error', errorTemp);
                        console.log('error', errorTemp);
                    }
                })
            }
        });
}
function DeleteMarginRule(RowNumber) {
    Swal.fire({
        title: "Are you sure ?",
        text: "You will be deleting this rule!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
        showCancelButton: true,
        showCloseButton: true,
        focusConfirm: false,
        focusCancelButton: true,

    })
        .then((willdelete) => {
            if (willdelete.value == true) {
                var Conditions_dtl_Id = $("#objRuleConditions_Dtl_Id_" + RowNumber).val()
                var RuleId = $("#objRuleConditions_Id_" + RowNumber).val()
                $.ajax({
                    async: true,
                    type: "POST",
                    url: '/MarginRule/DeleteRuleConditionById',
                    dataType: 'json',
                    data: { Id: Conditions_dtl_Id, RuleConditionId: RuleId },
                    success: function (res) {
                        if (res.code == 200) {
                            //$("#frmModel").modal('hide');
                            //$("#rulesDetailsData").resetForm();
                            NotifyMsg('success', res.message);
                            window.location.href = "/MarginRule/index";

                        } else {
                            NotifyMsg('error', res.message);
                        }
                    },
                    error: function (errorTemp) {
                        NotifyMsg('error', errorTemp);
                        console.log('error', errorTemp);
                    }
                })
            }
        });
}


function GetLeverageRuleInstrumentByRuleId(ruleId, instrumentId) {

    $('#tblInstrument').DataTable(
        {
            ajax: {
                url: "/LevarageRuleImport/Get_LeverageRuleInstrumentByRuleId",
                type: "POST",
                data: { id: ruleId, InstrumentId: instrumentId }
            },
            destroy: true,
            stateSave: true,
            autoWidth: false,
            paging: true,
            pageLength: 10,
            processing: true,
            serverSide: true,
            columns: [

                {
                    "title": "InstrumentName", "data": "instrumentName",
                    "render": function (data, type, row) {

                        if (countRuleInstrument > 10) {
                            countRuleInstrument = 1;
                        }
                        var html = "";
                        html += "<input id='hdnLeverageId_" + countRuleInstrument + "' value=" + row.leverageId + " type='hidden'>"
                        html += "<input id='instrumentid_" + countRuleInstrument + "' value=" + row.masterInstrumentId + " type='hidden'>"
                        html += "<input class='form-control' id='masterInstrumentName_" + countRuleInstrument + "'  value='" + row.masterInstrumentName + "' type='text' readonly='readonly'>";
                        return html;
                    }
                    , "orderable": false, "width": "2%"
                },
                {
                    "title": "SymbolGroupName", "data": "symbolGroupName",
                    "render": function (data, type, row) {

                        var html = "";
                        html += "<input id='symbolGroup_" + countRuleInstrument + "' value=" + row.symbolGroupId + " type='hidden''>";
                        html += "<input class='form-control' id='symbolGroupName_" + countRuleInstrument + "' value=" + row.symbolGroupName + " type='text' readonly='readonly'>";
                        return html;
                    }
                    , "orderable": false, "width": "2%"
                },
                {
                    "title": "Day", "data": "day",
                    "render": function (data, type, row) {

                        var html = "";
                        html += "<input class='form-control' id='Day_" + countRuleInstrument + "' value=" + row.day + " style='width:100px;'>";
                        return html;
                    }
                    , "orderable": false, "width": "2%"
                },
                {
                    "title": "StartTime", "data": "startTime",
                    "render": function (data, type, row) {

                        var html = "";
                        html += "<input class='form-control' id='StartTime_" + countRuleInstrument + "' value=" + row.timeFrom + " style='width:100px;'>";
                        return html;
                    }
                    , "orderable": false, "width": "2%"
                },
                {
                    "title": "EndTime", "data": "endTime",
                    "render": function (data, type, row) {

                        var html = "";
                        html += "<input class='form-control' id='EndTime_" + countRuleInstrument + "' value=" + row.timeTo + " style='width:100px;'>";
                        return html;
                    }
                    , "orderable": false, "width": "2%"
                },
                {
                    "title": "Priority", "data": "priority2",
                    "render": function (data, type, row) {

                        var html = "";
                        html += "<input class='form-control' id='Priority_" + countRuleInstrument + "' value=" + row.priority2 + " type='number' style='width:100px;'>";
                        return html;
                    }
                    , "orderable": false, "width": "2%"
                },
                {
                    "title": "BandFrom", "data": "bandFrom",
                    "render": function (data, type, row) {

                        var html = "";
                        html += "<input class='form-control valid number-separator' id='BandFrom_" + countRuleInstrument + "' value=" + row.bandFrom + " style='width:100px;'>";
                        return html;
                    }
                    , "orderable": false, "width": "2%"
                },
                {
                    "title": "BandTo", "data": "bandTo",
                    "render": function (data, type, row) {

                        var html = "";
                        html += "<input class='form-control valid number-separator' id='BandTo_" + countRuleInstrument + "' value=" + row.bandTo + " style='width:100px;'>";
                        return html;
                    }
                    , "orderable": false, "width": "2%"
                },
                {
                    "title": "LeverageAmount", "data": "leverageAmount",
                    "render": function (data, type, row) {
                        var html = "";

                        html += "<input class='form-control' id='LeverageAmount_" + countRuleInstrument + "' value=" + row.leverageAmount + " type='number' style='width:100px;'>";
                        return html;
                    }
                    , "orderable": false, "width": "2%"
                },
                {
                    "title": "IsNewPosition", "data": "isNewPosition",
                    "render": function (data, type, row) {
                        var html = "<select class='form-control number-separator' id='NewPosition_" + countRuleInstrument + "' style='border:1px solid;'>";
                        if (row.isNewPosition == "False") {
                            html += "<option>Yes</option><option selected ='selected'>No</option > ";
                        }
                        else {
                            html += "<option selected ='selected'>Yes</option><option>No</option > ";
                        }
                        html += "</select>";
                        return html;
                    }
                    , "orderable": false, "width": "2%"
                },
                {
                    "title": "LegalEntity", "data": "legalEntityId",
                    "render": function (data, type, row) {

                        var legalentity = JSON.parse($('#LegalEntityList').val());
                        var html = "<select class='form-control number-separator' id='LegalEntity_" + countRuleInstrument + "' style='border:1px solid;'>";
                        for (var i = 0; i < legalentity.length; i++) {
                            if (row.legalEntityId == legalentity[i].id) {
                                html += "<option value='" + legalentity[i].Id + "' selected='selected'>" + legalentity[i].Name + "</option>";
                            }
                            else {
                                html += "<option value='" + legalentity[i].Id + "'>" + legalentity[i].Name + "</option>";
                            }
                        }

                        html += "</select>";
                        return html;
                    }
                    , "orderable": false, "width": "2%"
                },
                {
                    "title": "Action", "data": "id",
                    "render": function (data, type, row) {

                        var html = "";
                        html += "<div class='form-group'>";
                        html += "<a href='javascript:void(0)' value='Delete' onclick='DeleteLeverageRule($(this))' style='display:block;width:100%;padding: 0.375rem 0.75rem;'>";
                        html += "<i class='fa fa-trash' style='font-size: large;margin-top: 8px;' aria-hidden='true'></i>";
                        html += "</a>";
                        html += "</div>";
                        if (countRuleInstrument < 10) {
                            countRuleInstrument++;
                        }

                        return html;
                    }
                    , "orderable": false, "width": "2%"
                },
            ],
            scrollX: true,
            scrollCollapse: true,
            scrollY: '500px',
            // responsive: true,
            "lengthMenu": [
                [5, 10, 15, 20, 1000],
                [5, 10, 15, 20, 1000] // change per page values here
            ],

        }
    );
}