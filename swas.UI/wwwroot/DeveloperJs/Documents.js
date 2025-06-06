
document.addEventListener('DOMContentLoaded', function () {
    $('#downloadButton').click(function () {
        var selectedItems = document.querySelectorAll('.checklist-item:checked');
        var selectedDetails = [];

        selectedItems.forEach(function (checkbox) {
            selectedDetails.push(checkbox.getAttribute('data-att-path'));
        });

        $.ajax({
            type: 'POST',
            url: '@Url.Action("DownloadAction", "Documents")',
            data: {
                selectedCheckboxes: selectedDetails
            },
            success: function (response) {
                if (response.error) {
                    console.error('Error: ' + (response.errorMessage || 'An error occurred.'));
                } else if (response.downloadLink) {
                    displayPreview(response.downloadLink);
                } else if (response.pdfContent) {
                    downloadMergedPDF(response.pdfContent);
                } else {
                    console.error('Invalid response format');
                }
            },
            error: function (error) {
                console.error('AJAX Error:', error);
            }
        });
    });

    function displayPreview(downloadLink) {
        const pdfContainer = document.getElementById("pdf-container");

        if (pdfContainer) {
            const embed = document.createElement("object");

            if (downloadLink.startsWith("http")) {
                // If the downloadLink is a URL, set it as the data attribute
                embed.data = downloadLink;
            } else {
                // If the downloadLink is a file path, create a Blob and use it as the data
                const blob = new Blob([downloadLink], { type: "application/pdf" });
                embed.data = URL.createObjectURL(blob);
            }

            embed.type = "application/pdf";
            embed.width = "100%";
            embed.height = "370px";

            embed.onerror = function (event) {
                console.error('Error loading PDF:', event);
            };

            pdfContainer.innerHTML = "";
            pdfContainer.appendChild(embed);
        } else {
            console.error("Element with id 'pdf-container' not found.");
        }
    }

    function checkPDFHeader(input) {
        const file = input.files[0];
        if (file) {
            const reader = new FileReader();

            reader.onloadend = function (e) {
                if (e.target.readyState === FileReader.DONE) {
                    const arr = new Uint8Array(e.target.result).subarray(0, 4);
                    const header = Array.from(arr).map(byte => byte.toString(16)).join("").toUpperCase();

                    if (header === "25504446") {
                        displayPreview(URL.createObjectURL(file));
                    } else {
                        Swal.fire("Please select a valid PDF file.");
                        input.value = "";
                    }
                }
            };
            reader.readAsArrayBuffer(file);
        }
    }
});


    $(document).ready(function () {
        $('#Soft').on('click', '.MargeButton', function (e) {
            e.preventDefault();

            var projId = $(this).data('proj-id');
            var projName = $(this).data('proj-name');

            window.location.href = '/Documents/DocumentHistory?projId=' + projId + '&projName=' + projName;
        });
    });
