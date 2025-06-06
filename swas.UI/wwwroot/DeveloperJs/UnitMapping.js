$(document).ready(function () {
    var UnitId = $("#Spnmodalunitid").html();

    $("#ddlStage").change(function () {
        mMsaterStage(0, "ddlSubStage", 6, $("#ddlStage").val(), 100);
    });
    $("#ddlSubStage").change(function () {
        mMsater($("#ddlSubStage").val(), "ddlAction", 7, $("#ddlSubStage").val());
    });

    $(document).on('click', '.btn-editmapping', function () {
        var closestRow = $(this).closest("tr");
        var hiddenData = closestRow.find(".hiddenData");
        $('#SpnmodalStageid').val(hiddenData.find(".spanMappingStageId").text());
        $('#spanmodalStatusActionsMappingId').text(hiddenData.find(".spanStatusActionsMappingId").text());
        mMsater(hiddenData.find(".spanMappingStages").text(), "ddlStage", 5, 0);
        mMsaterStage(hiddenData.find(".spanMappingSubStages").text(), "ddlSubStage", 6, hiddenData.find(".spanMappingStages").text(), 100);
        mMsater(hiddenData.find(".spanMappingActions").text(), "ddlAction", 7, hiddenData.find(".spanMappingSubStages").text());
    });


    function fetchAndUpdateTable(StageId, StatusId) {
        $.ajax({
            type: 'POST',
            url: '/UnitDtls/GetMappingByUnitId',
            data: { StageId: StageId, StatusId: StatusId },
            dataType: "json",
            success: function (data) {
                var tableRows = '';
                for (var i = 0; i < data.length; i++) {
                    tableRows += '<tr>';
                    tableRows += '<td>' + data[i].stagesName + '</td>';
                    tableRows += '<td>' + data[i].subStagesName + '</td>';
                    tableRows += '<td>' + data[i].actionsName + '</td>';
                    tableRows += '<td class="hiddenData" style="display: none;">' +
                        '<span class="spanStatusActionsMappingId">' + data[i].statusActionsMappingId + '</span>' +
                        '<span class="spanMappingStageId">' + data[i].stagesId + '</span>' +
                        '<span class="spanMappingSubStages">' + data[i].subStagesId + '</span>' +
                        '<span class="spanMappingActions">' + data[i].actionsId + '</span>' +
                        '</td>';
                    //tableRows += '<td>' +
                    //    '<button class="btn btn-primary btn-editmapping">Edit</button>' +
                    //    '<button class="btn btn-primary btn-deletemapping">Delete</button>' +
                    //    '</td>';
                    tableRows += '</tr>';
                }
                $('#mappingdetails').html(tableRows);
            },
            error: function (error) {
                alert("Error fetching data");
            }
        });
    }

    $(".btn-Mapping").click(function () {
        $("#SpnmodalStageid").html($(this).closest("tr").find("#CurrentStageId").html());
        $("#SpnmodalStatusId").html($(this).closest("tr").find("#CurrentStatusId").html());

        var StageId = $("#SpnmodalStageid").html();
        var StatusId = $("#SpnmodalStatusId").html();

        mMsater(0, "ddlStage", 5, 0);

        $('#unitMapping').modal('show');
        $(".Fwdtitle").html("Status Mapping");

        fetchAndUpdateTable(StageId, StatusId);

        mMsater($(this).closest("tr").find("#CurrentStageId").html(), "ddlStage", 5, 0);
        mMsater($(this).closest("tr").find("#CurrentStatusId").html(), "ddlSubStage", 6, $(this).closest("tr").find("#CurrentStageId").html());
        mMsater(1, "ddlAction", 7, $(this).closest("tr").find("#CurrentStatusId").html());

        $('#unitMapping').modal('show');
        $(".Fwdtitle").html("Status Mapping");
    });

    $('#ddlSubStage').select2();
    $('#ddlSubStageedit').select2();

    $('#btnsave').click(function () {
        var StageId = $('#SpnmodalStageid').html();
        var StagesId = $('#ddlStage').val();
        var SubStagesId = $('#ddlSubStage').val();
        var ActionsId = $('#ddlAction').val();
        var StatusActionsMappingId = $("#spanmodalStatusActionsMappingId").html();

        var data = {
            StatusActionsMappingId: StatusActionsMappingId,
            StageId: StageId,
            StagesId: StagesId,
            SubStagesId: SubStagesId,
            ActionsId: ActionsId
        };

        $.ajax({
            type: 'POST',
            url: '/UnitDtls/AddMapping',
            data: data,
            success: function (response) {
                if (response.message === "StatusActionsMapping data is already in the table") {
                    Swal.fire({
                        position: "top-mid",
                        icon: "error",
                        title: "Data is already in the table!",
                        showConfirmButton: true
                    });
                } else {
                    Swal.fire({
                        position: "top-mid",
                        icon: "success",
                        title: "Data saved successfully",
                        showConfirmButton: true
                    });

                    var StageId = $('#ddlStage').val();
                    var StatusId = $('#ddlAction').val();
                    fetchAndUpdateTable(StageId, StatusId); // Refresh table data
                }
            },
            error: function (error) {
                alert("Error saving data");
            }
        });
    });

    $(document).on('click', '.btn-deletemapping', function () {
        var closestRow = $(this).closest("tr");
        var hiddenData = closestRow.find(".hiddenData");
        var StatusActionsMappingId = hiddenData.find(".spanStatusActionsMappingId").text();

        var data = {
            StatusActionsMappingId: StatusActionsMappingId
        };

        $.ajax({
            type: 'POST',
            url: '/UnitDtls/DeleteMapping',
            data: data,
            success: function (response) {
                if (response == 1) {
                    Swal.fire({
                        position: "top-mid",
                        icon: "success",
                        title: "Data deleted successfully",
                        showConfirmButton: true
                    });

                    var StageId = $('#SpnmodalStageid').html();
                    var StatusId = $('#SpnmodalStatusId').html();
                    fetchAndUpdateTable(StageId, StatusId); // Refresh table data
                } else {
                    Swal.fire({
                        position: "top-mid",
                        icon: "error",
                        title: "Something went wrong!",
                        showConfirmButton: true
                    });
                }
            },
            error: function (error) {
                alert("Error deleting data");
            }
        });
    });
});










