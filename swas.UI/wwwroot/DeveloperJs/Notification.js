//$(document).ready(function () {
//    updateNotificationCountForChat(3, 'InterUserMsgCount'); // For Inter-User Msg
//});
//$(document).ready(function () {
//    updateNotificationCount(1, 'ProjectCommentCount'); // For project comments
//    updateNotificationCount(2, 'InboxCount'); // For inbox
//});

//function AddNotification(ProjId, type, unitid) {

//    $.ajax({
//        url: '/Notification/AddNotification',
//        type: 'POST',
//        data: {
//            "ProjId": ProjId,
//            "type": type,
//            "unitid": unitid, "__RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
//        },
//        success: function (response) {
//            console.log(response);
//            if (response && response === 1) {
//                Swal.fire({
//                    position: 'top-end',
//                    icon: 'success',
//                    title: 'Project Notification Added  successfully',
//                    showConfirmButton: false,
//                    timer: 700
//                });
//            }
//            updateNotificationCount(1, 'ProjectCommentCount'); // For project comments
//            updateNotificationCount(2, 'InboxCount'); // For inbox
//            updateNotificationCountForChat(3, 'InterUserMsgCount'); // For Inter-User Msg

//        },
//        error: function (error) {
//            console.error('Error occurred:', error);
//            // Handle error if needed
//        }
//    });
//}

//function IsReadNotification(ProjId, type) {

//    $.ajax({
//        url: '/Notification/IsReadNotification',
//        type: 'POST',
//        data: {
//            "ProjId": ProjId,
//            "type": type
//        },
//        success: function (response) {
//            console.log(response);
//            updateNotificationCount(1, 'ProjectCommentCount'); // For project comments
//            updateNotificationCount(2, 'InboxCount'); // For inbox
//            updateNotificationCountForChat(3, 'InterUserMsgCount'); // For Inter-User Msg
//        }
//    });
//}


//function UndoNotification(ProjId, type, ToUnitId) {

//    $.ajax({
//        url: '/Notification/UndoNotification',
//        type: 'POST',
//        data: {
//            "ProjId": ProjId,
//            "type": type,
//            "ToUnitId": ToUnitId
//        },
//        success: function (response) {
//            console.log(response);

//        }
//    });
//}




//function UnReadNotification(ProjId, type) {
//    $.ajax({
//        url: '/Notification/UnReadNotification',
//        type: 'POST',
//        data: {
//            "ProjId": ProjId,
//            "type": type
//        },
//        success: function (response) {
//            console.log(response);

//        }
//    });
//}
//function updateNotificationCount(type, elementId) {
//    $.ajax({
//        url: `/Notification/GetNotificationCount?type=${type}`, // Pass the notification type as a query parameter
//        type: 'GET',
//        success: function (count) {
//            // Check if the count is valid, if not, set it to 0
//            count = count || 0;

//            // Update the badge text based on the count
//            $('.' + elementId).html(count);

//            // Show or hide the badge based on the count
//            if (count > 0) {
//                /*  $(`#${elementId}`).show();*/
//                $("." + elementId).removeClass("d-none")
//            } else {
//                /* $(`#${elementId}`).hide();*/
//                $("." + elementId).addClass("d-none")
//            }
//        },
//        error: function () {
//            console.error(`Could not retrieve ${elementId} count.`);
//            // In case of an error, set the count to 0 and hide the badge
//            $(`#${elementId}`).text(0).hide();
//        }
//    });
//}
//function updateNotificationCountForChat(type, elementId) {
//    $.ajax({
//        //url: `/Notification/GetNotificationCount?type=${type}`, // Pass the notification type as a query parameter
//        url: `/Notification/GetNotificationCountForChat`,  // Pass the notification type as a query parameter
//        type: 'GET',
//        success: function (count) {
//            // Check if the count is valid, if not, set it to 0
//            count = count || 0;

//            // Update the badge text based on the count
//            /*$('.' + elementId).html(count);*/

//            // Show or hide the badge based on the count
//            if (count > 0) {
//                $("." + elementId).removeClass("d-none")
//                /* $("." + elementId).removeClass("d-none").html('<ion-icon name="ellipse-outline"></ion-icon>');*/
//            } else {
//                //$("." + elementId).addClass("d-none")
//                $("." + elementId).addClass("d-none").empty();
//            }
//        },
//        error: function () {
//            console.error(`Could not retrieve ${elementId} count.`);
//            // In case of an error, set the count to 0 and hide the badge
//            $(`#${elementId}`).text(0).hide();
//        }
//    });
//}


