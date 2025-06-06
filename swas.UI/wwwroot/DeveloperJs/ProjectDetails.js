$(document).ready(function () {

    var param = sessionStorage.getItem("spntabType");
    
    if (param != null) {
        if (param == "XR12") {
            $("#tabinbox").removeClass("active-link");
            $("#tabsent").addClass("active-link");
            
            $("#tabcompleted").removeClass("active-link");
            $("#tabdraft").removeClass("active-link");

            $("#sent").addClass("active-tab");
            $("#inbox").removeClass("active-tab");
            $("#Completed").removeClass("active-tab");
        } else if (param == "XRDC") {
            $("#tabinbox").addClass("active-link");
            $("#tabsent").removeClass("active-link");
           
            $("#tabcompleted").removeClass("active-link");
            
            $("#tabdraft").removeClass("active-link");

            $("#sent").removeClass("active-tab");
            $("#inbox").addClass("active-tab");
            $("#Completed").removeClass("active-tab");
           
        } else if (param == "XR") {
            $("#tabinbox").removeClass("active-link");
            $("#tabsent").removeClass("active-link");
            $("#tabcompleted").addClass("active-link");
            $("#tabdraft").removeClass("active-link");

            $("#sent").removeClass("active-tab");
            $("#inbox").removeClass("active-tab");
            $("#completed").addClass("active-tab");
           
        }
        sessionStorage.setItem("spntabType", null);
    }


    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
  return new bootstrap.Tooltip(tooltipTriggerEl)
})
   

    $(".processDetail").click(function () {
        var $row = $(this).closest('tr');
        var ProjId = $row.find("#SpnCurrentProjId").text().trim();
        var psmId = $row.find("#SpnCurrentpsmId").text().trim();
        $('#confirmationModal').modal('show');

        $('#confirmSend').off('click').on('click', function () {
            var dateValue = $('#datepicker').val();
            var currentDate = new Date();

            // Add server's current time if only a date is selected
            var FwdDateForComment = '';
            if ($('#datepicker').attr('type') === 'date') {
                //if (!dateValue) {
                //    alert('Please select a date .');
                //    return;
                //}
                //var currentTime = currentDate.toTimeString().split(' ')[0]; // Get current time in HH:mm:ss
                //FwdDateForComment = dateValue + ' ' + currentTime;
                const formattedDate = currentDate.toLocaleString("sv-SE").replace("T", " ");
                FwdDateForComment = formattedDate;

            } else if ($('#datepicker').attr('type') === 'datetime-local') {
                if (!dateValue) {
                    alert('Please select date and time.');
                    return;
                }
                FwdDateForComment = dateValue.replace('T', ' '); // Format datetime-local to space-separated
            }
            $('#confirmationModal').modal('hide');
            SentForComment(ProjId, psmId, 0, FwdDateForComment);
            AddNotification(ProjId, 1, 0);
            ProcessProjConfirm(ProjId);
        });
       
        IsReadInbox($(this).closest("tr").find("#SpnCurrentpsmId").html());
       
        IsReadNotification($(this).closest("tr").find("#SpnCurrentProjId").html(), 2);
    });

    $('#x').on('hidden.bs.modal', function () {
        $('#datepicker').datepicker('setDate', null);
    });
});

function GetForCommentStakeHolder(ProjId, psmId) {

    $.ajax({
        url: '/UnitDtls/GetAllStakeHolderComment',
        type: 'POST',
        data: { "Id": 0 },
        success: function (response) {
            //console.log(response);


            if (response != null) {

                for (var i = 0; i < response.length; i++) {
                    SentForComment(ProjId, psmId, response[i].unitid)

                }
                //SentForComment(ProjId, psmId);

                ProcessProjConfirm(ProjId)
            }

        }
    });
}

