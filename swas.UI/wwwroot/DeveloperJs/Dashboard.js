var tittle = "";
var Status = "";
$(document).ready(function () {
    GetAllDashbaordCount();

    $("#IsNotduplicate").change(function () {


        var ischecked = $(this).is(':checked');
        if (ischecked)
            getProjGetsummay($("#spndashboardstatusId").html(), false);
        else
            getProjGetsummay($("#spndashboardstatusId").html(), true);
    });

    $('[data-toggle="tooltip"]').tooltip();

});




function GetAllDashbaordCount() {
    $.ajax({
        type: "POST",
        url: '/Home/GetDashboardCount',
        data: {
            "Id": 0,

        },
        success: function (data) {
             //console.log("DashboardData01", data)
            var dtoDashboardHeaderlst = data.dtoDashboardHeaderlst;
            var dTOApprovedCountlst = data.dtoApprovedCountlst;
            var dTODashboardCountlstForAction = data.dtoDashboardCountlstForAction;

            var listitem = '';
            var stageId = 0;
            var tot = 0;
            var peding = 0;
            var sent = 0;
            if (data != null) {
                dtoDashboardHeaderlst.sort((a, b) => a.stageId - b.stageId || a.statseq - b.statseq);

                for (var i = 0; i < dtoDashboardHeaderlst.length; i++) {
                    if (stageId != dtoDashboardHeaderlst[i].stageId) {
                        if (stageId != 0) {
                            listitem += '</div>';
                        }
                        listitem += '<div class="newprojectheading text-center"> ' + dtoDashboardHeaderlst[i].stages + ' </div>';

                        listitem += '<div class="r-1" style="margin-top: 13px;">';
                    }


                    listitem += '<div class="cd-1  " style="background-color:white">';

                    tot = 0;
                    peding = 0;
                    sent = 0;
                    var icons = "";
                    var DTODashboardCount = data.dtoDashboardCountlst.filter(function (element) { return element.stagesId == dtoDashboardHeaderlst[i].stageId && element.statusId == dtoDashboardHeaderlst[i].statusId; });
                    if (parseInt(dtoDashboardHeaderlst[i].statusId) == 2 || parseInt(dtoDashboardHeaderlst[i].statusId) == 3
                        || parseInt(dtoDashboardHeaderlst[i].statusId) == 22 || parseInt(dtoDashboardHeaderlst[i].statusId) == 31
                        || parseInt(dtoDashboardHeaderlst[i].statusId) == 37) {

                        if (parseInt(dtoDashboardHeaderlst[i].statusId) == 2)
                            var ForAction = dTODashboardCountlstForAction.filter(function (e) { return e.actionId == 10 });
                        else if (parseInt(dtoDashboardHeaderlst[i].statusId) == 3)
                            var ForAction = dTODashboardCountlstForAction.filter(function (e) { return e.actionId == 11 });
                        else if (parseInt(dtoDashboardHeaderlst[i].statusId) == 22)
                            var ForAction = dTODashboardCountlstForAction.filter(function (e) { return e.actionId == 3 && e.stagesId == 2 });
                        else if (parseInt(dtoDashboardHeaderlst[i].statusId) == 31)
                            var ForAction = dTODashboardCountlstForAction.filter(function (e) { return e.actionId == 3 && e.stagesId == 3 });
                        else if (parseInt(dtoDashboardHeaderlst[i].statusId) == 37)
                            var ForAction = dTODashboardCountlstForAction.filter(function (e) { return e.actionId == 3 && e.stagesId == 1 });

                        for (var j = 0; j < ForAction.length; j++) {

                            tot += ForAction[j].tot;

                            if (ForAction[j].isComplete == false) {
                                peding += ForAction[j].tot;
                            }
                            if (ForAction[j].isComplete == true) {
                                sent += ForAction[j].tot;
                            }
                        }

                    }
                    else {
                        for (var j = 0; j < DTODashboardCount.length; j++) {


                            tot += DTODashboardCount[j].tot;

                            if (DTODashboardCount[j].isComplete == false) {
                                peding += DTODashboardCount[j].tot;
                            }
                            if (DTODashboardCount[j].isComplete == true) {
                                sent += DTODashboardCount[j].tot;
                            }

                        }
                    }

                    listitem += '<div class="icon-container ApprovedProj cursorpointer"><span class="d-none" id="spnstatusId">' + dtoDashboardHeaderlst[i].statusId + '</span>';
                    //listitem += '<div class="icon-container ApprovedProj cursorpointer"><span class="d-none" id="spnstatus">' + dtoDashboardHeaderlst[i].status + '</span>';
                    /* listitem += '<img src="/assets/images/icons/' + dtoDashboardHeaderlst[i].icons +'" alt="Icon" style="height:25px">';*/
                    if (dtoDashboardHeaderlst[i].statusId == 2 || dtoDashboardHeaderlst[i].statusId == 3 || dtoDashboardHeaderlst[i].statusId == 22 || dtoDashboardHeaderlst[i].statusId == 31 || dtoDashboardHeaderlst[i].statusId == 37) {
                        if (dtoDashboardHeaderlst[i].statusId == 3)
                            listitem += '<img src="/assets/images/icons/prog.png" alt="Icon" style="height:25px" data-toggle="tooltip" data-placement="top" title="Total No of proj approved at this stage">';
                        else listitem += '<img src="/assets/images/icons/rejec.png" alt="Icon" style="height:25px">';
                        listitem += '<h5 style="margin-top: 25px;" > </h5>';
                    } else {
                        listitem += '<img src="/assets/images/icons/prog.png" alt="Icon" style="height:25px" data-toggle="tooltip" data-placement="top" title="Total No of proj approved at this stage">';
                        var approvedcount = dTOApprovedCountlst.filter(function (element) { return element.statusId == dtoDashboardHeaderlst[i].statusId; });
                        if (approvedcount.length > 0)
                            /* listitem += '<span class="d-none" id="spnstatusActionsMappingId">' + approvedcount[0].statusActionsMappingId + '</span><h5 style="margin-top: 8px;" >' + approvedcount[0].total + ' </h5>';*/
                            if (dtoDashboardHeaderlst[i].status.includes("BISAG-N")) {
                                listitem += '<span class="d-none" id="spnstatusActionsMappingId">' + approvedcount[0].statusActionsMappingId + '</span>' +
                                    '<h5 style="margin-top: 8px; padding-top: 10px;" data-toggle="tooltip" data-placement="top" title="Total No of proj approved at this stage">' + approvedcount[0].total + ' </h5>';
                            }
                            else {
                                listitem += '<span class="d-none" id="spnstatusActionsMappingId">' + approvedcount[0].statusActionsMappingId + '</span>' +
                                    '<h5 style="margin-top: 8px;" data-toggle="tooltip" data-placement="top" title="Total No of proj approved at this stage">' + approvedcount[0].total + ' </h5>';    
                            }

                        else
                            listitem += '<span class="d-none" id="spnstatusActionsMappingId">0</span><h5 style="margin-top: 8px;" >0 </h5>';
                    }
                    listitem += '<div class="t-1 statusprojsummry d-none">' + dtoDashboardHeaderlst[i].status + '</div> ';
                    listitem += '</div>';
                    listitem += '<div class="cursorpointer btnGetsummay "><span class="d-none" id="spnstatusId">' + dtoDashboardHeaderlst[i].statusId + '</span>';
                    //listitem += '<div class="cursorpointer btnGetsummay "><span class="d-none" id="spnstatus">' + dtoDashboardHeaderlst[i].status + '</span>';
                    listitem += '<div class="">';

                    if (dtoDashboardHeaderlst[i].status.includes("BISAG-N")) {
                        listitem += '<div class="t-1 statusprojsummry" style="padding-top: 7px;" >' + dtoDashboardHeaderlst[i].status + '</div> ';
                    }
                    else {
                        listitem += '<div class="t-1 statusprojsummry">' + dtoDashboardHeaderlst[i].status + '</div> ';
                    }

                    if (dtoDashboardHeaderlst[i].status.includes("BISAG-N")) {

                        listitem += '<span class="badge badge-light text-black d-none" style="font-size:18px" data-toggle="tooltip" data-placement="top" ><span class="badge bg-danger">' + peding + '</span> / <span class="badge bg-success">' + sent + '</span></span>';

                        listitem += ' </div>';
                        listitem += '';
                        listitem += ' <div class="mb-2">';
                        listitem += '<span class="badge badge-primary mr-2 d-none" style="font-size:14px" data-toggle="tooltip" data-placement="top" title="Total No of transaction at this stage">' + tot + '</span>';

                    }
                    else {
                        listitem += '<span class="badge badge-light text-black" style="font-size:18px" data-toggle="tooltip" data-placement="top" ><span class="badge bg-danger">' + peding + '</span> / <span class="badge bg-success">' + sent + '</span></span>';


                        listitem += ' </div>';
                        listitem += '';
                        listitem += ' <div class="mb-2">';
                        listitem += '<span class="badge badge-primary mr-2" style="font-size:14px" data-toggle="tooltip" data-placement="top" title="Total No of transaction at this stage">' + tot + '</span>';
                    }

                    listitem += ' </div>';
                    listitem += ' </div>';

                    listitem += ' </div>';

                    listitem += '';

                    stageId = dtoDashboardHeaderlst[i].stageId;
                }



                $("#carddashboardcount").html(listitem);
                //$("body").on("click", ".ProjectWiseStatus", function () {
                //    $('#modalProjectWiseStatus').modal('show');
                //    ProjectWiseStatus()
                //});


                $("body").on("click", ".ApprovedProj", function () {
                    var spnstatusId = $(this).closest("div").find("#spnstatusId").html();
                    //var spnstatus = $(this).closest("div").find("#spnstatus").html();
                    var spnstatusActionsMappingId = $(this).closest("div").find("#spnstatusActionsMappingId").html();

                    tittle = "Total Proj Movement: " + $(this).closest("div").find(".statusprojsummry").html();
                    if (parseInt(spnstatusActionsMappingId) == 1) {

                        Status = 'Accepted';
                    }
                    else if (parseInt(spnstatusActionsMappingId) == 9) {
                        Status = 'obsn Raised';
                    }
                    else if (parseInt(spnstatusActionsMappingId) == 113) {
                        Status = 'Rectified';
                    }
                    else if (parseInt(spnstatusActionsMappingId) == 48 || parseInt(spnstatusActionsMappingId) == 53) {
                        Status = 'Approved';
                    }

                    else if (parseInt(spnstatusActionsMappingId) == 60) {
                        tittle = "Closed Project";
                        Status = 'Closed';
                    }
                    else if (parseInt(spnstatusActionsMappingId) == 68 || parseInt(spnstatusActionsMappingId) == 73 || parseInt(spnstatusActionsMappingId) == 63
                        || parseInt(spnstatusActionsMappingId) == 78 || parseInt(spnstatusActionsMappingId) == 83 || parseInt(spnstatusActionsMappingId) == 88) {
                        Status = 'Completed';
                    }

                    else if (parseInt(spnstatusActionsMappingId) == 26 || parseInt(spnstatusActionsMappingId) == 31 || parseInt(spnstatusActionsMappingId) == 37) {
                        Status = 'Approved';
                    }

                    if (parseInt(spnstatusActionsMappingId) == 0) {
                        Swal.fire({
                            icon: "error",
                            title: "Oops...",
                            text: "Data Not Found!",

                        });
                    } else {

                        if (spnstatusId == 2 || spnstatusId == 3 || spnstatusId == 22) {

                        } else if (spnstatusId == 1041) {
                            $('#ProjectApprovedTittle').html(tittle);
                            $('#BISAG-N').modal('show');
                            getProjBisagN(spnstatusId, spnstatusActionsMappingId);
                        } else {
                            $('#ProjectApprovedTittle').html(tittle);
                            $('#ProjApproved').modal('show');

                            getProjApproved(spnstatusId, spnstatusActionsMappingId);
                        }
                    }
                });
                $("body").on("click", ".btnGetsummay", function () {

                    var spnstatusId = $(this).closest("div").find("#spnstatusId").html();
                    if (spnstatusId != 1041) {
                        $('#ProjGetsummay').modal('show');
                        ($(this).closest("div").find(".statusprojsummry").html());
                        $('#ProjectSummaryTittle').html("Total Proj Movement: " + $(this).closest("div").find(".statusprojsummry").html());
                        $('#IsNotduplicate').prop('checked', false);

                        getProjGetsummay(spnstatusId, true);
                    }


                })
            }

        },
        error: function () {
            alert('Error fetching comments.');
        }
    });
}

