// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $(".country-card-container").on("scroll", function () {
        var scrollLeft = $(this).scrollLeft();
        if (scrollLeft > 0) {
            $(this).addClass("scrolled");
        } else {
            $(this).removeClass("scrolled");
        }
    });

    $(".country-card-container").on("wheel", function (event) {
        event.preventDefault();
        var scrollAmount = event.originalEvent.deltaY;
        $(this).scrollLeft($(this).scrollLeft() + scrollAmount);
    });
});