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

// send email
function sendEmail() {
    $.ajax({
        type: 'GET',
        url: '/Admin/SendEmail',
        success: function (data) {
            var message = 'Email Sent';
        }
    });
}

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

                    if (entry.Caption !== null && entry.CaptionTitle !== null) {
                        entries = entries + "<h3 class='hide-caption'>" + entry.CaptionTitle+"</h3>" +
                            "<p class='hide-caption'>" + entry.Caption+"</p>";
                    }

                    var logoClass = "blog-modal-carousel-item-caption-logo-black";
                    if (entry.CaptionColour == 1) {
                        logoClass = "blog-modal-carousel-item-caption-logo-white";
                    }  
                    entries = entries + "<div class='logo blog-modal-carousel-item-caption-logo " + logoClass+"'></div>";
                    
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
            triggerCarousel();
            return false;
        }
    });
}

function triggerCarousel() {
    if (document.querySelector('.carousel') !== null) {
        $('.carousel').carousel({
            interval: 3000
        });

        $('.carousel').carousel('cycle');
    }
}

// upload before and after images
function showUploadModal(id) {
    $('#PurchasedItemId').val(id);
    $('#upload-title').text('Upload Before and After');
    $('#upload-sub-title').text('Show the world your transformation!');
    $('#upload-modal').modal();
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
            $('.' + controlId).remove();
            $('.basket-total').text(calculateTotal());
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
            $('.' + controlId).val(data.Quantity);
            calculateTotal();
        }
    });

    return false;
}

