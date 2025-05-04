//$(document).ready(function () {
//    var url = '/Cart_23TH0024/ItemCartCount';
//    $.get(url, function (cart) {
//        $('.shopping-cart span').text(cart);
//    });
//    $('.ajaxForm').submit(async function (event) {
       
//        event.preventDefault();
//        var form = $(this);
//        var productId = form.find('input[name="productId"]').val();
//        var quantity = form.find('input[name="quantity"]').val();
//        var submitButton = $('input[type=submit]:focus');
//        $('#loading').show();
//        try {
//            var data = await $.ajax({
//                url: '/Cart_23TH0024/AddToCart',
//                type: 'POST',
//                data: {
//                    productId: productId, quantity: quantity,
//                    /*__RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()*/
//                }
//            });
//            if (data.success) {
//                if (submitButton.attr('name') === 'addToCart') {
//                    var cart = await $.get(url);
//                    $('.shopping-cart span').text(cart);
//                    submitButton.replaceWith('<span class="check-mark">✓</span>');
//                } else if (submitButton.attr('name') === 'buyNow') {
//                    location.href = '/Cart_23TH0024/Index';
//                }
//            } else {
//                $('#result').html('Đã có lỗi xảy ra: ' + JSON.stringify(data)); 
//            }
//        } catch (error) {
//            console.error("Error:", error); 
//            $('#result').html('Lỗi trong khi xử lý yêu cầu. Chi tiết lỗi: ' + JSON.stringify(error));  
//        }
//        finally {
//            $('#loading').hide();
//        }
//    });
//    $(".btn-quantity button").on("click", function () {
//        var quantityInput = $(this).siblings('input');
//        var currentValue = parseInt(quantityInput.val());
//        var productId = parseInt(quantityInput.attr("data-id"));
//        if ($(this).hasClass('decreaseQty') && currentValue > 1) {
//            quantityInput.val(currentValue - 1);
//        } else if ($(this).hasClass('increaseQty')) {
//            quantityInput.val(currentValue + 1);
//        }
//        currentValue = parseInt(quantityInput.val());
//        $.ajax({
//            url: 'Cart_23TH0024/UpdateQuantity',
//            type: 'POST',
//            data: { productId: productId, quantity: currentValue },
//            success: function (data) {
//                if (data.success) {
//                    location.reload();
//                } else {
//                    $('#result').html('Đã có lỗi xảy ra');
//                }
//            },
//            error: function (xhr, status, error) {
//                console.log("Status: " + status);
//                console.log("Error: " + error);
//                console.log("Response Text: " + xhr.responseText);
//                $('#result').html('Lỗi trong khi xử lý yêu cầu. Chi tiết lỗi: ' + error);
//            }
//        });
//    });
//    $(".payment-method li").on("click", function () {
//        $(this).find(".body-content").slideToggle();
//        $(".payment-method li").not(this).find(".body-content").slideUp();
//    });
//    $(".color label").on("click", function () {
//        $(this).toggleClass("check");
//    });
   
//    $(".form-search .title").on("click", function () {
//        $(this).next().slideToggle();
//        $(this).find("label i").toggleClass("fa-chevron-up fa-chevron-down");
//    });

//    //document.getElementById('submitButton').addEventListener('click', () => {
//    //    const form = event.target.closest('form');
//    //    if (form.checkValidity()) { form.submit(); }
//    //    else { alert('Vui lòng nhập đầy đủ thông tin!'); }
//    //});
//    document.getElementById("uploadForm").onsubmit = function() {
//        document.getElementById("loading").style.display = "block";
//        document.getElementById("uploadBtn").disabled = true;
//    };
//    $(window).scroll(function () {
//        if ($(this).scrollTop() > 70) {
//            $('.sticky').addClass('fixed');
//        } else {
//            $('.sticky').removeClass('fixed');
//        }
//    });
//});
