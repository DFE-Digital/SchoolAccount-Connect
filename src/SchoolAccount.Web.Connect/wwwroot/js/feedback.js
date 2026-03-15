(function () {
    function postTelemetry(postUrl, payload) {
        fetch(postUrl, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        }).catch(function () { });

        try {
            if (window.appInsights && typeof window.appInsights.trackEvent === 'function') {
                window.appInsights.trackEvent(
                    { name: 'page_feedback_response' },
                    payload
                );
            }
        } catch (e) {
        }
    }

    function initialiseFooterFeedback(root) {
        var button = root.querySelector('[data-feedback-button]');
        if (!button) {
            return;
        }

        var postUrl = root.getAttribute('data-feedback-post-url') || '/feedback/page-useful';
        var variant = root.getAttribute('data-feedback-variant') || 'feedback-button';

        button.addEventListener('click', function () {
            var payload = {
                pageId: this.getAttribute('data-page-id'),
                value: this.getAttribute('data-feedback-value') || 'clicked',
                variant: variant,
                action: this.getAttribute('data-feedback-action') || 'feedback.clicked'
            };

            postTelemetry(postUrl, payload);
        });
    }

    function initialisePageUsefulFeedback(root) {
        var initial = root.querySelector('[data-feedback-initial]');
        var followup = root.querySelector('[data-feedback-followup]');
        var buttons = root.querySelectorAll('[data-feedback-value]');
        var cancelButton = root.querySelector('[data-feedback-cancel]');
        var feedbackLink = root.querySelector('[data-feedback-link]');

        var postUrl = root.getAttribute('data-feedback-post-url') || '/feedback/page-useful';
        var variant = root.getAttribute('data-feedback-variant') || 'feedback.yes.no';

        var selectedValue = null;
        var selectedPageId = null;

        function showFollowup() {
            if (!initial || !followup) {
                return;
            }

            initial.classList.add('govuk-!-display-none');
            followup.classList.remove('govuk-!-display-none');
        }

        function showInitial() {
            if (!initial || !followup) {
                return;
            }

            followup.classList.add('govuk-!-display-none');
            initial.classList.remove('govuk-!-display-none');

            buttons.forEach(function (button) {
                button.classList.remove('dfe-page-feedback__choice--selected');
            });

            selectedValue = null;
            selectedPageId = null;
        }

        buttons.forEach(function (button) {
            button.addEventListener('click', function () {
                selectedValue = this.getAttribute('data-feedback-value');
                selectedPageId = this.getAttribute('data-page-id');

                buttons.forEach(function (b) {
                    b.classList.remove('dfe-page-feedback__choice--selected');
                });

                this.classList.add('dfe-page-feedback__choice--selected');

                postTelemetry(postUrl, {
                    pageId: selectedPageId,
                    value: selectedValue,
                    variant: variant,
                    action: 'feedback.response'
                });

                showFollowup();
            });
        });

        if (cancelButton) {
            cancelButton.addEventListener('click', function () {
                if (selectedValue && selectedPageId) {
                    postTelemetry(postUrl, {
                        pageId: selectedPageId,
                        value: selectedValue,
                        variant: variant,
                        action: 'feedback.cancel.pressed'
                    });
                }

                showInitial();
            });
        }

        if (feedbackLink) {
            feedbackLink.addEventListener('click', function () {
                if (selectedValue && selectedPageId) {
                    postTelemetry(postUrl, {
                        pageId: selectedPageId,
                        value: selectedValue,
                        variant: variant,
                        action: 'feedback.clicked'
                    });
                }
            });
        }
    }

    function initialiseConnectBanner(root) {
        var feedbackLink = root.querySelector('[data-connect-banner-link]');
        var dismissButton = root.querySelector('[data-connect-banner-dismiss]');

        var postUrl = root.getAttribute('data-feedback-post-url') || '/feedback/page-useful';
        var variant = root.getAttribute('data-feedback-variant') || 'feedback-banner';
        var pageId = root.getAttribute('data-page-id');

        if (pageId) {
            postTelemetry(postUrl, {
                pageId: pageId,
                value: 'shown',
                variant: variant,
                action: 'feedback.shown'
            });
        }

        if (feedbackLink) {
            feedbackLink.addEventListener('click', function () {
                postTelemetry(postUrl, {
                    pageId: this.getAttribute('data-page-id') || pageId,
                    value: this.getAttribute('data-feedback-value') || 'clicked',
                    variant: this.getAttribute('data-feedback-variant') || variant,
                    action: this.getAttribute('data-feedback-action') || 'feedback.clicked'
                });
            });
        }

        if (dismissButton) {
            dismissButton.addEventListener('click', function () {
                postTelemetry(postUrl, {
                    pageId: this.getAttribute('data-page-id') || pageId,
                    value: this.getAttribute('data-feedback-value') || 'dismissed',
                    variant: this.getAttribute('data-feedback-variant') || variant,
                    action: this.getAttribute('data-feedback-action') || 'feedback.dismissed'
                });

                root.classList.add('govuk-!-display-none');
            });
        }
    }

    document.addEventListener('DOMContentLoaded', function () {
        document.querySelectorAll('[data-feedback-component="footer"]').forEach(function (root) {
            initialiseFooterFeedback(root);
        });

        document.querySelectorAll('[data-feedback-component="page-useful"]').forEach(function (root) {
            initialisePageUsefulFeedback(root);
        });

        document.querySelectorAll('[data-feedback-component="connect-banner"]').forEach(function (root) {
            initialiseConnectBanner(root);
        });
    });
})();