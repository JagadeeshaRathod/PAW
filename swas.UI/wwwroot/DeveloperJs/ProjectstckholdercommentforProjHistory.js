$(document).ready(function () {

    sessionStorage.setItem("spntabType", $("#spntabType").html());

    GetAllComments2();

    GetAllComments1();

});

function GetAllComments2() {    
    $.ajax({
        type: "POST",
        url: '/Projects/GetAllCommentBypsmId_UnitId',
        data: {
            "PsmId": 0,
            "stakeholderId": 1,
            "ProjId": $(".ProjectcommentprojId").html()
        },
        success: function (data) {
            //console.log("CommentData", data);
            var projectName = data[0].projectName;
            //console.log("First Project Name:", projectName);

            var tableHTML = '<table class="table" style="width:100%; border: 1px solid black; border-collapse:collapse;">';
            tableHTML += '<thead>';
            tableHTML += '<tr>';
            //tableHTML += '<th style="display: none;">Project Name</th>';
            tableHTML += '<th style="background-color: #044c92; color: white; border: 1px solid black;">Ser No</th>';
            tableHTML += '<th style="background-color: #044c92; color: white; border: 1px solid black;">Stakeholder</th>';
            tableHTML += '<th style="background-color: #044c92; color: white; border: 1px solid black;">Datetime</th>';
            tableHTML += '<th style="background-color: #044c92; color: white; border: 1px solid black;">Comment</th>';
            tableHTML += '<th style="background-color: #044c92; color: white; border: 1px solid black;">Status</th>';
            tableHTML += '<th style="background-color: #044c92; color: white; border: 1px solid black;">PDF</th>';            
            tableHTML += '</tr>';
            tableHTML += '</thead>';
            tableHTML += '<tbody>';

            if (data != null) {
                for (var i = 0; i < data.length; i++) {
                    tableHTML += '<tr>';
                    //tableHTML += '<td style="display: none;" class="project-name">' + data[i].projectName + '</td>';
                    tableHTML += '<td style="border: 1px solid black;width:1% !important">' + (i + 1) + '</td>';
                    tableHTML += '<td style="border: 1px solid black;">' + data[i].stakeholder + ' (' + data[i].userDetails + ')</td>'; 
                    tableHTML += '<td style="border: 1px solid black;">' + DateFormateddMMyyyyhhmmss(data[i].date) + '</td>';
                    tableHTML += '<td style="border: 1px solid black;">' + data[i].comments + '</td>';
                    if (data[i].status == "Accepted")
                        tableHTML += '<td style="border: 1px solid black;"><span class="badge badge-success text-white">' + data[i].status + '</span></td>';
                    else if (data[i].status == "Obsn")
                        tableHTML += '<td style="border: 1px solid black;"><span class="badge badge-warning text-white">' + data[i].status + '</span></td>';
                    else if (data[i].status == "Info")
                        tableHTML += '<td style="border: 1px solid black;"><span class="badge badge-success text-white">' + data[i].status + '</span></td>';
                    else
                        tableHTML += '<td style="border: 1px solid black;"><span class="badge badge-danger text-white">' + data[i].status + '</span></td>';

                    tableHTML += '<td style="border: 1px solid black;">';
                    if (data[i].state !== null && data[i].attpath !== null && data[i].attpath !== '') {
                        tableHTML += '<a href="/Home/WaterMark3?id=' + data[i].attpath + '" target="_blank">';
                        tableHTML += '<img src="/assets/images/icons/pdfimg.png" alt="PDF icon" style="width: 24px; height: 24px;">';
                        tableHTML += '</a>';
                    }
                    tableHTML += '</td>';
                    tableHTML += '</tr>';
                }
            }

            tableHTML += '</tbody>';
            tableHTML += '</table>';

            $('#ChatBox').empty().html(tableHTML);

            //$('#ChatBox .table').DataTable({
            //    "paging": true,    // Enable paging
            //    "ordering": true,  // Enable ordering
            //    "info": true,       // Enable info
            //    "dom": '<"row"<"col-sm-12 col-md-6 add-comment-btn"><"col-sm-12 col-md-6"f>>rt<"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',

            //});

            //$('#ChatBox .table').DataTable({
            //    paging: true,
            //    ordering: true,
            //    info: true,
            //    dom: '<"row"<"col-sm-12 col-md-6 add-comment-btn"><"col-sm-12 col-md-6"Bf>>rt<"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',
            //    buttons: [
            //        'copy', 'excel', 'csv', 'pdf'
            //    ]
            //});

            var table = $('#ChatBox .table').DataTable({
                paging: true,
                ordering: true,
                info: true,
                dom: '<"row"<"col-sm-2 col-md-1 add-comment-btn"><"col-sm-12 col-md-11"Bf>>rt<"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',
                buttons: [
                    'copy',
                    'excel',
                    'csv',
                    {
                        text: 'PDF',
                        extend: 'pdfHtml5',
                        action: function (e, dt, node, config) {
                            PdfDiv();
                        }
                    }
                ]
            });

            // Insert the +Add Comment button inside the left container
            $('<button type="button" class="btn btn-primary">+Add Comment</button>')
                .appendTo('.add-comment-btn')
                .on('click', function () {
                    // Your custom +Add Comment button click logic here
                    alert('Add Comment clicked!');
                });

            function PdfDiv() {
                var popupWin = window.open('', '_blank', 'top=100,width=900,height=500,location=no');
                popupWin.document.open();

                var tableStyles = `
            <style type="text/css">
                table {
                    width: 100%;
                    border-collapse: collapse;
                    margin-bottom: 20px;
                }
                thead {
                    vertical-align: bottom;
                    background-color: red;
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
            </style>
        `;

                var filteredData = table.rows({ search: 'applied' }).data().toArray();

                var tableHTML = '<table>';

                tableHTML += '<thead><tr>';
                table.columns().header().each(function (header) {
                    tableHTML += '<th>' + header.innerHTML + '</th>';
                });
                tableHTML += '</tr></thead>';

                tableHTML += '<tbody>';
                for (var i = 0; i < filteredData.length; i++) {
                    tableHTML += '<tr>';
                    for (var j = 0; j < filteredData[i].length; j++) {
                        tableHTML += '<td>' + filteredData[i][j] + '</td>';
                    }
                    tableHTML += '</tr>';
                }
                tableHTML += '</tbody></table>';

                var ipadds = $("#ipAdd").html().replace(/\n/g, ' | ').trim();
                var watermarkText = ipadds;

                popupWin.document.write('<html><head>'
                    + tableStyles + '</head><body onload="window.print()">'
                    + tableHTML + `<div style="transform: rotate(-45deg); z-index:10000; opacity: 0.3; color: #8e9191; position: fixed; left: 6%; top: 39%; font-size: 80px; font-weight: 500; display: grid; justify-content: center; align-content: center;">`
                    + watermarkText + '</div></body></html>');

                popupWin.document.close();
            }


            if ($("#IsCommentPsmiId").html() != 0)
                $("div.add-comment-btn").html('<button id="add-comment" class="btn btn-primary"><i class="fas fa-plus"></i> Add Comment</button>');

            $("#add-comment").on("click", function () {

                $("#ProjectcommentForStackHolderprojId").html($(".ProjectcommentprojId").html())
                $("#ProjectcommentForStackHolderPsmId").html($("#IsCommentPsmiId").html())
                mMsater(0, "ddlStatus", 4, 0)
                $("#ProjCommentModal").modal('show');
                GetAllComments($("#IsCommentPsmiId").html(), $(".ProjectcommentprojId").html());

                // Added from here for pop up heading with project name in comment (added by Divyanshu on 10/02/2025)
                var words = projectName.split(" ");
                // Limit to 6 words and add "..." if needed
                var shortProjName = words.length > 6 ? words.slice(0, 6).join(" ") + "..." : projectName;
                var finalTitle = "Mov History: " + shortProjName;
                $('#addComment').text(finalTitle);

            });


        },
        error: function () {
            alert('Error fetching comments.');
        }
    });
}


