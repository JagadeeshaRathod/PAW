

$(document).ready(function () {
    $("#ddlUnitId").change(function () {
        var selectedMode = $(this).val();
    });
    $("#projunderprocess").click(function () {

       
        $("#ProjunderprocessModal").modal('show');
        
    });
    $("#projWhiteListed").click(function () {


        $("#ProjWhiteListedProjectModal").modal('show');

    });

    $(".EditWhiteListedProj").click(function (e) {

        var id = $(this).data('id'); 
        $('#WhiteListedProjectDetail').modal('hide');

        $.ajax({
            url: '/Home/GetWhiteListedProjectById',
            type: 'GET',
            data: { id: id },
                success: function (data) {
                    if (data) {
                        $('#edit_Id').val(data.id);
                        $('#edit_ProjName').val(data.projName);
                       
                        var hostedOnMap = {
                            "1": "LAN",
                            "2": "ADN",
                            "3": "Internet",
                            "4": "Standalone"
                        };

                        var hostedOnVal = data.mHostTypeId ? data.mHostTypeId.toString() : "";
                        $('#edit_HostedOn').val(hostedOnVal);
                      
                        if ($('#edit_HostedOn').val() != hostedOnVal) {
                            $('#edit_HostedOn option').each(function () {
                                if ($(this).val() == hostedOnVal) {
                                    $(this).prop('selected', true);
                                } else {
                                    $(this).prop('selected', false);
                                }
                            });
                        }                        
                        $('#edit_Appt').val(data.appt);
                        $('#edit_Sponser').val(data.fmn);
                        $('#edit_TelNo').val(data.contactNo);
                        $('#edit_Clearance').val(data.clearence ? data.clearence.substring(0, 10) : '');
                        $('#edit_Cert').val(data.certNo);
                        $('#edit_ValidUpto').val(data.validUpto ? data.validUpto.substring(0, 10) : '');
                        $('#edit_Remarks').val(data.remarks);

                        $('#EditWhiteListedProjectModal').modal('show');
                    }
                },
            error: function () {
                alert('Failed to fetch project details.');
            }
        });
    });

    $('#editWhiteListedForm').submit(function (e) {
        e.preventDefault();       
        $.ajax({
            url: '/Home/UpdateWhiteListedProject', 
            type: 'POST',
            data: $('#editWhiteListedForm').serialize(),
            success: function (data) {
                if (data == 1) {
                    
                    Swal.fire({
                        title: 'Success!',
                        text: 'Updated Successfully',
                        icon: 'success',
                        timer: 3000,             
                        timerProgressBar: true,
                        showConfirmButton: false, 
                        willClose: () => {
                            $('#EditWhiteListedProjectModal').modal('hide');
                            location.reload();
                        }
                    });
                }
                else
                {
                    Swal.fire({
                        title: 'Error!',
                        text: 'Update Failde!',
                        icon: 'error',
                        timer: 2000,
                        confirmButtonText: 'OK'
                    });
                }                
            },
            error: function () {
                Swal.fire({
                    title: 'Error!',
                    text: 'Something went wrong!',
                    icon: 'error',
                    confirmButtonText: 'OK'
                });
            }
        });
    });

});

//$(document).on('click', '.EditWhiteListedProj', function (e) {
//    e.preventDefault();
//    var id = $(this).data('id');
//    $('#WhiteListedProjectDetail').modal('hide');

//    $.ajax({
//        url: '/Home/GetWhiteListedProjectById',
//        type: 'GET',
//        data: { id: id },
//        success: function (data) {
//            if (data) {
//                $('#edit_Id').val(data.id);
//                $('#edit_ProjName').val(data.projName);

//                var hostedOnVal = data.mHostTypeId ? data.mHostTypeId.toString() : "";
//                $('#edit_HostedOn').val(hostedOnVal);

//                if ($('#edit_HostedOn').val() != hostedOnVal) {
//                    $('#edit_HostedOn option').each(function () {
//                        $(this).prop('selected', $(this).val() == hostedOnVal);
//                    });
//                }

//                $('#edit_Appt').val(data.appt);
//                $('#edit_Sponser').val(data.fmn);
//                $('#edit_TelNo').val(data.contactNo);
//                $('#edit_Clearance').val(data.clearence ? data.clearence.substring(0, 10) : '');
//                $('#edit_Cert').val(data.certNo);
//                $('#edit_ValidUpto').val(data.validUpto ? data.validUpto.substring(0, 10) : '');
//                $('#edit_Remarks').val(data.remarks);

//                $('#EditWhiteListedProjectModal').modal('show');
//            }
//        },
//        error: function () {
//            alert('Failed to fetch project details.');
//        }
//    });
//});


function ValInData(input) {
    var regex = /[^a-zA-Z0-9/ ]/g;
    input.value = input.value.replace(regex, "");
}

function ButtonClick() {

    if (ButtonText === 'Sign In' && flag === 'True') {
        var signInUrl = '/Home/Index';
        window.location.href = signInUrl;
    }
    else {
        if (ButtonText === 'Sign Up') {
            var signUpUrl = '/Identity/Account/Register';
            window.open(signUpUrl, '_blank');
        }
        else {
            Swal.fire({
                title: 'Warning!',
                //text: 'Contact ddgit for authorization before signing in.',
                text: 'Contact DDGIT(Tel ) for Necessary Admin Approval before Signing In',
                icon: 'warning',
                confirmButtonColor: '#ffc107',
                confirmButtonText: 'OK'                
            }).then((result) => {
                // Check if the user clicked the OK button
                if (result.isConfirmed) {
                    // Clear session on the server-side via an AJAX request
                    $.ajax({
                        url: '/Home/ClearSession', // The server-side action to clear the session
                        type: 'POST',
                        success: function () {
                            // Once session is cleared, open the login page
                            var signUpUrl = '/Identity/Account/Login';
                            window.open(signUpUrl, '_self'); // Open login in the same window or tab
                        },
                        error: function () {
                            // Handle error if the session could not be cleared
                            Swal.fire({
                                title: 'Error!',
                                //text: 'Contact ddgit for authorization before signing in.',
                                text: 'Contact DDGIT/ASDC for Necessary Admin Approval before Signing in1111.',
                                icon: 'error',
                                confirmButtonText: 'OK'
                            });
                        }
                    });
                }
            });
            return false;
        }

    }
}