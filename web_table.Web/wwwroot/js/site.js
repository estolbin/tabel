$(function () {
    $("#loaderbody").addClass('hide');

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
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
    var groupColumn = 0;
    let table  = $('#timeShiftTable').DataTable
        ({
            rowGroup: {
                dataSrc: [2, 3]
            },
            columnDefs: [
                {
                    targets: [2, 3],
                    visible: false,
                }
            ],
            ordering: false,
            searching: false,
            paging: true,
            info: false,
            autoWidth: true,
            scrollX: true,
            scrollCollapse: true,
            fixedColumns: {
                start: 2,
            },
            fixedHeader: {
                header: true
            },
            language: {
                url: '//cdn.datatables.net/plug-ins/2.0.7/i18n/ru.json',
            },
            initComplete: () => { table.columns.adjust().draw(); }
        });

})



