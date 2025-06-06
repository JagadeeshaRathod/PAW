var memberTable = "";

$(document).ready(function () {

    GetProjCommentsByUnitId();

    $("#btnStatusUpdate").unbind().click(function () {

        requiredFields = $('#projectcommentforstackholder').find('.requiredField');
        var allFieldsComplete = true;
        requiredFields.each(function (index) {
            if (this.value.length == 0) {
                $(this).addClass('is-invalid');
                allFieldsComplete = false;
            } else {
                $(this).removeClass('is-invalid');
            }
        });

        //var fileInput = $("#uploadfile");
        //if (fileInput.val().trim() === "") {
        //    fileInput.addClass('is-invalid');
        //    allFieldsComplete = false;
        //} else {
        //    fileInput.removeClass('is-invalid');
        //}

        if (!allFieldsComplete) {


        }
        else {
            SendMsg();
        }
    });

});

function IsUnReadInbox(psmId) {

    $.ajax({
        url: '/Projects/IsUnReadInbox',
        type: 'POST',
        data: { "PsmId": psmId },
        success: function (response) {
            //console.log(response);

        }
    });
}
function GetProjCommentsByUnitId() {
    var listItem = "";

    /*let boldCount = 0;*/
    $.ajax({
        url: '/Projects/GetProjCommentsByUnitId',
        type: 'POST',
        data: { "Id": 0 },
        success: function (response) {
            /*console.log("GetAllProjectByUnitId", response);*/
            if (response != "null" && response != null) {

                if (response == -1) {
                    Swal.fire({
                        text: ""
                    });
                }
                else if (response == 0) {
                    listItem += "<tr><td class='text-center' colspan=6>No Record Found</td></tr>";

                    $("#DetailBody").html(listItem);

                }

                else {

                    var count = 1;
                    for (var i = 0; i < response.length; i++) {
                        var date = new Date(response[i].timeStamp);
                        var TimeStamp =
                            ("0" + date.getDate()).slice(-2) + '-' +
                            ("0" + (date.getMonth() + 1)).slice(-2) + '-' +
                            date.getFullYear() + ' ' +
                            ("0" + date.getHours()).slice(-2) + ':' +
                            ("0" + date.getMinutes()).slice(-2) + ':' +
                            ("0" + date.getSeconds()).slice(-2);


                        if (response[i].isComment == false) {
                            listItem += "<tr class='bold-text'>";
                            /* boldCount++;*/
                        } else {
                            listItem += "<tr>";
                        }
                        listItem += "<td class='noExport d-none'><span class='noExport d-none' id='spnProjId'>" + response[i].projId + "</span><span class='noExport d-none' id='spnpsmId'>" + response[i].psmId + "</span></td>";
                        //listItem += "<td class='align-middle'><span id='divName'>" + count + "</span></td>";
                        listItem += "<td class='align-middle ser-no'>" + (i + 1) + "</td>";

                        listItem += "<td class='align-middle'>";
                        listItem += "<a  href='/Projects/ProjHistory?EncyID=" + encodeURIComponent(response[i].encyID) + "'>";
                        listItem += "<span id='projectName' class='projNameDetail' >" + response[i].projectName + "</span>";
                        listItem += "</a>";
                        listItem += "</td>";
                        listItem += "<td class='align-middle'><span id='stakeholder'>" + response[i].stakeholder + "</span></td>";
                        listItem += "<td class='align-middle'><span id='TimeStamp'>" + TimeStamp + "</span></td>";
                        if (response[i].stkStatusId == 1) {
                            listItem += "<td class='align-middle'><span id='status'>Accepted</span></td>";
                            listItem += "<td class='align-middle'><span id='btnedit'><button type='button' class='cls-btncomment btn-icon btn-round btn-success mr-1'><i class='fas fa-comment'></i></button></td>";

                        }
                        else if (response[i].stkStatusId == 5) {
                            listItem += "<td class='align-middle'><span id='status'>Info</span></td>";
                            listItem += "<td class='align-middle'><span id='btnedit'><button type='button' class='cls-btncomment btn-icon btn-round btn-success mr-1'><i class='fas fa-comment'></i></button></td>";
                        }
                        else if (response[i].stkStatusId == 2) {
                            listItem += "<td class='align-middle'><span id='status'>Obsn</span></td>";
                            listItem += "<td class='align-middle'><span id='btnedit'><button type='button' class='cls-btncomment btn-icon btn-round  btn-warning mr-1'><i class='fas fa-comment'></i></button></td>";

                        }
                        else if (response[i].stkStatusId == 3) {
                            listItem += "<td class='align-middle'><span id='status'>Rejected</span></td>";
                            listItem += "<td class='align-middle'><span id='btnedit'><button type='button' class='cls-btncomment btn-icon btn-round btn-danger mr-1'><i class='fas fa-comment'></i></button></td>";
                        }
                        else {
                            listItem += "<td class='align-middle'><span id='status'>Pending</span></td>";
                            listItem += "<td class='align-middle'><span id='btnedit'><button type='button' class='cls-btncomment btn-icon btn-round btn-danger mr-1'><i class='fas fa-comment'></i></button></td>";
                        }
                        listItem += "</tr>";
                        count++;

                    }

                    $("#DetailBody").html(listItem);
                    var table = $('#Comment').DataTable({
                        lengthChange: true,
                        dom: 'lBfrtip',
                        retrieve: true,
                        bDestroy: true,
                        pageLength: -1, // Show all entries by default // 
                        lengthMenu: [[10, 25, 50, -1], [10, 25, 50, "All"]], //
                        order: [[1, 'asc']], // Ensure Ser No is sorted in ascending order
                        buttons: [
                            {
                                extend: 'excel',
                                text: 'Excel',
                                exportOptions: {
                                    columns: ':visible:not(:last-child)',
                                    format: {
                                        body: function (data, row, column, node) {
                                            var excelRowData = $(data).text().trim();
                                            return column === 0 ? row + 1 : excelRowData; // Fix Ser No in export
                                        }
                                    }
                                }
                            },
                            {
                                extend: 'csv',
                                exportoptions: {
                                    columns: ':visible:not(:last-child)',
                                    format: {
                                        body: function (data, row, column, node) {
                                            var csvrowdata = $(data).text().trim();
                                            return column === 0 ? row + 1 : csvrowdata; // fix ser no in export
                                        }
                                    }
                                }
                            },                            
                            {
                                extend: 'pdfHtml5',
                                text: 'PDF',
                                exportOptions: {
                                    columns: ':visible:not(:last-child)'
                                },
                                action: function (e, dt, node, config) {
                                    PdfDiv();
                                }
                            }                           
                        ],
                        searchBuilder: {
                            conditions: {
                                num: {
                                    'MultipleOf': {
                                        conditionName: 'Multiple Of',
                                        init: function (that, fn, preDefined = null) {
                                            var el = $('<input>').on('input', function () { fn(that, this); });
                                            if (preDefined !== null) {
                                                $(el).val(preDefined[0]);
                                            }
                                            return el;
                                        },
                                        inputValue: function (el) {
                                            return $(el[0]).val();
                                        },
                                        isInputValid: function (el, that) {
                                            return $(el[0]).val().length !== 0;
                                        },
                                        search: function (value, comparison) {
                                            return value % comparison === 0;
                                        }
                                    }
                                }
                            }
                        }
                    });

                    function PdfDiv() {
                        var table = $('#Comment').DataTable();
                        var filteredData = table.rows({ search: 'applied' }).data().toArray();

                        // Extract headers while excluding the last column (Action)
                        var headers = [];
                        table.columns(':visible').header().each(function (header, index) {
                            if (index !== table.columns().count() - 1) { // Exclude "Action" column
                                headers.push($(header).text().trim());
                            }
                        });

                        // Extract data while excluding the last column (Action) and fixing Ser No
                        var data = [];
                        for (var i = 0; i < filteredData.length; i++) {
                            var rowData = [];
                            for (var j = 0; j < filteredData[i].length - 1; j++) { // Exclude "Action" column
                                var cleanText = $(filteredData[i][j]).text().trim(); // Strip HTML tags
                                rowData.push(j === 0 ? i + 1 : cleanText); // Fix Ser No (Starts at 1)
                            }
                            data.push(rowData);
                        }

                        var tableHTML = '<table>';
                        tableHTML += '<thead><tr>';
                        headers.forEach(header => {
                            tableHTML += '<th>' + header + '</th>';
                        });
                        tableHTML += '</tr></thead><tbody>';

                        data.forEach(row => {
                            tableHTML += '<tr>';
                            row.forEach(cell => {
                                tableHTML += '<td>' + cell + '</td>';
                            });
                            tableHTML += '</tr>';
                        });

                        tableHTML += '</tbody></table>';

                        var watermarkText = $("#IpAddress").html();

                        var popupWin = window.open('', '_blank', 'top=100,width=900,height=500,location=no');
                        popupWin.document.open();

                        var tableStyles = `
    <style type="text/css">
        table {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 20px;
        }
        th, td {
            padding: 8px;
            border: 1px solid #ddd;
            text-align: center;
        }
        th {
            background-color: #f2f2f2;
            color: black;
        }
    </style>`;

                        popupWin.document.write(`
    <html>
    <head>${tableStyles}</head>
    <body onload="window.print()">
        ${tableHTML}
        <div style="transform: rotate(-45deg);z-index:10000;opacity: 0.3;color: BLACK;
        position:fixed;left: 6%; top: 39%;color: #8e9191;font-size: 80px; font-weight: 500px;
        display: grid;justify-content: center;align-content: center;">
        ${watermarkText}
        </div>
    </body>
    </html>`);

                        popupWin.document.close();
                    }

                    /* $('#ProjectCommentCount').html(boldCount);*/

                    $("body").on("click", ".cls-btncomment", function () {
                        $("#ProjectcommentForStackHolderprojId").html($(this).closest("tr").find("#spnProjId").html());
                        $("#ProjectcommentForStackHolderPsmId").html($(this).closest("tr").find("#spnpsmId").html());

                        IsReadComment($(this).closest("tr").find("#spnProjId").html(), $(this).closest("tr").find("#spnpsmId").html());
                        IsReadNotification($(this).closest("tr").find("#spnProjId").html(), 1);
                        $(this).closest("tr").removeClass("bold-text")
                        reset()
                        mMsater(0, "ddlStatus", 4, 0)
                        $("#ProjCommentModal").modal('show');
                        GetAllComments($("#ProjectcommentForStackHolderPsmId").html(), $("#ProjectcommentForStackHolderprojId").html());

                        // Added from here for pop up heading with project name in comment (added by Divyanshu on 04/02/2025)
                        //var projName = $(this).closest("tr").find("#projectName").html() + "  " + "Comments";
                        //$('#addComment').text(projName);
                        var projName = $(this).closest("tr").find("#projectName").html();
                        var words = projName.split(" ");
                        // Limit to 6 words and add "..." if needed
                        var shortProjName = words.length > 6 ? words.slice(0, 6).join(" ") + "..." : projName;
                        //var finalTitle = "Mov History: " + shortProjName;
                        var finalTitle = "Mov History: " + projName;
                        $('#addComment').text(finalTitle);
                    });

                    $("body").on("click", ".projNameDetail", function () {

                        IsReadComment($(this).closest("tr").find("#spnProjId").html(), $(this).closest("tr").find("#spnpsmId").html());
                        IsReadNotification($(this).closest("tr").find("#spnProjId").html(), 1);

                    });


                }
            }
            else {
                listItem += "<tr><td class='text-center' colspan=6>No Record Found</td></tr>";

                $("#DetailBody").html(listItem);

            }
        },
        error: function (result) {
            Swal.fire({
                text: ""
            });
        }
    });
}




