﻿$(document).ready(function () {
    var tip = $("#tip").text();
    var skola = $("#selectSkola").val();
    var akt = $("#selectAktivnost").val();
    var saved1 = window.sessionStorage.getItem(skola + tip + "_select");
    if (saved1 != null && saved1 != akt) {
        reloadPage(saved1 + "/Index");
    }
    else if (window.sessionStorage.getItem(skola + tip + "_godina") != null) {
        var saved = window.sessionStorage.getItem(skola + tip + "_godina");
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
    var tip = $("#tip").text();
    var skola = $("#selectSkola").val();
    var akt = $("#selectAktivnost").val();
    var god = $("#selectGodina").val();
    if (god > 0) {
        var id = window.sessionStorage.getItem(skola + tip + "_idUcenik");
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
    var tip = $("#tip").text();
    var skola = $("#selectSkola").val();
    var val = $("#selectGodina").val();
    if (val != "0") {
        $.ajax({
            url: '/NastavnikAnaliza/OdabirNastavnika',
            success: function (data) {
                $("#tablica").html(data);
                $("#dataTableNastavnici").dataTable({ stateSave: true });
                var akt = $("#selectAktivnost").val();
                var saved = window.sessionStorage.getItem(skola + tip + "_godina");
                if (saved != null && saved == $("#selectGodina").val()) {
                    
                }
            },
            error: function (request, status, error) {
                showSnackBar("Dogodila se greška prilikom obrađivanja Vašeg zahtjeva");
            }
        });
    }
    else {
        $("#detalji").empty();
    }
    $("#tablica").empty();    
}

function pokaziDetalje(id) {
    var tip = $("#tip").text();
    var skola = $("#selectSkola").val();
    var val = $("#selectGodina").val();
    var akt = $("#selectAktivnost").val();    
    window.sessionStorage.setItem(skola + tip + "_idNastavnik", id);
    $.ajax({
        url: akt + '/Detalji?id=' + id + '&godina=' + val,
        success: function (data) {
            $("#detalji").html(data);
        },
        error: function (request, status, error) {
            showSnackBar("Dogodila se greška prilikom obrađivanja Vašeg zahtjeva");
        }
    });
    window.sessionStorage.setItem(skola + tip + "_godina", val);    
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
    var tip = $("#tip").text();
    var skola = $("#selectSkola").val();
    var val = $("#selectAktivnost").val();
    if (val == "/PopisUcenika" || $("#selectGodina").val() == "0" || !$("#selectRazred").length || $("#selectRazred").val() == "0") {
        window.sessionStorage.setItem(skola + tip + "_select", val);
        reloadPage(val + "/Index");
    }
    else if (/*$("#selectRazred").length==0 &&*/ $("#selectRazred").val() != "0") {
        var id = window.sessionStorage.getItem(skola + tip + "_idUcenik");
        window.sessionStorage.setItem(skola + tip + "_select", val);
        pokaziDetalje(id);
    }
    else {
        window.sessionStorage.setItem(skola + tip + "_select", val);
        val = val + "/Index";
        reloadPage(val);
    }
}