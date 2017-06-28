function setMenu(el) {
    $('#login_selection li a').each(function (i) {
        $('#' + $(this).attr('view')).hide();
        $(this).removeClass('active');

    });
    $(el).addClass('active');
    $('#' + $(el).attr('view')).show();
}

function setupNewUserFields() {
    $('#new_user_given').focus();
    $('input[name*="new_user_field"]').each(function () {
        $(this).val('');
    });
}

function checkKey(el, event) {
    var key = event.keyCode || event.charCode;
    if (key === 13) {
        var found = false;
        $('input[name*="new_user_field"]').each(function (i) {
            if (found === true) {
                $(this).focus();
                found = false;
            }
            if ($(this).attr('id') === el.id) {
                found = true;
            }
        });
    }
}
