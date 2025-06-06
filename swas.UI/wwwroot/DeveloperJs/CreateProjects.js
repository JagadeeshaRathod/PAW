var current = 1;
$(document).ready(function () {
    //$("#1").hide();
    //$("#3").show();
    $(function () {
        $(".datepicker1").datepicker({ dateFormat: 'dd-mm-yy' });
    });

    $("#sponsorNameInput").val($("#SponsorName").html());

    $("#spanProjectId").html($("#ProjId").val());
    mMsater($("#StakeHolderId").val(), "ddlStakeHolderId", 1, 0)
    mMsater($("#HostTypeID").val(), "ddlHostTypeID", 2, 0)
    mMsater($("#Apptype").val(), "ddlApptype", 3, 0)

    var current_fs, next_fs, previous_fs; //fieldsets
    var opacity;


    $("#uploadButton").click(function () {

        var requiredFields = $('#fwduploaditems').find('.requiredField');
        var allFieldsComplete = true;
        requiredFields.each(function (index) {
            if (this.value.length == 0) {
                $(this).addClass('is-invalid');
                allFieldsComplete = false;
            } else {
                $(this).removeClass('is-invalid');
            }
        });
        if (!allFieldsComplete) {

            Swal.fire({
                title: 'Error!',
                text: 'Please complete all required fields',
                icon: 'error',
                showConfirmButton: false,
                timer: 1000
            })
        }
        else {
            UploadFiles();
        }
    });

    $("#finalupload").click(function () {
        ProjectSubmited($(this));

    });
    $("#draftUpload").click(function () {
        ProjectSaveAsDraft($(this));
    });
    $("#submitUpload").click(function () {

        var requiredFields = $('#tablebasic2').find('.requiredField');
        var allFieldsComplete = true;
        requiredFields.each(function (index) {
            if (this.value.length == 0) {
                $(this).addClass('is-invalid');
                allFieldsComplete = false;
            } else {
                $(this).removeClass('is-invalid');
            }
        });

        // Validate character limits
        $('.char-limit').each(function () {
            var inputField = $(this);
            var maxLength = inputField.data('maxlength'); // Get max length from data-maxlength attribute
            var currentLength = inputField.val().length;
            var errorMsg = inputField.closest('td').find('.charErrorMsg');

            if (currentLength > maxLength) {
                inputField.addClass('is-invalid');
                errorMsg.show();  // Show error message
                allFieldsComplete = false; // Mark form as invalid
            } else {
                inputField.removeClass('is-invalid');
                errorMsg.hide();  // Hide error message if within limit
            }
        });

        if (!allFieldsComplete) {

            Swal.fire({
                title: 'Error!',
                text: 'Please complete all required fields',
                icon: 'error',
                showConfirmButton: false,
                timer: 1000
            })
        }
        else {
            AddProject(this);
        }


    });
    $(".previous").click(function () {

        current_fs = $(this).parent();
        prev_fs = $(this).parent().prev();

        //Add Class Active
        $("#progressbar li").eq($("fieldset").index(current_fs)).removeClass("active");
        $("#progressbar li").eq($("fieldset").index(prev_fs)).addClass("active");

        //show the next fieldset
        prev_fs.show();
        //hide the current fieldset with style
        current_fs.animate({ opacity: 0 }, {
            step: function (now) {
                // for making fielset appear animation
                opacity = 1 - now;

                current_fs.css({
                    'display': 'none',
                    'position': 'relative'
                });
                prev_fs.css({ 'opacity': opacity });
            },
            duration: 600
        });


    });

    $("#tempBasicDetails").click(function () {
        var number = 1 + Math.floor(Math.random() * 15000);
        const predate = DateFormateyyy_mm_dd(new Date() - 1);
        const todaydate = DateFormateyyy_mm_dd(new Date());

        $("#ProjName").val("New Project" + number);
        $("#InitiatedDate").val(predate);
        $("#CompletionDate").val(todaydate);
        $("#IsWhitelisted").val("YES" + number);
        $("#InitialRemark").val("Remarks" + number);
        $("#AimScope").val("AimScope" + number);
        $("#HQandITinfraReqd").val("HQandITinfraReqd" + number);
        $("#ContentofSWApp").val("Content of SWApp" + number);
        $("#ReqmtJustification").val("Reqmt Justification" + number);
        $("#UsabilityofProposedAppln").val("Usability of Proposed" + number);

        $("#ddlStakeHolderId").val(1);
        $("#ddlHostTypeID").val(1);
        $("#ddlApptype").val(1);


        $("#DetlsofUserBase").val("DetlsofUserBase" + number);
        $("#EnvisagedCost").val(number);
        $("#NWBandWidthReqmt").val("NWBa" + number);
        $("#MajTimeLines").val("MajTimeLines" + number);
        $("#TechStackProposed").val("TechStackProposed" + number);
        $("#DataSecurity_backup").val("DataSecurity_backup" + number);
        $("#TypeofSW").val("TypeofSW" + number);
        $("#BeingDevpInhouse").val("BeingDevpInhouse" + number);
        $("#EndorsmentbyHeadof").val("EndorsmentbyHeadof" + number);
    });
    $(".requiredField").change(function () {


        if (this.value.length == 0) {
            $(this).addClass('is-invalid');

        } else {
            $(this).removeClass('is-invalid');
            $(this).addClass('is-valid');
        }
    });
    $(".requiredField").blur(function () {


        if (this.value.length == 0) {
            $(this).addClass('is-invalid');

        } else {
            $(this).removeClass('is-invalid');
            $(this).addClass('is-valid');
        }
    });
    $("#btnbasic").click(function () {


        requiredFields = $('#tablebasic').find('.requiredField');
        var allFieldsComplete = true;
        requiredFields.each(function (index) {
            if (this.value.length == 0) {
                $(this).addClass('is-invalid');
                allFieldsComplete = false;
            } else {
                $(this).removeClass('is-invalid');
            }
        });
        // Validate character limits

        $('.char-limit').each(function () {
            var inputField = $(this);
            var maxLength = inputField.data('maxlength'); // Get max length from data-maxlength attribute
            var currentLength = inputField.val().length;
            var errorMsg = inputField.closest('td').find('.charErrorMsg');

            if (currentLength > maxLength) {
                inputField.addClass('is-invalid');
                errorMsg.show();  // Show error message
                allFieldsComplete = false; // Mark form as invalid
            } else {
                inputField.removeClass('is-invalid');
                errorMsg.hide();  // Hide error message if within limit
            }
        });

        if (!allFieldsComplete) {

            Swal.fire({
                title: 'Error!',
                text: 'Please complete all required fields',
                icon: 'error',
                showConfirmButton: false,
                timer: 1000
            })
        }
        else {
            current_fs = $(this).parent();
            next_fs = $(this).parent().next();


            //Add Class Active
            $("#progressbar li").eq($("fieldset").index(next_fs)).addClass("active");

            //show the next fieldset
            next_fs.show();
            //hide the current fieldset with style
            current_fs.animate({ opacity: 0 }, {
                step: function (now) {
                    // for making fielset appear animation
                    opacity = 1 - now;

                    current_fs.css({
                        'display': 'none',
                        'position': 'relative'
                    });
                    next_fs.css({ 'opacity': opacity });
                },
                duration: 600
            });

        }
        return allFieldsComplete;
    });



    $("body").on("click", ".btndeleteProject", function () {

        Swal.fire({
            title: 'Are you sure?',
            text: "You want to Delete ",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#072697',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Delete It!'
        }).then((result) => {
            if (result.value) {


                DeleteProject($(this).closest("tr").find(".tblspnprojectid").html());

            }
        });
    });
});
function AddProject(thistag) {

    //var initiatedDate = $("#InitiatedDate").val();
    //var completionDate = $("#CompletionDate").val();

    //// Get current time, or any specific time you want to append
    //var currentTime = new Date(); // Get current date and time
    //var hours = currentTime.getHours().toString().padStart(2, '0');
    //var minutes = currentTime.getMinutes().toString().padStart(2, '0');
    //var seconds = currentTime.getSeconds().toString().padStart(2, '0');

    //var timeString = hours + ":" + minutes + ":" + seconds;

    //// Combine date with the time (assuming you want the current time appended)
    //initiatedDate = initiatedDate + " " + timeString;
    //completionDate = completionDate + " " + timeString;
    var initialDate = $('#InitiatedDate').val();
    var completionDate = $('#CompletionDate').val();
    var currentDate = new Date();
    var currentTime = currentDate.toLocaleTimeString('en-US', { hour12: false });
    var InitiatedDate = initialDate + ' ' + currentTime;
    var CompletionDate = completionDate + ' ' + currentTime;
    console.log("Inititated date:", initialDate + ", Complition Date: ", InitiatedDate)
    $.ajax({
        url: '/Projects/AddProject',
        type: 'POST',
        data: {
            "ProjId": $("#ProjId").val(),
            "ProjName": $("#ProjName").val(),
            //  "InitiatedDate": $("#InitiatedDate").val(),
            "InitiatedDate": InitiatedDate,
            //  "CompletionDate": $("#CompletionDate").val(),
            "CompletionDate": CompletionDate,
            "IsWhitelisted": $("#IsWhitelisted").val(),
            "InitialRemark": $("#InitialRemark").val(),
            "StakeHolderId": $("#ddlStakeHolderId").val(),
            "AimScope": $("#AimScope").val(),
            "HQandITinfraReqd": $("#HQandITinfraReqd").val(),
            "HostTypeID": $("#ddlHostTypeID").val(),
            "ContentofSWApp": $("#ContentofSWApp").val(),
            "ReqmtJustification": $("#ReqmtJustification").val(),
            "UsabilityofProposedAppln": $("#UsabilityofProposedAppln").val(),
            "Apptype": $("#ddlApptype").val(),
            "DetlsofUserBase": $("#DetlsofUserBase").val(),
            "EnvisagedCost": $("#EnvisagedCost").val(),
            "NWBandWidthReqmt": $("#NWBandWidthReqmt").val(),
            "MajTimeLines": $("#MajTimeLines").val(),
            "TechStackProposed": $("#TechStackProposed").val(),
            "DataSecurity_backup": $("#DataSecurity_backup").val(),
            "TypeofSW": $("#TypeofSW").val(),
            "BeingDevpInhouse": $("#BeingDevpInhouse").val(),
            "EndorsmentbyHeadof": $("#EndorsmentbyHeadof").val(),
            "CurrentPslmId": $("#CurrentPslmId").val(),
            "ProjCode": $("#ProjCode").val(),
            "Sponsor": $("#sponsorNameInput").val(),

            "Detlsof_OS": $("#Detlsof_OS").val(),
            "ProposedDB_Engine": $("#ProposedDB_Engine").val(),
            "DetlsofSw_Architecture": $("#DetlsofSw_Architecture").val(),
            "DetlsofProposed_Architecture": $("#DetlsofProposed_Architecture").val(),
            "DetlsPki_IAM": $("#DetlsPki_IAM").val(),
            "Technology_dependencies": $("#Technology_dependencies").val(),
            "Database_reqmts": $("#Database_reqmts").val(),
            "Enhancement_upgradation": $("#Enhancement_upgradation").val(),
            "Details_licensing": $("#Details_licensing").val()

        }, //get the search string
        success: function (result) {
            if (result == -2) {
                Swal.fire({
                    title: "success!",
                    text: "User has been Updated!",
                    icon: "success"
                });
            }
            else if (result == -3) {
                Swal.fire({
                    title: "Error!",
                    text: "Project Name Already Exists!",
                    icon: "Error"
                });
            }
            else if (result == -4) {
                Swal.fire({
                    title: "success!",
                    text: "Incorrect Data!",
                    icon: "Error"
                });
            }
            //else if (result.indcludes("Error")) {
            //else if (result.includes("Error")) {
            //    Swal.fire({
            //        icon: 'error',
            //        title: 'Oops...',
            //        text: result,
            //    })
            //}
            else if (result != null) {
                
                $("#spanProjectId").html(result.projId);
                $("#spanCurrentPslmId").html(result.currentPslmId);

                AttechHistory();
                current_fs = $(thistag).parent();
                next_fs = $(thistag).parent().next();

                //Add Class Active
                $("#progressbar li").eq($("fieldset").index(next_fs)).addClass("active");

                //show the next fieldset
                next_fs.show();
                //hide the current fieldset with style
                current_fs.animate({ opacity: 0 }, {
                    step: function (now) {
                        // for making fielset appear animation
                        opacity = 1 - now;

                        current_fs.css({
                            'display': 'none',
                            'position': 'relative'
                        });
                        next_fs.css({ 'opacity': opacity });
                    },
                    duration: 600
                });
            }
        }
    });
}
function FwdProjConfirm(thisdata) {
    $.ajax({
        url: '/Projects/FwdProjConfirm',
        type: 'POST',
        data: { "PslmId": $("#spanCurrentPslmId").html() },
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

                current_fs = $(thisdata).parent();
                next_fs = $(thisdata).parent().next();


                //Add Class Active
                $("#progressbar li").eq($("fieldset").index(next_fs)).addClass("active");

                //show the next fieldset
                next_fs.show();
                //hide the current fieldset with style
                current_fs.animate({ opacity: 0 }, {
                    step: function (now) {
                        // for making fielset appear animation
                        opacity = 1 - now;

                        current_fs.css({
                            'display': 'none',
                            'position': 'relative'
                        });
                        next_fs.css({ 'opacity': opacity });
                    },
                    duration: 600
                });


            }

        }
    });
}


