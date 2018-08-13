function showSnackBar(message) {
	var x = document.getElementById("snackbar");
	x.innerHTML = message;
	x.className = "show";
	setTimeout(function () {x.className = x.className.replace("show", ""); }, 3000);
}

function reloadPage(path) {
	$.ajax({
		url: path,
		success: function (data) {
			$('#content').html(data);
		},
		error: function (xhr, ajaxOptions, thrownError) {
			alert(xhr.responseText);
		}
	});
}

function showModal(path) {
	$.ajax({
		url: path,
		success: function (data) {
			$('#modalContainer').html(data);
			$('#modal').modal('show');
		},
		error: function (xhr, ajaxOptions, thrownError) {
			alert(xhr.responseText);
		}
	});
}

function reloadWithData(path, id, message) {
	var dt = $(id).serialize();
	$.ajax({
		url: path,
		type: "POST",
		data: dt,
		success: function (data) {
			$("#content").html(data);
			if ($(data)[0].id !== "newPlan") {
				showSnackBar(message);
			}
		},
		error: function (request, status, error) {
			console.log(request.responseText);
		}
	});
}

function hideModal(path, id, message) {
	var dt = $(id).serialize();
	$.ajax({
		url: path,
		type: "POST",
		data: dt,
		success: function (data) {
			if ($(data)[0].className === "modal-dialog") {
				$("#modalContainer").html(data);
			} else if ($(data)[0].className === "inner") {
				$('#modal').modal('hide');
				$('#modalContainer').removeData();
				$('.modal-backdrop').remove();
				$("#content").html(data);
				if (message !== null) {
					showSnackBar(message);
				}
			}
		},
		error: function (request, status, error) {
			console.log(request.responseText);
		}
	});
}


function ValidateNumber(e) {
	var evt = (e) ? e : window.event;
	var charCode = (evt.keyCode) ? evt.keyCode : evt.which;
	if (charCode > 31 && (charCode < 48 || charCode > 57)) {
		return false;
	}
	return true;
};

$(document).ready(function () {
	$(document).on("change", "#godPlanoviChange", (function () {
		$.ajax({
			url: "/MjesecniPlan/Index?Plan=" + $(this).val(),
			success: function (data) {
				$("#content").html(data);
			}
		});
	}));
});

$(document).ready(function () {
	$(document).on("change", "#godPlanoviChangeSS", (function () {
		$.ajax({
			url: "/PlanSs/Index?Plan=" + $(this).val(),
			success: function (data) {
				$("#content").html(data);
			}
		});
	}));
});
