(function () {
    var dismissedKey = "feedback-banner-dismissed";

    function postFeedback(postUrl, payload) {
        fetch(postUrl, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        }).catch(function () { });
    }

    function isBannerDismissed() {
        try {
            return sessionStorage.getItem(dismissedKey) === "true";
        } catch (e) {
            return false;
        }
    }

    function dismissBanner() {
        try {
            sessionStorage.setItem(dismissedKey, "true");
        } catch (e) { }
    }

    function initialiseBanner(root) {
        if (isBannerDismissed()) {
            root.classList.add("govuk-!-display-none");
            return;
        }

        var dismissButton = root.querySelector("[data-feedback-dismiss]");
        var feedbackLink = root.querySelector("[data-feedback-exit-link]");
        var postUrl = root.getAttribute("data-feedback-post-url") || "/feedback/page-useful";
        var pageId = root.getAttribute("data-page-id");
        var ctaType = root.getAttribute("data-cta-type");

        if (dismissButton) {
            dismissButton.addEventListener("click", function () {
                postFeedback(postUrl, {
                    eventName: "connect_cta_dismissed",
                    pageId: pageId,
                    ctaType: ctaType,
                    selectedAnswer: null
                });

                dismissBanner();
                root.classList.add("govuk-!-display-none");
            });
        }

        if (feedbackLink) {
            feedbackLink.addEventListener("click", function () {
                root.classList.add("govuk-!-display-none");
            });
        }
    }

    document.addEventListener("DOMContentLoaded", function () {
        document.querySelectorAll('[data-feedback-component="page-useful"]').forEach(function (root) {
            initialisePageUsefulFeedback(root);
        });

        document.querySelectorAll('[data-feedback-component="banner"]').forEach(function (root) {
            initialiseBanner(root);
        });
    });
})();