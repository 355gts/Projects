﻿// nav menu scroll event handlers
$(window).scroll(function () {
    OffsetMenu();
    return true;
});

$(window).resize(function () {
    OffsetMenu();
    return true;
});

$(function () {
    OffsetMenu();
    return true;
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
            cache: false,
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

            var indicators = '';
            var entries = '';
            var active = 'active item-active';

            $('#blog-modal-carousel-wrapper').empty();

            if (data.images !== undefined) {
                $.each(data.images, function (index, entry) {
                    if (index === 0) {
                        indicators = "<li data-target='#blog-modal-carousel' data-slide-to='" + index + "' class='active'></li>\n";
                    } else {
                        indicators = indicators + "<li data-target='#blog-modal-carousel' data-slide-to='" + index + "'></li>\n";
                        active = '';
                    }               

                    entries = entries + "<div class='item " + active + " blog-modal-carousel-item'>" +
                        "<img src='" + entry.ImagePath + "' class='blog-modal-carousel-item-image' />" +
                        "<div class='carousel-caption blog-modal-carousel-item-caption'>";

                    if (entry.Caption !== '' && entry.CaptionTitle !== '') {
                        entries = entries + "<h3>Los Angeles</h3>" +
                            "<p>LA is always so much fun!</p>";

                        var logoClass = "blog-modal-carousel-item-caption-logo-black";
                        if (entry.CaptionColour == 1) {
                            logoClass = "blog-modal-carousel-item-caption-logo-white";
                        }  
                        entries = entries + "<div class='logo blog-modal-carousel-item-caption-logo " + logoClass+"'></div>";
                    }
                    
                    entries = entries + "</div>" +
                                        "</div>";

                });
            }

            if ((indicators !== '') && (entries !== '')) {

                indicators = "<ol id='carousel-indicators' class='carousel-indicators'>"+indicators+"</ol>";
                entries = "<div id='carousel-inner' class='carousel-inner'>" + entries + "</div>";

                var leftArrow = "<a class='left carousel-control' href='#blog-modal-carousel' data-slide='prev'> " +
                    "<span class='glyphicon glyphicon-chevron-left'></span>" +
                    "<span class='sr-only'>Previous</span>" +
                    "</a>";
                var rightArrow = "<a class='right carousel-control' href='#blog-modal-carousel' data-slide='next'>" +
                    "<span class='glyphicon glyphicon-chevron-right'></span>" +
                    "<span class='sr-only'>Next</span>" +
                    "</a>";


                var carousel = "<div id='blog-modal-carousel' class='carousel slide blog-modal-carousel-wrapper' data-ride='carousel'>" +
                    indicators +
                    entries +
                    leftArrow +
                    rightArrow +
                    "</div>";

                $('#blog-modal-carousel-wrapper').append(carousel);
            }
            
            $('#blog-modal').modal();
            return false;
        }
    });
}

// add item to shopping basket
function addToBasket(dropdownId) {
    
    $.ajax({
        type: 'POST',
        cache: false,
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
        cache: false,
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
        cache: false,
        url: '/Home/GetBasketItemCount',
        success: function (data) {
            if (data.items > 0) {
                // populate the items badge and show the basket div
                $('#basket-badge').text(data.items);

                if (!$('#basket-cart-container').is(":visible")) {
                    $('#basket-cart-container').animate({ width: 'toggle' }, "slow");
                }
            }
            else if (data.items <= 0 && $('#basket-cart-container').is(":visible")) {
                $('#basket-cart-container').animate({ width: 'toggle' }, "slow");
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
        cache: false,
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
        cache: false,
        url: '/Home/CalculateTotal',
        success: function (data) {
            if (data.TotalPrice == 0) {
                $('#basket-wrapper').hide();
                $('#no-items').show();
            }
            else {
                $('#basket-total').text("£" + data.TotalPrice);
            }
        }
    });
}

// show-hide account registration inputs
$(document).ready(function () {
    $('.register-checkbox').change(function () {
        if (this.checked) {
            $('.registration-element').each(function (i, obj) {
                $(obj).removeClass('element-hidden');
            });
        }
        else {
            $('.registration-element').each(function (i, obj) {
                $(obj).addClass('element-hidden');
            });
        }
    });
});

function addPlanOption(planId) {
    var description = $("#add-description").val();
    var duration = $("#add-duration").val();
    var price = $("#add-price").val();
    var index = parseInt($("#plan-option-count").val());
    var markup = "<tr>" +
    "<input class='basket-value' id='Options[" + index + "].ItemType' name='Options[" + index + "].ItemType' type='hidden' value='Plan' > " +
    "<input class='basket-value' id='Options[" + index + "].PlanId' name='Options[" + index + "].PlanId' type='hidden' value='" + planId + "' > " +
    "<td>" +
    "<input id='Options[" + index + "].Description' name='Options[" + index + "].Description' type='text' value='" + description + "' > " +
    "</td>" +
    "<td>" +
    "<input id='Options[" + index + "].Duration' name='Options[" + index + "].Duration' type='text' value='" + duration + "' > " +
    "</td>" +
    "<td>" +
    "<input id='Options[" + index + "].Price' name='Options[" + index + "].Price' type='text' value='" + price + "' > " +
    "</td>" +
    "</tr>";
    $("table tbody").append(markup);
    $("#add-description").val('');
    $("#add-duration").val('');
    $("#add-price").val('');
    $("#plan-option-count").val(index+1);
}
