
var data = '';
var dynamicColumns = [];
var selectedData = [];

//var IOInstrument = JSON.stringify(@ViewBag.IoInstrument)
$(document).ready(() => {
	//debugger;
	

	$.ajax({
		type: "GET",
		async: false,
		url: '/InstrumentMapping/GetAllLP',
		success: function (data) {
			
			if (data != null) {
				var dynamicColumn = {};
				dynamicColumn['data'] = "name";
				dynamicColumn['title'] = "Instrument Name";
				dynamicColumn['render'] = function (data, type, row) { let NameHtml = `<p>${row.name}</p>`; return NameHtml; };
				dynamicColumn['orderable'] = false;
				dynamicColumn['width'] = "3%";
				dynamicColumns.push(dynamicColumn);

				for (var i = 0; i < data.length; i++) {
					dynamicColumn = {};
					var lpName = data[i].lpName;
					
					dynamicColumn['data'] = "name";
					dynamicColumn['title'] = "" + data[i].lpName + "";
					dynamicColumn['render'] = (function (lpName) {
						return function (data, type, row) {
							return bindDropdown(row.id, "dropDownChange", lpName);
						};
					})(lpName);
					dynamicColumn['orderable'] = false;
					dynamicColumn['width'] = "3%";
					
					dynamicColumns.push(dynamicColumn);

				}
			}
			
		}
	});
	




	fetchInstrumentGrid.init();
	//$(document).on('search.dt', '#instrumentMapping', function () {

	//});

});
var fetchInstrumentGrid = function () {
	let initTable1 = function () {
		let table = $('#instrumentMapping');

		let oTable = table.dataTable({
			"stateSave": true,
			"autoWidth": false,
			"paging": true,
			// Internationalisation. For more info refer to http://datatables.net/manual/i18n
			"language": {
				"aria": {
					"sortAscending": ": activate to sort column ascending",
					"sortDescending": ": activate to sort column descending"
				},
				"emptyTable": "No data available in table",
				"info": "Showing _START_ to _END_ of _TOTAL_ entries",
				"infoEmpty": "No entries found",
				"infoFiltered": "(filtered1 from MAX total entries)",
				//"lengthMenu": "MENU entries",
				"search": "Search:",
				"zeroRecords": "No matching records found"
			},
			"pageLength": 10,
			"processing": true,
			"serverSide": true,
			"ajax": {
				async: false,
				cache: false,
				url: '/InstrumentMapping/All_Instrument',
				type: "post",
			},
			"columns": dynamicColumns,
			//"columns": [
			//	{
			//		"title": "Instrument Name", "data": "name",
			//		"render": function (data, type, row) {
			//			let NameHtml = `<p>${row.name}</p>`;
			//			return NameHtml;
			//		}
			//		, "orderable": false, "width": "3%"
			//	},
			//	{
			//		"title": "GateIO", "data": "name",
			//		"render": function (data, type, row) {
			//			debugger
			//			return bindDropdown(row.id, "dropDownChange", "gateIOInstrument");

			//		}
			//		, "orderable": false, "width": "3%"
			//	},
			//	{
			//		"title": "LMAX", "data": "name",
			//		"render": function (data, type, row) {
			//			return bindDropdown(row.id, "dropDownChange", "lmaxInstrument");
			//		}
			//		, "orderable": false, "width": "3%"
			//	}
			//],

			// setup responsive extension: http://datatables.net/extensions/responsive/
			responsive: true,

			"lengthMenu": [
				[5, 10, 15, 20],
				[5, 10, 15, 20] // change per page values here
			],
			// set the initial value

			//"dom": "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>", // horizobtal scrollable datatable

			// Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
			// setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/plugins/datatables/plugins/bootstrap/dataTables.bootstrap.js).
			// So when dropdowns used the scrollable div should be removed.
			//"dom": "<'row' <'col-md-12'T>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r>t<'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
		});
	}

	return {
		init: function () {
			if ($.fn.DataTable.isDataTable("#instrumentMapping")) {
				$('#instrumentMapping').dataTable().fnDestroy();
				//$('#divUserTable').html('<table class="table table-striped" width="100%" id="UserTable"></table>');
			}
			initTable1();
		}
	};
}();

