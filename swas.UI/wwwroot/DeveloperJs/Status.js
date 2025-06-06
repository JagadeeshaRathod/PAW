$('.delete-btn').click(function () {

    var statusId = $(this).data('id');

    var url = $(this).data('url');

    $.ajax({
        url: '/Status/Delete',
        type: 'POST',
        data: { statusId: statusId },
        success: function (response) {
            if (response === 1) {
                Swal.fire({
                    position: 'top-end',
                    icon: 'success',
                    title: 'Record Deleted successfully',
                    showConfirmButton: false,
                    timer: 1500
                }).then(function () {
                    window.location.href = url;
                });
            }
            else {


            }
        },
        error: function (xhr, status, error) {

        }
    });
});


$(document).ready(function () {
    $("#statseq").on("input", function () {

        var inputValue = $(this).val();

        var numericValue = parseFloat(inputValue);

        if (numericValue < 0) {

            $(this).val('');

            Swal.fire({
                title: 'Negative Value Not Allowed',
                text: 'Please Enter Possitive Values...',
                icon: 'error',
                confirmButtonText: 'OK'
            });
        }
    });
});