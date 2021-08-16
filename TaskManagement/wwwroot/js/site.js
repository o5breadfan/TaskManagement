$(function () {
    var PlaceHolderElement = $('#PlaceHolderHere');
    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            PlaceHolderElement.html(data);
            PlaceHolderElement.find('.modal').modal('show');
        })
    })

    PlaceHolderElement.on('click', '[data-save="modal"]', function (event) {

        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var sendData = form.serialize();
        $.post(actionUrl, sendData).done(function (data) {
            PlaceHolderElement.find('.modal').modal('hide');
        })
    })

})

var createButton = document.getElementById("CreateOrEditTask");
if (createButton) {
    createButton.addEventListener("click", function () {
        var url = $(this).data("url");
        $.ajax({
            type: "GET",
            url: url,
            success: function (res) {
                $("#form-modal .modal-body").html(res);
                $("#form-modal").modal('show');
            }
        })
    }, true)
}

/*var ajaxForm = document.getElementById("AjaxForm");*/
/*if (ajaxForm) {*/
//ajaxForm.addEventListener("submit", function (ajaxForm) {
ajaxForm = form => {
        try {
            $.ajax({
                type: "POST",
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    if (res.isValid) {
                        $("#TreeView").html(res.html);
                        $("#form-modal .modal-body").html('');
                        $("#form-modal").modal('hide');
                    }
                    else
                        $("#form-modal .modal-body").html(res.html);
                },
                error: function (err) {
                    console.log(err)
                }

            })
        }
        catch (e) {
            console.log(e);
    }

    return false;
}
