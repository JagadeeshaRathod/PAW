function mMsater(sectid = '', ddl, TableId, ParentId) {

    var userdata =
    {
        "id": TableId,
        "ParentId": ParentId,

    };
    $.ajax({
        url: '/Master/GetAllMasterTableforddl',
        contentType: 'application/x-www-form-urlencoded',
        data: userdata,
        type: 'POST',

        success: function (response) {
            if (response != "null" && response != null) {
                if (response == 0) {
                    listItemddl += '<option  Value = "", Disabled = true, Selected = true>--Select--</option>';
                    /* listItemddl += '<option value="">Please Select</option>';*/
                    $("#" + ddl + "").html(listItemddl);
                }

                else {

                    var listItemddl = "";

                    /* listItemddl += '<option value="">Please Select</option>';*/
                    listItemddl += '<option  Value = "", Disabled = true, Selected = true>--Select--</option>';


                    for (var i = 0; i < response.length; i++) {
                        listItemddl += '<option value="' + response[i].id + '">' + response[i].name + '</option>';
                    }
                    $("#" + ddl + "").html(listItemddl);

                    //if (TableId == 5 || TableId == 7 || TableId == 8) {

                    //    if (sectid != '') {
                    //        $("#" + ddl + " option").filter(function () {
                    //            return this.text == sectid;
                    //        }).attr('selected', true);

                    //    }
                    //}
                    //else
                    //{
                    if (sectid != '') {
                        $("#" + ddl + "").val(sectid);

                    }

                    //}


                }
            }
            else {
                //Swal.fire({
                //    text: "No data found Offrs"
                //});
            }
        },
        error: function (result) {
            Swal.fire({
                text: "Error"
            });
        }
    });
}

function mMsaterfwdStage(sectid = '', ddl, TableId, ParentId, type) {
    var userdata =
    {
        "id": TableId,
        "ParentId": ParentId,

    };
    $.ajax({
        url: '/Master/GetAllMasterTableforddl',
        contentType: 'application/x-www-form-urlencoded',
        data: userdata,
        type: 'POST',

        success: function (response) {
            if (response != "null" && response != null) {
                if (response == 0) {
                    listItemddl += '<option value="">Please Select</option>';
                    $("#" + ddl + "").html(listItemddl);
                }

                else {

                    var listItemddl = "";

                    listItemddl += '<option value="">Please Select</option>';


                    for (var i = 0; i < response.length; i++) {
                        listItemddl += '<option value="' + response[i].id + '">' + response[i].name + '</option>';
                    }
                    $("#" + ddl + "").html(listItemddl);

                    //if (TableId == 5 || TableId == 7 || TableId == 8) {

                    //    if (sectid != '') {
                    //        $("#" + ddl + " option").filter(function () {
                    //            return this.text == sectid;
                    //        }).attr('selected', true);

                    //    }
                    //}
                    //else
                    //{
                    if (sectid != '') {
                        $("#" + ddl + "").val(sectid);

                    }
                    if (type == 1) {
                        if ($(this).closest("tr").find("#SpnprojectIsProcess").html() == 'False') {
                            $("#ddlfwdStage option[value='2']").remove();
                            $("#ddlfwdStage option[value='3']").remove();
                        }
                        else {
                            /* $("#ddlfwdStage option[value='1']").remove();*/   /*ajayUpdate*/

                        }
                    }
                    else if (type == 2) {
                        $("#ddlfwdStage option[value='2']").remove();
                        $("#ddlfwdStage option[value='3']").remove();
                    }
                    //}


                }
            }
            else {
                //Swal.fire({
                //    text: "No data found Offrs"
                //});
            }
        },
        error: function (result) {
            Swal.fire({
                text: "Error"
            });
        }
    });
}
function mMsaterStage(sectid = '', ddl, TableId, ParentId, StakeHolderId) {

    var userdata =
    {
        "id": TableId,
        "ParentId": ParentId,
        "StakeHolderId": StakeHolderId,

    };
    $.ajax({
        url: '/Master/GetStagebyStakeHolderId',
        contentType: 'application/x-www-form-urlencoded',
        data: userdata,
        type: 'POST',

        success: function (response) {
            if (response != "null" && response != null) {
                if (response == 0) {
                    listItemddl += '<option value="">Please Select</option>';
                    $("#" + ddl + "").html(listItemddl);
                }

                else {

                    var listItemddl = "";

                    listItemddl += '<option value="">Please Select</option>';


                    for (var i = 0; i < response.length; i++) {
                        listItemddl += '<option value="' + response[i].id + '">' + response[i].name + '</option>';
                    }
                    $("#" + ddl + "").html(listItemddl);

                    //if (TableId == 5 || TableId == 7 || TableId == 8) {

                    //    if (sectid != '') {
                    //        $("#" + ddl + " option").filter(function () {
                    //            return this.text == sectid;
                    //        }).attr('selected', true);

                    //    }
                    //}
                    //else
                    //{
                    if (sectid != '') {
                        $("#" + ddl + "").val(sectid);

                    }

                    //}


                }
            }
            else {
                //Swal.fire({
                //    text: "No data found Offrs"
                //});
            }
        },
        error: function (result) {
            Swal.fire({
                text: "Error"
            });
        }
    });
}

