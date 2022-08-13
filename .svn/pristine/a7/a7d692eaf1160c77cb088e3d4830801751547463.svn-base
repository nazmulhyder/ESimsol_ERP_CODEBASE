
//this plugin developed by sagor
Array.prototype.upperCase = function () {
    for (var i = 0; i < this.length; i++) {
        this[i] = this[i].toUpperCase()
    }
}

Array.prototype.lowerCase = function () {
    for (var i = 0; i < this.length; i++) {
        this[i] = this[i].toLowerCase()
    }
}

Array.prototype.skip = function (count) {
    if (count <= this.length)
        this.slice(count)
    else
        throw 'Index out of range';
}

Array.prototype.take = function (count) {
    if (count <= this.length)
        this.slice(0, count)
    else
        throw 'Index out of range';
}

Array.prototype.getRange = function (index,count) {
    var arr = [];
    if (index < this.length && (index + count) <= this.length) {
        for (var i = index; i < (index + count) ; i++) {
            arr.push(this[i]);
        }
        return arr;
    }
    else
        throw 'Index out of range';
}

Array.prototype.removeAt = function (index) {
    if (index < this.length)
        this.splice(index, 1);
    else
        throw 'Index out of range';
}

Array.prototype.removeRange = function (index,count) {
    if (index < this.length && (index+count)< this.length)
        this.splice(index, count);
    else
        throw 'Index out of range';
}

Array.prototype.select = function (key) {
    var arr = [];
    this.forEach(function (obj) {
        arr.push(obj[key]);
    });
    return arr;
}


//Array.prototype.join = function (char,key) {
//    var result = "";
//    this.forEach(function (obj) {
//        result += (obj[key] + char);
//    });
//    return result.substring(0, result.length - 1);
//}

Array.prototype.selectMany = function (keys) {
   //not implemented;
}

Array.prototype.sum = function () {
    var result = 0;
    this.forEach(function (value) {
        result += parseFloat(value);
    });
    return result;
}

Array.prototype.avg = function () {
    return this.sum()/this.length;
}

Array.prototype.any = function () {
    //debugger;
    if (this != null && this.length > 0)
        return true;
    else
        return false
};

Array.prototype.first = function () {
    //debugger;
    if (this.length>0)
        return this[0];
    else
        return null;
};

Array.prototype.last = function () {
    //debugger;
    if (this.length > 0)
        return this[this.length-1];
    else
        return null;
};

Array.prototype.separator = function (char) {
    //debugger;
    var result="";
    if (this.length > 0)
    {
        char=(char!=undefined && char!=null && char!="")? char:',';

        this.forEach(function (value) {
            result += value +char;
        });
        return result.substring(0, result.length - 1);
    }
    else
        return result;
};



//Array.prototype.last = function (predicate, def) {
//    debugger;
//    var l = this.length;
//    if (!predicate) return l ? this[l - 1] : def == null ? null : def;
//    while (l-- > 0)
//        if (predicate(this[l], l, this))
//            return this[l];
//    return def == null ? null : def;
//};
