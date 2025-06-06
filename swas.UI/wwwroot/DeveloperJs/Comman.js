$(document).ready(function () {
    var pad = "00"
    var datef2 = new Date();
    var months = "" + `${(datef2.getMonth() + 1)}`;
    var days = "" + `${(datef2.getDate())}`;
    var monthsans = pad.substring(0, pad.length - months.length) + months
    var dayans = pad.substring(0, pad.length - days.length) + days
    var year = `${datef2.getFullYear()}`;
    var hh = pad.substring(0, pad.length - `${datef2.getHours()}`.length) + `${datef2.getHours()}`;
    var mm = pad.substring(0, pad.length - `${datef2.getMinutes()}`.length) + `${datef2.getMinutes()}`;
   // var mm = `${datef2.getMinutes()}`;
    var ss = `${datef2.getSeconds()}`;

    //var today = new Date().toISOString().split('T');  // Get today's date in YYYY-MM-DD format

    //today = today[0] + 'T' + today[1].substring(0,5);
    //var today = year + `-` + monthsans + `-` + dayans + `T` + hh + `:` + mm


    var today = year + `-` + monthsans + `-` + dayans;
   
    if ($("#isclaneder").html() == 1) {

        $('input[type=date]').attr('min', today);
        $('.datepicker1').datepicker({
            minDate: 0
        });
        $("#InitiatedDate").val(today);
        $('#InitiatedDate').attr('readonly', true);
    }   
    else {
        $('input[type=date]').attr('max', today);
        $('.datepicker1').datepicker();

    }
    $('.datetimepicker1').datepicker();

    // Remove the max date setting for CompletionDate to allow future selection
    $("#InitiatedDate").change(function () {

        $('#CompletionDate').val("");
        $('#CompletionDate').attr('min', $("#InitiatedDate").val());
        $('input[type=date]').attr('max', null);
    })
    



    $('.form-control').keypress(function (e) {
        // Get the key code of the pressed key
        // Get the key code of the pressed key
        var keyCode = e.which;
       
        // Allow only alphabets (A-Z, a-z) and numbers (0-9)
        if ((keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (keyCode >= 48 && keyCode <= 57) || (keyCode == 32)) {
            return true; // Allow the keypress
        } else {

            if (keyCode == 46 || keyCode == 44 || keyCode == 40 || keyCode == 41 || keyCode == 45 || keyCode == 58 || keyCode == 47)
                return true; // Allow the keypress
            else {
                alert('Only Alphabets and Numbers allowed');
            return false; // Block the keypress
            }
            
        }
    });
});
function DateFormateyyy_mm_dd(date) {

    var todaysDate = new Date();
    var datef1 = new Date(date);
    //if (datef1.setHours(0, 0, 0, 0) == todaysDate.setHours(0, 0, 0, 0)) {
    //    // Date equals today's date

    //    return 'Today';
    //}
    //else {
    var datef2 = new Date(date);
    var months = "" + `${(datef2.getMonth() + 1)}`;
    var days = "" + `${(datef2.getDate())}`;
    var pad = "00"
    var monthsans = pad.substring(0, pad.length - months.length) + months
    var dayans = pad.substring(0, pad.length - days.length) + days
    var year = `${datef2.getFullYear()}`;
    var hh = `${datef2.getHours()}`;
    var mm = `${datef2.getMinutes()}`;
    var ss = `${datef2.getSeconds()}`;
    if (hh < 10) hh = "0" + hh;
    if (mm < 10) mm = "0" + mm;
    if (ss < 10) ss = "0" + ss;
    if (year > 1902) {

        var datemmddyyyy = year + `-` + monthsans + `-` + dayans
        return datemmddyyyy;
    }
    else {
        return '';
    }
    // }

    //`${datef2.getFullYear()}/` + monthsans + `/` + dayans ;
}
function DateFormateddMMyyyyhhmmss(date) {

    var todaysDate = new Date();
    var datef1 = new Date(date);
    //if (datef1.setHours(0, 0, 0, 0) == todaysDate.setHours(0, 0, 0, 0)) {
    //    // Date equals today's date

    //    return 'Today';
    //}
    //else {
    var datef2 = new Date(date);
    var months = "" + `${(datef2.getMonth() + 1)}`;
    var days = "" + `${(datef2.getDate())}`;
    var pad = "00"
    var monthsans = pad.substring(0, pad.length - months.length) + months
    var dayans = pad.substring(0, pad.length - days.length) + days
    var year = `${datef2.getFullYear()}`;
    var hh = `${datef2.getHours()}`;
    var mm = `${datef2.getMinutes()}`;
    var ss = `${datef2.getSeconds()}`;
    if (hh < 10) hh = "0" + hh;
    if (mm < 10) mm = "0" + mm;
    if (ss < 10) ss = "0" + ss;
    if (year > 1902) {

        var datemmddyyyy = dayans + `/` + monthsans + `/` + year + ` ` + hh + `:` + mm + `:` + ss
        return datemmddyyyy;
    }
    else {
        return '';
    }
    // }

    //`${datef2.getFullYear()}/` + monthsans + `/` + dayans ;
}
function DateCalculateago(fmDate, end_actual_time) {
    ////////ago///////////
    var ago = "";
    var start_actual_time = fmDate;
   /* var end_actual_time = new Date();*/

    start_actual_time = new Date(start_actual_time);
    end_actual_time = new Date(end_actual_time);

    var diff = end_actual_time - start_actual_time;

    var diffSeconds = diff / 1000;
    var HH = Math.floor(diffSeconds / 3600);
    var MM = Math.floor(diffSeconds % 3600) / 60;

    var formatted = ((HH < 10) ? ("0" + HH) : HH) + ":" + ((MM < 10) ? ("0" + MM.toFixed(0)) : MM.toFixed(0))

    var futureDate = new Date();
    var todayDate = new Date(fmDate);
    var milliseconds = end_actual_time.getTime() - start_actual_time.getTime();
    var hours = Math.floor(milliseconds / (60 * 60 * 1000));
    var formatted1 = formatted.substring(0, 2);
    if (hours <= 24) {
        ago = formatted +' Min </h6>';
    }
    else /*if (hours <= 730)*/
    {
        
        ago = Math.floor(hours / 24) + ' Days (' + formatted  +')</h6>';;
    }
    //else if (hours <= 8766) {
    //    ago = Math.floor(Math.floor(hours / 24) / 30) + ' Months</h6>';;
    //}
    //else {
    //    ago = Math.floor(Math.floor(Math.floor(hours / 24) / 30) / 12) + ' Years</h6>';;
    //}
    return ago;
}

function formatDateToDDMMYYYY(date) {
    // Parse the input date
    var dateObj = new Date(date);

    // Ensure the date is valid
    if (isNaN(dateObj.getTime())) {
        return ''; // Return an empty string if the date is invalid
    }

    // Extract day, month, and year
    var day = dateObj.getDate().toString().padStart(2, '0'); // Ensure 2 digits
    var month = (dateObj.getMonth() + 1).toString().padStart(2, '0'); // Ensure 2 digits
    var year = dateObj.getFullYear();

    // Return the formatted date
    return `${day}-${month}-${year}`;
}
