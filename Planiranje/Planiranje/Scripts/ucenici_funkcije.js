$(document).ready(function () {
    var akt = $("#selectAktivnost").val();
    if (window.sessionStorage.getItem(akt + "_god") != null) {
        var saved = window.sessionStorage.getItem(akt + "_god");
        $("#selectGodina").val(saved);
        promjenaGodine();
    }
});
function promjenaGodine() {
    var val = $("#selectGodina").val();
    if (val != "0") {
        $.ajax({
            url: '/PracenjeUcenika/OdabirRazreda?godina=' + val,
            success: function (data) {
                $("#razredi").html(data);
                var akt = $("#selectAktivnost").val();
                var saved = window.sessionStorage.getItem(akt + "_god");
                if (saved != null && saved == $("#selectGodina").val()) {
                    var saved1 = window.sessionStorage.getItem(akt + "_raz");
                    if (saved1 != null) {
                        $("#selectRazred").val(saved1);
                        promjenaRazreda();
                    }
                }
            },
            error: function (request, status, error) {
                showSnackBar("Dogodila se greška prilikom obrađivanja Vašeg zahtjeva");
            }
        });
    }
    else {
        $("#razredi").empty();
    }
    $("#tablica").empty();
    $("#detalji").empty();
}
function promjenaRazreda() {
    var val = $("#selectRazred").val();
    var god = $("#selectGodina").val();
    var akt = $("#selectAktivnost").val();
    
    if (val != "0") {
        $.ajax({
            url: '/PracenjeUcenika/OdabirUcenika?razred=' + val + '&godina=' + god,
            success: function (data) {
                $("#tablica").html(data);
                $("#dataTable").dataTable();
                var saved = window.sessionStorage.getItem(akt + "_god");
                var saved1 = window.sessionStorage.getItem(akt + "_raz");
                var id = window.sessionStorage.getItem(akt + "_id");
                if (saved != null && saved1 != null) {
                    if (saved == god && saved1 == val && id != null) {
                        pokaziDetalje(id);
                    }
                }
            },
            error: function (request, status, error) {
                showSnackBar("Dogodila se greška prilikom obrađivanja Vašeg zahtjeva");
            }
        });
        window.sessionStorage.setItem(akt + "_god", god);
        window.sessionStorage.setItem(akt + "_raz", val);
    }

    else {
        $("#tablica").empty();
        $("#detalji").empty();
    }
}
function pokaziDetalje(id) {
    var val = $("#selectGodina").val();
    var akt = $("#selectAktivnost").val();
    window.sessionStorage.setItem("UC_idUcenik", id);
    window.sessionStorage.setItem(akt + "_id", id);
    $.ajax({
        url: akt+'/Detalji?id=' + id + '&godina=' + val,
        success: function (data) {
            $("#detalji").html(data);
        },
        error: function (request, status, error) {
            showSnackBar("Dogodila se greška prilikom obrađivanja Vašeg zahtjeva");
        }
    });
}
function potvrdi(path, id, poruka) {
    var dt = $(id).serialize();
    $.ajax({
        url: path,
        type: "POST",
        data: dt,
        success: function (data) {
            showSnackBar(poruka);
        },
        error: function (request, status, error) {
            showSnackBar("Dogodila se greška prilikom obrađivanja Vašeg zahtjeva");
        }
    });
}
function odustani(path, id) {
    $.ajax({
        url: path,
        success: function (data) {
            $(id).html(data);
        },
        error: function (request, status, error) {
            showSnackBar("Dogodila se greška prilikom obrađivanja Vašeg zahtjeva");
        }
    });
}
function zatvoriModal(path, id, spremi, poruka) {
    var dt = $(id).serialize();
    $.ajax({
        url: path,
        type: "POST",
        data: dt,
        success: function (data) {
            if ($(data)[0].tagName === "P") {
                $(spremi).html(data);
                $('#modal').modal('hide');
                $('#modalContainer').removeData();
                $('.modal-backdrop').remove();
                showSnackBar(poruka);
            }
            else if ($(data)[0].tagName === "DIV") {
                $("#modalContainer").html(data);
            }
            else {
                location.reload();
            }
        },
        error: function (request, status, error) {
            showSnackBar("Dogodila se greška prilikom obrađivanja Vašeg zahtjeva");
        }
    });
}
function promjenaAktivnost() {
    var val = $("#selectAktivnost").val();
    if (val == "/PopisUcenika" || $("#selectGodina").val() == "0" || !$("#selectRazred").length || $("#selectRazred").val()=="0") {
        window.sessionStorage.setItem("UC_select", val);
        reloadPage(val + "/Index");
    }
    else if (/*$("#selectRazred").length==0 &&*/ $("#selectRazred").val() != "0") {        
        var id = window.sessionStorage.getItem("UC_idUcenik");
        pokaziDetalje(id);
    }
    else {
        window.sessionStorage.setItem("UC_select", val);
        val = val + "/Index";       
        reloadPage(val);
    }    
}
