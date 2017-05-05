window.dynamicNumbersBound = false;
var wow = new WOW();
WOW.prototype.show = function (box) {
    wow.applyStyle(box);
    if (typeof box.parentNode !== 'undefined' && hasClass(box.parentNode, 'dynamic-numbers') && !window.dynamicNumbersBound) {
        bindDynamicNumbers();
        window.dynamicNumbersBound = true;
    }
    return box.className = "" + box.className + " " + wow.config.animateClass;
};
wow.init();

function hasClass(element, cls) {
    return (' ' + element.className + ' ').indexOf(' ' + cls + ' ') > -1;
}

function bindDynamicNumbers() {
    $('.dynamic-number').each(function () {
        var startNumber = $(this).text();
        var endNumber = $(this).data('dnumber');
        var dynamicNumberControl = $(this);

        $({ numberValue: startNumber }).animate({ numberValue: endNumber }, {
            duration: 4000,
            easing: 'swing',
            step: function () {
                $(dynamicNumberControl).text(Math.ceil(this.numberValue));
            }
        });
    });
}


$(function () {
    $("#backTop").backTop({
        position: 400,
        speed: 500,
        color: 'red'
    });

    $("img").on("contextmenu", function () {
        return false;
    });

})

function GetPage(url, target) {
    $.get(url, function (data) {
        $(target).html(data);
    });
}

function ActivateNavigation(elem) {
    $(".main-nav , .user-nav ").find(elem).addClass('current-menu-item');
}