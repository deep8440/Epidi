
var countRuleInstrument = 0;
var countLPRuleDetails = 0;

$("#IsMobileView").on('change', function () {
	if ($(this).is(':checked')) {
		$(this).attr('value', 'true');
	} else {
		$(this).attr('value', 'false');
	}


});

$("#IsBrokerCommission").on('change', function () {
	if ($(this).is(':checked')) {
		$(this).attr('value', 'true');
	} else {
		$(this).attr('value', 'false');
	}


});


function GetInstrumentWithCommissionRuleEditTable(response) {
	debugger;
	var strHTML = "";
	if (countRuleInstrument == "0") {
		strHTML += `<thead><tr>`;
		strHTML += `<th>#</th>`;
		strHTML += `<th>InstrumentName</th>`;
		strHTML += `<th>SymbolGroup</th>`;
		strHTML += `<th><p>Buy Fee<br>Per<br>$1000<br>Notional($)</p></th>`;
		strHTML += `<th><p>Sell Fee<br>Per<br>$1000<br>Notional($)</p></th>`;
		strHTML += `<th><p>Buy Fee<br>(%)<br>Notional</p></th>`;
		strHTML += `<th><p>Sell Fee<br>(%)<br>Notional</p></th>`;
		strHTML += `<th><p>Fee Buy<br>Per<br>Units($)</p></th>`;
		strHTML += `<th><p>Fee Sell<br>Units($)<p/></th>`;
		strHTML += `<th>Feecharge</th>`;
		strHTML += `<th>RoundingBuy</th>`;
		strHTML += `<th>RoundingSell</th>`;
		strHTML += `<th>DecimalCut</th>`;
		strHTML += `<th>DaysToApply</th>`;
		strHTML += `<th>Action</th>`;
		strHTML += `</tr></thead>`;
	}
	strHTML += `<tbody><tr>`;
	strHTML += `<td>${countRuleInstrument + 1}`;
	strHTML += `</td>`;
	strHTML += `<td>`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input id="commissionRuleInstrumentId_${countRuleInstrument}" type="hidden">`;	
	if (response == null || response == 'undefined') {
		strHTML += `<select class="form-control" id="instrumentid_${countRuleInstrument}" style="border:1px solid;" readonly="readonly">`;
		strHTML += `</select>`;
	}
	else {
		strHTML += `<input id="instrumentid_${countRuleInstrument}" value="${response.instrumentId}" type="hidden">`;
		strHTML += `<input class="form-control" id="instrumentName_${countRuleInstrument}"  value="${response.instrumentName}" type="text" readonly="readonly">`;
	}
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td>`;
	strHTML += `<div class="form-group" style="width:130px;">`;
	
	if (response == null || response == 'undefined') {
		strHTML += `<select disabled class="form-control" id="symbolGroup_${countRuleInstrument}" style="border:1px solid;" readonly="readonly">`;
		strHTML += `</select>`;
	}
	else {
		strHTML += `<input id="symbolGroup_${countRuleInstrument}" value="${response.symbolGroupId}" type="hidden">`;
		strHTML += `<input class="form-control" id="symbolGroupName_${countRuleInstrument}" value="${response.symbolGroup}" type="text" readonly="readonly">`;
	}
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="BuyFeePer1000Notional_${countRuleInstrument}">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="SellFeePerNotional_${countRuleInstrument}"> `;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="BuyFeeNotional_${countRuleInstrument}">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="SellFeeNotional_${countRuleInstrument}">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="FeeBuyPerUnits_${countRuleInstrument}">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="FeeSellUnits_${countRuleInstrument}">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="Feecharge_${countRuleInstrument}">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="RoundingBuy_${countRuleInstrument}">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="RoundingSell_${countRuleInstrument}">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="DecimalCut_${countRuleInstrument}">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="DaysToApply_${countRuleInstrument}">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td>`;
	strHTML += `<div class="form-group">`;
	strHTML += `<a href="javascript:void(0)" value="Delete" onclick="DeleteCommissionRuleInstrument(${response.id},${response.commissionRuleId})" style=" display: block; width: 100%; height: calc(2.25rem + 2px); padding: 0.375rem 0.75rem;">`;
	strHTML += `<i class="fa fa-trash" style="font-size: large;margin-top: 8px;" aria-hidden="true"></i>`;
	strHTML += `</a>`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `</tr></tbody>`;

	$("#tblInstrument").append(strHTML);
	if ($.fn.DataTable.isDataTable("#tblInstrument")) {
		$('#tblInstrument').dataTable().fnDestroy();
		//$('#divUserTable').html('<table class="table table-striped" width="100%" id="UserTable"></table>');
	}
	$("#tblInstrument").dataTable({
		"paging": true,
		"pageLength": 10
	});

	if (response != undefined) {
		debugger;
		BindIntruments(countRuleInstrument, response.instrumentId);
		BindSymbolGroups(countRuleInstrument, response.symbolGroupId);
		$("#commissionRuleInstrumentId_" + countRuleInstrument).val(response.id);
		$("#instrumentid_" + countRuleInstrument).val(response.instrumentId);
		$("#symbolGroup_" + countRuleInstrument).val(response.symbolGroupId);
		$("#BuyFeePer1000Notional_" + countRuleInstrument).val(response.buyFeePer1000Notional);
		$("#SellFeePerNotional_" + countRuleInstrument).val(response.sellFeePerNotional1000);
		$("#BuyFeeNotional_" + countRuleInstrument).val(response.buyFeeNotional);
		$("#SellFeeNotional_" + countRuleInstrument).val(response.sellFeeNotional);
		$("#FeeBuyPerUnits_" + countRuleInstrument).val(response.feeBuyPerUnits);
		$("#FeeSellUnits_" + countRuleInstrument).val(response.feeSellUnits);
		$("#Feecharge_" + countRuleInstrument).val(response.feecharge);
		$("#DaysToApply_" + countRuleInstrument).val(response.daysToApply);
		$("#RoundingBuy_" + countRuleInstrument).val(response.roundingBuy);

		$("#RoundingSell_" + countRuleInstrument).val(response.roundingSell);
		$("#DecimalCut_" + countRuleInstrument).val(response.decimalCut);
	}
	else {
		//BindIntruments(countRuleInstrument);
		//BindSymbolGroups(countRuleInstrument);
	}
	countRuleInstrument = countRuleInstrument + 1;
}

