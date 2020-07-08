(function () {
    "use strict";

    const Selectors = {
        LOGO: ".brand"
    };

    const ClassNames = {
        FADE_OUT: "fade-out"
    };

    class Logo {
        static initialize() {
            const element = document.querySelector(Selectors.LOGO);
            if (element) {
                Logo.animate(element);
            }
        }

        static animate(element) {
            // ignores scrolling done by browser on refresh of an already scrolled page
            setTimeout(() => {
                let scrolling = false;
                window.addEventListener("scroll", () => {
                    scrolling = true;
                });

                setInterval(() => {
                    if (scrolling) {
                        if (window.scrollY > 0) {
                            element.classList.add(ClassNames.FADE_OUT);
                        } else {
                            element.classList.remove(ClassNames.FADE_OUT);
                        }

                        scrolling = false;
                    }
                }, 100);
            }, 100);
        }
    }

    Logo.initialize();
}());
