// nav menu scroll event handlers
$(window).scroll(function () {
    OffsetMenu();
})

$(window).resize(function () {
    OffsetMenu();
});

$(function () {
    OffsetMenu();
});

// checks whether the nav menu should anchor to the top of the screen
function OffsetMenu() {
    var distance = $('.menu').offset().top;
    var $window = $(window);

    $('#nav').removeClass('sticky');
    $('#nav-back-mask').removeClass('nav-back-mask-padded');

    if ($(window).scrollTop() >= $('.menu').offset().top) {
        $('#nav').addClass('sticky');
        $('#nav-back-mask').addClass('nav-back-mask-padded');
    }
}

// hides full page image when not on index
$(function () {
    if (window.location.pathname != '/' || window.location.pathname.toLowerCase().indexOf('index') == 0) {
        $('#full-page-image').hide();
    }
    else {
        $('#full-page-image').show();
    }
});

// submit the mailing list subscription
$(function () {
    $('#subscribe').click(function () {
        $.ajax({
            type: 'POST',
            url: '/Home/SubscribeToMailingList',
            data: {
                emailAddress: $('#emailAddress').val(),
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (data) {
                $('#emailAddress').val('');
                $('#emailAddress').attr('placeholder', 'Thanks!');
            }
        });
    });
});