function BindIntruments(count, defaultVal = "") {
	$.ajax({
		async: true,
		type: "POST",
		url: 'GetInstrumentsDropDown',
		dataType: 'json',
		data: {},
		success: function (response) {
			debugger
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
		url: 'GetAllSymbolGroupDropDown',
		dataType: 'json',
		data: {},
		success: function (response) {
			debugger
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

//function GetCommissionRuleInstrumentByCommissionRuleId(ruleId) {
//	debugger;
//	$("#Loader").show();
//	$.ajax({
//		async: true,
//		type: "GET",
//		url: '/CommissionRule/Get_CommissionRuleInstrumentByCommissionRuleId',
//		dataType: 'json',
//		data: { Rule_Id: ruleId },
//		success: function (response) {
//			debugger
//			$("#tblInstrument").empty();
//			countRuleInstrument = 0;
//			for (var i = 0; i < response.length; i++) {
//				////Instrument design               
//				GetInstrumentWithCommissionRuleEditTable(response[i]);
//				$("#Loader").hide();
//			}
//		},
//		error: function (errorTemp) {
//			console.log('error', errorTemp);
//		}
//	})
//}

function SaveCommissionRulesCondiDtl() {

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

function SaveCommissionInstrumentDataDtl() {
	
	IsValid = true;
	var rulesInstrument = [];
	//var total_cnt = document.getElementById("myTable").rows.length;
	for (var i = 0; i <= countRuleInstrument; i++) {
		if (($("#instrumentid_" + i).val() != "") && ($("#instrumentid_" + i).val() != null)) {
			var obj = {

				Id: $("#commissionRuleInstrumentId_" + i).val(),
				InstrumentId: $("#instrumentid_" + i).val(),
				InstrumentName: $("#instrumentName_" + i).val().trim(),
				SymbolGroupId: $("#symbolGroup_" + i).val(),
				SymbolGroup: $("#symbolGroupName_" + i).val().trim(),
				BuyFeePer1000Notional: $("#BuyFeePer1000Notional_" + i).val(),
				SellFeePerNotional1000: $("#SellFeePerNotional_" + i).val(),
				BuyFeeNotional: $("#BuyFeeNotional_" + i).val(),
				SellFeeNotional: $("#SellFeeNotional_" + i).val(),
				FeeBuyPerUnits: $("#FeeBuyPerUnits_" + i).val(),
				FeeSellUnits: $("#FeeSellUnits_" + i).val(),
				Feecharge: $("#Feecharge_" + i).val(),
				DaystoApply: $("#DaysToApply_" + i).val(),
				RoundingBuy: $("#RoundingBuy_" + i).val(),
				RoundingSell: $("#RoundingSell_" + i).val(),
				DecimalCut: $("#DecimalCut_" + i).val()
			}
			rulesInstrument.push(obj);
		}
	}
	$("#instrumentData").val(JSON.stringify(rulesInstrument));
}

function SaveCommissionRuleData() {

	SaveCommissionRulesCondiDtl();
	SaveCommissionInstrumentDataDtl();
	var formData = new FormData();
	formData.append("Id", $("#Id").val());
	formData.append("Comment", $("#Comment").val());
	formData.append("TimeToApply", $("#TimeToApply").val());
	formData.append("Priority", $("#Priority").val());
	formData.append("WhenToApplyComboId", $("#WhenToApplyComboId").val());
	formData.append("OrderComment", $("#OrderComment").val());
	formData.append("Leverage", $("#Leverage").val());
	formData.append("UnitsTypeId", $("#UnitsTypeId").val());
	formData.append("WhereToApplyId", $("#WhereToApplyId").val());
	formData.append("PercentageValue", $("#PercentageValue").val());
	formData.append("objRuleConditionViewModelstr", $("#rulesCondtionData").val());
	formData.append("objRuleInstrumentViewModelstr", $("#instrumentData").val());
	formData.append("IsMobileView", $("#IsMobileView").val());
	formData.append("IsBrokerCommission", $("#IsBrokerCommission").val());



	$.ajax({
		type: "POST",
		url :'AddCommissionRule',
		/*url: 'AddCoAddCommissionRulemmissionRule',*/
		data: formData,
		contentType: false,
		processData: false,
		success: function (res) {
		
			if (res.statusCode == 200) {
				//$("#frmModel").modal('hide');
				//$("#rulesDetailsData").resetForm();
				$("#Loader").hide();
				NotifyMsg('success', res.message);
				window.location.href = "/CommissionRule/Edit?Id=" + $("#Id").val();

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

function DeleteCommissionRuleInstrument(id, commissionruleId) {
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
					url: 'DeleteCommissionRuleInstrument',
					dataType: 'json',
					data: { Id: id, CommissionRuleId: commissionruleId },
					success: function (res) {
						debugger;
						if (res.code == 200) {
							//$("#frmModel").modal('hide');
							//$("#rulesDetailsData").resetForm();
							NotifyMsg('success', res.message);
							window.location.href = "/CommissionRule/Edit?Id=" + commissionruleId;

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
					async: true,
					type: "POST",
					url: 'DeleteCommissonRuleConditionDtlById',
					dataType: 'json',
					data: { RuleConditionDtlId: Conditions_dtl_Id, RuleConditionId: RuleId },
					success: function (res) {
						if (res.code == 200) {
							//$("#frmModel").modal('hide');
							//$("#rulesDetailsData").resetForm();
							NotifyMsg('success', res.message);
							window.location.href = "/CommissionRule/Edit?Id=" + $("#Id").val();

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

function GetCommissionRuleInstrumentByCommissionRuleId(ruleId) {
	$('#tblInstrument').DataTable(
		{
			ajax: {
				url: "/CommissionRule/Get_CommissionRuleInstrumentByCommissionRuleId",
				type: "POST",
				data: { Rule_Id: ruleId }
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
						
						var html = "";
						html += "<input id='commissionRuleInstrumentId_" + countRuleInstrument + "' value=" + row.id +" type='hidden'>"
						html += "<input id='instrumentid_"+countRuleInstrument+"' value="+row.instrumentId+" type='hidden'>"
						html += "<input class='form-control' id='instrumentName_" + countRuleInstrument + "'  value='" + row.instrumentName + "' type='text' readonly='readonly'>";
						return html;
					}
					, "orderable": false, "width": "2%"
				},
				{
					"title": "SymbolGroupName", "data": "symbolGroupName",
					"render": function (data, type, row) {
						
						var html = "";
						html += "<input id='symbolGroup_"+countRuleInstrument+"' value="+row.symbolGroupId+" type='hidden''>";
						html += "<input class='form-control' id='symbolGroupName_"+countRuleInstrument+"' value="+row.symbolGroup+" type='text' readonly='readonly'>";
						return html;
					}
					, "orderable": false, "width": "2%"
				},
				{
					"title": "BuyFeePer1000Notional", "data": "buyFeePer1000Notional",
					"render": function (data, type, row) {
						
						var html = "";
						html += "<input class='form-control' id='BuyFeePer1000Notional_" + countRuleInstrument + "' value=" + row.buyFeePer1000Notional +">";
						return html;
					}
					, "orderable": false, "width": "2%"
				},
				{
					"title": "SellFeePerNotional", "data": "sellFeePerNotional",
					"render": function (data, type, row) {
						
						var html = "";
						html += "<input class='form-control' id='SellFeePerNotional_" + countRuleInstrument + "' value=" + row.sellFeePerNotional1000 +">";
						return html;
					}
					, "orderable": false, "width": "2%"
				},
				{
					"title": "BuyFeeNotional", "data": "buyFeeNotional",
					"render": function (data, type, row) {
						
						var html = "";
						html += "<input class='form-control' id='BuyFeeNotional_" + countRuleInstrument + "' value=" + row.buyFeeNotional + ">";
						return html;
					}
					, "orderable": false, "width": "2%"
				},
				{
					"title": "SellFeeNotional", "data": "sellFeeNotional",
					"render": function (data, type, row) {
						
						var html = "";
						html += "<input class='form-control' id='SellFeeNotional_" + countRuleInstrument + "' value=" + row.sellFeeNotional + ">";
						return html;
					}
					, "orderable": false, "width": "2%"
				},
				{
					"title": "FeeBuyPerUnits", "data": "feeBuyPerUnits",
					"render": function (data, type, row) {
						
						var html = "";
						html += "<input class='form-control' id='FeeBuyPerUnits_" + countRuleInstrument + "' value=" + row.feeBuyPerUnits + ">";
						return html;
					}
					, "orderable": false, "width": "2%"
				},
				{
					"title": "FeeSellUnits", "data": "feeSellUnits",
					"render": function (data, type, row) {
						
						var html = "";
						html += "<input class='form-control' id='FeeSellUnits_" + countRuleInstrument + "' value=" + row.feeSellUnits + ">";
						return html;
					}
					, "orderable": false, "width": "2%"
				},
				{
					"title": "Feecharge", "data": "feecharge",
					"render": function (data, type, row) {
						
						var html = "";
						html += "<input class='form-control' id='Feecharge_" + countRuleInstrument + "' value=" + row.feecharge + ">";
						return html;
					}
					, "orderable": false, "width": "2%"
				},
				{
					"title": "RoundingBuy", "data": "roundingBuy",
					"render": function (data, type, row) {
						
						var html = "";
						html += "<input class='form-control' id='RoundingBuy_" + countRuleInstrument + "' value=" + row.roundingBuy + ">";
						return html;
					}
					, "orderable": false, "width": "2%"
				},
				{
					"title": "RoundingSell", "data": "roundingSell",
					"render": function (data, type, row) {
						
						var html = "";
						html += "<input class='form-control' id='RoundingSell_" + countRuleInstrument + "' value=" + row.roundingSell + ">";
						return html;
					}
					, "orderable": false, "width": "2%"
				},
				{
					"title": "DecimalCut", "data": "decimalCut",
					"render": function (data, type, row) {
						
						var html = "";
						html += "<input class='form-control' id='DecimalCut_" + countRuleInstrument + "' value=" + row.decimalCut + ">";
						return html;
					}
					, "orderable": false, "width": "2%"
				},
				{
					"title": "DaysToApply", "data": "daysToApply",
					"render": function (data, type, row) {
						
						var html = "";
						html += "<input class='form-control' id='DaysToApply_" + countRuleInstrument + "' value=" + row.daysToApply + ">";
						return html;
					}
					, "orderable": false, "width": "2%"
				},
				{
					"title": "Action", "data": "daysToApply",
					"render": function (data, type, row) {
						
						var html = "";
						html += "<div class='form-group'>";
						html += "<a href='javascript:void(0)' value='Delete' onclick='DeleteCommissionRuleInstrument(" + row.id + "," + row.commissionRuleId +")' style='display:block;width:100%;padding: 0.375rem 0.75rem;'>";
						html += "<i class='fa fa-trash' style='font-size: large;margin-top: 8px;' aria-hidden='true'></i>";
						html += "</a>";
						html += "</div>";
						countRuleInstrument++;
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