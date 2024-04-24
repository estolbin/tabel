$(function () {
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