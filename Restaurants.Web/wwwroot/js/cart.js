$(function () {
    $.ajax({
        type: "POST",
        url: "/cart/updateQuantityInCart",
        dataType: "text",
        success: function (msg) {
            
            let cart = document.getElementById("cart-quantity");
            if (cart == null) {
                return;
            }
            document.getElementById("cart-quantity").innerHTML = msg;
        },
        error: function (req, status, error) {
            alert(error);
        }
    });
});