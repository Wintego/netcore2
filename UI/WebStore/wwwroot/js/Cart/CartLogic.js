import { setTimeout } from "timers";
import { Function } from "core-js";

Cart = {
    _properties: {
        addToCartLink: "",
        getCartViewLink: "",
        removeFromCartLink: ""
    },

    init: function (properties) {
        $.extend(Cart._properties, properties);
        Cart.initAddToCart();
    },

    initEvents: function () {
        $("a.CallAddToCart").click(Cart.addToCart);
        $(".cart_quantity_delete").click(Cart.removeFromCart);
        $(".cart_quantity_up").click(Cart.incrementItem);
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
                var value = parseInt($(".cart", container).val());
            })
            .fail(function () console.log("incrementItem Error"))
    }
}