$(document).ready(function () {
    // Fetching data for the first chart
    $.ajax({
        url: '/Home/indexToBarChartS',
        method: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.error) {
                console.error('Error fetching data:', data.error);
                return;
            }

            var monthNames = [...new Set(data.map(item => item.MonthNameYr))];
            var unitNames = [...new Set(data.map(item => item.unitname))];

            var datasets = unitNames.map(unitName => {
                var totalInData = [];
                var totalOutData = [];

                monthNames.forEach(month => {
                    var monthData = data.find(item => item.MonthNameYr === month && item.unitname === unitName);
                    if (monthData) {
                        totalInData.push(monthData.TotalIn);
                        totalOutData.push(monthData.TotalOut);
                    } else {
                        totalInData.push(0);
                        totalOutData.push(0);
                    }
                });

                var totalInColor = getRandomColor();
                var totalOutColor = getRandomColor();

                return [{
                    label: unitName + ' Proj In',
                    data: totalInData,
                    backgroundColor: totalInColor,
                    stack: unitName,
                }, {
                    label: unitName + ' Proj Out',
                    data: totalOutData,
                    backgroundColor: totalOutColor,
                    stack: unitName,
                }];

            }).flat();

            var ctx = document.getElementById('myChart').getContext('2d');
            var myChart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: monthNames,
                    datasets: datasets
                },
                options: {
                    scales: {
                        x: {
                            stacked: true,
                            title: {
                                display: true,
                                text: 'Month Name'
                            }
                        },
                        y: {
                            stacked: true,
                            title: {
                                display: true,
                                text: 'Total In/Total Out'
                            }
                        }
                    }
                }
            });

            // Set the title for the first chart
            $('#chartTitle').text('Bar Chart Title');
        }
    });

    // Fetching data for the second chart
    $.ajax({
        url: '/Home/indexToPieChart',
        method: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.error) {
                console.error('Error fetching data:', data.error);
                return;
            }

            updatePieChart(data);

            // Set the title for the second chart
            $('#chart1Title').text('Pie Chart Title');
        },
        error: function (error) {
            console.error('Error fetching data:', error);
        }
    });
});


