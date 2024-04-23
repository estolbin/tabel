// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    $("#loaderbody").addClass('hide');

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $('#loaderbody').addClass('hide');
    });
});

showInPopup = (url, title) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $("#form-modal .modal-body").html(res);
            $("#form-modal .modal-title").html(title);
            $("#form-modal").modal('show');
        }
    })
}

//$('#depList').on('change', function (event) {
//    var form = $(event.target).parents('form');
//    form.submit();
//})

//$('#orgList').on('change', function (event) {
//   var form = $(event.target).parents('form');
//    form.submit();
//})

$('tr[id^="org_"').click(function () {
    console.log($(this).attr("id"));
    var className = $(this).attr("id");
    let id = className.replace("org_", "");
    $(".organization_" + id).toggle();
})

