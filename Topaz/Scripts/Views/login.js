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
    checkPassword();
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

function checkPassword() {
    var checkPass = checkPasswordOptions();
    var checkDetails = checkUserDetails();
    var final = checkPass === true && checkDetails === true;
    $('#submit_create_user').prop('disabled', final);
}

function checkUserDetails() {
    var user = $('#new_user_given').val() + $('#new_user_surname').val();
    var email = $('#new_user_email').val();

    var fail = false;
    if (user.length < 2) {
        setPasswordFieldOk('#name_length', false, '#name_length_ng', '#name_length_ok');
        fail = true;
    } else {
        setPasswordFieldOk('#name_length', true, '#name_length_ok', '#name_length_ng');
    }

    if (email.indexOf('@') === -1 || email[email.length - 1] === '.') {
        setPasswordFieldOk('#email_length', false, '#email_length_ng', '#email_length_ok');
        fail = true;
    } else {
        setPasswordFieldOk('#email_length', true, '#email_length_ok', '#email_length_ng');
    }

    return fail;
}

function checkPasswordOptions() {
    var pass = $('#new_user_password').val();
    var confirm = $('#new_user_confirm').val();

    var uppercase = false;
    var number = false;
    for (var i = 0; i < pass.length; i++) {
        if ('A' <= pass[i] && pass[i] <= 'Z') {
            uppercase = true;
        }
        if ('0' <= pass[i] && pass[i] <= '9') {
            number = true;
        }
    }

    var fail = false;
    if (pass.length < 6) {
        setPasswordFieldOk('#password_length', false, '#6_char_ng', '#6_char_ok');
        fail = true;
    } else {
        setPasswordFieldOk('#password_length', true, '#6_char_ok', '#6_char_ng');
    }


    if (uppercase === false) {
        setPasswordFieldOk('#password_uppercase', false, '#pass_uppercase_ng', '#pass_uppercase_ok');
        fail = true;
    } else {
        setPasswordFieldOk('#password_uppercase', true, '#pass_uppercase_ok', '#pass_uppercase_ng');
    }

    if (number === false) {
        setPasswordFieldOk('#password_number', false, '#pass_number_ng', '#pass_number_ok');
        fail = true;
    } else {
        setPasswordFieldOk('#password_number', true, '#pass_number_ok', '#pass_number_ng');
    }

    if (pass !== confirm || pass.length === 0) {
        setPasswordFieldOk('#password_match', false, '#pass_match_ng', '#pass_match_ok');
        fail = true;
    } else {
        setPasswordFieldOk('#password_match', true, '#pass_match_ok', '#pass_match_ng');
    }

    return fail;
}

function setPasswordFieldOk(field, set, showOpt, hideOpt) {
    var ok = 'check_password_ok';
    var ng = 'check_password_error';

    var check = ok;
    var fail = ng;
    if (set === false) {
        check = ng;
        fail = ok;
    }

    if (!$(field).hasClass(check)) {
        $(field).addClass(check);
    }
    if ($(field).hasClass(fail)) {
        $(field).removeClass(fail);
    }

    $(showOpt).show();
    $(hideOpt).hide();
}

function createNewUser() {
    var data = {};
    data['Given'] = $('#new_user_given').val();
    data['Other'] = $('#new_user_other').val();
    data['Surname'] = $('#new_user_surname').val();
    data['Email'] = $('#new_user_email').val();
    data['Mobile'] = $('#new_user_mobile').val();
    data['Password'] = $('#new_user_password').val();
    $('#submit_create_user').prop('disabled', true);

    $.post('/User/CreateNewUser', { newUser: data }).done(function(result) {
        alert(result);
        setMenu($('#existing_user_login'));
        setupNewUserFields();
    }).fail(function(result) {
        alert('Sorry, something went wrong\n' + result);
    }).always(function() {
        $('#submit_create_user').prop('disabled', false);
    });
}

function checkLogin() {
    var user = $('#existing_username').val();
    var pass = $('#existing_password').val();
    if (user.length === 0 || pass.length === 0) {
        return;
    }
    $('#submit_login').text('Working...');
    $('#submit_login').prop('disabled', true);

    $.post('/User/CheckLogin', { user: user, password: pass }).done(function(result) {
        if (result === 'ok') {
            window.location = '/';
            return;
        }

        $('#login_error_message').text(result);
        $("#login_panel").effect("shake", { times: 2 });
        $('#existing_password').val('');
        $('#existing_password').focus();
    }).fail(function(result) {
        $('#login_error_message').text(result);
        $("#login_panel").effect("shake", { times: 2 });
        $('#existing_password').val('');
        $('#existing_password').focus();
    }).always(function(result) {
        $('#submit_login').text('Submit');
        $('#submit_login').prop('disabled', false);
    });
}

