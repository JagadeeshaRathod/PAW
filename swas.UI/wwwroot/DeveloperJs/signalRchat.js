$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/chatHub")
        .build();

    connection.start().catch(err => console.error(err.toString()));

    connection.on("ReceiveMessage", (userMapChatId, message) => {
        UserChat(userMapChatId, $("#spnToUserIdMapChatId").html(), message);
    });

    function SaveChat(Msg) {
        const userMapChatId = $("#spnUserMapChatId").html();
        connection.invoke("SendMessage", userMapChatId, Msg).catch(err => console.error(err.toString()));
        $("textarea#type_msg").val("");
    }

    $(document).ajaxStart(function () {
        $("#loading").hide();
    }).ajaxStop(function () {
        $("#loading").hide();
    });

    $("#send_btn").click(function () {
        if ($("textarea#type_msg").val() == "") {
            $("textarea#type_msg").addClass('is-invalid');
        } else {
            SaveChat($("textarea#type_msg").val());
        }
    });

    $('#type_msg').keypress(function (e) {
        var key = e.which;
        if (key == 13 && !e.shiftKey) {
            if ($("textarea#type_msg").val() == "") {
                $("textarea#type_msg").addClass('is-invalid');
            } else {
                SaveChat($("textarea#type_msg").val());
            }
            return false;
        }
    });

    GetAllUsers();

    function GetAllUsers() {
        $.ajax({
            url: '/ChatSignalR/GetAllUsers',
            type: 'POST',
            data: { "Id": 0 },
            success: function (response) {
                if (response.length) {
                    var listitem = "";
                    response.forEach(user => {
                        listitem += '<li class="">';
                        listitem += '<div class="d-flex bd-highlight chatrequest " style="padding: 5px 5px 0px 5px;">';
                        listitem += ' <div class="img_cont">';
                        listitem += '<div class="circleimg" style="background-color:' + displayFixedColorAlphabet(user.offr_Name[0].toUpperCase()) + ';color:#ffff">' + user.offr_Name.substr(0, 2).toUpperCase() + '</div>';
                        if (user.total > 0) listitem += ' <span class="online_icon"></span>';
                        listitem += '</div>';
                        listitem += '<div class="user_info">';
                        listitem += '<span class="d-none" id="chatprofileId">' + user.id + '</span>';
                        listitem += '<span id="profName">' + user.rank + ' ' + user.offr_Name + '</span>';
                        listitem += ' <p>' + user.userName + '</p>';
                        listitem += '</div>';
                        listitem += '</div>';
                        listitem += ' </li>';
                    });
                    $(".contacts").html(listitem);
                }
            }
        });
    }

    function UserChat(userMapChatId, FromUserId, sortname) {
        $.ajax({
            url: '/ChatSignalR/GetUserMapChat',
            type: 'POST',
            data: { "UserMapChatId": userMapChatId, "FromUserId": FromUserId },
            success: function (response) {
                var listitem = response.map(chat => {
                    return `<div class="${chat.type == 1 ? "d-flex justify-content-start mb-4" : "d-flex justify-content-end mb-4"}">
                                <div class="msg_cotainer">
                                    ${chat.msg}
                                    <span class="msg_time">${DateFormateddMMyyyyhhmmss(chat.createdOn)}</span>
                                </div>
                            </div>`;
                }).join('');
                $(".msg_card_body").html(listitem);
                $(".msg_card_body").animate({ scrollTop: $('.msg_card_body').prop("scrollHeight") }, 1000);
            }
        });
    }
});

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
