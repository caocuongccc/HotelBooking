$(function () {

    $.ajaxSetup({ cache: false });

    $("a[data-modal]").on("click", function (e) {

        // hide dropdown if any
        $(e.target).closest('.btn-group').children('.dropdown-toggle').dropdown('toggle');

        $('#loading').show();
        $('#myModalContent').load(this.href, function () {
            

            $('#myModal').modal({
                /*backdrop: 'static',*/
                keyboard: true
            }, 'show');
            $('#myModal').on('shown.bs.modal', function (e) {
                $('#loading').hide();
                
            });
            bindForm(this);
        });
        //$('#loading').hide();
        return false;
    });

    $('#myModal').on('show.bs.modal', function (e) {
        //$("style").remove();
    })

});

function bindForm(dialog) {
    
    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#myModal').modal('hide');
                    //Refresh
                    location.reload();
                } else {
                    $('#myModalContent').html(result);
                    bindForm();
                }
            }
        });
        return false;
    });
}