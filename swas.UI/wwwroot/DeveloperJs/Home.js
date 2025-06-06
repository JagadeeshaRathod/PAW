
$(document).ready(function () {
    $('.table-button').on('click', function () {
        var $button = $(this);
        var stakeHolderId = $button.data('stakeholder-id');
        var projId = $button.data('proj-id');
        var psmId = $button.data('psm-id');
        aler("Sanal");
        // Update the input elements with the values
        $('#StakeholdertextId').val(stakeHolderId);
        $('#ProjtextId').val(projId);
        $('#PsmToProj').val(psmId);

        // Add logic to change the button color here
        var status = $button.closest('td').attr('class');
        $button.removeClass('green red yellow'); // Remove existing color classes
        $button.addClass(status); // Add the new color class
    });
});




document.onreadystatechange = function () {
    if (document.readyState === 'interactive') {
        var popupTriggers = document.getElementById("preDev");
        var popupOverlay = document.getElementById("popupOverlay");
        var closeButton = document.getElementById("closeButton");

        popupTriggers.onclick = function () {
            popupOverlay.style.display = "block";
        };

        closeButton.onclick = function () {
            popupOverlay.style.display = "none";
        };

        popupOverlay.onclick = function (event) {
            if (event.target === popupOverlay) {
                popupOverlay.style.display = "none";
            }
        };
    }
};






document.onreadystatechange = function () {
    if (document.readyState === 'interactive') {
        var popupTrigger = document.getElementById("popupTrigger");
        var popupContent = document.getElementById("popupContent");

        popupTrigger.onclick = function () {
            if (popupContent.style.display === "none" || popupContent.style.display === "") {
                popupContent.style.display = "block";
            } else {
                popupContent.style.display = "none";
            }
        };
    }
};




var TeamDetailPostBackURL = '/Projects/AttDetails';
$(function () {
    $(".anchorDetail").click(function () {

        var $buttonClicked = $(this);
        var id = $buttonClicked.attr('data-id');
        var options = { "backdrop": "static", keyboard: true };
        $.ajax({
            type: "GET",
            url: TeamDetailPostBackURL,
            contentType: "application/json; charset=utf-8",
            data: { "Id": id },
            datatype: "json",
            success: function (data) {

                $('#myModalContent').html(data);
                $('#myModal').modal(options);
                $('#myModal').modal('show');

            },
            error: function () {
                alert("Dynamic content load failed.");
            }
        });

    });

});






var myChart1;

$(document).ready(function () {

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

                var unitNames = [...new Set(data.map(item => item.unitname))];

                var colors = []; // Store unique colors for each unit

                // // Generate unique colors for each unit
                // unitNames.forEach(unitName => {
                //     colors.push(getRandomColorss()); // One color for TotalIn
                //     colors.push(getRandomColorss()); // Another color for TotalOut
                // });

                //  

                var totalInColor = getRandomColorss(); // Get a random color for TotalIn bars
                var totalOutColor = getRandomColorss(); // Get a random color for TotalOut bars


                var colors = []; // Store unique colors for each unit

                // Generate unique colors for each unit
                unitNames.forEach(unitName => {
                    colors.push(getRandomColorss()); // One color for TotalIn
                    colors.push(getRandomColorss()); // Another color for TotalOut
                });

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

            }).flat(); // Use flat() to flatten the array of arrays into a single array


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
        }
    });



    // Function to lighten a given color
    function lightenColor(color, percent) {
        var num = parseInt(color.replace("#", ""), 16),
            amt = Math.round(2.55 * percent),
            R = (num >> 16) + amt,
            B = (num >> 8 & 0x00FF) + amt,
            G = (num & 0x0000FF) + amt;
        return "#" + (0x1000000 + (R < 255 ? R < 1 ? 0 : R : 255) * 0x10000 + (B < 255 ? B < 1 ? 0 : B : 255) * 0x100 + (G < 255 ? G < 1 ? 0 : G : 255)).toString(16).slice(1);
    }


    // Function to generate random color
    function getRandomColorss() {
        var letters = '0123456789ABCDEF';
        var color = '#';
        for (var i = 0; i < 6; i++) {
            color += letters[Math.floor(Math.random() * 16)];
        }
        return color;
    }



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

            // var titles = data.map(item => item.Status);
            // var chartData = data.map(item => item.TotalProj);

            // updatePieChart(titles, chartData);

        },
        error: function (error) {

            console.error('Error fetching data:', error);
        }
    });


})



