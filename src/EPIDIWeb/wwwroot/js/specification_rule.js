
var countRuleInstrument = 0;
var countLPRuleDetails = 0;
function GetInstrumentWithSpecificationRuleEditTable(response) {
	var strHTML = "";
	//if (countRuleInstrument == "0") {
	//	strHTML += `<thead><tr>`;
	//	strHTML += `<th>#</th>`;
	//	strHTML += `<th>InstrumentName</th>`;
	//	strHTML += `<th>SymbolGroup</th>`;
	//	strHTML += `<th>TTFrom</th>`;
	//	strHTML += `<th>TTTo</th>`;
	//	strHTML += `<th>QTFrom</th>`;
	//	strHTML += `<th>QTTo</th>`;
	//	strHTML += `<th>TradeStatus</th>`;
	//	strHTML += `<th>AverageSpread</th>`;
	//	strHTML += `<th>Decimals</th>`;
	//	strHTML += `<th>SymbolDenomination</th>`;
	//	strHTML += `<th>Unit Description</th>`;
	//	strHTML += `<th>ZerosTobeGrouped</th>`;
	//	strHTML += `<th>Action</th>`;
	//	strHTML += `</tr></thead><tbody>`;
	//}
	strHTML += `<tr>`;
	strHTML += `<td>${countRuleInstrument + 1}`;
	strHTML += `</td>`;
	strHTML += `<td>`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input id="tradeStatusId_${countRuleInstrument}" type="hidden">`;
	strHTML += `<input id="instrumentid_${countRuleInstrument}" type="hidden">`;
	strHTML += `<input id="symbolGroup_${countRuleInstrument}" type="hidden">`;
	strHTML += `<input disabled class="form-control" id="InstrumentName_${countRuleInstrument}">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td>`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input disabled class="form-control" id="SymbolGroup_${countRuleInstrument}">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="TTFrom_${countRuleInstrument}" type="time" step="1" min="00:00" max="23:59">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="TTTo_${countRuleInstrument}" type="time" step="1" min="00:00" max="23:59">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="QTFrom_${countRuleInstrument}" type="time" step="1" min="00:00" max="23:59">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="QTTo_${countRuleInstrument}" type="time" step="1" min="00:00" max="23:59">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<select class="form-control" id="TradeStatus_${countRuleInstrument}" style="border:1px solid;width:118px">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="AverageSpread_${countRuleInstrument}">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="Decimals_${countRuleInstrument}">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="SymbolDenomination_${countRuleInstrument}">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="UnitDescription_${countRuleInstrument}">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="ZerosToBeGrouped_${countRuleInstrument}">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td>`;
	strHTML += `<div class="form-group">`;
	strHTML += `<a href="javascript:void(0)" value="Delete" onclick="DeleteSpecificatonRuleInstrument(${0},${response.instrumentId})" style=" display: block; width: 100%; height: calc(2.25rem + 2px); padding: 0.375rem 0.75rem;">`;
	strHTML += `<i class="fa fa-trash" style="font-size: large;margin-top: 8px; margin-left: 10px;" aria-hidden="true"></i>`;
	strHTML += `</a>`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `</tr>`;

	$("#tblInstrument").append(strHTML);


	if (response != undefined) {
		//BindIntruments(countRuleInstrument, response.instrumentId);
		//BindSymbolGroups(countRuleInstrument, response.symbolGroupId);
		BindTradeStatus(countRuleInstrument, response.tradeId);
		$("#tradeStatusId_" + countRuleInstrument).val(response.tradeId);
		$("#instrumentid_" + countRuleInstrument).val(response.instrumentId);
		$("#symbolGroup_" + countRuleInstrument).val(response.symbolGroupId);
		$("#InstrumentName_" + countRuleInstrument).val(response.instrumentName);
		$("#SymbolGroup_" + countRuleInstrument).val(response.symbolGroup);
		$("#TTFrom_" + countRuleInstrument).val(response.ttFrom);
		$("#TTTo_" + countRuleInstrument).val(response.ttTo);
		$("#QTFrom_" + countRuleInstrument).val(response.qtFrom);
		$("#QTTo_" + countRuleInstrument).val(response.qtTo);
		$("#TradeStatus_" + countRuleInstrument).val(response.tardeId);
		$("#AverageSpread_" + countRuleInstrument).val(response.averageSpread);
		$("#Decimals_" + countRuleInstrument).val(response.decimals);
		$("#SymbolDenomination_" + countRuleInstrument).val(response.symbolDenomination);
		$("#UnitDescription_" + countRuleInstrument).val(response.unitDescription);
		$("#ZerosToBeGrouped_" + countRuleInstrument).val(response.zerosToBeGrouped);
	}
	else {
		//BindIntruments(countRuleInstrument);
		//BindSymbolGroups(countRuleInstrument);
		BindTradeStatus(countRuleInstrument);
	}
	countRuleInstrument = countRuleInstrument + 1;
}

