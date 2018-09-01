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
			if ($(data)[1].tagName === "META") {
				location.reload();
			} else {
				$('#content').html(data);
			}
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
			if ($(data)[1].tagName === "META") {
				location.reload();
			} else {
				$('#modalContainer').html(data);
				$('#modal').modal('show');
			}
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
			if ($(data)[1].tagName === "META") {
				location.reload();
			} else {
				$("#content").html(data);
				if ($(data)[0].id !== "newPlan") {
					showSnackBar(message);
				}
			}
		},
		error: function (request, status, error) {
			console.log(request.responseText);
		}
	});
}

function hideModalA(event, path, id, message) {
	if (event.type === "keydown") {
		if (event.keyCode === 13) {
			event.preventDefault();
			hideModal(path, id, message);
		}
	}
}

function hideModal(path, id, message) {
	var dt = $(id).serialize();
	$.ajax({
		url: path,
		type: "POST",
		data: dt,
		success: function (data) {
			if ($(data)[1].tagName === "META") {
				location.reload();
			} else {
				if ($(data)[0].className === "modal-dialog") {
					$("#modalContainer").html(data);
					$('#newName').focus();
				} else if ($(data)[0].className === "inner") {
					$('#modal').modal('hide');
					$('#modalContainer').removeData();
					$('.modal-backdrop').remove();
					$("#content").html(data);
					if (message !== null) {
						showSnackBar(message);
					}
				}
			}
		},
		error: function (request, status, error) {
			console.log(request.responseText);
		}
	});
}

function appendText(source, destination) {
	var dropDown = document.getElementById(source);
	var sourceText = " - " + dropDown.options[dropDown.selectedIndex].text;
	if (dropDown.options[dropDown.selectedIndex].value !== "") {
		var destinationText = document.getElementById(destination).value;
		var lines = destinationText.split('\n');

		var LineIndex = lines.indexOf(sourceText);
		if (LineIndex === -1) {
			lines.push(sourceText);
		}
		if (lines[0] === "") {
			lines.shift();
		}
		var output = lines.join("\n");
		document.getElementById(destination).value = output;
	}
}

function removeText(source, destination) {
	var dropDown = document.getElementById(source);
	var sourceText = " - " + dropDown.options[dropDown.selectedIndex].text;
	var destinationText = document.getElementById(destination).value;
	var lines = destinationText.split('\n');

	var LineIndex = lines.indexOf(sourceText);
	if (LineIndex !== -1) {
		lines.splice(LineIndex, 1);
	}
	var output = lines.join("\n");
	
	if (dropDown.options[dropDown.selectedIndex].value !== "") {
		document.getElementById(destination).value = output;
	}
}

$(document).ready(function () {
	$(document).on("change", "#godPlanoviChange", function () {
		$.ajax({
			url: "/MjesecniPlan/Index?Plan=" + $(this).val(),
			success: function (data) {
				$("#content").html(data);
			}
		});
	});

	$(document).on("change", "#godPlanoviChangeSS", function () {
		$.ajax({
			url: "/PlanSs/Index?Plan=" + $(this).val(),
			success: function (data) {
				$("#content").html(data);
			}
		});
	});
});
