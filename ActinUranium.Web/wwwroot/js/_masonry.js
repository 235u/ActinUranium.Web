(function () {
    "use strict";

    const Selector = {
        //// ITEM: ".card-columns .card"
        ITEM: ".masonry .card"
    };

    const HEIGHT_QUANTUM_IN_PIXELS = 25;

    class Masonry {
        static initialize() {
            let items = document.querySelectorAll(Selector.ITEM);
            for (let item of items) {
                this.quantizeHeight(item);
            }
        }

        static quantizeHeight(element) {            
            let actualHeight = element.getBoundingClientRect().height;
            let quantumCount = Math.floor(actualHeight / HEIGHT_QUANTUM_IN_PIXELS);
            if ((actualHeight % HEIGHT_QUANTUM_IN_PIXELS) > 0) {
                quantumCount += 1;
            }

            // TODO: Switch to css classes; test for visibility issues.
            element.style.height = (quantumCount * HEIGHT_QUANTUM_IN_PIXELS) + "px";
            element.style.visibility = "visible";
        }
    }

    Masonry.initialize();
}());
