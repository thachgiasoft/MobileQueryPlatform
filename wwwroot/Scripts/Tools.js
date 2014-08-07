var Tools = $.extend({}, Tools);

/*命名空间 
register 方法注册命名空间*/

Tools.Namespace = new Object();
Tools.Namespace.register = function (fullNS) {
    // 将命名空间切成N部分, 比如Grandsoft.GEA等
    var nsArray = fullNS.split('.');
    var sEval = "";
    var sNS = "";
    for (var i = 0; i < nsArray.length; i++) {
        if (i != 0) sNS += ".";
        sNS += nsArray[i];
        // 依次创建构造命名空间对象（假如不存在的话）的语句
        // 比如先创建Grandsoft，然后创建Grandsoft.GEA，依次下去
        sEval += "if (typeof(" + sNS + ") == 'undefined') " + sNS + " = new Object();"
    }
    if (sEval != "") eval(sEval);
};

/*将form表单内的元素序列化为对象，扩展Jquery的一个方法*/
Tools.serializeObject = function (form) { 
    var o = {};
    $.each(form.serializeArray(), function (index) {
        if (o[this['name']]) {
            o[this['name']] = o[this['name']] + "," + this['value'];
        } else {
            o[this['name']] = this['value'];
        }
    });
    return o;
};


/*对象转JsonString*/
Tools.ObjectToString=function (O) {
        var S = [];
        var J = "";
        if (Object.prototype.toString.apply(O) === '[object Array]') {
            for (var i = 0; i < O.length; i++)
                S.push(Tools.ObjectToString(O[i]));
            J = '[' + S.join(',') + ']';
        }
        else if (Object.prototype.toString.apply(O) === '[object Date]') {
            J = "new Date(" + O.getTime() + ")";
        }
        else if (Object.prototype.toString.apply(O) === '[object RegExp]' || Object.prototype.toString.apply(O) === '[object Function]') {
            J = O.toString();
        }
        else if (Object.prototype.toString.apply(O) === '[object Object]') {
            for (var i in O) {
                O[i] = typeof (O[i]) == 'string' ?  O[i]  : (typeof (O[i]) === 'object' ? Tools.ObjectToString(O[i]) : O[i]);
                S.push('"'+i+'"' + ':\"' + O[i]+"\"");
            }
            J = '{' + S.join(',') + '}';
        }

    return J;
};




