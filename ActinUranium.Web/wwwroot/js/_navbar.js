(function () {
    "use strict";

    const Selectors = {
        ITEM: "nav .btn"
    };

    class Navbar {
        static initialize() {
            if (this.isHomeView()) {
                window.addEventListener("load", () => {
                    const elements = document.querySelectorAll(Selectors.ITEM);
                    if (elements.length > 0) {
                        Navbar.animate(elements);
                    }
                });
            }
        }

        static isHomeView() {
            return document.location.pathname === "/";
        }

        static animate(elements) {
            Navbar.expand(elements);

            document.addEventListener("scroll", () => {
                Navbar.collapse(elements);
            }, { once: true });
        }

        static expand(elements) {
            let transitionDelay = 0;
            for (let element of elements) {
                element.style.transitionDelay = transitionDelay + "s";
                element.style.left = "0";
                transitionDelay += 0.1;
            }
        }

        static collapse(elements) {
            for (let element of elements) {
                element.style.transitionDelay = "0s";
                element.style.left = "calc(60px - 100%)";

                element.onmouseenter = () => {
                    element.style.left = "0";
                };

                element.onmouseleave = () => {
                    element.style.left = "calc(60px - 100%)";
                };
            }
        }
    }

    Navbar.initialize();
}());
