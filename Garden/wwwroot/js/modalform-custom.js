
$(function () {
    var placeholderElement = $('#modal-placeholder');

    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });
    });

    function submitEnter() {
        $('#save_btn').trigger('click');
    }

    placeholderElement.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();

        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataToSend = form.serialize();

        $.post(actionUrl, dataToSend).done(function (data) {

            var newBody = $('.modal-body', data);
            placeholderElement.find('.modal-body').replaceWith(newBody);

            var isValid = newBody.find('[name="IsValid"]').val() == 'True';
            if (isValid) {
                var replaceURL = newBody.find('[name="ReplaceURL"]').val();
                var replaceTarget = newBody.find('[name="ReplaceTarget"]').val();

                placeholderElement.find('.modal').modal('hide');

                if (!replaceTarget) {
                    if (!replaceURL) {
                        location.reload();
                    }
                    else {
                        window.location.href = replaceURL;
                    }
                }
                else {
                    if (!replaceURL) {
                        location.reload();
                    }
                    else {
                        $(replaceTarget).load(replaceURL);
                    }
                }
            }
        });
    });
});
