(function () {
    
    // ------------------------------------------------------
    // Cookie consent
    // ------------------------------------------------------
    async function setConsent(value) {
        const response = await fetch("/cookies/consent", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ value: value })
        });

        if (!response.ok) {
            throw new Error("Failed to save cookie consent");
        }
    }
    
    // ------------------------------------------------------
    // Cookie banner
    // ------------------------------------------------------
    var banner = document.getElementById("dfe-cookie-banner");
    var cookieConsentConfig = window.cookieConsentConfig;

    if (banner && cookieConsentConfig) {
        var acceptBtn = document.getElementById("dfe-cta-acceptCookies");
        var declineBtn = document.getElementById("dfe-cta-declineCookies");

        if (acceptBtn) {
            acceptBtn.addEventListener("click", async function () {
                try {
                    await setConsent(cookieConsentConfig.acceptedValue);
                    banner.style.display = "none";
                } catch {
                    // Keep the banner visible if consent could not be saved.
                }
            });
        }

        if (declineBtn) {
            declineBtn.addEventListener("click", async function () {
                try {
                    await setConsent(cookieConsentConfig.rejectedValue);
                    banner.style.display = "none";
                } catch {
                    // Keep the banner visible if consent could not be saved.
                }
            });
        }
    }

    // ------------------------------------------------------
    // Cookies page form
    // ------------------------------------------------------
    var cookiesForm = document.getElementById("dfe-cookies-form");
    var cookiesNotification = document.getElementById("cookiesNotification");

    if (cookiesForm && cookieConsentConfig) {
        cookiesForm.addEventListener("submit", async function (e) {
            e.preventDefault();

            var selected = cookiesForm.querySelector("input[name=\"analytics-cookies\"]:checked");
            if (!selected) return;

            var value = selected.value === "yes"
                ? cookieConsentConfig.acceptedValue
                : cookieConsentConfig.rejectedValue;

            try {
                await setConsent(value);

                if (cookiesNotification) {
                    cookiesNotification.style.display = "";
                    cookiesNotification.focus();
                }
            } catch {
                // Do not show the confirmation message if consent could not be saved.
            }
        });
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