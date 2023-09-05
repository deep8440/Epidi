var TradeRuleUniversalValues = [];

var tempIbName = [];
var tempInstument = [];
var tempCountryName = [];
var tempLegalEntity = [];
var tempLpName = [];
var countRuleInstrument = 1;
var countLPRuleDetails = 0;
var data = '';
var dynamicTradeRulesColumns = [];

//$(document).ready(() => {
//    debugger;
//    $.ajax({
//        type: "GET",
//        url: 'Get_TradeUniversalValues',
//        dataType: 'json',
//        success: function (data) {
//            debugger;
//            if (data != null) {
//                for (var i = 0; i < data.length; i++) {
//                    dynamicColumn = {};
//                    dynamicColumn['data'] = "values";
//                    dynamicColumn['title'] = "Instrument Name";
//                    dynamicColumn['render'] = function (data, type, row) { let NameHtml = `<p>${row.values}</p>`; return NameHtml; };
//                    dynamicColumn['orderable'] = false;
//                    dynamicColumn['width'] = "3%";
//                    dynamicColumns.push(dynamicColumn);

//                }
//            }

//        }
//    });

//    fetchRuleInstrumentGrid.init();
//});

//function GetPriorityTradeRule(ruleId) {
//    debugger;
//    $.ajax({
//        async: true,
//        type: "GET",
//        url: 'Get_TradeUniversalValues',
//        dataType: 'json',
//        success: function (data) {
//            debugger;
//            if (data != null) {

//                    var dynamicColumn = {};
//                dynamicColumn['data'] = "instrumentName";
//                dynamicColumn['title'] = "Instrument Name";
//                dynamicColumn['render'] = function (data, type, row) { let NameHtml = `<p>${row.instrumentName}</p>`; return NameHtml; };
//                    dynamicColumn['orderable'] = false;
//                    dynamicColumn['width'] = "3%";
//                dynamicColumns.push(dynamicColumn);

//                //for (var i = 0; i < TradeRuleUniversalValues.length; i++) {

//                //}

//            }

//        }
//    });


//}