function GetAllComments1() {

    $.ajax({
        type: "POST",
        url: '/Projects/GetAllCommentBypsmId_UnitId',
        data: {
            "PsmId": 0,
            "stakeholderId": 1,
            "ProjId": $(".ProjectcommentprojId").html()
        },
        success: function (data) {
            //// Assuming the API returns PsmId as part of the response object
            //if (data && data.length > 0 && data[0].psmId !== undefined) {
            //    // Bind the PsmId to a hidden field
            //    $('#hiddenPsmId').val(data[0].psmId); // Ensure you have an input field with ID "hiddenPsmId"
            //}
            var tableHTML = '<table style="width:100%; border: 1px solid black; border-collapse:collapse;">';
            tableHTML += '<thead>';
            tableHTML += '<tr>';
            tableHTML += '<th style="width:5%;text-align:center;border:1px solid black;border-collapse:collapse">Ser No</th>';
            tableHTML += '<th style="text-align: center; border: 1px solid black;">Stakeholder</th>';
            tableHTML += '<th style="text-align: center; border: 1px solid black;">Datetime</th>';
            tableHTML += '<th style="text-align: center; border: 1px solid black;">Comment</th>';
            tableHTML += '<th style="text-align: center; border: 1px solid black;">Status</th>';
            tableHTML += '</tr>';
            tableHTML += '</thead>';
            tableHTML += '<tbody>';

            if (data != null) {
                for (var i = 0; i < data.length; i++) {
                    var date = new Date(data[i].date);
                    var formattedDate = ("0" + date.getDate()).slice(-2) + '-' +
                        ("0" + (date.getMonth() + 1)).slice(-2) + '-' +
                        date.getFullYear() + ' ' +
                        ("0" + date.getHours()).slice(-2) + ':' +
                        ("0" + date.getMinutes()).slice(-2) + ':' +
                        ("0" + date.getSeconds()).slice(-2);

                    tableHTML += '<tr>';
                    tableHTML += '<td style="border: 1px solid black;">' + (i + 1) + '</td>';
                    tableHTML += '<td style="border: 1px solid black;">' + data[i].stakeholder + '</td>';
                    tableHTML += '<td style="border: 1px solid black;">' + formattedDate + '</td>';
                    tableHTML += '<td style="border: 1px solid black;">' + data[i].comments + '</td>';
                    tableHTML += '<td style="border: 1px solid black;">' + (data[i].status == "Accepted" ? '<span class="badge badge-success text-white">' + data[i].status + '</span>' : '<span class="badge badge-danger text-white">' + data[i].status + '</span>') + '</td>';
                    tableHTML += '</tr>';
                }
            }

            tableHTML += '</tbody>';
            tableHTML += '</table>';

            $('#ChatBoxPDF').empty().html(tableHTML);

            $('#ChatBoxPDF .table').DataTable({
                "paging": false,
                "ordering": false,
                "info": false
            });
        },
        error: function () {
            alert('Error fetching comments.');
        }
    });
}


