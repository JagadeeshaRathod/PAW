var table1 = $('#HeldTable').DataTable({
    lengthChange: false,
    dom: 'lBfrtip',
    buttons: [
        'copy',
        'excel',
        'csv',
        {
            text: 'PDF',
            extend: 'pdfHtml5',
            action: function (e, dt, node, config) {
                var popupWin = window.open('', '_blank', 'top=100,width=900,height=500,location=no');
                popupWin.document.open();

                var tableStyles = `
                                                                                                  <style type="text/css">
                                                                                                      table {
                                                                                                          width: 100%;
                                                                                                          border-collapse: collapse;
                                                                                                          margin-bottom: 20px;
                                                                                                      }

                                                                                                      .table > thead {
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

                // var table = $('#NewProjDetails').DataTable();

                var filteredData = table1.rows({ search: 'applied' }).data().toArray();

                var tableHTML = '<table>';

                tableHTML += '<thead>';
                tableHTML += '<tr>';
                table1.columns().header().each(function (header) {
                    tableHTML += '<th>' + header.innerHTML + '</th>';
                });
                tableHTML += '</tr>';
                tableHTML += '</thead>';

                tableHTML += '<tbody>';
                for (var i = 0; i < filteredData.length; i++) {
                    tableHTML += '<tr>';
                    for (var j = 0; j < filteredData[i].length; j++) {
                        tableHTML += '<td>' + filteredData[i][j] + '</td>';
                    }
                    tableHTML += '</tr>';
                }
                tableHTML += '</tbody>';

                tableHTML += '</table>';

                var watermarkText = '@(TempData["ipadd"])';

                popupWin.document.write('<html><head>'
                    + tableStyles + '</head><body onload="window.print()">'
                    + tableHTML + '<div style="transform: rotate(-45deg);z-index:10000;opacity: 0.3;color: BLACK; position:fixed;top: auto; left: 6%; top: 39%;color: #8e9191;font-size: 80px; font-weight: 500px;display: grid;justify-content: center;align-content: center;">'
                    + ip + '</div></body></html>');

                popupWin.document.close();
            }
        },
        {
            text: 'View Deatils',
            action: function (e, dt, node, config) {
                $("#ProjunderprocessModal").modal('show');
            }
        }
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

var table2 = $('#WhitelistedTable').DataTable({
    lengthChange: false,
    dom: 'lBfrtip',
    buttons: [
        'copy',
        'excel',
        'csv',
        {
            text: 'PDF',
            extend: 'pdfHtml5',
            action: function (e, dt, node, config) {
                var popupWin = window.open('', '_blank', 'top=100,width=900,height=500,location=no');
                popupWin.document.open();

                var tableStyles = `
                                                                                                  <style type="text/css">
                                                                                                      table {
                                                                                                          width: 100%;
                                                                                                          border-collapse: collapse;
                                                                                                          margin-bottom: 20px;
                                                                                                      }

                                                                                                      .table > thead {
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

                // var table = $('#NewProjDetails').DataTable();

                var filteredData = table2.rows({ search: 'applied' }).data().toArray();

                var tableHTML = '<table>';

                tableHTML += '<thead>';
                tableHTML += '<tr>';
                table2.columns().header().each(function (header) {
                    tableHTML += '<th>' + header.innerHTML + '</th>';
                });
                tableHTML += '</tr>';
                tableHTML += '</thead>';

                tableHTML += '<tbody>';
                for (var i = 0; i < filteredData.length; i++) {
                    tableHTML += '<tr>';
                    for (var j = 0; j < filteredData[i].length; j++) {
                        tableHTML += '<td>' + filteredData[i][j] + '</td>';
                    }
                    tableHTML += '</tr>';
                }
                tableHTML += '</tbody>';

                tableHTML += '</table>';

                var watermarkText = '@(TempData["ipadd"])';

                popupWin.document.write('<html><head>'
                    + tableStyles + '</head><body onload="window.print()">'
                    + tableHTML + '<div style="transform: rotate(-45deg);z-index:10000;opacity: 0.3;color: BLACK; position:fixed;top: auto; left: 6%; top: 39%;color: #8e9191;font-size: 80px; font-weight: 500px;display: grid;justify-content: center;align-content: center;">'
                    + $("#IpAddress").html() + '</div></body></html>');

                popupWin.document.close();
            }
        },
        {
            text: 'View Deatils',
            action: function (e, dt, node, config) {
                $("#ProjWhiteListedProjectModal").modal('show');
            }
        }
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

var table3 = $('#WhitelistedTableModal').DataTable({
    lengthChange: false,
    dom: 'lBfrtip',
    buttons: [
        'copy',
        'excel',
        'csv',
        {
            text: 'PDF',
            extend: 'pdfHtml5',
            action: function (e, dt, node, config) {
                var popupWin = window.open('', '_blank', 'top=100,width=900,height=500,location=no');
                popupWin.document.open();

                var tableStyles = `
                                                                                                  <style type="text/css">
                                                                                                      table {
                                                                                                          width: 100%;
                                                                                                          border-collapse: collapse;
                                                                                                          margin-bottom: 20px;
                                                                                                      }

                                                                                                      .table > thead {
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

                // var table = $('#NewProjDetails').DataTable();

                var filteredData = table3.rows({ search: 'applied' }).data().toArray();

                var tableHTML = '<table>';

                tableHTML += '<thead>';
                tableHTML += '<tr>';
                table3.columns().header().each(function (header) {
                    tableHTML += '<th>' + header.innerHTML + '</th>';
                });
                tableHTML += '</tr>';
                tableHTML += '</thead>';

                tableHTML += '<tbody>';
                for (var i = 0; i < filteredData.length; i++) {
                    tableHTML += '<tr>';
                    for (var j = 0; j < filteredData[i].length; j++) {
                        tableHTML += '<td>' + filteredData[i][j] + '</td>';
                    }
                    tableHTML += '</tr>';
                }
                tableHTML += '</tbody>';

                tableHTML += '</table>';

               

                popupWin.document.write('<html><head>'
                    + tableStyles + '</head><body onload="window.print()">'
                    + tableHTML + '<div style="transform: rotate(-45deg);z-index:10000;opacity: 0.3;color: BLACK; position:fixed;top: auto; left: 6%; top: 39%;color: #8e9191;font-size: 80px; font-weight: 500px;display: grid;justify-content: center;align-content: center;">'
                    + ip + '</div></body></html>');

                popupWin.document.close();
            }
        },
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

var table4 = $('#HeldTablemodal').DataTable({
    lengthChange: false,
    dom: 'lBfrtip',
    buttons: [
        'copy',
        'excel',
        'csv',
        {
            text: 'PDF',
            extend: 'pdfHtml5',
            action: function (e, dt, node, config) {
                var popupWin = window.open('', '_blank', 'top=100,width=900,height=500,location=no');
                popupWin.document.open();

                var tableStyles = `
                                                                                                  <style type="text/css">
                                                                                                      table {
                                                                                                          width: 100%;
                                                                                                          border-collapse: collapse;
                                                                                                          margin-bottom: 20px;
                                                                                                      }

                                                                                                      .table > thead {
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

                // var table = $('#NewProjDetails').DataTable();

                var filteredData = table4.rows({ search: 'applied' }).data().toArray();

                var tableHTML = '<table>';

                tableHTML += '<thead>';
                tableHTML += '<tr>';
                table4.columns().header().each(function (header) {
                    tableHTML += '<th>' + header.innerHTML + '</th>';
                });
                tableHTML += '</tr>';
                tableHTML += '</thead>';

                tableHTML += '<tbody>';
                for (var i = 0; i < filteredData.length; i++) {
                    tableHTML += '<tr>';
                    for (var j = 0; j < filteredData[i].length; j++) {
                        tableHTML += '<td>' + filteredData[i][j] + '</td>';
                    }
                    tableHTML += '</tr>';
                }
                tableHTML += '</tbody>';

                tableHTML += '</table>';

                var watermarkText = '@(TempData["ipadd"])';

                popupWin.document.write('<html><head>'
                    + tableStyles + '</head><body onload="window.print()">'
                    + tableHTML + '<div style="transform: rotate(-45deg);z-index:10000;opacity: 0.3;color: BLACK; position:fixed;top: auto; left: 6%; top: 39%;color: #8e9191;font-size: 80px; font-weight: 500px;display: grid;justify-content: center;align-content: center;">'
                    + ip + '</div></body></html>');

                popupWin.document.close();
            }
        },
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
setTimeout(function () {
    var coomatable = $('#tbldata').DataTable({
        lengthChange: true,
        order: [[1, 'desc']]
        dom: 'lBfrtip',
        exportOptions: {
            columns: "thead th:not(.noExport)"
        },
        buttons: [
            'copy',
            'excel',
            'csv',
            {
                text: 'PDF',
                extend: 'pdfHtml5',
                exportOptions: {
                  
                    search: 'applied',
                    order: 'applied',
                    columns: "thead th:not(.noExport)"
                },
                action: function (e, dt, node, config) {
                    var popupWin = window.open('', '_blank', 'top=100,width=900,height=500,location=no');
                    popupWin.document.open();

                    var tableStyles = `
                                                                                                  <style type="text/css">
                                                                                                      table {
                                                                                                          width: 100%;
                                                                                                          border-collapse: collapse;
                                                                                                          margin-bottom: 20px;
                                                                                                      }

                                                                                                      .table > thead {
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

                    // var table = $('#NewProjDetails').DataTable();

                    var filteredData = coomatable.rows({ search: 'applied' }).data().toArray();

                    var tableHTML = '<table>';

                    tableHTML += '<thead>';
                    tableHTML += '<tr>';
                    coomatable.columns().header().each(function (header) {
                        if (header.innerHTML!="")
                        tableHTML += '<th>' + header.innerHTML + '</th>';
                    });
                    tableHTML += '</tr>';
                    tableHTML += '</thead>';

                    tableHTML += '<tbody>';
                    for (var i = 0; i < filteredData.length; i++) {
                        tableHTML += '<tr>';
                        for (var j = 0; j < filteredData[i].length; j++) {
                            if(j!=0)
                            tableHTML += '<td>' + filteredData[i][j] + '</td>';
                        }
                        tableHTML += '</tr>';
                    }
                    tableHTML += '</tbody>';

                    tableHTML += '</table>';



                    popupWin.document.write('<html><head>'
                        + tableStyles + '</head><body onload="window.print()">'
                        + tableHTML + '<div style="transform: rotate(-45deg);z-index:10000;opacity: 0.3;color: BLACK; position:fixed;top: auto; left: 6%; top: 39%;color: #8e9191;font-size: 80px; font-weight: 500px;display: grid;justify-content: center;align-content: center;">'
                        + $("#IpAddress").html() + '</div></body></html>');

                    popupWin.document.close();
                }
            }
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
    })

}, 1000);