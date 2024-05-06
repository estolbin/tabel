﻿$(function () {
    $("#loaderbody").addClass('hide');

    $(document).on('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).on('ajaxStop', function () {
        $('#loaderbody').addClass('hide');
    });
});

const showInPopup = (url, title) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (response) {
            $("#form-modal .modal-body").html(response);
            $("#form-modal .modal-title").html(title);
            $("#form-modal").modal('show');
        }
    });
}

$('tr[id^="org_"').click(function () {
    const id = $(this).attr("id").replace("org_", "");
    $(".organization_" + id).toggle();
})

$('#select_period').change(function () {
    const period = $(this).val();
    //alert(period)
    $.ajax({
        url: '/TimeShift/SavePeriodId',
        type: 'POST',
        data: { periodId: period },
        success: function (response) {
            //window.location.reload();
            window.location.href = '/TimeShift/Index';
        }
    })
})


$(document).ready(function () {
    let table  = $('#timeShiftTable').DataTable
        ({
            ordering: false,
            searching: false,
            paging: false,
            info: false,
            scrollX: true,
            scrollCollapse: true,
            fixedColumns: {
                start: 2,
                heightMatch: 'auto'
            },
            fixedHeader: true
        });
})