const containerTabs = document.getElementById("tabs");

const getInfoTabs = container => {
    return [...container.querySelectorAll(".tabs__content__item")];
};

const getLinksTab = container => {
    return [...container.querySelectorAll("a[data-tab]")];
};

const activateTab = (tabId) => {
    const tabsInfo = getInfoTabs(containerTabs);

    tabsInfo.forEach(tab => {
        const isActive = tab.getAttribute("id") === tabId;
        tab.classList.toggle("active-tab", isActive);
    });
};

const activateLink = (link) => {
    const linksTab = getLinksTab(containerTabs);
    linksTab.forEach(tabLink => {
        tabLink.classList.toggle("active-link", tabLink === link);
    });
};

const handleTabClick = event => {
    const clickedElement = event.target;
    if (clickedElement.tagName === "A" && clickedElement.hasAttribute("data-tab")) {
        event.preventDefault();
        const tabId = clickedElement.getAttribute("data-tab");
        activateTab(tabId);


        activateLink(clickedElement);
    }
};

containerTabs.onclick = handleTabClick;

// Simulate click event on the first tab to load its content on page load
const firstTabLink = getLinksTab(containerTabs)[0];
if (firstTabLink) {
    firstTabLink.click();
}



$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
});

function openPopup(id, status) {
    var newUrl = "/Projects/ProjStatDashBdView?id=" + encodeURIComponent(id) + "&status=" + encodeURIComponent(status);

    window.location.href = newUrl;

}




function openPopup(id) {
    document.getElementById('modalContent').innerText = id;
    $('#myModal').modal('show');
    window.location.href = '/ControllerName/ActionName?id=' + id;
}

$(document).ready(function () {
    // Handle dropdown change event
    $("#ddlUnitId").change(function () {
        // Here you can perform actions based on the selected mode
        var selectedMode = $(this).val();
        // You may fetch additional data based on the selected mode using AJAX if needed
    });
});

function ValInData(input) {
    var regex = /[^a-zA-Z0-9/ ]/g;
    input.value = input.value.replace(regex, "");
}

$(document).ready(function () {
    $(document).on('click', '.pluscircless', function () {
        $('#RegisterX').modal('show');

    });

});

$(document).ready(function () {
    $(document).on('click', '.pluscircle', function () {
        $('#UnitAdd').modal('show');

    });

});

function processButtonClick() {
    var buttonColor = "@(ViewBag.ProcessButtonColor)";

    Swal.fire({
        title: 'Are you sure?',
        text: 'Do you want to proceed?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: buttonColor === 'green' ? '#28a745' : (buttonColor === 'red' ? '#dc3545' : '#007bff'), // Green for "OK", Red for "Cancel"
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'OK',
        cancelButtonText: 'Cancel'

    }).then((result) => {
        if (result.isConfirmed) {
            if (buttonColor === 'green') {
                var signInUrl = '/Home/Index';
                window.location.href = signInUrl;
            } else if (buttonColor === 'red') {
                var signUpUrl = '/Identity/Account/Register';
                window.open(signUpUrl, '_blank');
            }
        }
    });
};

$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
});

function openPopup(id, status) {
    var newUrl = "/Projects/ProjStatDashBdView?id=" + encodeURIComponent(id) + "&status=" + encodeURIComponent(status);

    window.location.href = newUrl;

}