$(document).ready(function () {

    var TeamDetailPostBackURL = '/Projects/AttDetails';
    $(function () {
        /*$(".anchorDetail").click(function () {*/
        $(document).on("click", ".anchorDetail", function () {

            var $buttonClicked = $(this);
            var id = $buttonClicked.attr('data-id');
            var options = { "backdrop": "static", keyboard: true };
            $.ajax({
                type: "GET",
                url: TeamDetailPostBackURL,
                contentType: "application/json; charset=utf-8",
                data: { "Id": id },
                datatype: "json",
                success: function (datadata) {
                    
                    $('#myModalPagehistoryAttechment').modal('show');
                    $('#myModalContenthistoryAttechment').html(datadata);
                   /* $('#myModal').modal(options);*/
                    

                },
                error: function () {
                    alert("Dynamic content load failed.");
                }
            });

        });

    });

    $(document).on('click', '.pdf', function () {
        $('#ViewRecordsHistory').modal('show');
    });
});

document.addEventListener("DOMContentLoaded", function () {
    const pdfInput = document.getElementById("pdfInput");
    const pdfList = document.getElementById("pdfList");
    const selectedFileList = document.getElementById("selected-file-list");
    const remarksInput = document.getElementById("remarksInput");
    const uploadedFiles = [];

    pdfInput.addEventListener("change", handleFileUpload);

    function handleFileUpload(event) {

        const files = event.target.files;
        for (const file of files) {
            const fileType = file.type;

            if (fileType.includes("image") || fileType === "application/pdf") {
                const pdfItem = createPDFItem(file);
                pdfList.appendChild(pdfItem);
                uploadedFiles.push({
                    file: file,
                    pdfItem: pdfItem,
                    remarks: ""
                });

                pdfInput.value = "";
                updateSelectedFileList();
            }
        }
    }

    function createPDFItem(file) {
        const pdfItem = document.createElement("div");
        pdfItem.className = "pdf-item";
        pdfItem.setAttribute("data-filename", file.name);

        const pdfPreview = document.createElement("div");
        pdfPreview.className = "pdf-preview";

        const reader = new FileReader();
        reader.onload = function () {
            const fileType = file.type;

            if (fileType.includes("image")) {
                pdfPreview.innerHTML = `<img src="${reader.result}" alt="Image Preview" class="image-preview">`;
            } else if (fileType === "application/pdf") {
                pdfPreview.innerHTML = `<embed src="${reader.result}" type="application/pdf" width="100%" height="100%" />`;
            }
        };
        reader.readAsDataURL(file);

        const deleteButton = document.createElement("button");
        deleteButton.innerHTML = "&times;";
        deleteButton.className = "delete-button";
        deleteButton.addEventListener("click", () => {
            pdfList.removeChild(pdfItem);
            const index = uploadedFiles.findIndex(item => item.pdfItem === pdfItem);
            if (index !== -1) {
                uploadedFiles.splice(index, 1);
            }
            pdfInput.value = "";

            updateSelectedFileList();
        });

        pdfItem.appendChild(pdfPreview);
        pdfItem.appendChild(deleteButton);

        return pdfItem;
    }

    function formatFileSize(size) {
        const units = ['B', 'KB', 'MB', 'GB'];
        let index = 0;
        while (size >= 1024 && index < units.length - 1) {
            size /= 1024;
            index++;
        }
        return `${size.toFixed(2)} ${units[index]}`;
    }

    function updateSelectedFileList() {
        const selectedFileList = document.getElementById("selectedFileList");
        selectedFileList.innerHTML = "";

        for (const uploadedFile of uploadedFiles) {
            const listItem = document.createElement("li");
            const remarks = uploadedFile.remarks !== "" ? uploadedFile.remarks : "No Remarks";
            const fileSize = formatFileSize(uploadedFile.file.size);
            listItem.innerHTML = `<a href="#" class="popup-link">${uploadedFile.file.name} (${fileSize}) - Remarks: ${remarks}</a>`;
            selectedFileList.appendChild(listItem);
        }
    }

    document.addEventListener("DOMContentLoaded", function () {
        const popup = document.getElementById("popup");
        const closePopupButton = document.getElementById("closePopup");

        const popupLinks = document.querySelectorAll(".popup-link");
        popupLinks.forEach(link => {
            link.addEventListener("click", function (event) {
                event.preventDefault();
                popup.style.display = "block";
            });
        });

        closePopupButton.addEventListener("click", function () {
            popup.style.display = "none";
        });
    });



    remarksInput.addEventListener("input", function () {
        const remarks = remarksInput.value;
        const lastFileIndex = uploadedFiles.length - 1;
        uploadedFiles[lastFileIndex].remarks = remarks;
        updateSelectedFileList();
    });
    updateSelectedFileList();
});

