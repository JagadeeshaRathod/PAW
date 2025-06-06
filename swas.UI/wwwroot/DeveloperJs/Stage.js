$(document).ready(function () {


    $('.delete-btn').click(function () {
         
        var stageId = $(this).data('id');

        var url = $(this).data('url');

        $.ajax({
            url: '/Stages/DeleteConfirmed',
            type: 'POST',
            data: { stageId: stageId },
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


    $('.btnupdate').click(function () {
         
        var stageId = $(this).data('id');
        var Stages = $(this).data('name');

        var url = $(this).data('url');

        $.ajax({
            url: '/Stages/EditFrom',
            type: 'POST',
            data: {
                stageId: id,
                Stages: stage
            },
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



});

<script>
    function validateForm() {
        var form = document.querySelector('form.needs-validation');
    if (form.checkValidity() === false) {
        event.preventDefault();
    event.stopPropagation();
        }
    form.classList.add('was-validated');
    return form.checkValidity();
    }
</script>