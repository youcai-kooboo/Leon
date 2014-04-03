var kooboo = kooboo || {};
(function (kb) {
    var EventManager = function () {
        this.Events = new kb.Dictionary();
    };
    EventManager.prototype = {
        //register event
        add: function (eventName) {
            var eventNames = eventName.split(",");
            for (var i = 0, j = eventNames.length; i < j; i++) {
                if (!this.Events.hasKey(eventNames[i])) {
                    this.Events.set(eventNames[i], []);
                }
            }
            return this;
        },
        //register event callback
        on: function (eventName, fn, scope) {
            var self = this;
            var callFnList = this.get(eventName);
            if (callFnList) {
                callFnList.push({
                    scope: scope || self,
                    fn: fn
                });
            }
            return this;
        },
        //fire event
        fire: function (eventName) {
            var callFnList = this.get(eventName);
            if (callFnList) {
                for (var i = 0, j = callFnList.length; i < j; i++) {
                    var callFn = callFnList[i];
                    var args = [].slice.call(arguments, 1);
                    callFn.fn.apply(callFn.scope, args);
                }
            }
            return this;
        },
        //get event object
        get: function (eventName) {
            return this.Events.get(eventName);
        },
        //off event
        off: function (eventName, fn) {
            if (this.Events.hasKey(eventName)) {
                if (fn) {
                    var lst = this.get(eventName);
                    for (var m = 0, n = lst.length; m < n; m++) {
                        if (lst[m] == fn) {
                            lst.splice(m, 1);
                        }
                    }
                } else {
                    this.Events.remove(eventName);
                }
            }
            return this;
        },
        //destroy event mananger
        destroy: function () {
            this.Events = null;
        }
    };
    kb.EventManager = EventManager;
})(kooboo);