function SendMsg() {

    var formData = new FormData();
    var totalFiles = document.getElementById("uploadfile").files.length;
    for (var i = 0; i < totalFiles; i++) {
        var file = document.getElementById("uploadfile").files[i];
        formData.append("uploadfile", file);

    }

    var dateValue = $('#CommentDateFwd').val();
    var currentDate = new Date();

    // Add server's current time if only a date is selected
    var commentDateTime = '';
    if ($('#CommentDateFwd').attr('type') === 'date') {
        if (!dateValue) {
            alert('Please select a date .');
            return;
        }
        var currentTime = currentDate.toTimeString().split(' ')[0]; // Get current time in HH:mm:ss
        commentDateTime = dateValue + ' ' + currentTime;
    } else if ($('#CommentDateFwd').attr('type') === 'datetime-local') {
        if (!dateValue) {
            alert('Please select date and time.');
            return;
        }
        commentDateTime = dateValue.replace('T', ' '); // Format datetime-local to space-separated
    }



    formData.append("Comments", $("#Comments").val());
    formData.append("StkStatusId", $("#ddlStatus").val());
    formData.append("ProjectId", $("#ProjectcommentForStackHolderprojId").html());
    formData.append("psmid", $("#ProjectcommentForStackHolderPsmId").html());
    //formData.append("CommentDate", $("#CommentDateFwd").val());
    formData.append("CommentDate", commentDateTime);
    $.ajax({
        type: "POST",
        url: '/Projects/SendCommentonProject',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response == 0) {

            }
            if (response == 1) {
                Swal.fire({
                    position: 'top-end',
                    icon: 'success',
                    title: 'Comment Sent successfully',
                    showConfirmButton: false,
                    timer: 3000
                }).then(() => {
                    //  if ($("#ddlStatus").val() == 1) {
                    FwdProjConfirm($("#ProjectcommentForStackHolderPsmId").html());
                    // }


                    GetAllComments($("#ProjectcommentForStackHolderPsmId").html(), $("#ProjectcommentForStackHolderprojId").html());
                    GetProjCommentsByUnitId();
                    //IsUnReadComment($("#ProjectcommentForStackHolderprojId").html());
                    //GetNotification($("#ProjectcommentForStackHolderprojId").html());
                    UnReadNotification($("#ProjectcommentForStackHolderprojId").html(), 2);
                    IsUnReadComment($("#ProjectcommentForStackHolderprojId").html(), $("#ProjectcommentForStackHolderPsmId").html());
                    reset();
                })


            }
            else if (response == 6) {
                Swal.fire({
                    position: 'top-end',
                    icon: 'Error',
                    /*title: 'No Amdts Allowed as the Project is Already Accepted By You !',*/
                    title: '<div style="text-align: left;">' +
                        '<ol style="margin: 0; padding-left: 20px; text-align: left;">' +
                        '<li>No Amdts Allowed as the Project is Already Accepted By You!</li>' +
                        '<li>However, only info is allowed after the project is accepted.</li>' +
                        '</ol>' +
                        '</div>',
                    showConfirmButton: true,

                });
            }
            else if (response == 8) {
                Swal.fire({
                    position: 'top-end',
                    icon: 'Error',
                    title: 'Pdf Size allow less then 10 Mb !',
                    showConfirmButton: true,

                });
            }




            //IsReadComment($("#ProjectcommentForStackHolderprojId").html());
        },
        error: function (error) {


        }
    });
}
function GetAllComments(PsmId, projId) {
    $.ajax({
        type: "POST",
        url: '/Projects/GetAllCommentBypsmId_UnitId',
        data: {
            "PsmId": PsmId,
            "stakeholderId": 1,
            "ProjId": projId
        },
        success: function (data) {

            var commentContainer = '';
            var userDetails = '';
            if (data != null) {
                for (var i = 0; i < data.length; i++) {
                    var date = new Date(data[i].date);
                    var formattedDate =
                        ("0" + date.getDate()).slice(-2) + '-' +
                        ("0" + (date.getMonth() + 1)).slice(-2) + '-' +
                        date.getFullYear() + ' ' +
                        ("0" + date.getHours()).slice(-2) + ':' +
                        ("0" + date.getMinutes()).slice(-2) + ':' +
                        ("0" + date.getSeconds()).slice(-2);
                    if (data[i].userDetails == null)
                        userDetails = '';
                    else
                        userDetails = data[i].userDetails

                    commentContainer += '<div class="comment-box" style="text-align: justify;">'; // Use text-align: justify for justified text
                    commentContainer += '<div class="comment-header">';
                    commentContainer += '<div>';
                    commentContainer += '<span style="font-family: Arial; color: #0793f7;">' + data[i].stakeholder + ' (' + userDetails + ') </span>';
                    commentContainer += '<div style="margin-left: 0px;" class="comment-meta">' + DateFormateddMMyyyyhhmmss(data[i].date) + '</div>';
                    commentContainer += '</div>';
                    commentContainer += '<div>';
                    if (data[i].status == "Accepted" || data[i].status == "Info")
                        commentContainer += '<span class="comment-meta badge badge-success text-white">' + data[i].status + '</span>';
                    else if (data[i].status == "Obsn")
                        commentContainer += '<span class="comment-meta badge badge-warning text-white">' + data[i].status + '</span>';
                    else
                        commentContainer += '<span class="comment-meta badge badge-danger text-white">' + data[i].status + '</span>';
                    if (data[i].attpath !== '' && data[i].attpath !== null) {
                        commentContainer += '<a href="/Home/WaterMark3?id=' + data[i].attpath + '" target="_blank">';
                        commentContainer += '<img src="/assets/images/icons/pdfimg.png" alt="PDF icon" style="width: 24px; height: 24px;">';
                        commentContainer += '</a>';
                    }


                    commentContainer += '</span>';
                    commentContainer += '</div>';
                    commentContainer += '</div>';
                    commentContainer += '<div class="comment-content formated-text"><p>' + data[i].comments + '</p></div>';
                    commentContainer += '</div>';
                }



                commentContainer += '</div>'; // Close the container
                $('#ChatBoxForStackholdercomment').empty().html(commentContainer);




            }

        },
        error: function () {
            alert('Error fetching comments.');
        }
    });
}