function GetSpecificationRuleInstrumentByRuleId(ruleId) {
	$.ajax({
		async: false,
		type: "POST",
		url: '/ImportInstruments/All_Import_MasterInstrument',
		dataType: 'json',
		data: { Id: ruleId },
		success: function (response) {
			debugger
			$("#tblInstrument").empty();
			countRuleInstrument = 0;
			for (var i = 0; i < response.data.length; i++) {
				////Instrument design               
				GetInstrumentWithSpecificationRuleEditTable(response.data[i]);
			}

		},
		error: function (errorTemp) {
			console.log('error', errorTemp);
		}
	});
}

function BindIntruments(count, defaultVal = "") {
	$.ajax({
		async: true,
		type: "POST",
		url: 'GetAllInstrument',
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

function BindSymbolGroups(count, defaultVal = "") {
	$.ajax({
		async: true,
		type: "POST",
		url: 'GetSymbolGroup',
		dataType: 'json',
		data: {},
		success: function (response) {

			$("#symbolGroup_" + count).append(`<option value = "" > --Select--</option>`)
			for (var i = 0; i < response.length; i++) {
				$("#symbolGroup_" + count).append(`<option value = "${response[i].value}"> ${response[i].text}</option>`)
			}
			if (defaultVal != "") {
				$("#symbolGroup_" + count).val(defaultVal);
			}
		},
		error: function (errorTemp) {
			console.log('error', errorTemp);
		}
	})
}

function BindTradeStatus(count, defaultVal = "") {
	$.ajax({
		async: true,
		type: "POST",
		url: 'GetTradeStatus',
		dataType: 'json',
		data: {},
		success: function (response) {

			$("#TradeStatus_" + count).append(`<option value = "" > --Select--</option>`)
			for (var i = 0; i < response.length; i++) {
				$("#TradeStatus_" + count).append(`<option value = "${response[i].value}"> ${response[i].text}</option>`)
			}
			if (defaultVal != "") {
				$("#TradeStatus_" + count).val(defaultVal);
			}
		},
		error: function (errorTemp) {
			console.log('error', errorTemp);
		}
	})
}

function SaveRulesCondiDtl() {
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
	debugger;
	IsValid = true;
	var rulesInstrument = [];
	//var total_cnt = document.getElementById("myTable").rows.length;
	for (var i = 0; i <= countRuleInstrument; i++) {
		if (($("#instrumentid_" + i).val() != "") && ($("#instrumentid_" + i).val() != null)) {
			var obj = {

				MasterInstrumentId: $("#instrumentid_" + i).val(),
				SymbolGroupId: $("#symbolGroup_" + i).val(),
				TradeId: $("#tradeStatusId_" + i).val(),
				QTFrom: $("#QTFrom_" + i).val(),
				QTTo: $("#QTTo_" + i).val(),
				TTFrom: $("#TTFrom_" + i).val(),
				TTTo: $("#TTTo_" + i).val(),
				SymbolDenomination: $("#SymbolDenomination_" + i).val(),
				TradeStatus: $("#TradeStatus_" + i + " option:selected").text().trim(),
				AverageSpread: $("#AverageSpread_" + i).val(),
				Decimals: $("#Decimals_" + i).val(),
				UnitDescription: $("#UnitDescription_" + i).val(),
				ZerosToBeGrouped: $("#ZerosToBeGrouped_" + i).val(),
			}
			rulesInstrument.push(obj);
		}
	}
	$("#instrumentData").val(JSON.stringify(rulesInstrument));
}

function SaveSpecificationRuleData() {
	debugger;
	$("#Loader").show();
	SaveRulesCondiDtl();
	var formData = new FormData();
	formData.append("Id", $("#Id").val());
	formData.append("Comment", $("#Comment").val());
	formData.append("Priority", $("#Priority").val());
	formData.append("objRuleConditionViewModelstr", $("#rulesCondtionData").val());
	//formData.append("objRuleInstrumentViewModelstr", $("#instrumentData").val());
	
	$.ajax({
		type: "POST",
		url: 'UpdateSpecificationRule',
		data: formData,
		contentType: false,
		processData: false,
		success: function (res) {
			debugger;
			if (res.statusCode == 200) {
				$("#Loader").hide();
				NotifyMsg('success', res.message);
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

function DeleteSpecificatonRuleInstrument(id, instrumentId) {
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
					type: "POST",
					url: 'DeleteSpecificationRuleInstrument',
					dataType: 'json',
					data: { SpecificationRuleId: id, InstrumentId: instrumentId },
					success: function (res) {
						debugger;
						if (res.code == 200) {
							//$("#frmModel").modal('hide');
							//$("#rulesDetailsData").resetForm();
							NotifyMsg('success', res.message);
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
		});
}

function DeleteRule(RowNumber) {
	debugger
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
				debugger
				var Conditions_dtl_Id = $("#objRuleConditions_Dtl_Id_" + RowNumber).val()
				var RuleId = $("#objRuleConditions_Id_" + RowNumber).val()
				$.ajax({
					type: "POST",
					url: 'DeleteSpecificationRuleConditionDtlById',
					dataType: 'json',
					data: { RuleConditionDtlId: Conditions_dtl_Id, RuleConditionId: RuleId },
					success: function (res) {
						if (res.code == 200) {
							//$("#frmModel").modal('hide');
							//$("#rulesDetailsData").resetForm();
							NotifyMsg('success', res.message);
							location.reload();
							//window.location.href = "/SwapRule/index";

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
