$(document).ready(function () {
    sessionCheck();
});

function sessionCheck() {
    $.ajax({
        url: "apiService.asmx/SessionTimeout",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: 'POST',
        success: function (data, XMLHttpRequest) {
            var sessionval = data.d;
            $('#hd_Userid').val(sessionval);
            if (sessionval == 0) {
                window.location.href = '../../index.html';
            }
            else {
                bgLoad();
                CompanyProfile();
            }
        },
        error: function (XMLHttpRequest, errorThrown) {

        }
    });
}

function logout() {
    $.ajax({
        url: "apiService.asmx/SessionLogout",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        type: 'POST',
        success: function (data, XMLHttpRequest) {
            var sessionval = data.d;
            if (sessionval == 0) {
                window.location.href = 'index.html';
            }
        },
        error: function (XMLHttpRequest, errorThrown) {

        }
    });
}