function IsReadComment(ProjId, PsmId) {
    $.ajax({
        url: '/Projects/IsReadComment',
        type: 'POST',
        data: { "ProjId": ProjId, "PsmId": PsmId },
        success: function (response) {
            //console.log(response);
        }
    })
}


function GetNotificationInbox(ProjId) {
    alert("om");
    $.ajax({
        url: '/Home/GetNotificationInbox',
        type: 'POST',
        data: { "ProjId": ProjId },
        success: function (response) {

        }
    })
}

function IsUnReadComment(ProjId, PsmId) {
    $.ajax({
        url: '/Projects/IsUnReadComment',
        type: 'POST',
        data: {
            "ProjId": ProjId,
            "PsmId": PsmId
        },
        success: function (response) {
            //console.log(response);
            window.location.reload();
        }
    })
}

function IsReadInbox(psmId) {

    $.ajax({
        url: '/Projects/IsReadInbox',
        type: 'POST',
        data: { "PsmId": psmId },
        success: function (response) {
            //console.log(response);

        }
    });
}

//function IsUnReadNotification(ProjId) {

//    $.ajax({
//        url: '/Projects/IsUnReadNotification',
//        type: 'POST',
//        data: { "ProjId": ProjId },
//        success: function (response) {
//            console.log(response);

//        }
//    });
//}

//function IsCommentedUnreadNotification(ProjId) {

//    $.ajax({
//        url: '/Projects/IsCommentedUnreadNotification',
//        type: 'POST',
//        data: { "ProjId": ProjId },
//        success: function (response) {
//            console.log(response);

//        }
//    });
//}

//function IsReadNotification(ProjId) {

//    $.ajax({
//        url: '/Projects/IsReadNotification',
//        type: 'POST',
//        data: { "ProjId": ProjId },
//        success: function (response) {
//            console.log(response);

//        }
//    });
//}

function reset() {
    $("#Comments").val("");
    $("#ddlStatus").val(0);
    $("#uploadfile").val("");
}

function FwdProjConfirm(psmid) {
    $.ajax({
        url: '/Projects/FwdProjConfirm',
        type: 'POST',
        data: { "PslmId": psmid },
        success: function (response) {
            //console.log(response);


            if (response >= 1) {





            }

        }
    });
}


function IsCommentedUnreadNotification(ProjId) {

    $.ajax({
        url: '/Projects/IsCommentedUnreadNotification',
        type: 'POST',
        data: { "ProjId": ProjId },
        success: function (response) {
            //console.log(response);

        }
    });
}