// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(() => {
    $('#user img').on('click', function () {
        $('#modal-window').fadeIn();
    });
    $('#close').on('click', function () {
        $('#modal-window').fadeOut();
        $('#enter').fadeOut();
        $('#login-registration').fadeIn();
        return false;
    });
    $('.show-enter').on('click', function () {
        $('#login-registration').fadeOut(1);
        $('#enter').fadeIn();
    });
    $('#call').on('click', function () {
        $('#call-window').slideToggle();

    });
    $('#close-form').on('click', function () {
        $('#call-window').slideToggle();
    });

});