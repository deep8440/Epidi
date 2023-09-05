var countRuleInstrument = 0;
var countLPRuleDetails = 0;
function GetInstrumentWithSwapRuleEditTable(response) {
	debugger;
	var rows = $('#tblSwapRueInstrument').find('tbody tr');
	var lastTR = $("tr:last");
	var tdHtml = lastTR.find("td:first").html();
	countRuleInstrument = countRuleInstrument + 1;

	var strHTML = "";
		
	strHTML += `<tr>`;
	strHTML += `<td>`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input id="swapRuleInstrumentId_${countRuleInstrument}" type="hidden">`;
	strHTML += `<input id="objRuleInstrument_Id_${countRuleInstrument}" type="hidden">`;
	strHTML += `<input id="objRuleInstrumen_RuleId_${countRuleInstrument}" type="hidden">`;
	//strHTML += `<input id="instrumentid_${countRuleInstrument}" type="hidden">`;
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
	//strHTML += `<input id="instrumentid_${countRuleInstrument}" type="hidden">`;
	strHTML += `<input id="symbolGroup_${countRuleInstrument}" type="hidden">`;
	var symbols = JSON.parse($('#SymbolList').val());
	strHTML += `<select class="form-control" id="symbolGroup_${countRuleInstrument}" name="Symbols">`;
	$.each(symbols, function (i, j) {
		if (symbols[i].Group != undefined && symbols[i].Group != null && symbols[i].Group != '') {
			strHTML += '<option value="' + symbols[i].Id + '">' + symbols[i].Group + '</Option>';
		}
	});
	strHTML += `</select>`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="BuySwapPer1000Notional_${countRuleInstrument}">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="SellSwapPer1000Notional_${countRuleInstrument}"> `;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="BuySwapNotional_${countRuleInstrument}">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="SellSwapNotional_${countRuleInstrument}">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="TrippleSwapsDay_${countRuleInstrument}">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td style="width:100px;">`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="DaystoApply_${countRuleInstrument}">`;
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
	strHTML += `<input class="form-control" id="Priority_${countRuleInstrument}">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td>`;
	strHTML += `<div class="form-group">`;
	//strHTML += '<a href="javascript:void(0)" value="Delete" onclick="DeleteSwapRuleInstrument(response,${swapRuleId})" style="display:block;width:100%;height:calc(2.25rem + 2px);padding:0.375rem 0.75rem;"> <i class="fa fa-trash" style="font-size: large;margin-top: 8px;" aria-hidden="true"></i></a>'
	strHTML += `<a href="javascript:void(0)" value="Delete" onclick="DeleteSwapRuleN($(this))" style=" display: block; width: 100%; height: calc(2.25rem + 2px); padding: 0.375rem 0.75rem;">`;
	strHTML += `<i class="fa fa-trash" style="font-size: large;margin-top: 8px;" aria-hidden="true"></i>`;

	//strHTML += `<a href="javascript:void(0)" value="Delete" onclick="DeleteSwapRuleInstrument(${response.id},${response.swapRuleId})" style=" display: block; width: 100%; height: calc(2.25rem + 2px); padding: 0.375rem 0.75rem;">`;
	//strHTML += `<i class="fa fa-trash" style="font-size: large;margin-top: 8px;" aria-hidden="true"></i>`;
	strHTML += `</a>`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `</tr>`;

	$("#tblSwapRueInstrument").append(strHTML);


	if (response != undefined) {
		debugger;
		//BindIntruments(countRuleInstrument, response.instrumentId);
		//BindSymbolGroups(countRuleInstrument, response.symbolGroupId);
		$("#swapRuleInstrumentId_" + countRuleInstrument).val(response.id);
		$("#instrumentid_" + countRuleInstrument).val(response.instrumentId);
		$("#symbolGroup_" + countRuleInstrument).val(response.symbolGroupId);
		$("#instrumentName_" + countRuleInstrument).val(response.instrumentId);
		$("#symbolGroupName_" + countRuleInstrument).val(response.symbolGroup);
		$("#BuySwapPer1000Notional_" + countRuleInstrument).val(response.buySwapPer1000Notional);
		$("#SellSwapPer1000Notional_" + countRuleInstrument).val(response.sellSwapPer1000Notional);
		$("#BuySwapNotional_" + countRuleInstrument).val(response.buySwapNotional);
		$("#SellSwapNotional_" + countRuleInstrument).val(response.sellSwapNotional);
		$("#TrippleSwapsDay_" + countRuleInstrument).val(response.trippleSwapsDay);
		$("#DaystoApply_" + countRuleInstrument).val(response.daystoApply);
		$("#RoundingBuy_" + countRuleInstrument).val(response.roundingBuy);
		$("#RoundingSell_" + countRuleInstrument).val(response.roundingSell);
		$("#DecimalCut_" + countRuleInstrument).val(response.decimalCut);
		$("#Priority_" + countRuleInstrument).val(response.priority);
	}
	else {
		//BindIntruments(countRuleInstrument);
		//BindSymbolGroups(countRuleInstrument);
	}
	countRuleInstrument = countRuleInstrument + 1;
}

