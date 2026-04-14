(function () {

    // ------------------------------------------------------
    // Cookie banner
    // ------------------------------------------------------
    var banner = document.getElementById("dfe-cookie-banner");
    var cookieConsentConfig = window.cookieConsentConfig;

    if (banner && cookieConsentConfig) {
        var acceptBtn = document.getElementById("dfe-cta-acceptCookies");
        var declineBtn = document.getElementById("dfe-cta-declineCookies");

        function setConsent(value) {
            var parts = [
                cookieConsentConfig.cookieName + "=" + value,
                "Path=/",
                "Max-Age=" + (60 * 60 * 24 * 365),
                "SameSite=Lax"
            ];

            if (location.protocol === "https:") {
                parts.push("Secure");
            }

            document.cookie = parts.join("; ");
            banner.style.display = "none";
        }

        if (acceptBtn) {
            acceptBtn.addEventListener("click", function () {
                setConsent(cookieConsentConfig.acceptedValue);
            });
        }

        if (declineBtn) {
            declineBtn.addEventListener("click", function () {
                setConsent(cookieConsentConfig.rejectedValue);
            });
        }
    }

    // ------------------------------------------------------
    // Header search toggle
    // ------------------------------------------------------
    var searchToggle = document.getElementById("dfe-connect-search-toggle");
    var searchPanel = document.getElementById("dfe-connect-search-panel");

    if (searchToggle && searchPanel) {
        searchToggle.setAttribute("aria-expanded", "false");
        searchPanel.hidden = true;

        searchToggle.addEventListener("click", function () {
            var isOpen = searchToggle.getAttribute("aria-expanded") === "true";
            var nextOpen = !isOpen;

            searchToggle.setAttribute("aria-expanded", nextOpen ? "true" : "false");
            searchPanel.hidden = !nextOpen;

            if (nextOpen) {
                document.getElementById("dfe-connect-search-field")?.focus();
            }
        });
    }

    // ------------------------------------------------------
    // Mobile nav toggle
    // ------------------------------------------------------
    var mobileToggle = document.getElementById("dfe-connect-mobile-toggle");
    var mobileNav = document.getElementById("dfe-connect-mobile-nav");

    if (mobileToggle && mobileNav) {
        mobileToggle.setAttribute("aria-expanded", "false");
        mobileNav.hidden = true;

        mobileToggle.addEventListener("click", function () {
            var isOpen = mobileToggle.getAttribute("aria-expanded") === "true";
            var nextOpen = !isOpen;

            mobileToggle.setAttribute("aria-expanded", nextOpen ? "true" : "false");
            mobileNav.hidden = !nextOpen;
        });
    }
})();