function SentForComment(ProjId, psmId, unitid, FwdDateForComment) {
    $.ajax({
        url: '/Projects/ProcessMail',
        type: 'POST',
        data: {
            "ProjId": ProjId,
            "FwdDateForComment": FwdDateForComment,
            "unitid": unitid, "__RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (response) {
            //console.log(response);
            if (response && response === 1) {
                Swal.fire({
                    position: 'top-end',
                    icon: 'success',
                    title: 'Project Processed successfully',
                    showConfirmButton: false,
                    timer: 700
                });
            }

            window.location.reload();
        },
        error: function (error) {
            console.error('Error occurred:', error);
            // Handle error if needed
        }
    });
}
function SentForNotification(ProjId, psmId, unitid, FwdDateForComment) {
    $.ajax({
        url: '/Projects/ProcessNotification',
        type: 'POST',
        data: {
            "ProjId": ProjId,
            "FwdDateForComment": FwdDateForComment,
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

            window.location.reload();
        },
        error: function (error) {
            console.error('Error occurred:', error);
            // Handle error if needed
        }
    });
}
function ProcessProjConfirm(ProjId) {
    $.ajax({
        url: '/Projects/IsProcessProjConfirm',
        type: 'POST',
        data: { "ProjId": ProjId },
        success: function (response) {
            //console.log(response);
            if (response >= 1) {
                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Project Successfully Submitted..!",
                    showConfirmButton: false,
                    timer: 1500
                });

            }
        }
    });
}
function FwdProjConfirm(psmId) {
        $.ajax({
            url: '/Projects/FwdProjConfirm',
            type: 'POST',
            data: { "PslmId": psmId },
            success: function (response) {
                //console.log(response);
                if (response >= 1) {
                    Swal.fire({
                        position: "top-end",
                        icon: "success",
                        title: "Project Successfully Submitted..!",
                        showConfirmButton: false,
                        timer: 1500
                    });

                }
            }
        });
    }

