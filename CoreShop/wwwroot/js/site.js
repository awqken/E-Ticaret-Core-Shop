// ============================================================================
// CoreShop — storefront interactions (Volt design system)
// Wishlist (localStorage), button loading states, scroll reveal, navbar state,
// newsletter demo feedback. No dependencies.
// ============================================================================
(function () {
    "use strict";

    document.documentElement.classList.add("js");

    /* ── Toast helper (matches _Toasts markup) ─────────────────────────── */
    function showToast(message, kind) {
        var stack = document.querySelector(".toast-stack");
        if (!stack) {
            stack = document.createElement("div");
            stack.className = "toast-stack";
            stack.setAttribute("aria-live", "polite");
            document.body.appendChild(stack);
        }

        var toast = document.createElement("div");
        toast.className = "app-toast " + (kind === "warning" ? "toast-warning" : "toast-success");
        toast.setAttribute("role", "status");

        var icon = document.createElement("i");
        icon.className = kind === "warning"
            ? "fa-solid fa-triangle-exclamation"
            : "fa-solid fa-circle-check";
        icon.setAttribute("aria-hidden", "true");

        var text = document.createElement("span");
        text.className = "flex-grow-1";
        text.textContent = message;

        var close = document.createElement("button");
        close.type = "button";
        close.className = "btn-close";
        close.setAttribute("aria-label", "Kapat");
        close.addEventListener("click", function () { toast.remove(); });

        toast.append(icon, text, close);
        stack.appendChild(toast);
        window.setTimeout(function () { toast.remove(); }, 4000);
    }

    /* ── Wishlist — persisted per browser in localStorage ──────────────── */
    var WISH_KEY = "coreshop.wishlist";

    function readWishlist() {
        try {
            var raw = window.localStorage.getItem(WISH_KEY);
            var list = raw ? JSON.parse(raw) : [];
            return Array.isArray(list) ? list : [];
        } catch (_) {
            return [];
        }
    }

    function writeWishlist(list) {
        try {
            window.localStorage.setItem(WISH_KEY, JSON.stringify(list));
        } catch (_) { /* storage unavailable (private mode etc.) — ignore */ }
    }

    function initWishlist() {
        var wishlist = readWishlist();

        document.querySelectorAll(".wish-btn[data-product-id]").forEach(function (btn) {
            var id = Number(btn.dataset.productId);
            btn.setAttribute("aria-pressed", wishlist.includes(id) ? "true" : "false");

            btn.addEventListener("click", function () {
                var list = readWishlist();
                var index = list.indexOf(id);

                if (index >= 0) {
                    list.splice(index, 1);
                    btn.setAttribute("aria-pressed", "false");
                    showToast("Favorilerden çıkarıldı.", "success");
                } else {
                    list.push(id);
                    btn.setAttribute("aria-pressed", "true");
                    showToast("Favorilere eklendi. Bu tarayıcıda saklanır.", "success");
                }

                writeWishlist(list);
            });
        });
    }

    /* ── Button loading state on real form submits ─────────────────────── */
    function initLoadingButtons() {
        document.addEventListener("submit", function (event) {
            var form = event.target;
            if (!(form instanceof HTMLFormElement) || form.dataset.noLoading === "true") {
                return;
            }

            var button = form.querySelector('button[type="submit"], input[type="submit"]');
            if (button && !button.classList.contains("is-loading")) {
                // lock the button only if nothing (e.g. client validation) blocked the submit
                window.setTimeout(function () {
                    if (event.defaultPrevented) {
                        return;
                    }
                    button.classList.add("is-loading");
                    button.setAttribute("aria-busy", "true");
                }, 0);
            }
        });

        // restore state when navigating back from bfcache
        window.addEventListener("pageshow", function () {
            document.querySelectorAll(".btn.is-loading").forEach(function (btn) {
                btn.classList.remove("is-loading");
                btn.removeAttribute("aria-busy");
            });
        });
    }

    /* ── Scroll reveal ──────────────────────────────────────────────────── */
    function initReveal() {
        var items = document.querySelectorAll(".reveal");
        if (!items.length) {
            return;
        }

        if (!("IntersectionObserver" in window) ||
            window.matchMedia("(prefers-reduced-motion: reduce)").matches) {
            items.forEach(function (el) { el.classList.add("in"); });
            return;
        }

        var observer = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting) {
                    entry.target.classList.add("in");
                    observer.unobserve(entry.target);
                }
            });
        }, { threshold: .12, rootMargin: "0px 0px -40px 0px" });

        items.forEach(function (el) { observer.observe(el); });
    }

    /* ── Navbar elevation on scroll ─────────────────────────────────────── */
    function initNavbarState() {
        var navbar = document.querySelector(".navbar-shell");
        if (!navbar) {
            return;
        }

        var update = function () {
            navbar.classList.toggle("scrolled", window.scrollY > 8);
        };

        update();
        window.addEventListener("scroll", update, { passive: true });
    }

    /* ── Newsletter (demo store: no backend, honest client-side feedback) ─ */
    function initNewsletter() {
        var form = document.querySelector("[data-newsletter-form]");
        if (!form) {
            return;
        }

        form.addEventListener("submit", function (event) {
            event.preventDefault();

            var input = form.querySelector('input[type="email"]');
            if (!input || !input.value || !input.checkValidity()) {
                input && input.reportValidity();
                return;
            }

            input.value = "";
            showToast("Kaydın alındı! (Demo mağaza — e-posta gönderilmez.)", "success");
        });
    }

    document.addEventListener("DOMContentLoaded", function () {
        initWishlist();
        initLoadingButtons();
        initReveal();
        initNavbarState();
        initNewsletter();
    });
})();
