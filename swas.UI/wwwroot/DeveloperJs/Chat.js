$(document).ready(function () {
    $(document).ajaxStart(function () {
        $("#loading").hide();
    }).ajaxStop(function () {
        $("#loading").hide();
    }).ajaxError(function () {
        $("#loading").hide();
    });

    setInterval(function () {
        // GetAllUsers()
        if ($("#spnUserMapChatId").html() != 0)

            UserChat($("#spnUserMapChatId").html(), $("#spnToUserIdMapChatId").html(), $("#profsortname").html());

    }, 3000);


    $(".img_cont_msg").click(function () {
        alert(1)
    });
    $("#chatProfileSearch").keyup(function () {

        var armyno = $("#chatProfileSearch").val()

        GetAllUsers()

    });
    GetAllUsers()

    $("#type_msg").keypress(function () {

        $("#type_msg").removeClass('is-invalid');
    });

    $('#type_msg').keypress(function (e) {
        var key = e.which;
        if (key == 13 && !e.shiftKey)  // the enter key code
        {

            if ($("textarea#type_msg").val() == "") {
                $("textarea#type_msg").addClass('is-invalid');

            }
            else {

                SaveChat($("textarea#type_msg").val());
            }
        }
    });

    $("#send_btn").click(function () {
        if ($("textarea#type_msg").val() == "") {
            $("textarea#type_msg").addClass('is-invalid');

        }
        else {

            SaveChat($("textarea#type_msg").val());
        }
    });
});
function SaveChat(Msg) {
    $.ajax({
        url: '/Chat/SaveChat',
        type: 'POST',
        data: { "UserMapChatId": $("#spnUserMapChatId").html(), "Msg": Msg, },
        success: function (response) {
            var listitem = "";
            if (response != null) {
                $("#type_msg").html("");

                UserChat($("#spnUserMapChatId").html(), $("#spnToUserIdMapChatId").html(), $("#profsortname").html());
                $("textarea#type_msg").val("");
                GetAllUsers();
                //Swal.fire({
                //    position: "top-end",
                //    icon: "success",
                //    title: "Chat Sent success",
                //    showConfirmButton: false,
                //    timer: 1500
                //});
            }
        }
    });
}
function GetAllUsers() {
    $.ajax({
        url: '/Chat/GetAllUsers',
        type: 'POST',
        data: { "Id": 0 },
        success: function (response) {
            //console.log("chatResponse", response);
            if (response.length) {
                var listitem = "";
                for (var i = 0; i < response.length; i++) {

                    /* if ($("#chatProfileSearch").val() == "" || response[i].offr_Name.toLowerCase().indexOf($("#chatProfileSearch").val().toLowerCase()) !== -1) {*/
                    if ($("#chatProfileSearch").val() == "" || response[i].offr_Name.toLowerCase().indexOf($("#chatProfileSearch").val().toLowerCase()) !== -1 ||
                        response[i].userName.toLowerCase().indexOf($("#chatProfileSearch").val().toLowerCase()) !== -1) {
                        listitem += '<li class="">';
                        listitem += '<div class="d-flex bd-highlight chatrequest " style="padding: 5px 5px 0px 5px;">';
                        listitem += ' <div class="img_cont">';
                        //listitem += '<img src="/assets/images/icons/profilechat.png" class="rounded-circle user_img">';
                        listitem += '<div class="circleimg" style="background-color:' + displayFixedColorAlphabet(response[i].offr_Name.replace(/^\s+|\s+$/gm, '').substr(0, 1).toUpperCase()) + ';color:#ffff">' + response[i].offr_Name.replace(/^\s+|\s+$/gm, '').substr(0, 2).toUpperCase() + '</div>';
                        if (parseInt(response[i].total) > 0)
                            listitem += ' <span class="online_icon"></span>';
                        listitem += '</div>';
                        listitem += '<div class="user_info">';
                        listitem += '<span class="d-none" id="chatprofileId">' + response[i].id + '</span>';
                        listitem += '<span id="profName">' + response[i].rankName + ' ' + response[i].offr_Name + '</span>';
                        listitem += ' <p>' + response[i].userName + '</p>';
                        listitem += '</div>';
                        listitem += '</div>';
                        listitem += ' </li>';

                    }
                }
                $(".contacts").html(listitem);

                $("body").unbind().on("click", ".chatrequest", function () {

                    $(".chatrequest").removeClass('active');
                    $(".btnforchat").removeClass('d-none');
                    $(".bd-highlight").removeClass('d-none');
                    $(this).addClass("active");

                    $("#profsortname").html($(this).closest("div").find(".circleimg").html())
                    UserMapChat($(this).closest("div").find("#chatprofileId").html(), $(this).closest("div").find("#profName").html(), $(this).closest("div").find(".circleimg").html());
                   
                });
            }
        }
    });
}
function UserMapChat(ToUserId, profName, sortname) {
    $.ajax({
        url: '/Chat/SaveUserMapChat',
        type: 'POST',
        data: { "UserMapChatId": 0, "ToUserId": ToUserId, },
        success: function (response) {
            var listitem = "";
            if (response != null) {

                $("#profnamechat").html(profName);
                $("#profsortname").html(sortname);

                $("#spnUserMapChatId").html(response.userMapChatId);
                $("#spnToUserIdMapChatId").html(ToUserId);
                $("#img_contuser").html('<div class="circleimg" style="background-color:' + displayFixedColorAlphabet(sortname.replace(/^\s+|\s+$/gm, '').substr(0, 1).toUpperCase()) + ';color:#ffff" >' + sortname.toUpperCase() + '</div>');

                UserChat(response.userMapChatId, ToUserId, sortname);
                

            }
        }
    });
}
function UserChat(userMapChatId, FromUserId, sortname) {
    $("#loading").hide();
    $.ajax({
        url: '/Chat/GetUserMapChat',
        type: 'POST',
        data: { "UserMapChatId": userMapChatId, "FromUserId": FromUserId },
        success: function (response) {
            var listitem = "";
            if (response != null) {

                if (response.length > 0) {
                    for (var i = 0; i < response.length; i++) {
                        if (response[i].type == 1) {
                            listitem += '<div class="d-flex justify-content-start mb-4">';
                            listitem += '<div class="img_cont_msg">';
                            /*listitem += '<img src="/assets/images/icons/profilechat.png" class="rounded-circle user_img_msg">';*/
                            listitem += '<div class="circleimgchat" style="background-color:' + displayFixedColorAlphabet($(".spnOffr_Name").html().replace(/^\s+|\s+$/gm, '').substr(0, 1).toUpperCase()) + ';color:#ffff">' + $(".spnOffr_Name").html().substr(0, 2).toUpperCase() + '</div>';
                            listitem += '</div>';
                            listitem += '<div class="msg_cotainer"><span class="spnmsgchatId d-none">' + response[i].chatId + '</span>';
                            if (response[i].isRead == false) {
                                listitem += '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-check" viewBox="0 0 16 16">';
                                listitem += '<path d="M10.97 4.97a.75.75 0 0 1 1.07 1.05l-3.99 4.99a.75.75 0 0 1-1.08.02L4.324 8.384a.75.75 0 1 1 1.06-1.06l2.094 2.093 3.473-4.425z"/>';
                                listitem += '</svg>';
                            }
                            else {
                                listitem += '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512" width="16" height="16" fill="currentColor"> <path d="M342.6 86.6c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0L160 178.7l-57.4-57.4c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3l80 80c12.5 12.5 32.8 12.5 45.3 0l160-160zm96 128c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0L160 402.7 54.6 297.4c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3l128 128c12.5 12.5 32.8 12.5 45.3 0l256-256z"/></svg>';
                            }
                            listitem += '' + response[i].msg + '';
                            listitem += '<span class="msg_time">' + DateFormateddMMyyyyhhmmss(response[i].createdOn) + '</span>';
                            listitem += '</div>'; //<span class="msg_del"><i class="fas fa-trash"></i></span>
                            listitem += '</div>';
                        } else if (response[i].type == 2) {
                            listitem += '<div class="d-flex justify-content-end mb-4">';
                            listitem += '<div class="msg_cotainer_send">';
                            if (response[i].isRead == false) {
                                listitem += '<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-check" viewBox="0 0 16 16">';
                                listitem += '<path d="M10.97 4.97a.75.75 0 0 1 1.07 1.05l-3.99 4.99a.75.75 0 0 1-1.08.02L4.324 8.384a.75.75 0 1 1 1.06-1.06l2.094 2.093 3.473-4.425z"/>';
                                listitem += '</svg>';
                            }
                            else {
                                listitem += '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512" width="16" height="16" fill="currentColor"> <path d="M342.6 86.6c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0L160 178.7l-57.4-57.4c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3l80 80c12.5 12.5 32.8 12.5 45.3 0l160-160zm96 128c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0L160 402.7 54.6 297.4c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3l128 128c12.5 12.5 32.8 12.5 45.3 0l256-256z"/></svg>';
                            }
                            listitem += '' + response[i].msg + '';
                            listitem += ' <span class="msg_time_send">' + DateFormateddMMyyyyhhmmss(response[i].createdOn) + '</span>';
                            listitem += ' </div>';
                            listitem += ' <div class="img_cont_msg">';
                            listitem += '<div class="circleimgchat" style="background-color:' + displayFixedColorAlphabet(sortname.replace(/^\s+|\s+$/gm, '').substr(0, 1).toUpperCase()) + ';color:#ffff">' + sortname.toUpperCase() + '</div>';
                            /* listitem += '	<img src="/assets/images/icons/profilechat.png" class="rounded-circle user_img_msg">';*/
                            listitem += '</div>';
                            listitem += '</div>';
                        }
                    }

                    $(".msg_card_body").html(listitem);
                    $(".msg_card_body").animate({ scrollTop: $('.msg_card_body').prop("scrollHeight") }, 1000);
                    updateNotificationCountForChat(3, 'InterUserMsgCount');
                    GetAllUsers();
                    $("body").on("click", ".msg_del", function () {
                        // alert($(this).closest("div").find(".spnmsgchatId").html())
                        alert(1)
                    });
                }
                else {
                    $(".msg_card_body").html(listitem);
                }


            }

        }
    });
}

const colors = [
    '#759c84',
    '#d6dbdf',
    '#5d6d7e',
    '#5b7382',
    '#3498db',
    '#2c3e50',
    '#6da8a8',
    '#6495ED',
    '#000080',
    '#0000FF',
    '#008080',
    '#C0C0C0',
    '#999999',
    '#a85e5e',
    '#008000',
    '#454545',
    '#52be80',
    '#283747',
    '#1b4f72',
    '#1b4f72',
    '#d6dbdf',
    '#b3b6b7',
    '#28b463',
    '#aed6f1',
    '#17202a',
    '#73c6b6'
];

// Function to display a to z with fixed colors
function displayFixedColorAlphabet(latter) {

    const alphabet = 'abcdefghijklmnopqrstuvwxyz';

    for (let i = 0; i < alphabet.length; i++) {
        if (latter.toUpperCase() == alphabet[i].toUpperCase()) {

            return colors[i]
        }
    }
}