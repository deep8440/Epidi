
function AddModal(url, title) {
	debugger;
    $.ajax({
        url: url,
        Type: 'get',
        dataType: 'html',
		async: false,
		success: function (result) {
			debugger
            if (result != "undefined") {
                $("#frmModel .modal-body").html(result);
                $("#frmModel .modal-title").text(title);
                $("#frmModel").modal({ backdrop: 'static', keyboard: false });
                $("#frmModel").focus();

            }
        },
        error: function (result) {
            alert("Error");
        }
    });
}


const Toast = Swal.mixin({
	toast: true,
	position: 'top-end',
	showConfirmButton: false,
	timer: 3000
});

function NotifyMsg(msgType, message) {
	Toast.fire({
		icon: msgType,
		title: " " + message
	})
}

$(document).ready(function () {

    $("body").on("click", ".edt-rcd", function (e) {        
		e.preventDefault();
		var uri = $(this).attr('data-uri');
		var header = $(this).closest('table').attr('edit-header');
		$.get(uri, function (data) {
			$("#frmModel .modal-body").html(data);
            $("#frmModel .modal-title").html(header);
            $("#frmModel").modal('show');
		}).fail(function () {
			//NotifyMsg("getting errors", "warning");
		});
	});

	

	$("body").on("click", ".edt-dlt", function (e) {
		debugger;
		e.preventDefault();
		var currentLink = $(this);
		var datatableId = $(this).closest('table').attr('id');
		Swal.fire({
			title: 'Are you sure?',
			text: "You won't be able to revert this!",
			icon: 'warning',
			showCancelButton: true,
			confirmButtonColor: '#3085d6',
			cancelButtonColor: '#d33',
			confirmButtonText: 'Yes, delete it!'
		}).then((result) => {

			if (result != undefined && result.value) {

				var uri = currentLink.attr('data-uri');
				$.ajax({
					url: uri,
					type: "GET",
					dataType: "json",
					traditional: true,
					async: false,
					contentType: "application/json; charset=utf-8",
					success: function (res) {
						debugger;
						if (res[0].code == 200) {
							Swal.fire(
								'Deleted!',
								'Your file has been deleted.',
								'success'
							)
							debugger;
							var dataTableId = currentLink.closest("table").attr('id');
							if (dataTableId == "answerList") {

								currentLink.closest('tr').remove();
							}
							else {
								var table = currentLink.closest("#" + datatableId).DataTable();
								table.row(currentLink.closest("tr")).remove().draw();
							}
							//RefreshDataTable
							//NotifyMsg("this record has been deleted.", "success");
						}
						else {
							//NotifyMsg("this record didn't deleted.", "warning");
						}
					},
					error: function (data) {
						//if (data.responseText.indexOf('processLogin') > -1 && data.responseText.indexOf('forgetpassword') > -1) {
						//	NotifyMsg("Your Session has been expired.", "warning");
						//	window.location.href = "/Login";
						//}
						//else {
						//	NotifyMsg(data.responseText, "warning");
						//}
					}
				});
			}
		})
	});

})