$(document).ready(function () {
   
    updateNotificationCountForChat(3, 'InterUserMsgCount'); // For Inter-User Msg

    updateNotificationCount(1, 'ProjectCommentCount'); // For project comments
    updateNotificationCount(2, 'InboxCount'); // For inbox
});

function AddNotification(ProjId, type, unitid) {

    $.ajax({
        url: '/Notification/AddNotification',
        type: 'POST',
        data: {
            "ProjId": ProjId,
            "type": type,
            "unitid": unitid, "__RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (response) {
            //console.log(response);
            if (response && response === 1) {
                Swal.fire({
                    position: 'top-end',
                    icon: 'success',
                   //title: 'Project Notification Added  successfully',
                    title: 'Project Submit Successfully',
                    showConfirmButton: false,
                    timer: 700
                });
            }
            updateNotificationCount(1, 'ProjectCommentCount'); // For project comments
            updateNotificationCount(2, 'InboxCount'); // For inbox
            updateNotificationCountForChat(3, 'InterUserMsgCount'); // For Inter-User Msg

        },
        error: function (error) {
            console.error('Error occurred:', error);
            // Handle error if needed
        }
    });
}

function IsReadNotification(ProjId, type) {
    $.ajax({
        url: '/Notification/IsReadNotification',
        type: 'POST',
        data: {
            "ProjId": ProjId,
            "type": type
        },
        success: function (response) {
            //console.log(response);
            updateNotificationCount(1, 'ProjectCommentCount'); // For project comments
            updateNotificationCount(2, 'InboxCount'); // For inbox
            updateNotificationCountForChat(3, 'InterUserMsgCount'); // For Inter-User Msg
        }
    });
}


function UndoNotification(ProjId, type, ToUnitId) {

    $.ajax({
        url: '/Notification/UndoNotification',
        type: 'POST',
        data: {
            "ProjId": ProjId,
            "type": type,
            "ToUnitId": ToUnitId
        },
        success: function (response) {
            //console.log(response);

        }
    });
}




function UnReadNotification(ProjId, type) {
    $.ajax({
        url: '/Notification/UnReadNotification',
        type: 'POST',
        data: {
            "ProjId": ProjId,
            "type": type
        },
        success: function (response) {
            //console.log(response);

        }
    });
}
function updateNotificationCount(type, elementId) {
    $.ajax({
        url: `/Notification/GetNotificationCount?type=${type}`, // Pass the notification type as a query parameter
        type: 'GET',
        success: function (count) {
            // Check if the count is valid, if not, set it to 0
            count = count || 0;

            // Update the badge text based on the count
            $('.' + elementId).html(count);

            // Show or hide the badge based on the count
            if (count > 0) {
                /*  $(`#${elementId}`).show();*/
                $("." + elementId).removeClass("d-none")
            } else {
                /* $(`#${elementId}`).hide();*/
                $("." + elementId).addClass("d-none")
            }
        },
        error: function () {
            console.error(`Could not retrieve ${elementId} count.`);
            // In case of an error, set the count to 0 and hide the badge
            $(`#${elementId}`).text(0).hide();
        }
    });
}

function updateNotificationCountForChat(type, elementId) {
 
    $.ajax({
        //url: `/Notification/GetNotificationCount?type=${type}`, // Pass the notification type as a query parameter
        url: `/Notification/GetNotificationCountForChat`,  // Pass the notification type as a query parameter
        type: 'GET',
        success: function (count) {
            // Check if the count is valid, if not, set it to 0
            count = count || 0;

            // Update the badge text based on the count
            /*$('.' + elementId).html(count);*/

            // Show or hide the badge based on the count
            if (count > 0) {
                $("." + elementId).removeClass("d-none")
                /* $("." + elementId).removeClass("d-none").html('<ion-icon name="ellipse-outline"></ion-icon>');*/
            } else {
                //$("." + elementId).addClass("d-none")
                $("." + elementId).addClass("d-none").empty();
            }
        },
        error: function () {
            console.error(`Could not retrieve ${elementId} count.`);
            // In case of an error, set the count to 0 and hide the badge
            $(`#${elementId}`).text(0).hide();
        }
    });
}