function GetTradeRuleInstrumentByRuleId(rulID,instrumentId,lpId,day) {

	$('#tblInstrument').DataTable(
		{
			ajax: {
				url: "/TradeRule/Get_InstrumentPriorityByTradeRuleId",
				type: "POST",
				data: { Rule_Id: rulID, InstrumentId: instrumentId, LpId: lpId, Day: day }
			},
			destroy: true,
			stateSave: true,
			autoWidth: false,
			paging: true,
			pageLength: 10,
			processing: true,
			serverSide: true,
			columns: dynamicTradeRulesColumns,
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

//var fetchRuleInstrumentGrid = function () {
//	debugger;
//	var rulID = $('#Id').val();
//	let initTable1 = function () {
//		let table = $('#tblInstrument');

//		let oTable = table.dataTable({
//			"stateSave": true,
//			"autoWidth": false,
//			"paging": true,
//			// Internationalisation. For more info refer to http://datatables.net/manual/i18n
//			"language": {
//				"aria": {
//					"sortAscending": ": activate to sort column ascending",
//					"sortDescending": ": activate to sort column descending"
//				},
//				"emptyTable": "No data available in table",
//				"info": "Showing _START_ to _END_ of _TOTAL_ entries",
//				"infoEmpty": "No entries found",
//				"infoFiltered": "(filtered1 from MAX total entries)",
//				//"lengthMenu": "MENU entries",
//				"search": "Search:",
//				"zeroRecords": "No matching records found"
//			},
//			"pageLength": 10,
//			"processing": true,
//			"serverSide": true,
//			"ajax": {
//				async: false,
//				cache: false,
//				url: '/TradeRule/Get_InstrumentPriorityByTradeRuleId',
//				type: "POST",
//				data: { Rule_Id: rulID }
//			},
//			"columns": dynamicTradeRulesColumns,
//			scrollX: true,
//			scrollCollapse: true,
//			scrollY: '500px',
//			"lengthMenu": [
//				[5, 10, 15, 20],
//				[5, 10, 15, 20] // change per page values here
//			],

//			// set the initial value

//			//"dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>", // horizobtal scrollable datatable

//			// Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
//			// setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js).
//			// So when dropdowns used the scrollable div should be removed.
//			//"dom": "<'row' <'col-md-12'T>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
//		});
//	}

//	return {
//		init: function () {
//			if ($.fn.DataTable.isDataTable("#tblInstrument")) {
//				$('#tblInstrument').dataTable().fnDestroy();
//				//$('#divUserTable').html('<table class="table table-striped" width="100%" id="UserTable"></table>');
//			}
//			initTable1();
//		}
//	};
//}();


const bindTextBox = () => {
	debugger
	let returnHtml = ``;
	returnHtml = "<input class='form-control' id='Value_" + values + "'>";
	//returnHtml = `<select id="${dynamicId}" class="form-control js-select2" onchange="${changeFunction}(${instrumentMasterId},this,'${lpName}')">`;
	return returnHtml;
}
function GetTradeUniversalVal() {
	debugger;
	$.ajax({
		type: "GET",
		async: false,
		url: 'Get_TradeUniversalValues',
		dataType: 'json',
		success: function (data) {
			debugger;
			if (data != null) {

				var dynamicColumn = {};
				dynamicColumn['data'] = "instrumentName";
				dynamicColumn['title'] = "InstrumentName";
				dynamicColumn['render'] = function (data, type, row) {
					debugger;
					var table = $('#tblInstrument').DataTable();
					var table_length = table.data().count();
					countRuleInstrument = table_length;
					//if (table_length == countRuleInstrument) {
					//	countRuleInstrument = 1;
					//}
					let NameHtml = ``;
					NameHtml += `<input id="objRuleInstrument_Id_${countRuleInstrument}" type="hidden" value="${row.id}">`;
					NameHtml += `<input id="objRuleInstrumen_RuleId_${countRuleInstrument}" type="hidden" value="${row.ruleId}" >`;
					NameHtml += `<input id="RuleInstrumentId_${countRuleInstrument}" type="hidden" value="${row.ruleInstrumentId}">`;
					NameHtml += `<input id="TradeRulePriorityId_${countRuleInstrument}" type="hidden" value="${row.tblRulePriority_ID}">`;
					NameHtml += `<input class="form-control" id="instrumentid_${countRuleInstrument}" type="hidden" value="${row.instrumentId}">`;
					NameHtml += `<input class="form-control" id="instrumentName_${countRuleInstrument}"  value="${row.instrumentName}" disabled="disabled" style="width:100px;">`;
					return NameHtml;
				};
				dynamicColumn['orderable'] = false;
				dynamicColumn['width'] = "3%";
				dynamicTradeRulesColumns.push(dynamicColumn);

				dynamicColumn = {};
				dynamicColumn['data'] = "ibid";
				dynamicColumn['title'] = "IBName";
				dynamicColumn['render'] = function (data, type, row) {

					let NameHtml = ``;
					NameHtml += `<input id="ibid_${countRuleInstrument}" type="hidden" value="${row.ibid}">`;
					NameHtml += `<input class="form-control" id="ibName_${countRuleInstrument}" value="${row.ib}" disabled="disabled" style="width:100px;">`;
					return NameHtml;
				};
				dynamicColumn['orderable'] = false;
				dynamicColumn['width'] = "3%";
				dynamicTradeRulesColumns.push(dynamicColumn);

				dynamicColumn = {};
				dynamicColumn['data'] = "legalEntityId";
				dynamicColumn['title'] = "LegalEntityName";
				dynamicColumn['render'] = function (data, type, row) {

					let NameHtml = ``;
					NameHtml += `<input id="legalEntityId_${countRuleInstrument}" type="hidden" value="${row.legalEntityId}">`;
					NameHtml += `<input class="form-control" id="legalEntityName_${countRuleInstrument}" value="${row.legalEntity}" disabled="disabled" style="width:100px;">`;
					return NameHtml;
				};
				dynamicColumn['orderable'] = false;
				dynamicColumn['width'] = "3%";
				dynamicTradeRulesColumns.push(dynamicColumn);

				dynamicColumn = {};
				dynamicColumn['data'] = "countryId";
				dynamicColumn['title'] = "CountryName";
				dynamicColumn['render'] = function (data, type, row) {

					let NameHtml = ``;
					NameHtml += `<input id="countryId_${countRuleInstrument}" type="hidden" value="${row.countryId}">`;
					NameHtml += `<input class="form-control" id="countryName_${countRuleInstrument}" value="${row.countryName}" disabled="disabled" style="width:100px;">`;
					return NameHtml;
				};
				dynamicColumn['orderable'] = false;
				dynamicColumn['width'] = "3%";
				dynamicTradeRulesColumns.push(dynamicColumn);


				dynamicColumn = {};
				dynamicColumn['data'] = "uid";
				dynamicColumn['title'] = "UID";
				dynamicColumn['render'] = function (data, type, row) {

					let NameHtml = ``;
					NameHtml += `<input class="form-control" id="UId_${countRuleInstrument}" value="${row.uid}" style="width:100px;">`;
					return NameHtml;
				};
				dynamicColumn['orderable'] = false;
				dynamicColumn['width'] = "3%";
				dynamicTradeRulesColumns.push(dynamicColumn);

				dynamicColumn = {};
				dynamicColumn['data'] = "day";
				dynamicColumn['title'] = "Day";
				dynamicColumn['render'] = function (data, type, row) {

					let NameHtml = ``;
					NameHtml += `<input class="form-control" id="Day_${countRuleInstrument}" value="${row.day}" style="width:100px;">`;
					return NameHtml;
				};
				dynamicColumn['orderable'] = false;
				dynamicColumn['width'] = "3%";
				dynamicTradeRulesColumns.push(dynamicColumn);

				dynamicColumn = {};
				dynamicColumn['data'] = "startTime";
				dynamicColumn['title'] = "TimeFrom";
				dynamicColumn['render'] = function (data, type, row) {

					let NameHtml = ``;
					NameHtml += `<input class="form-control" id="StartTime_${countRuleInstrument}" value="${row.startTime}" style="width:120px;"  type="time" step=2>`;
					return NameHtml;
				};
				dynamicColumn['orderable'] = false;
				dynamicColumn['width'] = "3%";
				dynamicTradeRulesColumns.push(dynamicColumn);

				dynamicColumn = {};
				dynamicColumn['data'] = "endTime";
				dynamicColumn['title'] = "TimeTo";
				dynamicColumn['render'] = function (data, type, row) {

					let NameHtml = ``;
					NameHtml += `<input class="form-control" id="EndTime_${countRuleInstrument}" value="${row.endTime}" style="width:120px;"  type="time" step=2>`;
					return NameHtml;
				};
				dynamicColumn['orderable'] = false;
				dynamicColumn['width'] = "3%";
				dynamicTradeRulesColumns.push(dynamicColumn);


				dynamicColumn = {};
				dynamicColumn['data'] = "lpId";
				dynamicColumn['title'] = "LPId";
				dynamicColumn['render'] = function (data, type, row) {

					let NameHtml = ``;
					NameHtml += `<input id="lpId_${countRuleInstrument}" type="hidden" value="${row.lpId}">`;
					NameHtml += `<input class="form-control" id="LPName_${countRuleInstrument}" value="${row.lpName}" disabled="disabled" style="width:100px;">`;
					return NameHtml;
				};
				dynamicColumn['orderable'] = false;
				dynamicColumn['width'] = "3%";
				dynamicTradeRulesColumns.push(dynamicColumn);


				dynamicColumn = {};
				dynamicColumn['data'] = "hedgeType";
				dynamicColumn['title'] = "HedgeType";
				dynamicColumn['render'] = function (data, type, row) {

					let NameHtml = ``;
					NameHtml += `<input class="form-control" id="HedgeType_${countRuleInstrument}" value="${row.hedgeType}" style="width:100px;">`;
					return NameHtml;
				};
				dynamicColumn['orderable'] = false;
				dynamicColumn['width'] = "3%";
				dynamicTradeRulesColumns.push(dynamicColumn);

				dynamicColumn = {};
				dynamicColumn['data'] = "priority";
				dynamicColumn['title'] = "Priority";
				dynamicColumn['render'] = function (data, type, row) {

					let NameHtml = ``;
					NameHtml += `<input class="form-control" id="Priority_${countRuleInstrument}" value="${row.priority}" style="width:100px;">`;
					return NameHtml;
				};
				dynamicColumn['orderable'] = false;
				dynamicColumn['width'] = "3%";
				dynamicTradeRulesColumns.push(dynamicColumn);

				dynamicColumn = {};
				dynamicColumn['data'] = "volumeFrom";
				dynamicColumn['title'] = "VolumeFrom";
				dynamicColumn['render'] = function (data, type, row) {

					let NameHtml = ``;
					NameHtml += `<input class="form-control" id="VolumeFrom_${countRuleInstrument}" value="${row.volumeFrom}" style="width:100px;">`;
					return NameHtml;
				};
				dynamicColumn['orderable'] = false;
				dynamicColumn['width'] = "3%";
				dynamicTradeRulesColumns.push(dynamicColumn);

				dynamicColumn = {};
				dynamicColumn['data'] = "volumeTo";
				dynamicColumn['title'] = "VolumeTo";
				dynamicColumn['render'] = function (data, type, row) {

					let NameHtml = ``;
					NameHtml += `<input class="form-control" id="VolumeTo_${countRuleInstrument}" value="${row.volumeTo}" style="width:100px;">`;
					return NameHtml;
				};
				dynamicColumn['orderable'] = false;
				dynamicColumn['width'] = "3%";
				dynamicTradeRulesColumns.push(dynamicColumn);

				dynamicColumn = {};
				dynamicColumn['data'] = "coefficient";
				dynamicColumn['title'] = "Coefficient";
				dynamicColumn['render'] = function (data, type, row) {

					let NameHtml = ``;
					NameHtml += `<input class="form-control" id="Coefficient_${countRuleInstrument}" value="${row.coefficient}" style="width:100px;">`;
					return NameHtml;
				};
				dynamicColumn['orderable'] = false;
				dynamicColumn['width'] = "3%";
				dynamicTradeRulesColumns.push(dynamicColumn);

				dynamicColumn = {};
				dynamicColumn['data'] = "maxCoeffient";
				dynamicColumn['title'] = "MaxCoeffient";
				dynamicColumn['render'] = function (data, type, row) {

					let NameHtml = ``;
					NameHtml += `<input class="form-control" id="MaxCoeffient_${countRuleInstrument}" value="${row.maxCoefficient}" style="width:100px;">`;
					return NameHtml;
				};
				dynamicColumn['orderable'] = false;
				dynamicColumn['width'] = "3%";
				dynamicTradeRulesColumns.push(dynamicColumn);

				for (var i = 0; i < data.length; i++) {
					dynamicColumn = {};
					var j = i + 1;
					var values = data[i].values;
					dynamicColumn['data'] = "value" + j;
					dynamicColumn['title'] = data[i].values;
					dynamicColumn['render'] = (function (values) {
						return function (data, type, row) {
							return bindValues(row, j, values);
						};
					})(values);
					dynamicColumn['orderable'] = false;
					dynamicColumn['width'] = "3%";
					dynamicTradeRulesColumns.push(dynamicColumn);

				}

				dynamicColumn = {};
				dynamicColumn['data'] = "id";
				dynamicColumn['title'] = "Action";
				dynamicColumn['render'] = function (data, type, row) {

					let strHTML = ``;
					strHTML += `<div class="form-group">`;
					strHTML += `<a href="javascript:void(0)" value="Delete" onclick="DeleteLpRule(${row.ruleId},${row.tblRulePriority_ID})" style=" display: block; width: 100%; height: calc(2.25rem + 2px); padding: 0.375rem 0.75rem;">`;
					strHTML += `<i class="fa fa-trash" style="font-size: large;margin-top: 8px;" aria-hidden="true"></i>`;
					strHTML += `</a>`;
					strHTML += `</div>`;
					
					return strHTML;
				};
				dynamicColumn['orderable'] = false;
				dynamicColumn['width'] = "3%";
				dynamicTradeRulesColumns.push(dynamicColumn);
			}
		},
		error: function (errorTemp) {
			console.log('error', errorTemp);
		}
	});

	debugger
	var rulID = $('#Id').val();
	GetTradeRuleInstrumentByRuleId(rulID,0,0,'0');
	//fetchRuleInstrumentGrid.init();
}
const bindValues = (row, counter, values) => {
	
	let returnHtml = ``;
	if (values == "Value 1") {
		returnHtml = `<input class="form-control" id="Value1_${countRuleInstrument}"  value="${row.value1}" style="width:100px;">`;
	}
	if (values == "Value 2") {
		returnHtml = `<input class="form-control" id="Value2_${countRuleInstrument}"  value="${row.value2}" style="width:100px;">`;
	}
	if (values == "Value 3") {
		returnHtml = `<input class="form-control" id="Value3_${countRuleInstrument}"  value="${row.value3}" style="width:100px;">`;
	}
	if (values == "Value 4") {
		returnHtml = `<input class="form-control" id="Value4_${countRuleInstrument}"  value="${row.value4}" style="width:100px;">`;
	}
	if (values == "Value 5") {
		returnHtml = `<input class="form-control" id="Value5_${countRuleInstrument}"  value="${row.value5}" style="width:100px;">`;
	}
	if (values == "Value 6") {
		returnHtml = `<input class="form-control" id="Value6_${countRuleInstrument}"  value="${row.value6}" style="width:100px;">`;
	}
	if (values == "Value 7") {

		returnHtml = `<input class="form-control" id="Value7_${countRuleInstrument}"  value="${row.value7}" style="width:100px;">`;

	}
	if (values == "Value 8") {
		returnHtml = `<input class="form-control" id="Value8_${countRuleInstrument}"  value="${row.value8}" style="width:100px;">`;
	}
	if (values == "Value 9") {
		returnHtml = `<input class="form-control" id="Value9_${countRuleInstrument}"  value="${row.value9}" style="width:100px;">`;
	}
	if (values == "Value 10") {
		returnHtml = `<input class="form-control" id="Value10_${countRuleInstrument}"  value="${row.value10}" style="width:100px;">`;
	}
	if (values == "Value 11") {
		returnHtml = `<input class="form-control" id="Value11_${countRuleInstrument}"  value="${row.value11}" style="width:100px;">`;
	}
	if (values == "Value 12") {
		returnHtml = `<input class="form-control" id="Values12_${countRuleInstrument}"  value="${row.value12}" style="width:100px;">`;
	}
	if (values == "Value 13") {
		returnHtml = `<input class="form-control" id="Value13_${countRuleInstrument}"  value="${row.value13}" style="width:100px;">`;
	}
	if (values == "Value 14") {
		returnHtml = `<input class="form-control" id="Value14_${countRuleInstrument}"  value="${row.value14}" style="width:100px;">`;
	}
	if (values == "Value 15") {
		returnHtml = `<input class="form-control" id="Values_${countRuleInstrument}"  value="${row.value15}" style="width:100px;">`;
	}
	if (values == "Value 16") {
		returnHtml = `<input class="form-control" id="Value15_${countRuleInstrument}"  value="${row.value16}" style="width:100px;">`;
	}
	if (values == "Value 17") {
		returnHtml = `<input class="form-control" id="Value16_${countRuleInstrument}"  value="${row.value17}" style="width:100px;">`;
	}
	if (values == "Value 18") {
		returnHtml = `<input class="form-control" id="Value17_${countRuleInstrument}"  value="${row.value18}" style="width:100px;">`;
	}
	if (values == "Value 19") {
		returnHtml = `<input class="form-control" id="Value18_${countRuleInstrument}"  value="${row.value19}" style="width:100px;">`;
	}
	if (values == "Value 20") {
		returnHtml = `<input class="form-control" id="Value19_${countRuleInstrument}"  value="${row.value20}" style="width:100px;">`;
	}
	return returnHtml;
}

function BindLpRule(count, defaultVal = "") {
	if (tempLpName.length > 0) {
		$("#lpId_" + count).append(`<option value="">--Select--</option>`)
		for (var i = 0; i < tempLpName.length; i++) {
			$("#lpId_" + count).append(`<option value="${tempLpName[i].value}">${tempLpName[i].text}</option>`)
		}
		if (defaultVal != "") {
			$("#lpId_" + count).val(defaultVal);
		}
	}
	else {
		$.ajax({
			async: true,
			type: "POST",
			url: '/QuoteMarkUpRule/GetMarketLPDropDown',
			dataType: 'json',
			data: {},
			success: function (response) {
				tempLpName = [];
				$("#lpId_" + count).append(`<option value="">--Select--</option>`)
				for (var i = 0; i < response.length; i++) {
					$("#lpId_" + count).append(`<option value="${response[i].value}">${response[i].text}</option>`)
					var obj =
					{
						value: response[i].value,
						text: response[i].text,
					}
					tempLpName.push(obj);
				}
				if (defaultVal != "") {
					$("#lpId_" + count).val(defaultVal);
				}
			},
			error: function (errorTemp) {
				console.log('error', errorTemp);
			}
		})
	}
}
function BindIntruments(count, defaultVal = "") {
	debugger
	if (instruments.length > 0) {
		for (var i = 0; i < instruments.length; i++) {
			$("#instrumentid_" + count).append(`<option value = "${instruments[i].Value}">${instruments[i].Text}</option>`)
		}
		if (defaultVal != "") {
			$("#instrumentid_" + count).val(defaultVal);
		}
	}
	//else {
	//	$.ajax({
	//		async: true,
	//		type: "POST",
	//		url: '/QuoteMarkUpRule/GetInstrumentsDropDown',
	//		dataType: 'json',
	//		data: {},
	//		success: function (response) {
	//			tempInstument = [];
	//			$("#instrumentid_" + count).append(`<option value = "" > --Select--</option>`)
	//			for (var i = 0; i < response.length; i++) {
	//				$("#instrumentid_" + count).append(`<option value ="${response[i].value}">${response[i].text}</option>`)
	//				var obj =
	//				{
	//					value: response[i].value,
	//					text: response[i].text,
	//				}
	//				tempInstument.push(obj);
	//			}
	//			if (defaultVal != "") {
	//				$("#instrumentid_" + count).val(defaultVal);
	//			}
	//		},
	//		error: function (errorTemp) {
	//			console.log('error', errorTemp);
	//		}
	//	})
	//}

}
function BindIBName(count, defaultVal = "") {
	debugger;
	if (tempIbName.length > 0) {
		//let returnHtml = ``;
		//returnHtml = `<select id="ibName${count}" class="form-control">`;
		for (var i = 0; i < tempIbName.length; i++) {
			$("#ibid_" + count).append(`<option value = "${tempIbName[i].value}">${tempIbName[i].text}</option>`)
			//         if (defaultVal == tempIbName[i].value) {
			//             returnHtml += `<option selected value="${tempIbName[i].value}">${tempIbName[i].text}</option>`;
			//         }
			//         else {
			//             returnHtml += `<option value = "${tempIbName[i].value}">${tempIbName[i].text}</option>`;
			//}
		}
		if (defaultVal != "") {
			$("#ibid_" + count).val(defaultVal);
			//$("#ib_" + count).val(defaultVal);
		}
		//return returnHtml;
	}
	else {
		$.ajax({
			async: true,
			type: "POST",
			url: 'GetIBDropDown',
			dataType: 'json',
			data: {},
			success: function (response) {
				tempIbName = [];
				$("#ibid_" + count).append(`<option value = "" > --Select--</option>`)
				for (var i = 0; i < response.length; i++) {
					$("#ibid_" + count).append(`<option value = "${response[i].value}">${response[i].text}</option>`)
					var obj =
					{
						value: response[i].value,
						text: response[i].text,
					}
					tempIbName.push(obj);
				}
				if (defaultVal != "") {
					$("#ibid_" + count).val(defaultVal);
				}
			},
			error: function (errorTemp) {
				console.log('error', errorTemp);
			}
		})
	}
}
function BindLegalEntityName(count, defaultVal = "") {
	if (tempLegalEntity.length > 0) {
		for (var i = 0; i < tempLegalEntity.length; i++) {
			$("#legalEntityId_" + count).append(`<option value = "${tempLegalEntity[i].value}">${tempLegalEntity[i].text}</option>`)
		}
		if (defaultVal != "") {
			$("#legalEntityId_" + count).val(defaultVal);
		}
	}
	else {
		$.ajax({
			async: true,
			type: "POST",
			url: 'GetLegalEntityDropDown',
			dataType: 'json',
			data: {},
			success: function (response) {
				tempLegalEntity = [];
				$("#legalEntityId_" + count).append(`<option value = "" > --Select--</option>`)
				for (var i = 0; i < response.length; i++) {
					$("#legalEntityId_" + count).append(`<option value = "${response[i].value}">${response[i].text}</option>`)
					var obj =
					{
						value: response[i].value,
						text: response[i].text,
					}
					tempLegalEntity.push(obj);
				}
				if (defaultVal != "") {
					$("#legalEntityId_" + count).val(defaultVal);
				}
			},
			error: function (errorTemp) {
				console.log('error', errorTemp);
			}
		})
	}
}
function BindCountryName(count, defaultVal = "") {
	if (tempCountryName.length > 0) {
		for (var i = 0; i < tempCountryName.length; i++) {
			$("#countryId_" + count).append(`<option value = "${tempCountryName[i].value}">${tempCountryName[i].text}</option>`)
		}
		if (defaultVal != "") {
			$("#countryId_" + count).val(defaultVal);
		}
	}
	else {
		$.ajax({
			async: true,
			type: "POST",
			url: 'GetCountryDropDown',
			dataType: 'json',
			data: {},
			success: function (response) {
				tempCountryName = [];
				$("#countryId_" + count).append(`<option value = "" > --Select--</option>`)
				for (var i = 0; i < response.length; i++) {
					$("#countryId_" + count).append(`<option value = "${response[i].value}">${response[i].text}</option>`)
					var obj =
					{
						value: response[i].value,
						text: response[i].text,
					}
					tempCountryName.push(obj);
				}
				if (defaultVal != "") {
					$("#countryId_" + count).val(defaultVal);
				}
			},
			error: function (errorTemp) {
				console.log('error', errorTemp);
			}
		})
	}
}
function BindPositionTypeName(count, defaultVal = "") {
	$.ajax({
		async: true,
		type: "POST",
		url: 'GetPositionTypeDropDown',
		dataType: 'json',
		data: {},
		success: function (response) {
			$("#PositionTypeId_" + count).append(`<option value = "" > --Select--</option>`)
			for (var i = 0; i < response.length; i++) {
				$("#PositionTypeId_" + count).append(`<option value = "${response[i].value}">${response[i].text}</option>`)
			}
			if (defaultVal != "") {
				$("#PositionTypeId_" + count).val(defaultVal);
			}
		},
		error: function (errorTemp) {
			console.log('error', errorTemp);
		}
	})
}
function btnSearchClick() {
	
	var lpFilter = $("#LpId_filter").val();
	var instrumentFilter = $("#instrumentid_filter").val();
	var day = $("#day_filter").val();
	var mainId = $('#Id').val();
	GetTradeRuleInstrumentByRuleId(mainId, instrumentFilter,lpFilter,day);
	//GetRuleInstrumentByRuleId(mainId, lpFilter, instrumentFilter, day);
	$("#Loader").hide();
}
function GetTradeUniversalValues() {
	debugger;
	$.ajax({
		async: true,
		type: "GET",
		url: 'Get_TradeUniversalValues',
		dataType: 'json',
		success: function (response1) {
			for (var i = 0; i < response1.length; i++) {
				var obj =
				{
					Id: response1[i].id,
					Value: response1[i].values,
				}
				TradeRuleUniversalValues.push(obj);
			}
		}
	});
}




function DeleteLpRule(ruleId, rulePriorityId) {
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
					url: 'DeleteTradeRuleInstrument',
					dataType: 'json',
					data: { RuleId: ruleId, RulePriorityId: rulePriorityId },
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

//var countRuleInstrument = 0;
//function GetInstrumentWithPriorityInEditTable(response, values) {
//    debugger
//    var strHTML = "";
//    if (countRuleInstrument == "0") {
//        strHTML += `<thead><tr>`;
//        strHTML += `<th>#</th>`;
//        strHTML += `<th>Instruments</th>`;
//        strHTML += `<th>IB Name</th>`;
//        strHTML += `<th>Legal Entity</th>`;
//        strHTML += `<th>Country</th>`;
//        strHTML += `<th>UID</th>`;
//        strHTML += `<th>Day</th>`;
//        strHTML += `<th>Time to</th>`;
//        strHTML += `<th>Time from</th>`;

//        strHTML += `<th>LP</th>`;
//        strHTML += `<th>HedgeType</th>`;
//        strHTML += `<th>Priority</th>`;
//        strHTML += `<th>Volume From</th>`;
//        strHTML += `<th>Volume To</th>`;
//        strHTML += `<th>Coefficient</th>`;
//        strHTML += `<th>Position type</th>`;
//        strHTML += `<th>Max Coeffient</th>`;
//        for (var cnt = 0; cnt < values.length; cnt++) {
//            strHTML += `<th>${values[cnt].Value}</th>`;
//        }
//        strHTML += `<th>Action</th>`;
//        strHTML += `</tr></thead>`;
//    }
//    strHTML += `<tbody><tr>`;
//    strHTML += `<td>${countRuleInstrument + 1}</td>`;
//    strHTML += `<td>`;
//    strHTML += `<div class="form-group" style="width:130px;">`;
//    strHTML += `<input id="objRuleInstrument_Id_${countRuleInstrument}" type="hidden">`;
//    strHTML += `<input id="objRuleInstrumen_RuleId_${countRuleInstrument}" type="hidden"">`;
//    strHTML += `<input id="RuleInstrumentId_${countRuleInstrument}" type="hidden">`;
//    strHTML += `<input id="TradeRulePriorityId_${countRuleInstrument}" type="hidden">`;
//    //strHTML += `<select class="form-control" id="instrumentid_${countRuleInstrument}" style="border:1px solid;">`;
//    //strHTML += `</select>`;
//    if (response == null || response == 'undefined') {
//        strHTML += `<input id="instrumentid_${countRuleInstrument}" type="hidden">`;
//        strHTML += `<input class="form-control" value="" type="text" disabled="disabled">`;
//    }
//    else {
//        strHTML += `<input id="instrumentid_${countRuleInstrument}" type="hidden">`;
//        strHTML += `<input class="form-control" id="instrumentName_${countRuleInstrument}" value="${response.instrumentName}" type="text" disabled="disabled">`;
//    }
//    strHTML += `</div>`;
//    strHTML += `</td>`;
//    strHTML += `<td>`;
//    strHTML += `<div class="form-group" style="width:130px;">`;
//    strHTML += `<select class="form-control" id="ibName_${countRuleInstrument}" name="IB" disabled="disabled">`;
//    strHTML += `</select>`;
//    strHTML += `</div>`;
//    strHTML += `</td>`;
//    strHTML += `<td>`;
//    strHTML += `<div class="form-group" style="width:130px;">`;
//    strHTML += `<select class="form-control" id="legalEntityName_${countRuleInstrument}" name="LegalEntityId" disabled="disabled">`;
//    strHTML += `</select>`;
//    strHTML += `</div>`;
//    strHTML += `</td>`;
//    strHTML += `<td>`;
//    strHTML += `<div class="form-group" style="width:100px;">`;
//    strHTML += `<select id="countryName_${countRuleInstrument}" class="form-control" disabled="disabled">`;
//    strHTML += `</select>`;
//    strHTML += `</div>`;
//    strHTML += `</td>`;
//    strHTML += `<td>`;
//    strHTML += `<div class="form-group" style="width:100px;">`;
//    strHTML += `<input class="form-control" id="UId_${countRuleInstrument}" placeholder="UID" >`;
//    strHTML += `</div>`;
//    strHTML += `</td>`;
//    strHTML += `<td>`;
//    strHTML += `<div class="form-group" style="width:104px;">`;
//    strHTML += `<input class="form-control" id="Day_${countRuleInstrument}" name="Day">`;
//    strHTML += `</div>`;
//    strHTML += `</td>`;
//    strHTML += `<td>`;
//    strHTML += `<div class="form-group">`;
//    strHTML += `<input class="form-control" id="StartTime_${countRuleInstrument}" type="time" step=2>`;
//    strHTML += `</div>`;
//    strHTML += `</td>`;
//    strHTML += `<td>`;
//    strHTML += `<div class="form-group">`;
//    strHTML += `<input class="form-control" id="EndTime_${countRuleInstrument}" type="time" step=2>`;
//    strHTML += `</div>`;
//    strHTML += `</td>`;
//    strHTML += `<td>`;
//    strHTML += `<div class="form-group">`;
//    strHTML += `<select class="form-control" id="LpId_${countRuleInstrument}" style="width:100px;">`;
//    strHTML += `</select>`;
//    strHTML += `</div>`;
//    strHTML += `</td>`;
//    strHTML += `<td>`;
//    strHTML += `<div class="form-group">`;
//    strHTML += `<input class="form-control"  id="HedgeType_${countRuleInstrument}" placeholder="HedgeType">`;
//    strHTML += `</div>`;
//    strHTML += `</td>`;
//    strHTML += `<td>`;
//    strHTML += `<div class="form-group" style="width:60px;">`;
//    strHTML += `<input class="form-control" id="Priority_${countRuleInstrument}" placeholder="Priority" type="number">`;
//    strHTML += `</div>`;
//    strHTML += `</td>`;
//    strHTML += `<td>`;
//    strHTML += `<div class="form-group" style="width:90px;">`;
//    strHTML += `<input class="form-control" id="VolumeFrom_${countRuleInstrument}" placeholder="VolumeFrom" type="number">`;
//    strHTML += `</div>`;
//    strHTML += `</td>`;
//    strHTML += `<td>`;
//    strHTML += `<div class="form-group" style="width:90px;">`;
//    strHTML += `<input class="form-control" id="VolumeTo_${countRuleInstrument}" placeholder="VolumeTo" type="number">`;
//    strHTML += `</div>`;
//    strHTML += `</td>`;
//    strHTML += `<td>`;
//    strHTML += `<div class="form-group" style="width:90px;">`;
//    strHTML += `<input class="form-control" id="Coefficient_${countRuleInstrument}" placeholder="Coefficient" type="number">`;
//    strHTML += `</div>`;
//    strHTML += `</td>`;
//    strHTML += `<td>`;
//    strHTML += `<div class="form-group" style="width:100px;">`;
//    strHTML += `<select class="form-control" id="PositionTypeId_${countRuleInstrument}" style="width:100px;">`;
//    strHTML += `</select>`;
//    strHTML += `</div>`;
//    strHTML += `</td>`;
//    strHTML += `<td>`;
//    strHTML += `<div class="form-group">`;
//    strHTML += `<input class="form-control" id="MaxCoeffient_${countRuleInstrument}" placeholder="MaxCoeffient" type="number">`;
//    strHTML += `</div>`;
//    strHTML += `</td>`;
//    for (var i = 1; i <= values.length; i++) {
//        strHTML += `<td>`;
//        strHTML += `<div class="form-group" style="width:70px;">`;
//        var lblValId = values[i - 1].Value.trim().replace(" ", "");
//        strHTML += `<input class="form-control" id="${lblValId}_${countRuleInstrument}" placeholder="value${i}" type="number">`;
//        strHTML += `</div>`;
//        strHTML += `</td>`;
//    }
//    strHTML += `<td>`;
//    strHTML += `<div class="form-group">`;
//    strHTML += `<a href="javascript:void(0)" value="Delete" onclick="DeleteLpRule(1,1)" style=" display: block; width: 100%; height: calc(2.25rem + 2px); padding: 0.375rem 0.75rem;">`;
//    strHTML += `<i class="fa fa-trash" style="font-size: large;margin-top: 8px;" aria-hidden="true"></i>`;
//    strHTML += `</a>`;
//    strHTML += `</div>`;
//    strHTML += `</td>`;
//    strHTML += `</tr></tbody>`;

//    $("#tblInstrument").append(strHTML);
//    if ($.fn.DataTable.isDataTable("#tblInstrument")) {
//        $('#tblInstrument').dataTable().fnDestroy();
//        //$('#divUserTable').html('<table class="table table-striped" width="100%" id="UserTable"></table>');
//    }
//    $("#tblInstrument").dataTable({
//        "paging": true,
//        "pageLength": 500,
//        "filter": true
//    });


//    if (response != undefined) {
//        //BindIntruments(countRuleInstrument, response.instrumentId);
//        BindLpRule(countRuleInstrument, response.lpId);
//        BindIBName(countRuleInstrument, response.ibid);
//        BindPositionTypeName(countRuleInstrument, response.positionTypeId);
//        BindCountryName(countRuleInstrument, response.countryId);
//        BindLegalEntityName(countRuleInstrument, response.legalEntityId);
//        $("#objRuleInstrument_Id_" + countRuleInstrument).val(response.id);
//        $("#objRuleInstrumen_RuleId_" + countRuleInstrument).val(response.ruleId);
//        $("#instrumentid_" + countRuleInstrument).val(response.instrumentId);
//        $("#TradeRulePriorityId_" + countRuleInstrument).val(response.tblRulePriority_ID);
//        $("#HedgeType_" + countRuleInstrument).val(response.hedgeType),
//            $("#Day_" + countRuleInstrument).val(response.day);
//        $("#StartTime_" + countRuleInstrument).val(response.startTime.toString('HH:mm ss'));
//        $("#EndTime_" + countRuleInstrument).val(response.endTime.toString('HH:mm ss'));
//        $("#UId_" + countRuleInstrument).val(response.uid);
//        $("#Priority_" + countRuleInstrument).val(response.priority);
//        $("#VolumeFrom_" + countRuleInstrument).val(response.volumeFrom);
//        $("#VolumeTo_" + countRuleInstrument).val(response.volumeTo);
//        $("#Coefficient_" + countRuleInstrument).val(response.coefficient);
//        $("#MaxCoeffient_" + countRuleInstrument).val(response.maxCoefficient);
//        $("#RuleInstrumentId_" + countRuleInstrument).val(response.ruleInstrumentId);
//        for (var i = 1; i <= values.length; i++) {
//            //var valname = response.value+""+i;
//            //$("#value" + i + "_" + countRuleInstrument).val(valname);
//            //alert(valname);
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value1") {
//                $("#Value1_" + countRuleInstrument).val(response.value1);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value2") {
//                $("#Value2_" + countRuleInstrument).val(response.value2);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value3") {
//                $("#Value3_" + countRuleInstrument).val(response.value3);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value4") {
//                $("#Value4_" + countRuleInstrument).val(response.value4);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value5") {
//                $("#Value5_" + countRuleInstrument).val(response.value5);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value6") {
//                $("#Value6_" + countRuleInstrument).val(response.value6);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value7") {
//                $("#Value7_" + countRuleInstrument).val(response.value7);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value8") {
//                $("#Value8_" + countRuleInstrument).val(response.value8);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value9") {
//                $("#Value9_" + countRuleInstrument).val(response.value9);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value10") {
//                $("#Value10_" + countRuleInstrument).val(response.value10);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value11") {
//                $("#Value11_" + countRuleInstrument).val(response.value11);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value12") {
//                $("#Value12_" + countRuleInstrument).val(response.value12);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value13") {
//                $("#Value13_" + countRuleInstrument).val(response.value13);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value14") {
//                $("#Value14_" + countRuleInstrument).val(response.value14);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value15") {
//                $("#Value15_" + countRuleInstrument).val(response.value15);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value16") {
//                $("#Value16_" + countRuleInstrument).val(response.value16);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value17") {
//                $("#Value17_" + countRuleInstrument).val(response.value17);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value18") {
//                $("#Value18_" + countRuleInstrument).val(response.value18);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value19") {
//                $("#Value19_" + countRuleInstrument).val(response.value19);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value20") {
//                $("#Value20_" + countRuleInstrument).val(response.value20);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value21") {
//                $("#Value21_" + countRuleInstrument).val(response.value21);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value22") {
//                $("#Value22_" + countRuleInstrument).val(response.value22);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value23") {
//                $("#Value23_" + countRuleInstrument).val(response.value23);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value24") {
//                $("#Value24_" + countRuleInstrument).val(response.value24);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value25") {
//                $("#Value25_" + countRuleInstrument).val(response.value25);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value26") {
//                $("#Value26_" + countRuleInstrument).val(response.value26);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value27") {
//                $("#Value27_" + countRuleInstrument).val(response.value27);
//            }
//            if (values[i - 1].Value.trim().replace(" ", "") == "Value28") {
//                $("#Value28_" + countRuleInstrument).val(response.value28);
//            }
//        }


//        //$("#value23_" + countRuleInstrument).val(response.value23);
//        //$("#value24_" + countRuleInstrument).val(response.value24);
//        //$("#value25_" + countRuleInstrument).val(response.value25);
//        //$("#value26_" + countRuleInstrument).val(response.value26);
//        //$("#value27_" + countRuleInstrument).val(response.value27);
//        //$("#value28_" + countRuleInstrument).val(response.value28);
//    }
//    else {
//        debugger;
//        //BindIntruments(countRuleInstrument);
//        BindLpRule(countRuleInstrument);
//        BindIBName(countRuleInstrument);
//        BindPositionTypeName(countRuleInstrument);
//        BindCountryName(countRuleInstrument);
//        BindLegalEntityName(countRuleInstrument);
//    }
//    countRuleInstrument = countRuleInstrument + 1;
//}
function AddInstrumentsInEditTable(values) {
	debugger
	var table = $('#tblInstrument').DataTable();
	var table_length = table.data().count();
	countRuleInstrument = table_length + 1;
	
	var strHTML = "";
	//if (countRuleInstrument == "0") {
	//    strHTML += `<tr> `;
	//    strHTML += `<th>#</th>`;
	//    strHTML += `<th>Instruments</th>`;
	//    strHTML += `<th>IB Name</th>`;
	//    strHTML += `<th>Legal Entity</th>`;
	//    strHTML += `<th>Country</th>`;
	//    strHTML += `<th>UID</th>`;
	//    strHTML += `<th>Day</th>`;
	//    strHTML += `<th>Time to</th>`;
	//    strHTML += `<th>Time from</th>`;

	//    strHTML += `<th>LP</th>`;
	//    strHTML += `<th>HedgeType</th>`;
	//    strHTML += `<th>Priority</th>`;
	//    strHTML += `<th>Volume From</th>`;
	//    strHTML += `<th>Volume To</th>`;
	//    strHTML += `<th>Coefficient</th>`;
	//    strHTML += `<th>Position type</th>`;
	//    strHTML += `<th>Max Coeffient</th>`;
	//    for (var cnt = 0; cnt < values.length; cnt++) {
	//        strHTML += `<th>${values[cnt].Value}</th>`;
	//    }
	//    strHTML += `<th>Action</th>`;
	//    strHTML += `</tr>`;
	//}
	strHTML += `<tr>`;
	  /*  strHTML += `<td>${countRuleInstrument + 1}</td>`;*/
	strHTML += `<td>`;
	strHTML += `<div class="form-group" style="width:100px;">`;
	strHTML += `<input id="objRuleInstrument_Id_${countRuleInstrument}" type="hidden">`;
	strHTML += `<input id="objRuleInstrumen_RuleId_${countRuleInstrument}" type="hidden"">`;
	strHTML += `<input id="RuleInstrumentId_${countRuleInstrument}" type="hidden">`;
	strHTML += `<input id="TradeRulePriorityId_${countRuleInstrument}" type="hidden">`;
	strHTML += `<select class="form-control" id="instrumentid_${countRuleInstrument}" style="border:1px solid;">`;
	strHTML += `</select>`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td>`;
	strHTML += `<div class="form-group" style="width:100px;">`;
	strHTML += `<select class="form-control" id="ibid_${countRuleInstrument}" name="IB">`;
	strHTML += `</select>`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td>`;
	strHTML += `<div class="form-group" style="width:100px;">`;
	strHTML += `<select class="form-control" id="legalEntityId_${countRuleInstrument}" name="LegalEntityId">`;
	strHTML += `</select>`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td>`;
	strHTML += `<div class="form-group" style="width:100px;">`;
	strHTML += `<select id="countryId_${countRuleInstrument}" class="form-control">`;
	strHTML += `</select>`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td>`;
	strHTML += `<div class="form-group" style="width:100px;">`;
	strHTML += `<input class="form-control" id="UId_${countRuleInstrument}" placeholder="UID" >`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td>`;
	strHTML += `<div class="form-group" style="width:100px;">`;
	strHTML += `<input class="form-control" id="Day_${countRuleInstrument}" name="Day">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td>`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="StartTime_${countRuleInstrument}" type="time" step=2 style="width:100px;">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td>`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="EndTime_${countRuleInstrument}" type="time" step=2 style="width:100px;">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td>`;
	strHTML += `<div class="form-group">`;
	strHTML += `<select class="form-control" id="lpId_${countRuleInstrument}" style="width:100px;">`;
	strHTML += `</select>`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td>`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="HedgeType_${countRuleInstrument}" placeholder="HedgeType" style="width:100px;">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td>`;
	strHTML += `<div class="form-group" style="width:100px;">`;
	strHTML += `<input class="form-control" id="Priority_${countRuleInstrument}" placeholder="Priority" type="number">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td>`;
	strHTML += `<div class="form-group" style="width:100px;">`;
	strHTML += `<input class="form-control" id="VolumeFrom_${countRuleInstrument}" placeholder="VolumeFrom" type="number">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td>`;
	strHTML += `<div class="form-group" style="width:100px;">`;
	strHTML += `<input class="form-control" id="VolumeTo_${countRuleInstrument}" placeholder="VolumeTo" type="number">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `<td>`;
	strHTML += `<div class="form-group" style="width:100px;">`;
	strHTML += `<input class="form-control" id="Coefficient_${countRuleInstrument}" placeholder="Coefficient" type="number">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	//strHTML += `<td>`;
	//strHTML += `<div class="form-group" style="width:100px;">`;
	//strHTML += `<select class="form-control" id="PositionTypeId_${countRuleInstrument}" style="width:100px;">`;
	//strHTML += `</select>`;
	//strHTML += `</div>`;
	//strHTML += `</td>`;
	strHTML += `<td>`;
	strHTML += `<div class="form-group">`;
	strHTML += `<input class="form-control" id="MaxCoeffient_${countRuleInstrument}" placeholder="MaxCoeffient" type="number" style="width:100px;">`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	for (var i = 1; i <= values.length; i++) {
		strHTML += `<td>`;
		strHTML += `<div class="form-group" style="width:100px;">`;
		var lblValId = values[i - 1].Value.trim().replace(" ", "");
		strHTML += `<input class="form-control" id="${lblValId}_${countRuleInstrument}" placeholder="value${i}" type="number">`;
		strHTML += `</div>`;
		strHTML += `</td>`;
	}
	strHTML += `<td>`;
	strHTML += `<div class="form-group">`;
	strHTML += `<a href="javascript:void(0)" value="Delete" onclick="DeleteLpRule(1,1)" style=" display: block; width: 100%; height: calc(2.25rem + 2px); padding: 0.375rem 0.75rem;">`;
	strHTML += `<i class="fa fa-trash" style="font-size: large;margin-top: 8px;" aria-hidden="true"></i>`;
	strHTML += `</a>`;
	strHTML += `</div>`;
	strHTML += `</td>`;
	strHTML += `</tr>`;

	$("#tblInstrument").append(strHTML);

	debugger;
	BindIntruments(countRuleInstrument);
	BindLpRule(countRuleInstrument);
	BindIBName(countRuleInstrument);
	BindPositionTypeName(countRuleInstrument);
	BindCountryName(countRuleInstrument);
	BindLegalEntityName(countRuleInstrument);

	//countRuleInstrument = countRuleInstrument + 1;
}
function SaveTradeRule() {
	debugger;
	SaveRulesCondiDtl();
	SaveInstrumentDataDtl();
	var formData = new FormData();
	formData.append("Id", $("#Id").val());
	formData.append("Name", $("#Name").val());
	formData.append("Priority", $("#Priority").val());
	formData.append("objRuleConditionViewModelstr", $("#rulesCondtionData").val());
	formData.append("objRuleInstrumentViewModelstr", $("#instrumentData").val());
	$.ajax({
		type: "POST",
		url: 'SaveTradeRule',
		data: formData,
		contentType: false,
		processData: false,
		success: function (res) {
			if (res.statusCode == 200) {
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
	debugger
	IsValid = true;
	var rulesInstrument = [];
	//var total_cnt = document.getElementById("myTable").rows.length;
	for (var i = 1; i <= countRuleInstrument; i++) {
		if (($("#instrumentid_" + i).val() != "") && ($("#instrumentid_" + i).val() != null)) {
			var obj = {
				tblRulePriority_ID: $("#TradeRulePriorityId_" + i).val() == "" ? 0 : $("#TradeRulePriorityId_" + i).val(),
				RuleId: $("#Id").val(),
				Id: $("#RuleInstrumentId_" + i).val() == "" ? 0 : $("#RuleInstrumentId_" + i).val(),

				HedgeType: $("#HedgeType_" + i).val(),
				//VolumeFrom: $("#VolumeFrom_" + i).val(),
				//VolumeTo: $("#VolumeTo_" + i).val(),
				//Coefficient: $("#Coefficient_" + i).val(),
				MaxCoefficient: $("#MaxCoeffient_" + i).val(),
				Value1: $("#Value1_" + i).val(),
				Value2: $("#Value2_" + i).val(),
				Value3: $("#Value3_" + i).val(),
				Value4: $("#Value4_" + i).val(),
				Value5: $("#Value5_" + i).val(),
				Value6: $("#Value6_" + i).val(),
				Value7: $("#Value7_" + i).val(),
				Value8: $("#Value8_" + i).val(),
				Value9: $("#Value9_" + i).val(),
				Value10: $("#Value10_" + i).val(),
				Value11: $("#Value11_" + i).val(),
				Value12: $("#Value12_" + i).val(),
				Value13: $("#Value13_" + i).val(),
				Value14: $("#Value14_" + i).val(),
				Value15: $("#Value15_" + i).val(),
				Value16: $("#Value16_" + i).val(),
				Value17: $("#Value17_" + i).val(),
				Value18: $("#Value18_" + i).val(),
				Value19: $("#Value19_" + i).val(),
				Value20: $("#Value20_" + i).val(),
				Value21: $("#Value21_" + i).val(),
				Value22: $("#Value22_" + i).val(),

				InstrumentId: $("#instrumentid_" + i).val(),
				IBId: $("#ibid_" + i).val(),
				LegalEntityId: $("#legalEntityId_" + i).val(),
				CountryId: $("#countryId_" + i).val(),
				UId: $("#UId_" + i).val(),
				Day: $("#Day_" + i).val(),
				StartTime: $("#StartTime_" + i).val(),
				EndTime: $("#EndTime_" + i).val(),
				LPId: $("#lpId_" + i).val(),
				Priority: $("#Priority_" + i).val(),
				VolumeFrom: $("#VolumeFrom_" + i).val(),
				VolumeTo: $("#VolumeTo_" + i).val(),
				Coefficient: $("#Coefficient_" + i).val(),
				PositionTypeId: $("#PositionTypeId_" + i).val(),

				//InstrumentName: $("#instrumentName_" + i).val(),
				//LPName: $("#LpId_" + i + " option:selected").text(),
				//IB: $("#ib_" + i + " option:selected").text(),
				//LegalEntity: $("#LegalEntityId_" + i + " option:selected").text(),
				//Country: $("#CountryId_" + i + " option:selected").text(),
				//PositionType: $("#PositionTypeId_" + i + " option:selected").text(),

				InstrumentName: "",
				LPName: "",
				IB:"",
				LegalEntity: "",
				Country: "",
				PositionType: "",
			}
			rulesInstrument.push(obj);
		}
	}
	$("#instrumentData").val(JSON.stringify(rulesInstrument));
}
function ExportTradeRuleOld() {
	//SaveRulesCondiDtl();
	SaveInstrumentDataDtl();
	var formData = new FormData();
	formData.append("Id", $("#Id").val());
	formData.append("objRuleInstrumentViewModelstr", $("#instrumentData").val());
	$.ajax({
		type: "POST",
		url: 'ExportTradeRule',
		data: formData,
		contentType: false,
		processData: false,
		success: function (res) {
			if (res.statusCode == 200) {
				//$("#frmModel").modal('hide');
				//$("#rulesDetailsData").resetForm();
				NotifyMsg('success', res.message);

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
function ImportTradRuleEdit(RuleId) {

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