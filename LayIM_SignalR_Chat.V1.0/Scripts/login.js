function getCookie(name) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    if (arr = document.cookie.match(reg))
        return unescape(arr[2]);
    else
        return null;
}

function init() {
    var uid = getCookie('user_login_token');
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
   