const bindDropdown = (instrumentMasterId, changeFunction, lpName) => {
	
	let returnHtml = ``;
	let dynamicId = lpName + instrumentMasterId;
	returnHtml = `<select id="${dynamicId}" class="form-control js-select2" onchange="${changeFunction}(${instrumentMasterId},this,'${lpName}')">`;

	let index = -1;
	
	
	//for (let i = 0; i < selectedData.length; i++) {
	//	if (instrumentMasterId == selectedData[i].master_id) {
	//		if (instrumentName == "gateIOInstrument") {
	//			index = selectedData[i].GateIO;
	//		}
	//		if (instrumentName == "lmaxInstrument") {
	//			index = selectedData[i].Lmax;
	//		}
	//	}
	//}


	returnHtml += `<option value="0">---Select ${lpName} Instrument ---</option>`;
		var GateIOId = 0;
		var LMaxId = 0;
		$.ajax({
			type: "POST",
			async: false,
			url: '/InstrumentMapping/GetInstrumetntMappingDD',
			data: { MasterInstrumentId: instrumentMasterId, LPName: lpName },
			success: function (data) {
				if (data != null) {
					for (var i = 0; i < data.length; i++) {

						if (data[i].isSelected) {
							returnHtml += `<option selected value="${data[i].id}">${data[i].symbol}</option>`;
						}
						else {
							returnHtml += `<option value="${data[i].id}">${data[i].symbol}</option>`;
						}
					}
				}

			}
		});

		//for (const element of ioInstrument) {
		//	if (GateIOId != 0) {
		//		if (GateIOId == element.id) {
		//			returnHtml += `<option ${index == element.id ? "selected" : "selected"} value="${element.id}">${element.name}</option>`;
		//		}
		//		else {
		//			returnHtml += `<option ${index == element.id ? "selected" : ""} value="${element.id}">${element.name}</option>`;

		//		}
		//	}
		//	else {
		//		returnHtml += `<option ${index == element.id ? "selected" : ""} value="${element.id}">${element.name}</option>`;
		//	}
		//}
	
	//if (instrumentName == "lmaxInstrument") {
	//	returnHtml += `<option selected value="0">---Select L-Max Instrument ---</option>`;
	//	$.ajax({
	//		type: "POST",
	//		async: false,
	//		url: '/InstrumentMapping/GetInsertMappingDDSelected',
	//		data: { MasterInstrumentId: instrumentMasterId, LPId: 2, LPInstrumentId: 0 },
	//		success: function (data) {
	//			if (data > 0) {
	//				LMaxId = data;
	//			}

	//		}
	//	});
	//	for (const element of lmaxInstrument) {
	//		if (LMaxId != 0) {
	//			if (LMaxId == element.id) {
	//				returnHtml += `<option ${index == element.id ? "selected" : "selected"} value="${element.id}">${element.symbol}</option>`;
	//			}
	//			else {
	//				returnHtml += `<option ${index == element.id ? "selected" : ""} value="${element.id}">${element.symbol}</option>`;
	//			}
	//		}
	//		else {
	//			returnHtml += `<option ${index == element.id ? "selected" : ""} value="${element.id}">${element.symbol}</option>`;
	//		}
	//	}
	//}
	returnHtml += `</select>`;
	/*var select1 = $("#" + dynamicId).val();*/
	return returnHtml;
}

const saveInstrumentMapping = () => {
	//debugger
	
	if (selectedData.length == 0) {
		
		alert("please select the data");
		$("#Loader").hide()
		
	} else {

		$("#Loader").show()

		$.ajax({
			type: "POST",
			url: "/InstrumentMapping/AddInsertMapping",
			data: JSON.stringify(selectedData),
			contentType: "application/json; charset=utf-8",
			dataType: "json",
			//async: false,
			success: function (response) {
				selectedData = [];
				NotifyMsg('success', "Instrument saved successfully");
				//fetchInstrumentGrid.init();
				location.reload();
				$("#Loader").hide();
			},
			error: function (error) {
				selectedData = [];
				fetchInstrumentGrid.init();
				alert("Error");
				location.reload();
			}
		});
	}
}
const dropDownChange = (id, object, eventInstrument) => {
	//debugger

	let objects = { master_id: id, lpName: eventInstrument, instrumentId: object.value };
	selectedData.push(objects);

	//let index = -1;
	//for (let i = 0; i < selectedData.length; i++) {
	//	if (selectedData[i].master_id == id) {
	//		index = i;
	//		break;
	//	}
	//}
	//let gateIO = 0, lMax = 0;

	//if (eventInstrument == "gateIOInstrument") {
	//	gateIO = parseInt($(object).val());
	//}
	//if (eventInstrument == "lmaxInstrument") {
	//	lMax = parseInt($(object).val());
	//}
	//if (index == -1) {
	//	let objects = { master_id: id, GateIO: gateIO, Lmax: lMax, id: 0 };
	//	selectedData.push(objects);
	//}
	//else if (eventInstrument == "gateIOInstrument") {
	//	selectedData[index].GateIO = gateIO;
	//}
	//else if (eventInstrument == "lmaxInstrument") {
	//	selectedData[index].Lmax = lMax;
	//}
	//if (index == -1) {
	//	index = selectedData.length - 1;
	//	if (selectedData[index].Lmax == 0 && selectedData[index].GateIO == 0) {
	//		selectedData.splice(index, 1);
	//	}

	//} else {
	//	if (selectedData[index].Lmax == 0 && selectedData[index].GateIO == 0) {
	//		selectedData.splice(index, 1);
	//	}
	//}

}

