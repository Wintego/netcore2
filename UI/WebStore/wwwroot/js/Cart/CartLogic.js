import { setTimeout } from "timers";
import { Function } from "core-js";
import { totalmem } from "os";
import { futimes } from "fs";

Cart = {
    _properties: {
        addToCartLink: "",
        getCartViewLink: "",
        removeFromCartLink: "",
        decrementLink: ""
    },

    init: function (properties) {
        $.extend(Cart._properties, properties);
        Cart.initAddToCart();
    },

    initEvents: function () {
        $("a.CallAddToCart").click(Cart.addToCart);
        $(".cart_quantity_delete").click(Cart.removeFromCart);
        $(".cart_quantity_up").click(Cart.incrementItem);
        $(".cart_quantity_down").click(Cart.decrementItem);
    },
    addToCart: function (event) {
        event.preventDefault();
        var button = $(this);
        var id = button.data("id");
        $.get(Cart._properties.addToCartLink + "/" + id)
            .done(function () {
                Cart.showTooltip(button);
            })
            .fail(function () {
                console.log("AddToCart Error");
            });
    },

    refreshCartView: function () {
        var container = $("#CartContainer");
        $.get(Cart._properties.getCartViewLink)
            .done(function (result) {
                container.html(result);
            })
            .fail(function () {
                console.log("refreshCartView Error");
            });
    },
    showTooltip: function () {
        button.tooltip({
            title: "Добавлено в корзину"
        }).tooltip("show");
    }

    setTimeout(function() {
        button.tooltip("destroy");
    }, 1000);

    removeFromCart: function(event) {
        event.preventDefault();
        var button = $(this);
        var id = button.data("id");
        $.get(Cart._properties.removeFromCartLink + "/" + id)
            .done(function () {
                button.closest("tr").remove();
                Cart.refreshCartView();
            })
            .fail(function() console.log("refreshCartView Error"));
    }
    incrementItem: function () {
        event.preventDefault();
        var button = $(this);
        var container = button.closest("tr");

        var id = button.data("id");
        $.get(Cart._properties.addToCartLink + "/" + id)
            .done(function () {
                var value = parseInt($(".cart_quantity_input", container).val());
                $(".cart_quantity_input", container).val(value + 1);
                Cart.refreshPrice(container);
                Cart.refreshCartView();
            })
            .fail(function () console.log("incrementItem Error"));
    },
    refreshPrice: function (container) {
        var quantity = parseInt($(".cart_quantity_input", container).val());
        var price = parseFloat($(".cart_price", container).data("price"));
        var totalPrice = quantity * price;
        var value = totalPrice.toLocaleString("ru-RU", { style: "currency", currency: "RUB" });

        $(".cart_total", container).data("price", totalPrice);
        $(".cart_total", container).html(value);

        Cart.refreshTotalPrice();
    }
    decrementItem: function (event) {
        event.preventDefault();
        var button = $(this);
        var container = button.closest("tr");

        var id = button.data("id");
        $.get(Cart._properties.decrementLink + "/" + id)
            .done(function () {
                var value = parseInt($(".cart_quantity_input", container).val());
                if (value > 1) {
                    $(".cart_quantity_input", container).val(value - 1);
                    Cart.refreshPrice(container);
                }
                else {
                    container.remove();
                    Cart.refreshTotalPrice();
                }
                Cart.refreshCartView();
            })
            .fail(function () console.log("decrementItem Error"));
    }
    refreshTotalPrice: function () {
        var total = 0;
        $(".cart_total_price").each(function () {
            var price = parseFloat($(this).data("price"));
            total += price;
        });

        var value = total.toLocaleString("ru-RU", { style: "currency", currency: "RUB" });
        $("#totalOrderSum").html(value);
    }
}