//function getProjGetsummay(spnstatusId) {
//    var listItem = "";
//    var userdata =
//    {
//        "StatusId": spnstatusId,

//    };
//    $.ajax({
//        url: '/Home/GetDashboardStatusDetails',
//        contentType: 'application/x-www-form-urlencoded',
//        data: userdata,
//        type: 'POST',

//        success: function (response) {
//            if (response != "null" && response != null) {

//                if (response == -1) {
//                    Swal.fire({
//                        text: ""
//                    });
//                }
//                else if (response == 0) {
//                    listItem += "<tr><td class='text-center' colspan=7>No Record Found</td></tr>";

//                    $("#DetailBodysummary").html(listItem);
//                    $("#lblTotal").html(0);
//                }

//                else {

//                    var count = 1;
//                    for (var i = 0; i < response.length; i++) {

//                        listItem += "<tr>";

//                        listItem += "<td class='align-middle'><span id='ProjName'>" + count + "</span></td>";
//                        listItem += "<td class='align-middle'><span id='ProjName'>" + response[i].projName + "</span></td>";
//                        listItem += "<td class='align-middle'><span id='ProjName'>" + response[i].stakeHolder + "</span></td>";
//                        listItem += "<td class='align-middle'><span id='ProjName'>" + response[i].fromUnitName + "</span></td>";
//                        listItem += "<td class='align-middle'><span id='ProjName'>" + response[i].toUnitName + "</span></td>";