function ProjectSubmited(thisdata) {

    $.ajax({
        url: '/Projects/ProjectSubmited',
        type: 'POST',
        data: { "projid": $("#spanProjectId").html(), "type": 1 },
        success: function (response) {
            //console.log(response);

            if (response >= 1) {

                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Project Submit Successfully",
                    showConfirmButton: false,
                    timer: 1500
                });

                current_fs = $(thisdata).parent();
                next_fs = $(thisdata).parent().next();

                AddNotification($("#spanProjectId").html(), 2, 1);
                //Add Class Active
                $("#progressbar li").eq($("fieldset").index(next_fs)).addClass("active");

                //show the next fieldset
                next_fs.show();
                //hide the current fieldset with style
                current_fs.animate({ opacity: 0 }, {
                    step: function (now) {
                        // for making fielset appear animation
                        opacity = 1 - now;

                        current_fs.css({
                            'display': 'none',
                            'position': 'relative'
                        });
                        next_fs.css({ 'opacity': opacity });
                    },
                    duration: 600
                });


            }

        }
    });
}


function ProjectSaveAsDraft(thisdata) {
    $.ajax({
        url: '/Projects/ProjectSubmited',
        type: 'POST',
        data: { "projid": $("#spanProjectId").html(), "type": 2 },
        success: function (response) {
            //console.log(response);
            if (response >= 1) {
                Swal.fire({
                    position: "top-end",
                    icon: "success",
                    title: "Draft Saved Successfully",
                    showConfirmButton: false,
                    timer: 1500
                });

                // Disable Save button and apply blur effect
                $("#draftUpload").prop("disabled", true).removeClass("enabled-button").addClass("disabled-button");

                // Enable Submit button and remove blur effect
                $("#finalupload").prop("disabled", false).removeClass("disabled-button").addClass("enabled-button");
            }
        }
    });
}
function validationIsSuccessful() {
    // Your validation logic goes here
    // Return true if validation passes, false otherwise
    // Example:
    var inputVal = $("#ProjEdit_DetlsofUserBase").val();
    if (inputVal === "") {
        // Validation fails
        return false;
    }
    // Validation passes
    return true;
}
function DateFormateyyy_mm_dd(date) {


    var datef2 = new Date(date);
    var months = "" + `${(datef2.getMonth() + 1)}`;
    var days = "" + `${(datef2.getDate())}`;
    var pad = "00"
    var monthsans = pad.substring(0, pad.length - months.length) + months
    var dayans = pad.substring(0, pad.length - days.length) + days
    var year = `${datef2.getFullYear()}`;
    if (year > 1902) {

        var datemmddyyyy = year + `-` + monthsans + `-` + dayans
        return datemmddyyyy;
    }
    else {
        return '';
    }

    //`${datef2.getFullYear()}/` + monthsans + `/` + dayans ;
}

function DeleteProject(ProjectId) {
    $.ajax({
        url: '/Projects/DeleteProjects',
        type: 'POST',
        data: { "ProjectId": ProjectId },
        success: function (response) {
            //console.log(response);


            if (response == 1) {
                alert("Record Deleted successfully");

                location.reload();

            }

        }
    });
}

