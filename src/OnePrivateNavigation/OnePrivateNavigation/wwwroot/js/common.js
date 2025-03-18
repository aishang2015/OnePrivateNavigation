window.blazorCookie = {
    setCookie: function (key, value) {
        var cookies = document.cookie.split('; ');
        var isExist = false;

        var expires = "expires=Thu, 31 Dec 2099 23:59:59 GMT; path=/";

        for (var i = 0; i < cookies.length; i++) {
            var parts = cookies[i].split('=');
            if (parts[0] === key) {
                isExist = true;
                // 添加过期时间和路径
                document.cookie = key + "=" + encodeURIComponent(value) + "; " + expires;
                break;
            }
        }

        if (!isExist) {
            // 新建时添加过期时间和路径
            document.cookie = [key, encodeURIComponent(value)].join('=') + "; " + expires;
        }
    },
    getCookie: function (key) {
        var cookies = document.cookie.split('; ');
        var value = '';
        for (var i = 0; i < cookies.length; i++) {
            var parts = cookies[i].split('=');
            if (parts[0] === key) {
                value = decodeURIComponent(parts[1]);
                break;
            }
        }

        return value;
    },
    deleteCookie: function (key) {
        var cookies = document.cookie.split('; ');
        for (var i = 0; i < cookies.length; i++) {
            var parts = cookies[i].split('=');
            if (parts[0] === key) {
                document.cookie = parts[0] + '=;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
                break;
            }
        }
    }
};

window.navigationManager = {
    locateTo: function (url) {
        window.location.href = url;
    }
};