$(function () {
    var pop_power = $("#checking_pop_power");
    var pop_title = $("#checking_title");

    function HealthViewModel() {
        var self = this;
        self.state = ko.observable(0);
        self.isChecking = ko.computed(function () {
            return self.state() == 1 || self.state() == 3;
        });
        self.isVisiting = ko.computed(function () {
            return self.state() == 5 || self.state() == 7;
        });
        self.isRunning = ko.computed(function () {
            return self.isChecking() || self.isVisiting();
        });
        self.ran = ko.observable(false);
        self.entry = ko.observable(0);
        self.stateText = ko.computed(function () {
            var txt = 'Ready';
            switch (self.state()) {
                case 0:
                    txt = self.entry() == 0 ? 'Ready to check' : 'Ready to visit';;
                    break;
                case 1:
                    txt = 'Checking';
                    break;
                case 2:
                    txt = 'Check completed';
                    break;
                case 3:
                    txt = 'Check canceling';
                    break;
                case 4:
                    txt = 'Check canceled';
                    break;
                case 5:
                    txt = 'Visiting';
                    break;
                case 6:
                    txt = 'Visit completed';
                    break;
                case 7:
                    txt = 'Visit canceling';
                    break;
                case 8:
                    txt = 'Visit canceled';
                    break;
            }
            return txt;
        });
        self.totalPage = ko.observable(0);
        self.visitedPage = ko.observable(0);
        self.unVisitPage = ko.observable(0);
        self.pageRate = ko.observable(0);
        self.totalImage = ko.observable(0);
        self.visitedImage = ko.observable(0);
        self.unVisitImage = ko.observable(0);
        self.imageRate = ko.observable(0);
        self.totalCss = ko.observable(0);
        self.visitedCss = ko.observable(0);
        self.unVisitCss = ko.observable(0);
        self.cssRate = ko.observable(0);
        self.totalJs = ko.observable(0);
        self.visitedJs = ko.observable(0);
        self.unVisitJs = ko.observable(0);
        self.jsRate = ko.observable(0);

        self.reset = function () {
            self.state(0);
            self.ran(false);
            self.totalCss(0);
            self.totalPage(0);
            self.totalImage(0);
            self.totalJs(0);
            self.visitedCss(0);
            self.visitedPage(0);
            self.visitedImage(0);
            self.visitedJs(0);
            self.unVisitCss(0);
            self.unVisitImage(0);
            self.unVisitJs(0);
            self.unVisitPage(0);
            self.cssRate(0);
            self.jsRate(0);
            self.pageRate(0);
            self.imageRate(0);
        };

        self.set = function (data) {
            self.state(data.State);
            self.totalCss(data.TotalCss);
            self.totalPage(data.TotalPage);
            self.totalImage(data.TotalImage);
            self.totalJs(data.TotalJs);
            self.visitedCss(data.VisitedCss);
            self.visitedPage(data.VisitedPage);
            self.visitedImage(data.VisitedImage);
            self.visitedJs(data.VisitedJs);
            self.unVisitCss(data.TotalCss - data.VisitedCss);
            self.unVisitImage(data.TotalImage - data.VisitedImage);
            self.unVisitJs(data.TotalJs - data.VisitedJs);
            self.unVisitPage(data.TotalPage - data.VisitedPage);
            if (data.TotalCss > 0) {
                self.cssRate(((data.VisitedCss / data.TotalCss) * 100).toFixed(0));
            }
            if (data.TotalImage > 0) {
                self.imageRate(((data.VisitedImage / data.TotalImage) * 100).toFixed(0));
            }
            if (data.TotalJs > 0) {
                self.jsRate(((data.VisitedJs / data.TotalJs) * 100).toFixed(0));
            }
            if (data.TotalPage > 0) {
                self.pageRate(((data.VisitedPage / data.TotalPage) * 100).toFixed(0));
            }
        };
    }
    window.checkModel = new HealthViewModel();
    ko.applyBindings(checkModel);



    ko.applyBindings(checkModel);

    var popDialog = $("#checking-info").dialog({
        modal: true,
        autoOpen: false
    });

    var done = function (data) {
        if (data.Success) {
            checkModel.set(data);
        }
    }

    var health = new kooboo.HealthCheck({
        done: done,
        reCheckUrl: reCheckUrl,
        reVisitUrl: reVisitUrl,
        getStateUrl: getStateUrl,
        stopUrl: stopUrl
    });
    health.listener.on("recheck", function () {
        checkModel.reset();
        pop_title.text("Connecting...");
        popDialog.dialog("open");
    });
    health.listener.on("revisit", function () {
        checkModel.reset();
        pop_title.text("Connecting...");
        popDialog.dialog("open");
    });

    var flag = 1;
    health.listener.on("stateChange", function () {
        if (this.isRunning()) {
            checkModel.ran(true);
        }
        if (flag && this.isRunning()) {
            flag = 0;
            popDialog.dialog("open");
        }
        if (checkModel.ran() && !this.isRunning() && window.refreshTR) {
            $(".common-table .tbody table>tbody").append(window.refreshTR);
        }
    }, health);

    health.listener.on("recheckFail", function () {
        pop_title.text("Connect failed.");
    });

    health.getState();
    $("#monitor_health_check").on("click", function (e, d) {
        if (!health.isRunning()) {
            checkModel.entry(0);
            if (!checkModel.ran()) {
                checkModel.reset();
            }
            popDialog.dialog("open");
        }
    });
    $("#monitor_health_stop,#health_pop_stop").on("click", function (e, d) {
        health.stop();
    });
    $("#monitor_health_revisit").on("click", function (e, d) {
        if (!health.isRunning()) {
            checkModel.entry(1);
            if (!checkModel.ran()) {
                checkModel.reset();
            }
            popDialog.dialog("open");
        }
    });
    $("#health_pop_start").on("click", function () {
        if (checkModel.entry() == 1) {
            health.reVisit();
        } else {
            health.reCheck();
        }
        $(".tbody").find("table>tbody").empty();
        $(".pagination").remove();
        $(".common-table .thead input:checkbox.select-all").attr("checked", false);

    });
    $("#health_pop_close").on("click", function (e, d) {
        popDialog.dialog("close");
    });

    pop_power.on("click", function () {
        popDialog.dialog("open");
    });


    var revisit_power = $("#monitor_health_revisit");
    $(".common-table .thead").on("change", "input:checkbox.select-all", function () {
        if (!checkModel.ran() && !checkModel.isRunning()) {
            var postData = window.grid.getSelecteds();
            if (_.any(postData)) {
                revisit_power.show();
            } else {
                revisit_power.hide();
            }
        }
    });
    $(".common-table .tbody").on("click", function () {
        if (!checkModel.ran() && !checkModel.isRunning()) {
            var postData = window.grid.getSelecteds();
            if (_.any(postData)) {
                revisit_power.show();
            } else {
                revisit_power.hide();
            }
        }
    });
});