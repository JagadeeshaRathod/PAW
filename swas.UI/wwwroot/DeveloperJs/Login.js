$(document).ready(function () {


    $.ajax({
        url: '../../Home/CheckLogin',
        type: 'POST',
        data: { "RoleId": 1 }, //get the search string
        success: function (result) {

            if (result == 1) {
                window.location.replace("../../Home/Index");

            }


        }
    });

    //jQuery.ajax({
    //    type: "POST",
    //    url: "/Login.cshtml.cs/CheckLogin",
    //    success: function (data) { alert(data); },
    //    failure: function (errMsg) {
    //        alert(errMsg);
    //    }
    //});
});