function applyDiscountCode(codeControl) {
    $('.basket-discount-input').each(function (i, obj) {
        $(obj).attr('placeholder', 'Discount Code');
    });
    $('.discount-code-id').each(function (i, obj) {
        $(obj).val('');
    });

    var code = $('.'+ codeControl).val();

    if (code != '') {
        $.ajax({
            type: 'POST',
            cache: false,
            url: '/Home/ApplyDiscountCode',
            data: {
                code: code,
                __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (data) {
                if (data.Applied == true) {
                    hideApplyDiscountCode(data.Description);
                    $('.discount-code-id').each(function (i, obj) {
                        $(obj).val(data.DiscountCodeId);
                    });
                    $('.discount').each(function (i, obj) {
                        $(obj).val(data.Discount);
                    });
                    applyDiscount();
                }
                else {
                    $('.basket-discount-input').each(function (i, obj) {
                        $(obj).val('');
                        $(obj).attr('placeholder', 'Invalid Code!');
                    });
                    $('.discount').each(function (i, obj) {
                        $(obj).val('');
                    });
                }
                calculateTotal();
            }
        });
    }
    else {
        $('.basket-discount-input').each(function (i, obj) {
            $(obj).val('');
        });
    }

    return false;
}

function applyDiscount() {
    var discount = parseInt($('.discount').first().val());

    if (discount > 0) {
        applyDiscountToItems(discount);
    }
    else {
        removeDiscountFromItems();
    }
}

function applyDiscountToItems(discount) {
    
    $('.basket-wrapper').each(function (i, obj) {
        var totalCost = 0;
        $(obj).find('.basket-item').each(function (i, obj) {
            var itemPriceElement = $(obj).find('#basket-item-price')[0];
            var itemPriceHiddenElement = $(obj).find('#basket-item-hidden-price')[0];
            var itemQuantity = $(obj).find('.basket-item-quantity')[0];

            var discountedPrice = (parseFloat($(itemPriceHiddenElement).val()) - (parseFloat($(itemPriceHiddenElement).val()) / 100 * parseInt(discount))) * parseInt($(itemQuantity).val());
            if (Math.round(discountedPrice) !== discountedPrice) {
                discountedPrice = discountedPrice.toFixed(2);
            }

            $(itemPriceElement).val('£' + discountedPrice);
            totalCost = parseFloat(totalCost) + parseFloat(discountedPrice);
        });

        if (Math.round(totalCost) !== totalCost) {
            totalCost = totalCost.toFixed(2);
        }

        $(obj).find('.basket-total-row').each(function (i, obj) {
            var totalPriceElement = $(obj).find('#basket-total')[0];
            $(totalPriceElement).val("£" + totalCost);
        });
    });
}

function removeDiscountFromItems() {

    $('.basket-wrapper').each(function (i, obj) {
        var totalCost = 0;
        $(obj).find('.basket-item').each(function (i, obj) {
            var itemPriceElement = $(obj).find('#basket-item-price')[0];
            var itemPriceHiddenElement = $(obj).find('#basket-item-hidden-price')[0];
            var itemQuantity = $(obj).find('.basket-item-quantity')[0];
            var runningItemTotal = parseFloat($(itemPriceHiddenElement).val()) * parseInt($(itemQuantity).val());

            if (Math.round(runningItemTotal) !== runningItemTotal) {
                runningItemTotal = runningItemTotal.toFixed(2);
            }

            totalCost = parseFloat(totalCost) + parseFloat(runningItemTotal);
            $(itemPriceElement).val('£' + parseFloat(runningItemTotal));
        });

        $(obj).find('.basket-total-row').each(function (i, obj) {
            var totalPriceElement = $(obj).find('#basket-total')[0];
            $(totalPriceElement).val("£" + totalCost);
        });
    });
}

function removeDiscountCode() {
    $('.discount-code-id').each(function (i, obj) {
        $(obj).val('');
    });

    $.ajax({
        type: 'POST',
        cache: false,
        url: '/Home/RemoveDiscountCode',
        data: {
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (data) {
            removeDiscountFromItems();
            showApplyDiscountCode();
            //calculateTotal();
        }
    });

    return false;
}

function showApplyDiscountCode() {
    $('.basket-discount-input').each(function (i, obj) {
        $(obj).val('');
    });
    $('.apply-discount-code-wrapper').each(function (i, obj) {
        $(obj).removeClass('basket-hide-element');
    });
    $('.remove-discount-code-wrapper').each(function (i, obj) {
        $(obj).addClass('basket-hide-element');
    });
}

function hideApplyDiscountCode(discountDescription) {
    $('.basket-discount-description').each(function (i, obj) {
        $(obj).val(discountDescription);
    });
    $('.remove-discount-code-wrapper').each(function (i, obj) {
        $(obj).removeClass('basket-hide-element');
    });
    $('.apply-discount-code-wrapper').each(function (i, obj) {
        $(obj).addClass('basket-hide-element');
    });
}

function calculateTotal() {
    
    $.ajax({
        type: 'GET',
        cache: false,
        url: '/Home/CalculateTotal',
        success: function (data) {
            if (data.TotalPrice == 0) {
                $('.basket-wrapper').each(function (i, obj) {
                    $(obj).hide();
                });
                $('.basket-summary').each(function (i, obj) {
                    $(obj).hide();
                });
                $('.active-basket-button-wrapper').hide();
                $('#no-items').show();
            }
            else {
                $('.basket-total').each(function (i, obj) {
                    $(obj).val("£" + data.TotalPrice);
                });
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

function addBlogImage(blogId) {
    var file = $("#add-file").val();
    var imagePath = $("#add-image-path").val();
    var captionTitle = $("#add-caption-title").val();
    var caption = $("#add-caption").val();
    var logoColour = $("#add-logo-colour").val();
    var index = parseInt($("#blog-image-count").val());

    if (file === undefined) {
        file = '';
    }
    var markup = "<tr>" +
        "<input class='basket-value' id='BlogImages[" + index + "].BlogId' name='BlogImages[" + index + "].BlogId' type='hidden' value='" + blogId + "' > " +
        "<td>" +
        "<input class='basket-value' id='BlogImages[" + index + "].PostedFile' name='BlogImages[" + index + "].PostedFile' type='file' value='" + file + "' > " +
        "</td>";
    if (blogId > 0) {
        markup = markup + "<td>" +
            "<input id='BlogImages[" + index + "].ImagePath' name='BlogImages[" + index + "].ImagePath' type='text' value='' readonly='readonly'> " +
            "</td>";
    }
    markup = markup +"<td>" +
        "<input id='BlogImages[" + index + "].CaptionTitle' name='BlogImages[" + index + "].CaptionTitle' type='text' value='" + captionTitle + "' > " +
        "</td>" +
        "<td>" +
        "<input id='BlogImages[" + index + "].Caption' name='BlogImages[" + index + "].Caption' type='text' value='" + caption + "' > " +
        "</td>" +
        "<td>" +
        "<select id='BlogImages[" + index + "].CaptionColour' name='BlogImages[" + index + "].CaptionColour' data-val='true' data-val-required='The Logo Colour field is required.' style='width:100%'>" +
        "<option selectedBlack value='Black'>Black</option>" +
        "<option selectedWhite value='White'>White</option>" +
        "</select>"
        "</td>" +
        "</tr>";

        if (logoColour === 'Black') {
            markup = markup.replace("selectedBlack", "selected='selected'");
            markup = markup.replace("selectedWhite", "");
        } else {
            markup = markup.replace("selectedWhite", "selected='selected'");
            markup = markup.replace("selectedBlack", "");
        }


    $("table tbody").append(markup);
    $("#add-file").val('');
    $("#add-image-path").val('');
    $("#add-caption-title").val('');
    $("#add-caption").val('');
    $("#add-logo-colour").val('Black');
    $("#blog-image-count").val(index + 1);
}

$(function () {
    $(".datepicker").datepicker({ dateFormat: 'dd/mm/yy' });
});