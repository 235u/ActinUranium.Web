(function () {
    "use strict";

    const ClassName = {
        CARD: "card"
    };

    const Selector = {
        OVERLAY: ".card-overlay"
    };

    const Directions = new Map([
        [0, "top"],
        [1, "right"],
        [2, "bottom"],
        [3, "left"]
    ]);

    const ClassNamePostfix = {
        IN: "-in",
        OUT: "-out"
    };

    const AnimationClassNames = [
        `top${ClassNamePostfix.IN}`,
        `top${ClassNamePostfix.OUT}`,
        `right${ClassNamePostfix.IN}`,
        `right${ClassNamePostfix.OUT}`,
        `bottom${ClassNamePostfix.IN}`,
        `bottom${ClassNamePostfix.OUT}`,
        `left${ClassNamePostfix.IN}`,
        `left${ClassNamePostfix.OUT}`
    ];

    class CardOverlay {
        constructor(card, overlay) {
            this.card = card;
            this.overlay = overlay;

            this.card.addEventListener("mouseenter", (e) => this.update(e, ClassNamePostfix.IN));
            this.card.addEventListener("mouseleave", (e) => this.update(e, ClassNamePostfix.OUT));
        }

        update(e, postfix) {
            this.overlay.classList.remove(...AnimationClassNames);

            const direction = this.getDirection(e, this.card);
            const className = direction + postfix;

            this.overlay.classList.add(className);
        }

        // See: https://css-tricks.com/direction-aware-hover-effects/#article-header-id-2
        getDirection(e) {
            const w = this.card.offsetWidth;
            const h = this.card.offsetHeight;

            /**
             * calculate the x and y to get an angle to the center of the div from that x and y
             * get the x value relative to the center of the card and "normalize" it
             */
            const x = (e.clientX - this.card.getBoundingClientRect().x - w / 2) * (w > h ? h / w : 1);
            const y = (e.clientY - this.card.getBoundingClientRect().y - h / 2) * (h > w ? w / h : 1);

            /**
             * the angle and the direction from where the mouse came in/went out clockwise TRBL (0/1/2/3)
             * first calculate the angle of the point,
             * add 180 deg to get rid of the negative values,
             * divide by 90 to get the quadrant,
             * add 3 and do a modulo by 4 to shift the quadrants to a proper clockwise TRBL (top/right/bottom/left)
             */
            const d = Math.round((Math.atan2(y, x) * (180 / Math.PI) + 180) / 90 + 3) % 4;
            return Directions.get(d);
        }
    }

    window.addEventListener("load", () => {
        const cards = document.getElementsByClassName(ClassName.CARD);
        for (let card of cards) {
            const overlay = card.querySelector(Selector.OVERLAY);
            if (overlay) {
                new CardOverlay(card, overlay);
            }
        }
    });
}());
