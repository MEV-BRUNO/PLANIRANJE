$(document).ready(function () {
    var akt = $("#selectAktivnost").val();
    var saved1 = window.sessionStorage.getItem("UC_select");
    if (saved1 != null && saved1 != akt) {
        reloadPage(saved1 + "/Index");
    }
    else if (window.sessionStorage.getItem("UC_godina") != null) {
        var saved = window.sessionStorage.getItem("UC_godina");
        $("#selectGodina").val(saved);
        promjenaGodine();
    }
});
function provjeriEnter(event, path, form, target, poruka) {
    if (event.type === "keydown") {
        if (event.keyCode === 13) {
            event.preventDefault();
            zatvoriModal(path, form, target, poruka);
        }
    }
}
function ispis() {
    var akt = $("#selectAktivnost").val();
    var god = $("#selectGodina").val();
    if (god > 0) {
        var id = window.sessionStorage.getItem("UC_idUcenik");
        if (id != null) {
            window.open(akt + "/Ispis?godina=" + god + "&id=" + id);
        }
        else {
            showSnackBar("Odaberite učenika");
        }
    }
    else {
        showSnackBar("Odaberite godinu");
    }
}
function promjenaGodine() {
    var val = $("#selectGodina").val();
    if (val != "0") {
        $.ajax({
            url: '/PracenjeUcenika/OdabirRazreda?godina=' + val,
            success: function (data) {
                $("#razredi").html(data);
                var akt = $("#selectAktivnost").val();
                var saved = window.sessionStorage.getItem("UC_godina");
                if (saved != null && saved == $("#selectGodina").val()) {
                    var saved1 = window.sessionStorage.getItem("UC_razred");
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
                var saved = window.sessionStorage.getItem("UC_godina");
                var saved1 = window.sessionStorage.getItem("UC_razred");
                var id = window.sessionStorage.getItem("UC_idUcenik");
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
    }

    else {
        $("#tablica").empty();
        $("#detalji").empty();
    }
}
function pokaziDetalje(id) {
    var val = $("#selectGodina").val();
    var akt = $("#selectAktivnost").val();
    var raz = $("#selectRazred").val();
    window.sessionStorage.setItem("UC_idUcenik", id);    
    $.ajax({
        url: akt+'/Detalji?id=' + id + '&godina=' + val,
        success: function (data) {
            $("#detalji").html(data);
        },
        error: function (request, status, error) {
            showSnackBar("Dogodila se greška prilikom obrađivanja Vašeg zahtjeva");
        }
    });
    window.sessionStorage.setItem("UC_godina", val);
    window.sessionStorage.setItem("UC_razred", raz);
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
        window.sessionStorage.setItem("UC_select", val);
        pokaziDetalje(id);
    }
    else {
        window.sessionStorage.setItem("UC_select", val);
        val = val + "/Index";       
        reloadPage(val);
    }    
}
