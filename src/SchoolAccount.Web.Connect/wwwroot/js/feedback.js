(function () {

    function deleteCookie(name) {
        document.cookie =
            name + "=; Path=/; Expires=Thu, 01 Jan 1970 00:00:00 GMT";
    }

    function setCookie(name, value) {
        document.cookie =
            name + "=" + value + "; Path=/; SameSite=Lax";
    }

    window.resetPageFeedbackState = function () {
        deleteCookie("page_feedback_submitted");

        setTimeout(function () {
            window.location.reload();
        }, 100);
    };

    window.hideConnectBannerState = function () {
        setCookie("connect_banner_hidden", "true");

        setTimeout(function () {
            window.location.reload();
        }, 100);
    };

})();