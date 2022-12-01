
$(document).ready(function () {
    $('.fromdate').datepick({ dateFormat: 'dd/mm/yyyy' });
    $('.todate').datepick({ dateFormat: 'dd/mm/yyyy' });

    var d = new Date();
    var fromDate = '01/' + (d.getMonth() + 1) + '/' + d.getFullYear();
    var toDate = d.getDate() + '/' + (d.getMonth() + 1) + '/' + d.getFullYear();

    $('.fromdate').each(function (i, obj) {
        if ($(this).val() == '') {
            $(this).val(fromDate)
        }
    });

    $('.todate').each(function (i, obj) {
        if ($(this).val() == '') {
            $(this).val(toDate)
        }
    });
});