function mMsaterFwdTo(sectid = '', ddl, TableId, ParentId, StakeHolderId, type, value) {


    var userdata =
    {
        "id": TableId,
        "ParentId": ParentId,
        "StakeHolderId": StakeHolderId,
        "Type": type,
        "Value" : value

    };
    $.ajax({
        url: '/Master/GetFwdTo',
        contentType: 'application/x-www-form-urlencoded',
        data: userdata,
        type: 'POST',

        success: function (response) {
            debugger;
            if (response != "null" && response != null) {
                if (response == 0) {
                    listItemddl += '<option value="">Please Select</option>';
                    $("#" + ddl + "").html(listItemddl);
                }

                else {
                    if (type == 1) {

                        var listItemddl = "";

                        listItemddl += '<option value="">Please Select</option>';


                        for (var i = 0; i < response.length; i++) {
                            listItemddl += '<option value="' + response[i].id + '">' + response[i].name + '</option>';
                        }
                        $("#" + ddl + "").html(listItemddl);

                        // If autocomplete returned list, switch UI from searchBox to dropdown
                        if (response.length > 0) {
                            $("#searchBox").hide();
                            $("select[name='fwdoffrs']").show();
                        }

                        if (sectid != '') {
                            $("#" + ddl + "").val(sectid);

                        }

                    }
                    else {
                        var listItemddl = "";

                        listItemddl += '<option value="">Please Select</option>';


                        for (var i = 0; i < response.length; i++) {
                            listItemddl += '<option value="' + response[i].id + '">' + response[i].name + '</option>';
                        }
                        $("#" + ddl + "").html(listItemddl);
                        if (sectid != '') {
                            $("#" + ddl + "").val(sectid);

                        }
                    }
                }
            }
            else {                
            }
        },
        error: function (result) {
            Swal.fire({
                text: "Error"
            });
        }
    });
}

function GetAllComments(projectid) {
    $.ajax({
        type: "POST",
        url: '/Projects/GetAllCommentBypsmId_UnitId',
        data: {
            "PsmId": 0,
            "stakeholderId": 1,
            "ProjId": projectid
        },
        success: function (data) {

            var commentContainer = '';
            if (data != null) {

                for (var i = 0; i < data.length; i++) {
                    var date = new Date(data[i].date);
                    var formattedDate = ("0" + date.getDate()).slice(-2) + '-' + ("0" + (date.getMonth() + 1)).slice(-2) + '-' + date.getFullYear();

                    commentContainer += '<div class="comment-box" style="text-align: justify;">'; // Use text-align: justify for justified text
                    commentContainer += '<div class="comment-header">';
                    commentContainer += '<div>';
                    commentContainer += '<span style="font-family: Arial; font-weight: bold; color: #0793f7;">' + data[i].stakeholder + '</span>';
                    commentContainer += '<span style="margin-left: 10px;" class="comment-meta">' + formattedDate + '</span>';
                    commentContainer += '</div>';
                    commentContainer += '<div>';
                    if (data[i].status == "Accepted")
                        commentContainer += '<span class="comment-meta badge badge-success text-white">' + data[i].status + '</span>';
                    else
                        commentContainer += '<span class="comment-meta badge badge-danger text-white">' + data[i].status + '</span>';

                    commentContainer += '<span class="pdf-link">'; // Move the PDF link to the same line as status

                    if (data[i].state !== null) {

                        commentContainer += '<a href="/Home/WaterMark3?id=' + data[i].attpath + '" target="_blank">';
                        commentContainer += '&nbsp;&nbsp; &nbsp;&nbsp;<img src="/assets/images/icons/pdfimg.png" alt="PDF icon" style="width: 24px; height: 24px;">';
                        commentContainer += '</a>';
                    }


                    commentContainer += '</span>';
                    commentContainer += '</div>';
                    commentContainer += '</div>';
                    commentContainer += '<div class="comment-content">' + data[i].comments + '</div>';
                    commentContainer += '</div>';
                }

                commentContainer += '</div>'; // Close the container
                $('#ChatBox').empty().html(commentContainer);


            }

        },
        error: function () {
            alert('Error fetching comments.');
        }
    });
}

function DateFormatedd_mm_yyyy(date) {


    var datef2 = new Date(date);
    var months = "" + `${(datef2.getMonth() + 1)}`;
    var days = "" + `${(datef2.getDate())}`;
    var pad = "00"
    var monthsans = pad.substring(0, pad.length - months.length) + months
    var dayans = pad.substring(0, pad.length - days.length) + days
    var year = `${datef2.getFullYear()}`;
    if (year > 1902) {

        var datemmddyyyy = dayans + `/` + monthsans + `/` + year
        return datemmddyyyy;
    }
    else {
        return '';
    }

    //`${datef2.getFullYear()}/` + monthsans + `/` + dayans ;
}


function DateTimeFormatedd_mm_yyyy(date) {
    var dateObj = new Date(date); // Convert input date to Date object

    // Check if dateObj is a valid Date object
    if (Object.prototype.toString.call(dateObj) !== "[object Date]" || isNaN(dateObj.getTime())) {
        return ''; // Return empty string if date is invalid
    }

    // Format date as dd/mm/yyyy
    var day = dateObj.getDate().toString().padStart(2, '0');
    var month = (dateObj.getMonth() + 1).toString().padStart(2, '0');
    var year = dateObj.getFullYear();

    // Format time as HH:MM:SS
    var hours = dateObj.getHours().toString().padStart(2, '0');
    var minutes = dateObj.getMinutes().toString().padStart(2, '0');
    var seconds = dateObj.getSeconds().toString().padStart(2, '0');

    // Combine date and time
    var formattedDateTime = day + '/' + month + '/' + year + ' ' + hours + ':' + minutes + ':' + seconds;

    return formattedDateTime;
}
function DateTimeFormatedd_dd_mm_yyyy(dateString) {
    // Create a Date object from the input string
    const date = new Date(dateString);

    // Get the day, month, and year
    const day = String(date.getDate()).padStart(2, '0');  // Add leading zero if single digit
    const month = String(date.getMonth() + 1).padStart(2, '0');  // Get month (0-based)
    const year = date.getFullYear();

    // Return the date in the format: dd/mm/yyyy
    return `${day}/${month}/${year}`;
}

