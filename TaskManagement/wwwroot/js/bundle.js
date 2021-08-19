CreateEditTask = url => {
        $.ajax({
            type: "GET",
            url: url,
            success: function (res) {
                $("#form-modal .modal-body").html(res);
                $("#form-modal").modal('show');
            }
        });
}


SaveTaskInfo = (form, id = null) => {
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
                        if (id != null)
                            ShowTaskDetails("/Task/Details/" + id);
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

ShowTaskDetails = url => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $('#taskDetails').html(res);
        },
        error: function (res) {
            console.log(res);
        }
    })
}

DeleteTask = form => {
    if (confirm('Вы уверены, что хотите удалить запись ?')) {
        try {
            $.ajax({
                type: "POST",
                url: form.action,
                data: new FormData(form),
                contentType: false,
                processData: false,
                success: function (res) {
                    $("#TreeView").html(res.html);
                    $('#taskDetails').html('');
                },
                error: function (err) {
                    console.log(err)
                }

            })
        }
        catch  {
            
        }
    }
    return false;
}

UpdateStatus = (form,id) => {
    try {
        $.ajax({
            type: "POST",
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                $("#TreeView").html(res.html);
                ShowTaskDetails("/Task/Details/" + id);
            },
            error: function (err) {
                console.log(err)
            }

        })
    }
    catch {

    }
    return false;
}