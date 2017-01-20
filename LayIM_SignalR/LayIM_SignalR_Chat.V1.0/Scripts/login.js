function getCookie(name) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    if (arr = document.cookie.match(reg))
        return unescape(arr[2]);
    else
        return null;
}

function clearCookie(name) {
    document.cookie = name + '=0;expires=' + new Date(0).toUTCString();
}
function getQueryParameter(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

function init() {
    var key = 'user_login_token';

    var msg = getQueryParameter('msg');
    if (msg == 'unauthorized') {
        clearCookie(key);
        return;
    }

    var uid = getCookie(key);
    var loginPage = location.href.toLocaleLowerCase().split('?')[0].indexOf('login') > -1;
    if (!uid) {
        if (!loginPage) {
            location.href = '/home/login';
        }
    } else {
        if (loginPage) {
            location.href = '/';
        }
    }
}

window.onload =function(){
    init();
}
   