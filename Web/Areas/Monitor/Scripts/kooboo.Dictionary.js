var kooboo = kooboo || {};
(function (kb) {
    var Dictionary = function () {
        this.Keys = [];
        this.Values = [];
    };
    Dictionary.prototype = {
        set: function (key, val) {
            if (this.hasKey(key)) {
                var index = this._getIndex();
                this.Values.splice(index, 0, val);
            } else {
                this.Keys.push(key);
                this.Values.push(val);
            }
            return this;
        },
        get: function (key) {
            var index = this._getIndex(key);
            return this.Values[index];
        },
        remove: function (key) {
            var index = this._getIndex();
            this.Keys.splice(index, 1);
            this.Values.splice(index, 1);
            return this;
        },
        hasKey: function (key) {
            return this._getIndex(key) > -1;
        },
        _getIndex: function (key) {
            var index = -1;
            for (var i = 0, j = this.Keys.length; i < j; i++) {
                if (this.Keys[i] === key) {
                    index = i;
                    break;
                }
            }
            return index;
        }
    };
    kb.Dictionary = Dictionary;
})(kooboo);