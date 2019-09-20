(function () {
    "use strict";

    const Selectors = {
        MASTHEAD: ".masthead"
    };

    class Masthead {
        static initialize() {
            let masthead = document.querySelector(Selectors.MASTHEAD);
            if (masthead) {
                this.animate(masthead);
            }
        }

        static animate(element) {
            let scrolling = false;
            window.addEventListener("scroll", () => {
                scrolling = true;
                setInterval(() => {
                    if (scrolling) {
                        const mastheadHeight = element.getBoundingClientRect().height;
                        const mastheadOpacity = 1 - window.scrollY / mastheadHeight;
                        if (mastheadOpacity >= 0) {
                            element.style.opacity = mastheadOpacity;
                        }

                        scrolling = false;
                    }
                }, 100);
            });
        }
    }

    Masthead.initialize();
}());