//                        listItem += "<td class='align-middle'><span id='divName'>" + response[i].action + "</span></td>";

//                        if (response[i].isComplete) {
//                            listItem += "<td ><span class='badge badge-success' id='divName'>Accepted</span></td>";
//                        } else {
//                            if (response[i].stkStatusId == 2) {
//                                listItem += "<td ><span class='badge badge-warning' id='divName'>Obsn</span></td>";
//                            }
//                            else if (response[i].stkStatusId == 3) {
//                                listItem += "<td ><span class='badge badge-danger' id='divName'>Rejected</span></td>";

//                            }
//                            else {
//                                listItem += "<td ><span class='badge badge-danger' id='divName'>Pending</span></td>";
//                            }
//                        }

//                        listItem += "</tr>";
//                        count++;
//                    }

//                    $("#DetailBodysummary").html(listItem);




//                }
//            }
//            else {
//                listItem += "<tr><td class='text-center' colspan=7>No Record Found</td></tr>";

//                $("#DetailBodysummary").html(listItem);

//            }
//        },
//        error: function (result) {
//            Swal.fire({
//                text: ""
//            });
//        }
//    });

//}

function getProjApproved(spnstatusId, spnstatusActionsMappingId) {
    var listItem = "";
    var table = new DataTable('#dashboardApproved');
    table.destroy();
    /* alert(spnstatusActionsMappingId)*/
    var userdata = {
        "StatusId": spnstatusId,
        "statusActionsMappingId": spnstatusActionsMappingId,
    };

    $.ajax({
        url: '/Home/GetDashboardApproved',
        contentType: 'application/x-www-form-urlencoded',
        data: userdata,
        type: 'POST',
        success: function (response) {
            //console.log("GetDashboardApprovedData", response);
            if (response != "null" && response != null) {

                if (response == -1) {
                    Swal.fire({ text: "" });
                } else if (response == 0) {

                    $('#DetailBodyApproved').empty();
                    listItem += "<tr><td class='text-center' colspan='7'>No Record Found</td></tr>";
                    $("#DetailBodyApproved").html(listItem);
                    $("#lblTotal").html(0);


                } else {
                    var count = 1;

                    var unitId = $('#spndashboardUnitId').text().trim();
                    //alert(unitId);
                    
                    $('#DetailBodyApproved').empty();
                    $('#dashboardApproved').dataTable().fnClearTable();
                    $('#dashboardApproved').dataTable().fnDestroy();
                    for (var i = 0; i < response.length; i++) {

                        var projName = response[i].projName;
                        var words = projName.split(" ");
                        var shortProjName = words.length > 6 ? words.slice(0, 6).join(" ") + "..." : projName;

                        listItem += "<tr>";
                        listItem += "<td class='align-middle'>" + count + "</td>";
                        //listItem += "<td class='align-middle nowrap'><span id='ProjName'>" + response[i].projName + "</span></td>";
                        if (unitId == 1 || unitId == 2 || unitId == 3 || unitId == 4 || unitId == 5 || unitId == 7) {
                            listItem += "<td class='align-middle nowrap'>" +
                                "<a class='ProjName' title='" + shortProjName + "' data-proj-id='" + response[i].projId + "' data-proj-name='" + shortProjName + "' " +
                                "href='/Projects/ProjHistory?EncyID=" + response[i].encyID + "&Type=XRDC'>" +
                                shortProjName + "</a></td>";
                        } else {
                            listItem += "<td class='align-middle nowrap'><span id='ProjName'>" + shortProjName + "</span></td>";
                        }

                        listItem += "<td class='align-middle'><span id='ProjName'>" + response[i].stakeHolder + "</span></td>";
                        listItem += "<td class='align-middle'><span id='ProjName'>" + DateFormateddMMyyyyhhmmss(response[i].timeStamp) + "</span></td>";
                        //if (response[i].status != "BISAG-N") {
                        listItem += "<td ><span class='badge badge-success' id='divName'>" + Status + "</span></td>";
                        //}
                        listItem += "</tr>";
                        count++;
                    }

                    $("#DetailBodyApproved").html(listItem);

                    var table = $('#dashboardApproved').DataTable({
                        lengthChange: true,
                        retrieve: true,
                        bDestroy: true,

                        searching: true,
                        stateSave: true,
                        "order": [[0, "asc"]],
                        "ordering": true,
                        "paging": true,
                        dom: 'lBfrtip',
                        buttons: [
                            'copy',
                            'excel',
                            'csv',
                            //{
                            //    text: 'PDF',
                            //    extend: 'pdfHtml5',
                            //    action: function (e, dt, node, config) {
                            //        PdfDiv();
                            //    }
                            //},
                        ],
                        searchBuilder: {
                            conditions: {
                                num: {
                                    'MultipleOf': {
                                        conditionName: 'Multiple Of',
                                        init: function (that, fn, preDefined = null) {
                                            var el = $('<input/>').on('input', function () { fn(that, this) });

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
                    //var memberTable = $('#dashboardApproved').DataTable({
                    //    retrieve: true,
                    //    bDestroy: true,
                    //    lengthChange: false,
                    //    searching: true,
                    //    stateSave: true,
                    //    "order": [[0, "asc"]],
                    //    "ordering": true,
                    //    "paging": true,
                    //});

                }
            }
            else {

                $('#DetailBodyApproved').empty();
                listItem += "<tr><td class='text-center' colspan='7'>No Record Found</td></tr>";
                $("#DetailBodyApproved").html(listItem);



            }
        },
        error: function (result) {
            Swal.fire({ text: "" });
        }
    });
}
function getProjGetsummay(spnstatusId, IsDuplicate) {
    var listItem = "";

    $("#spndashboardstatusId").html(spnstatusId);
    var userdata = {
        "StatusId": spnstatusId,
        "IsDuplicate": IsDuplicate
    };

    $.ajax({
        url: '/Home/GetDashboardStatusDetails',
        contentType: 'application/x-www-form-urlencoded',
        data: userdata,
        type: 'POST',
        success: function (response) {
            //console.log("GetDashboardStatusDetails", response);
            if (response != "null" && response != null) {

                if (response == -1) {
                    Swal.fire({ text: "" });
                } else if (response == 0) {

                    $('#DetailBodysummary1').empty();
                    listItem += "<tr><td class='text-center' colspan='10'>No Record Found</td></tr>";
                    $("#DetailBodysummary1").html(listItem);
                    $("#lblTotal").html(0);


                } else {
                    var count = 1;
                    $('#dashboardDeatils').dataTable().fnClearTable();
                    $('#dashboardDeatils').dataTable().fnDestroy();

                    var unitId = $('#spndashboardUnitId').text().trim();
                    for (var i = 0; i < response.length; i++) {
                        
                        var projName = response[i].projName;
                        var words = projName.split(" ");
                        var shortProjName = words.length > 6 ? words.slice(0, 6).join(" ") + "..." : projName;

                        listItem += "<tr>";
                        listItem += "<td class='align-middle'><span id='ProjName'>" + count + "</span></td>";
                        //listItem += "<td class='align-middle'><span id='ProjName'>" + response[i].projName + "</span></td>";
                        //listItem += "<td class='align-middle'><span id='ProjName' title='" + projName + "'>" + shortProjName + "</span></td>";
                        if (unitId == 1 || unitId == 2 || unitId == 3 || unitId == 4 || unitId == 5 || unitId == 7/*|| response[i].stakeHolder == unitId*/) {
                            listItem += "<td class='align-middle'>" +
                                "<a class='ProjName' title='" + projName + "' data-proj-id='" + response[i].projId + "' data-proj-name='" + projName + "' " +
                                "href='/Projects/ProjHistory?EncyID=" + response[i].encyID + "&Type=XRDC'>" +
                                shortProjName + "</a></td>";
                        }
                        else {
                            listItem += "<td class='align-middle'><span id='ProjName' title='" + projName + "'>" + shortProjName + "</span></td>";
                        }                       
                        listItem += "<td class='align-middle'><span id='ProjName'>" + response[i].stakeHolder + "</span></td>";
                        listItem += "<td class='align-middle'><span id='ProjName'>" + response[i].fromUnitName + "</span></td>";
                        listItem += "<td class='align-middle'><span id='ProjName'>" + response[i].toUnitName + "</span></td>";
                        listItem += "<td class='align-middle'><span id='ProjName'>" + response[i].stage + "</span></td>";
                        listItem += "<td class='align-middle'><span id='ProjName'>" + response[i].status + "</span></td>";
                        listItem += "<td class='align-middle'><span id='divName'>" + response[i].action + "</span></td>";
                        listItem += "<td class='align-middle'><span id='divName'>" + DateFormateddMMyyyyhhmmss(response[i].dateTimeOfUpdate) + "</span></td>";
                        //listItem += "<td class='align-middle'><span id='divName'>" + DateFormateyyy_mm_dd(response[i].dateTimeOfUpdate) + "</span></td>";
                       
                        if (response[i].isComplete) {
                            
                            if (response[i].stkStatusId == 2) {
                                listItem += "<td ><span class='badge badge-warning' id='divName'>Obsn</span></td>";
                            } else if (response[i].stkStatusId == 3) {
                                listItem += "<td ><span class='badge badge-danger' id='divName'>Rejected</span></td>";
                            }
                            else if (response[i].stkStatusId == 5) {
                                listItem += "<td ><span class='badge badge-success' id='divName'>Info</span></td>";
                            }
                            else {
                                //listItem += "<td ><span class='badge badge-success' id='divName'>Processed</span></td>";
                                listItem += `<td ><span class='badge badge-success' id='divName' title='Processed by ${response[i].toUnitName}'>${response[i].toUnitName}</span></td>`;
                            }
                        }
                        else {
                            if (response[i].stkStatusId == 2) {
                                listItem += "<td ><span class='badge badge-warning' id='divName'>Obsn</span></td>";
                            } else if (response[i].stkStatusId == 3) {
                                listItem += "<td ><span class='badge badge-danger' id='divName'>Rejected</span></td>";
                            }
                            else if (response[i].stkStatusId == 5) {
                                listItem += "<td ><span class='badge badge-success' id='divName'>Info</span></td>";
                            } else {
                                //listItem += "<td ><span class='badge badge-danger' id='divName'>Pending</span></td>";
                                listItem += `<td ><span class='badge badge-danger' id='divName' title='Pending with ${response[i].toUnitName}'>${response[i].toUnitName}</span></td>`;
                            }
                        }

                        listItem += "</tr>";
                        count++;
                    }

                    $("#DetailBodysummary1").html(listItem);


                    var table = $('#dashboardDeatils').DataTable({
                        lengthChange: true,
                        retrieve: true,
                        bDestroy: true,
                        destroy: true,
                        searching: true,
                        /* stateSave: true,*/
                        "order": [[0, "asc"]],
                        "ordering": true,
                        "paging": true,
                        pageLength: 25,
                        dom: 'lBfrtip',
                        buttons: [
                            'copy',
                            'excel',
                            'csv',
                            //{
                            //    text: 'PDF',
                            //    extend: 'pdfHtml5',
                            //    action: function (e, dt, node, config) {
                            //        PdfDiv();
                            //    }
                            //},
                        ],
                        searchBuilder: {
                            conditions: {
                                num: {
                                    'MultipleOf': {
                                        conditionName: 'Multiple Of',
                                        init: function (that, fn, preDefined = null) {
                                            var el = $('<input/>').on('input', function () { fn(that, this) });

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

                    //setTimeout(function () {   //calls click event after a certain time
                    //    $('input[type=search]').val('');
                    //    $('input[type=search]').trigger("click");
                    //    $('input[type=search]').focus();
                    //}, 1000);

                }
            }
            else {

                $('#DetailBodysummary1').empty();
                listItem += "<tr><td class='text-center' colspan='10'>No Record Found</td></tr>";
                $("#DetailBodysummary1").html(listItem);



            }
        },
        error: function (result) {
            Swal.fire({ text: "" });
        }
    });
}

function updatePieChart(data) {
    var titles = data.map(item => item.Status);
    var chartData = data.map(item => item.TotalProj);

    var canvas = document.getElementById('myChart1');
    if (!canvas) {
        console.error("Canvas element 'myChart1' not found.");
        return;
    }
    var backgroundColors = generateRandomColors(titles.length);

    var ctx = canvas.getContext('2d');

    var myChart1 = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: titles,
            datasets: [{
                data: chartData,
                backgroundColor: backgroundColors,
                borderColor: backgroundColors, // Border color same as background color for consistency
                borderWidth: 1,
            }],
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true,
                },
            },
        },
    });
}

function getRandomColor() {
    const minBrightness = 130;
    let color;
    do {
        color = '#' + Math.floor(Math.random() * 16777215).toString(16);
        const rgb = parseInt(color.slice(1), 16);
        const r = (rgb >> 16) & 0xff;
        const g = (rgb >> 8) & 0xff;
        const b = (rgb >> 0) & 0xff;
        const brightness = (r + g + b) / 3;
        if (brightness < minBrightness) {
            color = null;
        }
    } while (color === null);
    return color;
}

function generateRandomColors(count) {
    const colors = [];
    for (let i = 0; i < count; i++) {
        let color;
        do {
            color = getRandomColor();
        } while (colors.includes(color));
        colors.push(color);
    }
    return colors;
}





document.addEventListener('DOMContentLoaded', function () {
    const modal = document.querySelector('.modal-content');
    const header = modal.querySelector('.modal-header');

    let isDragging = false;
    let startX, startY, initialX, initialY;

    header.addEventListener('mousedown', (e) => {
        isDragging = true;
        startX = e.clientX;
        startY = e.clientY;
        initialX = modal.offsetLeft;
        initialY = modal.offsetTop;
        document.addEventListener('mousemove', onMouseMove);
        document.addEventListener('mouseup', onMouseUp);
    });

    function onMouseMove(e) {
        if (isDragging) {
            const dx = e.clientX - startX;
            const dy = e.clientY - startY;
            modal.style.left = `${initialX + dx}px`;
            modal.style.top = `${initialY + dy}px`;
        }
    }

    function onMouseUp() {
        isDragging = false;
        document.removeEventListener('mousemove', onMouseMove);
        document.removeEventListener('mouseup', onMouseUp);
    }
});
function DateFormateddMMyyyyhhmmss(date) {

    var todaysDate = new Date();
    var datef1 = new Date(date);
    //if (datef1.setHours(0, 0, 0, 0) == todaysDate.setHours(0, 0, 0, 0)) {
    //    // Date equals today's date

    //    return 'Today';
    //}
    //else {
    var datef2 = new Date(date);
    var months = "" + `${(datef2.getMonth() + 1)}`;
    var days = "" + `${(datef2.getDate())}`;
    var pad = "00"
    var monthsans = pad.substring(0, pad.length - months.length) + months
    var dayans = pad.substring(0, pad.length - days.length) + days
    var year = `${datef2.getFullYear()}`;
    var hh = `${datef2.getHours()}`;
    var mm = `${datef2.getMinutes()}`;
    var ss = `${datef2.getSeconds()}`;
    if (hh < 10) hh = "0" + hh;
    if (mm < 10) mm = "0" + mm;
    if (ss < 10) ss = "0" + ss;
    if (year > 1902) {

        var datemmddyyyy = dayans + `/` + monthsans + `/` + year + ` ` + hh + `:` + mm + `:` + ss
        return datemmddyyyy;
    }
    else {
        return '';
    }
    // }

    //`${datef2.getFullYear()}/` + monthsans + `/` + dayans ;
}


function getProjBisagN(spnstatusId, spnstatusActionsMappingId) {
    var listItem = "";
    var table = $('#dashboardApprovedBisagN').DataTable(); // Initialize DataTable
    table.clear().destroy(); // Destroy the existing table instance

    var userdata = {
        "StatusId": spnstatusId,
        "statusActionsMappingId": spnstatusActionsMappingId,
    };

    $.ajax({
        url: '/Home/GetDashboardApproved',
        contentType: 'application/x-www-form-urlencoded',
        data: userdata,
        type: 'POST',
        success: function (response) {
            if (response != "null" && response != null) {

                if (response == -1) {
                    Swal.fire({ text: "No data found." });
                } else if (response == 0) {

                    $('#DetailBodyBisagN').empty();
                    listItem += "<tr><td class='text-center' colspan='7'>No Record Found</td></tr>";
                    $("#DetailBodyBisagN").html(listItem);
                    $("#lblTotal").html(0);

                } else {
                    var count = 1;

                    $('#DetailBodyBisagN').empty();
                    for (var i = 0; i < response.length; i++) {
                        listItem += "<tr>";
                        listItem += "<td class='align-middle'>" + count + "</td>";
                        listItem += "<td class='align-middle nowrap'><span id='ProjName'>" + response[i].projName + "</span></td>";
                        listItem += "<td class='align-middle'><span id='ProjName'>" + response[i].stakeHolder + "</span></td>";
                        listItem += "<td class='align-middle'><span id='ProjName'>" + DateFormateddMMyyyyhhmmss(response[i].timeStamp) + "</span></td>";
                        listItem += "</tr>";
                        count++;
                    }

                    $("#DetailBodyBisagN").html(listItem);

                    // Initialize the DataTable
                    var table = $('#dashboardApprovedBisagN').DataTable({
                        lengthChange: true,
                        retrieve: true,
                        bDestroy: true,
                        searching: true,
                        stateSave: true,
                        "order": [[0, "asc"]],
                        "ordering": true,
                        "paging": true,
                        dom: 'lBfrtip',
                        buttons: [
                            'copy',
                            'excel',
                            'csv',
                        ],
                        searchBuilder: {
                            conditions: {
                                num: {
                                    'MultipleOf': {
                                        conditionName: 'Multiple Of',
                                        init: function (that, fn, preDefined = null) {
                                            var el = $('<input/>').on('input', function () { fn(that, this) });

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

                }
            } else {
                $('#DetailBodyBisagN').empty();
                listItem += "<tr><td class='text-center' colspan='7'>No Record Found</td></tr>";
                $("#DetailBodyBisagN").html(listItem);
            }
        },
        error: function (result) {
            Swal.fire({ text: "Error loading data." });
        }
    });
}

//function getProjBisagN(spnstatusId, spnstatusActionsMappingId) {
//    var listItem = "";
//    var table = new DataTable('#dashboardApprovedBisagN');
//    table.destroy();
//    /* alert(spnstatusActionsMappingId)*/
//    var userdata = {
//        "StatusId": spnstatusId,
//        "statusActionsMappingId": spnstatusActionsMappingId,
//    };

//    $.ajax({
//        url: '/Home/GetDashboardApproved',
//        contentType: 'application/x-www-form-urlencoded',
//        data: userdata,
//        type: 'POST',
//        success: function (response) {
//            if (response != "null" && response != null) {

//                if (response == -1) {
//                    Swal.fire({ text: "" });
//                } else if (response == 0) {

//                    $('#DetailBodyBisagN').empty();
//                    listItem += "<tr><td class='text-center' colspan='7'>No Record Found</td></tr>";
//                    $("#DetailBodyBisagN").html(listItem);
//                    $("#lblTotal").html(0);


//                } else {
//                    var count = 1;


//                    $('#DetailBodyBisagN').empty();
//                    $('#dashboardApprovedBisagN').dataTable().fnClearTable();
//                    $('#DetailBodyBisagN').dataTable().fnDestroy();
//                    for (var i = 0; i < response.length; i++) {
//                        listItem += "<tr>";
//                        listItem += "<td class='align-middle'>" + count + "</td>";
//                        listItem += "<td class='align-middle nowrap'><span id='ProjName'>" + response[i].projName + "</span></td>";
//                        listItem += "<td class='align-middle'><span id='ProjName'>" + response[i].stakeHolder + "</span></td>";
//                        listItem += "<td class='align-middle'><span id='ProjName'>" + DateFormateddMMyyyyhhmmss(response[i].timeStamp) + "</span></td>";
//                        listItem += "</tr>";
//                        count++;
//                    }

//                    $("#DetailBodyBisagN").html(listItem);

//                    var table = $('#dashboardApprovedBisagN').DataTable({
//                        lengthChange: true,
//                        retrieve: true,
//                        bDestroy: true,

//                        searching: true,
//                        stateSave: true,
//                        "order": [[0, "asc"]],
//                        "ordering": true,
//                        "paging": true,
//                        dom: 'lBfrtip',
//                        buttons: [
//                            'copy',
//                            'excel',
//                            'csv',
//                        ],
//                        searchBuilder: {
//                            conditions: {
//                                num: {
//                                    'MultipleOf': {
//                                        conditionName: 'Multiple Of',
//                                        init: function (that, fn, preDefined = null) {
//                                            var el = $('<input/>').on('input', function () { fn(that, this) });

//                                            if (preDefined !== null) {
//                                                $(el).val(preDefined[0]);
//                                            }

//                                            return el;
//                                        },
//                                        inputValue: function (el) {
//                                            return $(el[0]).val();
//                                        },
//                                        isInputValid: function (el, that) {
//                                            return $(el[0]).val().length !== 0;
//                                        },
//                                        search: function (value, comparison) {
//                                            return value % comparison === 0;
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                    });
//                }
//            }
//            else {

//                $('#DetailBodyBisagN').empty();
//                listItem += "<tr><td class='text-center' colspan='7'>No Record Found</td></tr>";
//                $("#DetailBodyBisagN").html(listItem);



//            }
//        },
//        error: function (result) {
//            Swal.fire({ text: "" });
//        }
//    });
//}