$(document).ready(function () {
    function mMsater(id, elementId, type, parentId) {
        // Implement your mMsater function logic here
    }

    function mMsaterStage(id, elementId, type, parentId, status) {
        // Implement your mMsaterStage function logic here
    }

    function fetchAndUpdateTable(StageId, StatusId) {
        $.ajax({
            type: 'POST',
            url: '/UnitDtls/GetMappingByUnitId',
            data: { StageId: StageId, StatusId: StatusId },
            dataType: "json",
            success: function (data) {
                var tableRows = '';
                for (var i = 0; i < data.length; i++) {
                    tableRows += '<tr>';
                    tableRows += '<td>' + data[i].stagesName + '</td>';
                    tableRows += '<td>' + data[i].subStagesName + '</td>';
                    tableRows += '<td>' + data[i].actionsName + '</td>';
                    tableRows += '<td class="hiddenData" style="display: none;">' +
                        '<span class="spanStatusActionsMappingId">' + data[i].statusActionsMappingId + '</span>' +
                        '<span class="spanMappingStageId">' + data[i].stagesId + '</span>' +
                        '<span class="spanMappingSubStages">' + data[i].subStagesId + '</span>' +
                        '<span class="spanMappingActions">' + data[i].actionsId + '</span>' +
                        '</td>';
                    tableRows += '<td>' +
                        '<button class="btn btn-primary btn-editmapping">Edit</button>' +
                        '<button class="btn btn-primary btn-deletemapping">Delete</button>' +
                        '</td>';
                    tableRows += '</tr>';
                }
                $('#mappingdetails').html(tableRows);
            },
            error: function (error) {
                alert("Error fetching data");
            }
        });
    }

    $('#ddlStage').change(function () {
        mMsaterStage(0, "ddlSubStage", 6, $(this).val(), 100);
    });

    $('#ddlSubStage').change(function () {
        mMsater($(this).val(), "ddlAction", 7, $(this).val());
    });

    $(document).on('click', '.btn-editmapping', function () {
        var closestRow = $(this).closest("tr");
        var hiddenData = closestRow.find(".hiddenData");
        $('#SpnmodalStageid').text(hiddenData.find(".spanMappingStageId").text());
        $('#spanmodalStatusActionsMappingId').text(hiddenData.find(".spanStatusActionsMappingId").text());
        mMsater(hiddenData.find(".spanMappingStageId").text(), "ddlStage", 5, 0);
        mMsaterStage(hiddenData.find(".spanMappingSubStages").text(), "ddlSubStage", 6, hiddenData.find(".spanMappingStageId").text(), 100);
        mMsater(hiddenData.find(".spanMappingActions").text(), "ddlAction", 7, hiddenData.find(".spanMappingSubStages").text());
    });

    $(".btn-Mapping").click(function () {
        var currentRow = $(this).closest("tr");
        $("#SpnmodalStageid").text(currentRow.find("#CurrentStageId").text());
        $("#SpnmodalStatusId").text(currentRow.find("#CurrentStatusId").text());

        var StageId = $("#SpnmodalStageid").text();
        var StatusId = $("#SpnmodalStatusId").text();

        mMsater(0, "ddlStage", 5, 0);

        $('#unitMapping').modal('show');
        $(".Fwdtitle").text("Status Mapping");

        fetchAndUpdateTable(StageId, StatusId);

        mMsater(currentRow.find("#CurrentStageId").text(), "ddlStage", 5, 0);
        mMsater(currentRow.find("#CurrentStatusId").text(), "ddlSubStage", 6, currentRow.find("#CurrentStageId").text());
        mMsater(1, "ddlAction", 7, currentRow.find("#CurrentStatusId").text());
    });

    $('#ddlSubStage').select2();
    $('#ddlSubStageedit').select2();

    $('#btnsave').click(function () {
        var StageId = $('#SpnmodalStageid').text();
        var StagesId = $('#ddlStage').val();
        var SubStagesId = $('#ddlSubStage').val();
        var ActionsId = $('#ddlAction').val();
        var StatusActionsMappingId = $("#spanmodalStatusActionsMappingId").text();

        var data = {
            StatusActionsMappingId: StatusActionsMappingId,
            StageId: StageId,
            StagesId: StagesId,
            SubStagesId: SubStagesId,
            ActionsId: ActionsId
        };

        $.ajax({
            type: 'POST',
            url: '/UnitDtls/AddMapping',
            data: data,
            success: function (response) {
                if (response.message === "StatusActionsMapping data is already in the table") {
                    Swal.fire({
                        position: "top-mid",
                        icon: "error",
                        title: "Data is already in the table!",
                        showConfirmButton: true
                    });
                } else {
                    Swal.fire({
                        position: "top-mid",
                        icon: "success",
                        title: "Data saved successfully",
                        showConfirmButton: true
                    });

                    fetchAndUpdateTable(StageId, StatusId); // Refresh table data
                }
            },
            error: function (error) {
                alert("Error saving data");
            }
        });
    });

    $(document).on('click', '.btn-deletemapping', function () {
        var closestRow = $(this).closest("tr");
        var hiddenData = closestRow.find(".hiddenData");
        var StatusActionsMappingId = hiddenData.find(".spanStatusActionsMappingId").text();

        var data = {
            StatusActionsMappingId: StatusActionsMappingId
        };

        $.ajax({
            type: 'POST',
            url: '/UnitDtls/DeleteMapping',
            data: data,
            success: function (response) {
                if (response == 1) {
                    Swal.fire({
                        position: "top-mid",
                        icon: "success",
                        title: "Data deleted successfully",
                        showConfirmButton: true
                    });

                    var StageId = $('#SpnmodalStageid').text();
                    var StatusId = $('#SpnmodalStatusId').text();
                    fetchAndUpdateTable(StageId, StatusId); // Refresh table data
                } else {
                    Swal.fire({
                        position: "top-mid",
                        icon: "error",
                        title: "Something went wrong!",
                        showConfirmButton: true
                    });
                }
            },
            error: function (error) {
                alert("Error deleting data");
            }
        });
    });
});
