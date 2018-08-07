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

function hideModal(path, id, message) {
	var dt = $("#" + id).serialize();
	$.ajax({
		url: path,
		type: "POST",
		data: dt,
		success: function (data) {
			if ($(data)[0].className == "modal-dialog") {
				$("#modal").html(data);
			} else if ($(data)[0].className == "inner") {
				$('#modal').modal('hide');
				$('#modalContainer').html("");
				$('.modal-backdrop').remove();
				$("#content").html(data);
				showSnackBar(message);
			}
		},
		error: function (request, status, error) {
			console.log(request.responseText);
		}
	});
}

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
