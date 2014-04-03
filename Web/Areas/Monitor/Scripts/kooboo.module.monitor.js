(function ($) {
    $.fn.reportTab = function (calls) {
        var heads = $(".field-tabs:eq(0)", this).children("a");
        var bodys = $(".field-bodys:eq(0)", this).children("div");
        heads.on('click', function (e) {
            e.preventDefault();
            var id = this.href.substring(this.href.indexOf("#"));
            if (id) {
                heads.removeClass("on");
                $(this).addClass("on");
                bodys.hide();
                var cont = $(id);
                cont.show();
                if (calls) {
                    var idx = heads.index(this);
                    if (idx >= 0 && typeof calls[idx] === 'function') {
                        calls[idx].call(cont);
                    }
                }
            }
        });
        var hash = window.location.hash;
        if (hash) {
            var anchor = heads.filter("a[href='" + hash + "']");
            if (anchor.length) {
                anchor.click();
            } else {
                heads.first().click();
            }
        } else {
            var anchor = heads.filter("a[class*='on']");
            if (anchor.length) {
                anchor.click();
            } else {
                heads.first().click();
            }
        }
    };
})(jQuery);