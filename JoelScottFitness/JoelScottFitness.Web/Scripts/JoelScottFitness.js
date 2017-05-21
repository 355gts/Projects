// nav menu scroll event handlers
$(window).scroll(function () {
    OffsetMenu();
})

$(window).resize(function () {
    OffsetMenu();
});

// checks whether the nav menu should anchor to the top of the screen
function OffsetMenu() {
    var distance = $('.menu').offset().top;
    var $window = $(window);

    $('#nav').removeClass('sticky');
    $('#nav-back-mask').hide();

    if ($(window).scrollTop() >= $('.menu').offset().top) {
        $('#nav').addClass('sticky');
        $('#nav-back-mask').show();
    }
}