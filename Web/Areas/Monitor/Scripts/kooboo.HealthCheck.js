var kooboo = kooboo || {};
(function (kb) {
    var HealthCheck = function (cfg) {
        var self = this;
        self.config = {
            reCheckUrl: null,
            reVisitUrl: null,
            getStateUrl: null,
            stopUrl: null,
            done: null
        };
        $.extend(self.config, cfg);
        self.init();
    };

    HealthCheck.prototype = {
        events: ['recheck', 'stop', 'revisit', 'stateChange', 'recheckFail'],
        init: function () {
            var self = this;
            this.state = 0;
            this.listener = new kb.EventManager();
            $.each(self.events, function (ix, it) {
                self.listener.add(it);
            });
        },
        setState: function (state) {
            this.state = state;
            this.listener.fire("stateChange");
        },
        isRunning: function () {
            return this.state == 1 || this.state == 3 || this.state == 5 || this.state == 7;
        },
        getState: function () {
            var self = this;
            var fn = arguments.callee;
            $.ajax({
                url: self.config.getStateUrl,
                type: "POST",
                dataType: "json"
            }).done(function (data) {
                if (self.config.done) {
                    self.config.done.call(self, data);
                }
                self.setState(data.State);
                if (self.isRunning()) {
                    setTimeout(function () {
                        fn.call(self);
                    }, 2000);
                }
            });
        },
        reCheck: function () {
            var self = this;
            if (this.isRunning()) {
                return false;
            }
            self.listener.fire("recheck");
            $.ajax({
                url: this.config.reCheckUrl,
                type: "POST",
                dataType: "json",
                beforeSend: function () {
                    window.loading.show();
                },
                complete: function () {
                    window.loading.hide();
                }
            }).done(function (data) {
                if (data.Success && self.config.done) {
                    self.config.done.call(self, data);
                }
                if (data.Messages.length > 0) {
                    var str = data.Messages.join("<br />");
                    window.info.show(str, data.Success);
                }
                self.getState();
            }).fail(function () {
                self.listener.fire("recheckFail");
            });
        },
        reVisit: function () {
            var self = this;
            if (this.isRunning()) {
                return false;
            }
            var postData = window.grid.getSelecteds();
            if (_.any(postData)) {
                self.listener.fire("revisit");
                $.ajax({
                    url: this.config.reVisitUrl,
                    type: "POST",
                    data: postData,
                    dataType: "json"
                }).done(function (data) {
                    if (data.Success && self.config.done) {
                        self.config.done.call(self, data);
                    }
                    if (data.Messages.length > 0) {
                        var str = data.Messages.join("<br />");
                        window.info.show(str, data.Success);
                    }
                    self.getState();
                });
            }
        },
        stop: function () {
            var self = this;
            $.ajax({
                url: this.config.stopUrl,
                type: "POST",
                dataType: "json"
            }).done(function (data) {
                if (data.Messages.length > 0) {
                    var str = data.Messages.join("<br />");
                    window.info.show(str, data.Success);
                }
                if (data.Success) {
                    self.listener.fire("stop");
                }
            });
        }
    };
    kb.HealthCheck = HealthCheck;
})(kooboo);