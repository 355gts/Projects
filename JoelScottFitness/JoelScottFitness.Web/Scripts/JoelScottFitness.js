// nav menu scroll event handlers
$(window).scroll(function () {
    OffsetMenu();
});

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
    if (window.location.pathname !== '/' || window.location.pathname.toLowerCase().indexOf('index') === 0) {
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

// get blog post
function getBlog(id) {
        $.ajax({
        type: 'GET',
        url: '/Home/Blog/'+id,
        success: function (data) {
            $('#blog-title').text(data.title);
            $('#blog-sub-title').text(data.subTitle);
            $('#blog-date').text(data.date);
            $('#blog-content').text(data.content);
            $("#blog-modal").modal();
            return false;
        }
    });
}

// add item to shopping basket
function addToBasket(dropdownId) {
    
    $.ajax({
        type: 'POST',
        url: '/Home/AddToBasket',
        data: {
            id: $('#' + dropdownId + ' option:selected').val(),
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            getBasketItems();
        }
    });

    return false;
}

// remove item from shopping basket
function removeFromBasket(id, controlId) {

    $.ajax({
        type: 'POST',
        url: '/Home/RemoveFromBasket',
        data: {
            id: id,
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            $('#' + controlId).remove();
            $('#basket-total').text(calculateTotal());
            getBasketItems();
        }
    });

    return false;
}

// calls get basket items on page load to show the basket or not
$(function () {
    getBasketItems();
});

// get basket item count
function getBasketItems() {

    var toggled = false;

    $.ajax({
        type: 'GET',
        url: '/Home/GetBasketItemCount',
        success: function (data) {
            if (data.items > 0) {
                // populate the items badge and show the basket div
                $('#basket-badge').text(data.items);

                if (!$('#basket-container').is(":visible")) {
                    $('#basket-container').animate({ width: 'toggle' }, "slow");
                }
            }
            else if (data.items <= 0 && $('#basket-container').is(":visible")) {
                $('#basket-container').animate({ width: 'toggle' }, "slow");
            }
        }
    });
}

// increase an items quantity
function increaseQuantity(id, controlId) {

    var action = 'IncreaseQuantity';

    return changeQuantity(id, controlId, action);
}

// decrease an items quantity
function decreaseQuantity(id, controlId) {

    var action = 'DecreaseQuantity';

    return changeQuantity(id, controlId, action);
}

function changeQuantity(id, controlId, action) {

    $.ajax({
        type: 'POST',
        url: '/Home/' + action,
        data: {
            id: id,
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            $('#' + controlId).text(data.Quantity);
            $('#basket-total').text(calculateTotal());
        }
    });

    return false;
}

// calls get basket items on page load to show the basket or not
$(function () {
    calculateTotal();
});

function calculateTotal() {
    
    $.ajax({
        type: 'GET',
        url: '/Home/CalculateTotal',
        success: function (data) {
            $('#basket-total').text("£"+data.TotalPrice);
        }
    });
}