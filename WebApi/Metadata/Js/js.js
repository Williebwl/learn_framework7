function FirstToUpper(obj) {
    return obj[0].toLocaleUpperCase() + obj.substring(1);
}

function request(paras) {
    var paraString = location.search.substring(1).split("&");
    var paraObj = {}
    for (i = 0; j = paraString[i]; i++) {
        paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=") + 1, j.length);
    }
    var returnValue = paraObj[paras.toLowerCase()];
    if (typeof (returnValue) == "undefined") {
        return "";
    } else {
        return returnValue;
    }
}

function GetHost() {
    return window.location.protocol + "//" + window.location.host;
}

//function GetRoute(p) {
//    if (!p.length) return '';

//    var ps = '';

//    $.each(p, function (i, p) { ps += '/{' + p.Name + '}'; })

//    return ps;
//}

//function GetUrl(p) {
//    if (!p.length) return '';

//    var ps = '?';

//    $.each(p, function (i, p) { ps += (i > 0 ? '&' : '') + p.Name + '='; })

//    return ps;
//}