//function PrintDiv() {
//    var divToPrint = document.getElementById('widget-content');
//    var popupWin = window.open('', '_blank', 'width=800,height=800,location=Center,Center=20px');
//    popupWin.document.open();
//    var tableStyle = getComputedStyle(document.getElementById('htmlTopdf'));
//    popupWin.document.write('<style type="text/css">');
//    popupWin.document.write(tableStyle.cssText);
//    popupWin.document.write('</style>');
//    popupWin.document.write('<html><body onload="window.print()">' +
//        '<div id="print-container">' +
//        divToPrint.innerHTML +
//        '</div>' +
//        '<div style="transform: rotate(-45deg);z-index:10000;opacity: 0.3;color: BLACK; position:fixed;top: auto; left: 6%; top: 39%;color: #8e9191;font-size: 80px; font-weight: 500px;display: grid;justify-content: center;align-content: center;">' +
//        '@(TempData["ipadd"])' + " " +
//        '</html>');

//    popupWin.document.close();
//}

//function GetAllComments() {
//    $.ajax({
//        type: "POST",
//        url: '/Projects/GetAllCommentBypsmId_UnitId',
//        data: {
//            "PsmId": $("#ProjectcommentPsmId").html(),
//            "stakeholderId": 1,
//            "ProjId": $("#ProjectcommentprojId").html()
//        },
//        success: function (data) {

