(function () {
    const connectBannerDismissedKey = "connect_banner_hidden";

    function isBannerDismissed() {
        return sessionStorage.getItem(connectBannerDismissedKey) === "true";
    }

    function dismissBanner() {
        sessionStorage.setItem(connectBannerDismissedKey, "true");
    }

    function postFeedback(postUrl, payload) {
        fetch(postUrl, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        }).catch(function () { });
    }

    function initialisePageUsefulFeedback(root) {
        var initial = root.querySelector("[data-feedback-initial]");
        var followup = root.querySelector("[data-feedback-followup]");
        var buttons = root.querySelectorAll("[data-feedback-answer]");
        var cancelButton = root.querySelector("[data-feedback-cancel]");
        var feedbackLink = root.querySelector("[data-feedback-exit-link]");

        var postUrl = root.getAttribute("data-feedback-post-url") || "/feedback/page-useful";
        var pageId = root.getAttribute("data-page-id");
        var ctaType = root.getAttribute("data-cta-type");
        var selectedAnswer = null;

        function showFollowup() {
            if (!initial || !followup) {
                return;
            }

            initial.classList.add("govuk-!-display-none");
            followup.classList.remove("govuk-!-display-none");
        }

        function showInitial() {
            if (!initial || !followup) {
                return;
            }

            followup.classList.add("govuk-!-display-none");
            initial.classList.remove("govuk-!-display-none");

            buttons.forEach(function (button) {
                button.classList.remove("dfe-page-feedback__choice--selected");
            });

            selectedAnswer = null;
        }

        buttons.forEach(function (button) {
            button.addEventListener("click", function () {
                selectedAnswer = this.getAttribute("data-feedback-answer");

                buttons.forEach(function (b) {
                    b.classList.remove("dfe-page-feedback__choice--selected");
                });

                this.classList.add("dfe-page-feedback__choice--selected");

                postFeedback(postUrl, {
                    eventName: "connect_cta_yes_no_interaction",
                    pageId: pageId,
                    ctaType: ctaType,
                    selectedAnswer: selectedAnswer
                });

                showFollowup();
            });
        });

        if (cancelButton) {
            cancelButton.addEventListener("click", function () {
                postFeedback(postUrl, {
                    eventName: "connect_cta_cancelled",
                    pageId: pageId,
                    ctaType: ctaType,
                    selectedAnswer: selectedAnswer
                });

                showInitial();
            });
        }

        if (feedbackLink) {
            feedbackLink.addEventListener("click", function () {
                showInitial();
            });
        }
    }

    function initialisePageUsefulFeedback(root) {
        var initial = root.querySelector("[data-feedback-initial]");
        var followup = root.querySelector("[data-feedback-followup]");
        var buttons = root.querySelectorAll("[data-feedback-answer]");
        var cancelButton = root.querySelector("[data-feedback-cancel]");
        var feedbackLink = root.querySelector("[data-feedback-exit-link]");

        var postUrl = root.getAttribute("data-feedback-post-url") || "/feedback/page-useful";
        var pageId = root.getAttribute("data-page-id");
        var ctaType = root.getAttribute("data-cta-type");
        var selectedAnswer = null;

        function showFollowup() {
            if (!initial || !followup) {
                return;
            }

            initial.classList.add("govuk-!-display-none");
            followup.classList.remove("govuk-!-display-none");
        }

        function showInitial() {
            if (!initial || !followup) {
                return;
            }

            followup.classList.add("govuk-!-display-none");
            initial.classList.remove("govuk-!-display-none");

            buttons.forEach(function (button) {
                button.classList.remove("dfe-page-feedback__choice--selected");
            });

            selectedAnswer = null;
        }

        buttons.forEach(function (button) {
            button.addEventListener("click", function () {
                selectedAnswer = this.getAttribute("data-feedback-answer");

                buttons.forEach(function (b) {
                    b.classList.remove("dfe-page-feedback__choice--selected");
                });

                this.classList.add("dfe-page-feedback__choice--selected");

                postFeedback(postUrl, {
                    eventName: "connect_cta_yes_no_interaction",
                    pageId: pageId,
                    ctaType: ctaType,
                    selectedAnswer: selectedAnswer
                });

                showFollowup();
            });
        });

        if (cancelButton) {
            cancelButton.addEventListener("click", function () {
                postFeedback(postUrl, {
                    eventName: "connect_cta_cancelled",
                    pageId: pageId,
                    ctaType: ctaType,
                    selectedAnswer: selectedAnswer
                });

                showInitial();
            });
        }

        if (feedbackLink) {
            feedbackLink.addEventListener("click", function () {
                showInitial();
            });
        }
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
                dismissBanner();
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