function GetProjectMovHistory(ProjId) {
    var listitem = "";

    $.ajax({
        url: '/Projects/ProjectMovHistory',
        type: 'POST',
        data: { "ProjectId": ProjId },
        success: function (response) {
            /*console.log(response);*/
            if (response.dtoProjectMovHistorypsmlst.length) {
                listitem += '<div class="timeline-month">';
                
                /*listitem += '  ' + DateTimeFormatedd_mm_yyyy(new Date($.now())) + '';*/
                DTOProjectMovHistorypsmlst = response.dtoProjectMovHistorypsmlst;
                DTOProjectMovHistorycmdlst = response.dtoProjectMovHistorycmdlst;
                listitem += '  ' + DateTimeFormatedd_mm_yyyy(new Date($.now())) + '';
                listitem += '<span>' + DTOProjectMovHistorypsmlst.length + ' Entries</span>';
                listitem += '</div>';
                for (var i = 0; i < DTOProjectMovHistorypsmlst.length; i++) {

                    listitem += '<div class="timeline-section"> <div class="timeline-date"> ' + DateTimeFormatedd_mm_yyyy(DTOProjectMovHistorypsmlst[i].date) + '</div>';
                    //listitem += '<div class="timeline-section"> <div class="timeline-date"> ' + DateTimeFormatedd_dd_mm_yyyy(DTOProjectMovHistorypsmlst[i].date) + '</div>';

                    /* listitem += '<div class="timeline-section"> <div class="timeline-date"> ' + DateFormatedd_mm_yyyy(DTOProjectMovHistorypsmlst[i].date) + '</div>';*/
                    listitem += '<div class="row"><div class="col-sm-4">';
                    listitem += '<div class="timeline-box">';
                    if (DTOProjectMovHistorypsmlst[i].isComment == false) {
                        if (DTOProjectMovHistorypsmlst[i].actions == "FWD" && (DTOProjectMovHistorypsmlst[i].undoRemarks == "" || DTOProjectMovHistorypsmlst[i].undoRemarks == null))
                            listitem += '<div class="box-title bg-warning  text-white"><i class="fa-solid fa-forward" style="color: #FFD43B;"></i> ' + DTOProjectMovHistorypsmlst[i].actions + '</div>';
                        else if (DTOProjectMovHistorypsmlst[i].actions == "Obsn")
                            listitem += '<div class="box-title bg-warning text-white"><i class="fa-solid fa-rotate-left fa-xl" style="color: #ffff;"></i> ' + DTOProjectMovHistorypsmlst[i].actions + '</div>';

                        else if (DTOProjectMovHistorypsmlst[i].undoRemarks == "" || DTOProjectMovHistorypsmlst[i].undoRemarks == null)
                            listitem += '<div class="box-title bg-success text-white"><i class="fa-solid fa-circle-check fa-xl" style="color: #3adb00;"></i> ' + DTOProjectMovHistorypsmlst[i].actions + '</div>';
                        else
                            listitem += '<div class="box-title bg-danger text-white"><i class="fa-solid fa-rotate-left fa-xl" style="color: #ffff;"></i> ' + DTOProjectMovHistorypsmlst[i].actions + '</div>';



                        listitem += '<div class="box-content">';

                        listitem += '<div class="row">';
                        listitem += '<div class="col-md-4">';
                        listitem += '<div class="box-item"><strong>Stage</strong>: </div >';
                        listitem += '</div>';

                        listitem += '<div class="col-md-8">';
                        listitem += '<div class="box-item"><span class="badge rounded-pill bg-primary">' + DTOProjectMovHistorypsmlst[i].stages + '</span></div>';
                        listitem += '</div>';
                        listitem += '</div>';

                        listitem += '<div class="row">';
                        listitem += '<div class="col-md-4">';
                        listitem += '<div class="box-item"><strong>Sub Stage</strong>: </div >';
                        listitem += '</div>';

                        listitem += '<div class="col-md-8">';
                        listitem += '<div class="box-item"><span class="badge rounded-pill bg-warning text-dark">' + DTOProjectMovHistorypsmlst[i].status + '</span></div>'; /*+ DTOProjectMovHistorypsmlst[i].status + */
                        listitem += '</div>';
                        listitem += '</div>';


                        listitem += '<div class="row">';
                        listitem += '<div class="col-md-4">';
                        listitem += '<div class="box-item"><strong>From</strong>: </div >';
                        listitem += '</div>';

                        listitem += '<div class="col-md-8">';
                        listitem += '<div class="box-item"><span class="rounded-pill bg-secondary" style="color: white;">' + DTOProjectMovHistorypsmlst[i].fromUnitName + '</span></div>';
                        listitem += '</div>';
                        listitem += '</div>';


                        listitem += '<div class="row">';
                        listitem += '<div class="col-md-4">';
                        listitem += '<div class="box-item"><strong>TO</strong>: </div >';
                        listitem += '</div>';

                        listitem += '<div class="col-md-8">';
                        listitem += '<div class="box-item"><span class="badge rounded-pill bg-secondary">' + DTOProjectMovHistorypsmlst[i].toUnitName + '</span></div>';
                        listitem += '</div>';
                        listitem += '</div>';


                        listitem += '</div>';
                        listitem += '<div class="box-footer">' + DTOProjectMovHistorypsmlst[i].userDetails + '</div>';
                        listitem += '</div></div>';




                    }
                    else if (DTOProjectMovHistorypsmlst[i].isComment == true) {
                        listitem += '<div class="box-title bg-danger text-white"><i class="fa-solid fa-comments fa-xl" style="color: #ffff;"></i> ' + DTOProjectMovHistorypsmlst[i].toUnitName + ' for Comments</div>';


                        listitem += '<div class="box-content">';

                        listitem += '<div class="row">';
                        listitem += '<div class="col-md-4">';
                        listitem += '<div class="box-item"><strong>Stage</strong>: </div >';
                        listitem += '</div>';

                        listitem += '<div class="col-md-8">';
                        listitem += '<div class="box-item"><span class="badge rounded-pill bg-primary">' + DTOProjectMovHistorypsmlst[i].stages + '</span></div>';
                        listitem += '</div>';
                        listitem += '</div>';

                        listitem += '<div class="row">';
                        listitem += '<div class="col-md-4">';
                        listitem += '<div class="box-item"><strong>Sub Stage</strong>: </div >';
                        listitem += '</div>';

                        listitem += '<div class="col-md-8">';
                        listitem += '<div class="box-item"><span class="badge rounded-pill bg-warning text-dark">' + DTOProjectMovHistorypsmlst[i].toUnitName + ' For Comments</span></div>'; /*+ DTOProjectMovHistorypsmlst[i].status + */
                        listitem += '</div>';
                        listitem += '</div>';


                        listitem += '<div class="row">';
                        listitem += '<div class="col-md-4">';
                        listitem += '<div class="box-item"><strong>From</strong>: </div >';
                        listitem += '</div>';

                        listitem += '<div class="col-md-8">';
                        listitem += '<div class="box-item"><span class="rounded-pill bg-secondary" style="color: white;">' + DTOProjectMovHistorypsmlst[i].fromUnitName + '</span></div>';
                        listitem += '</div>';
                        listitem += '</div>';


                        listitem += '<div class="row">';
                        listitem += '<div class="col-md-4">';
                        listitem += '<div class="box-item"><strong>TO</strong>: </div >';
                        listitem += '</div>';

                        listitem += '<div class="col-md-8">';
                        listitem += '<div class="box-item"><span class="badge rounded-pill bg-secondary">' + DTOProjectMovHistorypsmlst[i].toUnitName + '</span></div>';
                        listitem += '</div>';
                        listitem += '</div>';
                        listitem += '</div>';
                        listitem += '<div class="box-footer ">' + DTOProjectMovHistorypsmlst[i].userDetails + '</div>';
                        listitem += '</div></div>';

                        var DTODashboardCount = DTOProjectMovHistorycmdlst.filter(function (element) { return element.psmId == DTOProjectMovHistorypsmlst[i].psmId; });


                        for (var c = 0; c < DTODashboardCount.length; c++) {
                            listitem += '<div class="col-sm-4">';
                            listitem += '<div class="timeline-box">';
                            listitem += '<div class="box-title">';
                            listitem += '<i class="fa fa-pencil text-info" aria-hidden="true"></i>  Comment On ' + DateFormateddMMyyyyhhmmss(DTODashboardCount[c].dateTimeOfUpdate) + '';
                            listitem += '</div>';
                            listitem += '<div class="box-content">';
                            if (DTODashboardCount[c].comments.length > 75)
                                listitem += '<div class="box-item" data-toggle="tooltip" data-placement="top" title="' + DTODashboardCount[c].comments + '">' + DTODashboardCount[c].comments.substring(0, 75) + ' ........</div>';
                            else
                                listitem += '<div class="box-item" >' + DTODashboardCount[c].comments + ' </div>';

                            listitem += '</div>';
                            if (DTODashboardCount[c].status == "Obsn")
                                listitem += '<div class="box-footer bg-warning">' + DTODashboardCount[c].status + ' by ' + DTODashboardCount[c].userDetails + '</div>'
                            else if (DTODashboardCount[c].status == "Observation" || DTODashboardCount[c].status == "Rejected")
                                listitem += '<div class="box-footer bg-danger">' + DTODashboardCount[c].status + ' by ' + DTODashboardCount[c].userDetails + '</div>';
                            else if (DTODashboardCount[c].status == "Accepted")
                                listitem += '<div class="box-footer bg-success ">' + DTODashboardCount[c].status + ' by ' + DTODashboardCount[c].userDetails + '</div>';
                            else
                                listitem += '<div class="box-footer">' + DTODashboardCount[c].status + ' by ' + DTODashboardCount[c].userDetails + '</div>';
                            listitem += '</div></div>';
                        }
                    }
                  
                    if (DTOProjectMovHistorypsmlst[i].remarks != "") {
                        //console.log(DTOProjectMovHistorypsmlst);
                        listitem += '<div class="col-sm-4">';
                        listitem += '<div class="timeline-box">';
                        listitem += '<div class="box-title">';
                        listitem += '<i class="fa fa-pencil text-info" aria-hidden="true"></i> Remarks On ' + DateTimeFormatedd_mm_yyyy(DTOProjectMovHistorypsmlst[i].date);
                        listitem += '</div>';
                        listitem += '<div class="box-content">';
                        /*listitem += ' <a class="btn btn-xs btn-default pull-right">Remarks</a>';*/

                        //console.log("IsPulledBack:", DTOProjectMovHistorypsmlst[i]?.IsPulledBack);
                        //console.log("UndoRemarks:", DTOProjectMovHistorypsmlst[i]?.UndoRemarks);
                        if (DTOProjectMovHistorypsmlst[i]?.isPulledBack === true && DTOProjectMovHistorypsmlst[i]?.undoRemarks == null) {
                            listitem += '<div class="box-item">' + '<strong>Pulled Back Remarks</strong> -  ' + DTOProjectMovHistorypsmlst[i].remarks + '</div>';
                        } else {
                            listitem += '<div class="box-item">' + DTOProjectMovHistorypsmlst[i].remarks +'</div>';
                        }

                        //listitem += '<div class="box-item">' + DTOProjectMovHistorypsmlst[i].remarks + '</div>';
                        listitem += '</div>';
                        if (DTOProjectMovHistorypsmlst[i].actions == "Obsn")
                            listitem += '<div class="box-footer bg-warning">' + DTOProjectMovHistorypsmlst[i].userDetails + '</div>';
                        else
                            listitem += '<div class="box-footer ">' + DTOProjectMovHistorypsmlst[i].userDetails + '</div>';
                        listitem += '</div></div>';
                    }
                    //if (DTOProjectMovHistorypsmlst[i].undoRemarks != null) {
                    //    listitem += '<div class="col-sm-4">';
                    //    listitem += '<div class="timeline-box">';
                    //    listitem += '<div class="box-title">';
                    //    listitem += '<i class="fa fa-pencil text-info" aria-hidden="true"></i>Undo Remarks';
                    //    listitem += '</div>';
                    //    listitem += '<div class="box-content">';
                    //    listitem += '<div class="box-item">' + DTOProjectMovHistorypsmlst[i].undoRemarks + '</div>';
                    //    listitem += '</div>';
                    //    listitem += '<div class="box-footer">' + DTOProjectMovHistorypsmlst[i].userDetails + '</div>';
                    //    listitem += '</div></div>';
                    //}
                    listitem += '</div></div>';
                    listitem += '';
                    listitem += '';
                }


                $("#projectmovfistory").html(listitem);

                //$(document).on('click', function () {
                //    location.reload();
                //});
            }
        }
    });
}
function Reset() {
    $("#spanFwdProjectId").html(0);
    $("#ddlfwdStage").val("");
    $("#ddlfwdSubStage").val("");
    $("#ddlfwdAction").val("");
    $("#txtRemarksfwd").val("");
    $("#ddlfwdFwdTo").val(0);
    $("#Reamarks").val("");
    $("#pdfFileInput").val("");
}
function IsReadInbox(psmId) {
   
    $.ajax({
        url: '/Projects/IsReadInbox',
        type: 'POST',
        data: { "PsmId": psmId },
        success: function (response) {
            console.log(response);
            
        }
    });
}

//function IsReadNotificationInbox(ProjId) {

//    $.ajax({
//        url: '/Projects/IsReadNotificationInbox',
//        type: 'POST',
//        data: { "PsmId": ProjId },
//        success: function (response) {
//            console.log(response);

//        }
//    });
//}