function GetSwapRuleInstrumentBySwapRuleId(ruleId) {
	debugger 
	
	$('#tblSwapRueInstrument').DataTable(
		{
			ajax: {
				url: "/SwapRule/Get_SwapRuleInstrumentBySwapRuleId",
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
						html += "<input id='swapRuleInstrumentId_" + countRuleInstrument + "' value=" + row.id + " type='hidden'>"
						html += "<input id='instrumentid_" + countRuleInstrument + "' value=" + row.instrumentId + " type='hidden'>"
						html += "<input id='symbolGroup_" + countRuleInstrument + "' value=" + row.symbolGroupId + " type='hidden''>";
						html += "<input class='form-control' id='instrumentName_" + countRuleInstrument + "'  value='" + row.instrumentName + "' type='text' readonly='readonly'>";

						return html;
					}
					, "orderable": false, "width": "2%"
				},
				{
					"title": "SymbolGroupName", "data": "symbolGroupName",
					"render": function (data, type, row) {

						var html = "";
						html += "<input class='form-control' id='symbolGroupName_" + countRuleInstrument + "' value=" + row.symbolGroup + " type='text' readonly='readonly'>";
						return html;
					}
					, "orderable": false, "width": "2%"
				},
				{
					"title": "BuySwapPer1000Notional", "data": "buySwapPer1000Notional",
					"render": function (data, type, row) {

						var html = "";
						html += "<input class='form-control' id='BuySwapPer1000Notional_" + countRuleInstrument + "' value=" + row.buySwapPer1000Notional + ">";
						return html;
					}
					, "orderable": false, "width": "2%"
				},
				{
					"title": "SellSwapPer1000Notional", "data": "sellSwapPer1000Notional",
					"render": function (data, type, row) {

						var html = "";
						html += "<input class='form-control' id='SellSwapPer1000Notional_" + countRuleInstrument + "' value=" + row.sellSwapPer1000Notional + ">";
						return html;
					}
					, "orderable": false, "width": "2%"
				},
				{
					"title": "BuySwapNotional", "data": "buySwapNotional",
					"render": function (data, type, row) {

						var html = "";
						html += "<input class='form-control' id='BuySwapNotional_" + countRuleInstrument + "' value=" + row.buySwapNotional + ">";
						return html;
					}
					, "orderable": false, "width": "2%"
				},
				{
					"title": "SellSwapNotional", "data": "sellSwapNotional",
					"render": function (data, type, row) {

						var html = "";
						html += "<input class='form-control' id='SellSwapNotional_" + countRuleInstrument + "' value=" + row.sellSwapNotional + ">";
						return html;
					}
					, "orderable": false, "width": "2%"
				},
				{
					"title": "TrippleSwapsDay", "data": "trippleSwapsDay",
					"render": function (data, type, row) {

						var html = "";
						html += "<input class='form-control' id='TrippleSwapsDay_" + countRuleInstrument + "' value=" + row.trippleSwapsDay + ">";
						return html;
					}
					, "orderable": false, "width": "2%"
				},
				{
					"title": "DaystoApply", "data": "daystoApply",
					"render": function (data, type, row) {

						var html = "";
						html += "<input class='form-control' id='DaystoApply_" + countRuleInstrument + "' value=" + row.daystoApply + ">";
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
					"title": "Priority", "data": "priority",
					"render": function (data, type, row) {

						var html = "";
						html += "<input class='form-control' id='Priority_" + countRuleInstrument + "' value=" + row.priority + ">";
						return html;
					}
					, "orderable": false, "width": "2%"
				},
				{
					"title": "Action", "data": "daysToApply",
					"render": function (data, type, row) {

						var html = "";


						html += "<div class='form-group'>";
						html += "<a href='javascript:void(0)' value='Delete' onclick='DeleteSwapRuleInstrument(" + row.id + "," + row.instrumentId + ")' style='display:block;width:100%;padding: 0.375rem 0.75rem;'>";
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


//function GetSwapRuleInstrumentBySwapRuleId(ruleId) {
//	$.ajax({
//		async: true,
//		type: "GET",
//		url: '/SwapRule/Get_SwapRuleInstrumentBySwapRuleId',
//		dataType: 'json',
//		data: { Rule_Id: ruleId },
//		success: function (response) {
//			$("#tblInstrument").empty();
//			countRuleInstrument = 0;
//			for (var i = 0; i < response.length; i++) {
//				////Instrument design               
//				GetInstrumentWithSwapRuleEditTable(response[i]);
//			}
//		},
//		error: function (errorTemp) {
//			console.log('error', errorTemp);
//		}
//	})
//}

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
					url: 'DeleteSwapRuleConditionDtlById',
					dataType: 'json',
					data: { RuleConditionDtlId : Conditions_dtl_Id, RuleConditionId: RuleId },
					success: function (res) {
						if (res.code == 200) {
							//$("#frmModel").modal('hide');
							//$("#rulesDetailsData").resetForm();
							NotifyMsg('success', res.message);
							window.location.href = "/SwapRule/index";

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


function BindIntruments(count, defaultVal = "") {
	$.ajax({
		async: true,
		type: "POST",
		url: 'GetInstrumentsDropDown',
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
		url: 'GetAllSymbolGroupDropDown',
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

function SaveInstrumentDataDtl1() {
	debugger;
	IsValid = true;
	var rulesInstrument = [];
	//var total_cnt = document.getElementById("myTable").rows.length;
	for (var i = 0; i <= countRuleInstrument; i++) {

		var tmp = $("#swapRuleInstrumentId_" + i).val();
		if (tmp == undefined || tmp == null || tmp == "")
		{
			tmp = 0;
        }
		if (($("#instrumentid_" + i).val() != "") && ($("#instrumentid_" + i).val() != null)) {
			var obj = {

				Id: tmp,
				SwapRuleId: $("#Id").val(),
				InstrumentId: $("#instrumentid_" + i).val(),
				InstrumentName: $("#instrumentid_" + i + " option:selected").text(),
				symbolGroupId: $('select#symbolGroup_'+ i +' option:selected').val(),
				SymbolGroup: $('select#symbolGroup_' + i + ' option:selected').text(),
				BuySwapPer1000Notional: $("#BuySwapPer1000Notional_" + i).val(),
				SellSwapPer1000Notional: $("#SellSwapPer1000Notional_" + i).val(),
				BuySwapNotional: $("#BuySwapNotional_" + i).val(),
				SellSwapNotional: $("#SellSwapNotional_" + i).val(),
				TrippleSwapsDay: $("#TrippleSwapsDay_" + i).val(),
				DaystoApply: $("#DaystoApply_" + i).val(),
				RoundingBuy: $("#RoundingBuy_" + i).val(),
				RoundingSell: $("#RoundingSell_" + i).val(),
				DecimalCut: $("#DecimalCut_" + i).val(),
				Priority: $("#Priority_" + i).val()
			}
			rulesInstrument.push(obj);
		}
	}
	$("#instrumentData").val(JSON.stringify(rulesInstrument));
}

function SaveSwapRuleData() {
	debugger;
	SaveRulesCondiDtl();
	SaveInstrumentDataDtl1();
	var formData = new FormData();
	formData.append("Id", $("#Id").val());
	formData.append("RuleName", $("#RuleName").val());
	formData.append("Comment", $("#Comment").val());
	formData.append("TimeToApply", $("#TimeToApply").val());
	formData.append("Priority", $("#Priority").val());
	formData.append("objRuleConditionViewModelstr", $("#rulesCondtionData").val());
	debugger
	formData.append("objRuleInstrumentViewModelstr", $("#instrumentData").val());

	$.ajax({
		type: "POST",
		url: 'AddSwapRule',
		data: formData,
		contentType: false,
		processData: false,
		success: function (res) {
			debugger;
			if (res.statusCode == 200) {
				//$("#frmModel").modal('hide');
				//$("#rulesDetailsData").resetForm();
				$("#Loader").hide();
				NotifyMsg('success', res.message);
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

function DeleteSwapRuleN(obj) {
	debugger
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
			$(object).parent().parent().parent().remove();
		}
	});
}


function DeleteSwapRuleInstrument(id, swapruleId) {
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
					url: 'DeleteSwapRuleInstrument',
					dataType: 'json',
					data: { Id: id, SwapRuleId: swapruleId },
					success: function (res) {
						debugger;
						if (res.code == 200) {
							//$("#frmModel").modal('hide');
							//$("#rulesDetailsData").resetForm();
							NotifyMsg('success', res.message);
							location.reload();
							//window.location.href = "/SwapRule/Edit?Id=" + swapruleId;

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