//            var commentContainer = '';
//            var userDetails = '';
//            if (data != null) {
//                for (var i = 0; i < data.length; i++) {
//                    var date = new Date(data[i].date);
//                    var formattedDate =
//                        ("0" + date.getDate()).slice(-2) + '-' +
//                        ("0" + (date.getMonth() + 1)).slice(-2) + '-' +
//                        date.getFullYear() + ' ' +
//                        ("0" + date.getHours()).slice(-2) + ':' +
//                        ("0" + date.getMinutes()).slice(-2) + ':' +
//                        ("0" + date.getSeconds()).slice(-2);
//                    if (data[i].userDetails == null)
//                        userDetails = '';
//                    else
//                        userDetails = data[i].userDetails

//                    commentContainer += '<div class="comment-box" style="text-align: justify;">'; // Use text-align: justify for justified text
//                    commentContainer += '<div class="comment-header">';
//                    commentContainer += '<div>';
//                    commentContainer += '<span style="font-family: Arial; color: #0793f7;">' + data[i].stakeholder + ' (' + userDetails + ') </span>';
//                    commentContainer += '<div style="margin-left: 0px;" class="comment-meta">' + DateFormateddMMyyyyhhmmss(data[i].date) + '</div>';
//                    commentContainer += '</div>';
//                    commentContainer += '<div>';
//                    if (data[i].status == "Accepted")
//                        commentContainer += '<span class="comment-meta badge badge-success text-white">' + data[i].status + '</span>';
//                    else if (data[i].status == "Obsn")
//                        commentContainer += '<span class="comment-meta badge badge-warning text-white">' + data[i].status + '</span>';
//                    else
//                        commentContainer += '<span class="comment-meta badge badge-danger text-white">' + data[i].status + '</span>';
//                    if (data[i].attpath !== '' && data[i].attpath !== null) {
//                        commentContainer += '<a href="/Home/WaterMark3?id=' + data[i].attpath + '" target="_blank">';
//                        commentContainer += '<img src="/assets/images/icons/pdfimg.png" alt="PDF icon" style="width: 24px; height: 24px;">';
//                        commentContainer += '</a>';
//                    }


//                    commentContainer += '</span>';
//                    commentContainer += '</div>';
//                    commentContainer += '</div>';
//                    commentContainer += '<div class="comment-content formated-text"><p>' + data[i].comments + '</p></div>';
//                    commentContainer += '</div>';
//                }



//                commentContainer += '</div>'; // Close the container
//                $('#ChatBox').empty().html(commentContainer);




//            }

//        },
//        error: function () {
//            alert('Error fetching comments.');
//        }
//    });
//}