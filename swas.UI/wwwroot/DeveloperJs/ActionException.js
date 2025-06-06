var UnitStatusMappingId = 0;
$(document).ready(function () {


    var table = $('#tblactionEx').DataTable({
        lengthChange: true,
        dom: 'lBfrtip',
        buttons: [
            'copy',
            'excel',
            'csv'
            
           
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

        mMsaterfwdStage(0, "ddlfwdStage", 5, 0);
        $("#ddlfwdStage").change(function () {
            mMsaterStage(0, "ddlfwdSubStage", 6, $("#ddlfwdStage").val(), $("#SpnStakeHolderId").html());
        });

        $("#ddlfwdSubStage").change(function () {
            mMsater(0, "ddlfwdAction", 7, $("#ddlfwdSubStage").val());
        });

        $("#ddlfwdAction").change(function () {
            mMsaterFwdTo(0, "ddlfwdFwdTo", 8, 0, $("#SpnFwdStakeHolderId").html(),0,"");
        });
  
});
$(".btn-Mapping").click(function () {
    resetPopup();
    $('#unitMapping').modal('show');
    $(".Fwdtitle").html("Unit & Status Mapping");

});

$(".btnSaveException").click(function () {
   
    CheckProjectExists(1);

    $('#ProjFwd').modal('hide');
});

$(".btnUpdateException").click(function () {
   
    CheckProjectExists(2);
    $('#ProjFwd').modal('hide');
});

$(document).on('click', '.btn-edit', function () {
    $('#unitMapping').modal('show');

    $(".ProjectsFwd").removeClass("d-none");
    $(".save").addClass("d-none");
    $(".update").removeClass("d-none");
    UnitStatusMappingId = $(this).closest("tr").find("#UnitStatusMapId").html();
    $("#spnUnitStatusMappingId").html($(this).closest("tr").find("#UnitStatusMapId").html());

    mMsaterfwdStage($(this).closest("tr").find("#spnStageId").html(), "ddlfwdStage", 5, 0, 1)
    mMsaterStage($(this).closest("tr").find("#spnStatusId").html(), "ddlfwdSubStage", 6, $(this).closest("tr").find("#spnStageId").html(), 0)
    mMsater($(this).closest("tr").find("#spnActionId").html(), "ddlfwdAction", 10, $(this).closest("tr").find("#spnStatusId").html())
    mMsaterFwdTo($(this).closest("tr").find("#spnToUnitId").html(), "ddlfwdFwdTo", 8, 0, $("#SpnFwdStakeHolderId").html(), $(this).closest("tr").find("#spnToUnitId").html(),0, "");

});

$('#unitMapping').on('hidden.bs.modal', function () {
    $(this).find('form').trigger('reset');
});


function CheckProjectExists(Type) {

    var userdata =
    {
        "UnitId": $("#ddlfwdFwdTo").val(),
        "ActionId": $("#ddlfwdAction").val(),
    };

    $.ajax({
        url: '/ActionException/CheckProjectExists',
        type: 'POST',
        data: userdata,
        success: function (response) {
            //console.log(response);
            if (response != null) {

                if (response == true) {
                    Swal.fire({
                        icon: "error",
                        title: "Oops...",
                        text: "Action Exception Already Exists!",

                    });
                }
                else if (response == false) {
                    if (Type == 1) {
                        SaveActionException();
                    }
                    else {
                        UpdateActionException();
                    }
                    
                }
                
            }

        }
    });


}

function SaveActionException() {

    var userdata =
    {
        "UnitStatusMappingId": UnitStatusMappingId,
        "UnitId": $("#ddlfwdFwdTo").val(),
        "StatusActionsMappingId": $("#ddlfwdAction").val()
    };
    $.ajax({
        url: '/ActionException/SaveActionException',
        type: 'POST',
        data: userdata,
        success: function (response) {
            //console.log(response);
            if (response != null) {
                
                Swal.fire({
                    icon: "success",
                    title: "Success...",
                    text: "Project is Submitted Successfully!",
                }).then(() => {
                    // Reload the page when the user clicks "OK"
                    window.location.reload();
                });

            }

        }
    });
}

function UpdateActionException() {
   
    var userdata =
    {
        "UnitStatusMappingId": UnitStatusMappingId,
        "UnitId": $("#ddlfwdFwdTo").val(),
        "StatusActionsMappingId": $("#ddlfwdAction").val()
    };

    $.ajax({
        url: '/ActionException/UpdateActionException',
        type: 'POST',
        data: userdata,
        success: function (response) {
            //console.log(response);
            if (response != null) {

                Swal.fire({
                    icon: "success",
                    title: "Success...",
                    text: "Project is Updated Successfully!",
                }).then(() => {
                    // Reload the page when the user clicks "OK"
                    window.location.reload();
                });

            }

        }
    });
}



function resetPopup() {

    $("#ddlfwdStage").val('');
    $("#ddlfwdSubStage").val('');
    $("#ddlfwdAction").val('');
    $("#ddlfwdFwdTo").val('');
}

