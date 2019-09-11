'use strict';

const Directions = new Map([
    [0, 'top'],
    [1, 'right'],
    [2, 'bottom'],
    [3, 'left']
]);

const ClassName = {
    CARD: 'card',        
    GRADIENT: 'bg-gradient',
    GROUP: 'card-group',
    OVERLAY: 'card-overlay'
};

const AnimationClassNames = [
    'top-in',
    'top-out',
    'right-in',
    'right-out',
    'bottom-in',
    'bottom-out',
    'left-in',
    'left-out'
];

const Postfix = {
    IN: '-in',
    OUT: '-out'
};

class CardOverlay {
    constructor(card, overlay) {
        this.card = card;
        this.overlay = overlay;

        this.card.addEventListener('mouseenter', (e) => this.update(e, Postfix.IN));
        this.card.addEventListener('mouseleave', (e) => this.update(e, Postfix.OUT));        
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

        // calculate the x and y to get an angle to the center of the div from that x and y
        // gets the x value relative to the center of the DIV and "normalize" it
        const x = (e.clientX - this.card.getBoundingClientRect().x - (w / 2)) * (w > h ? (h / w) : 1);
        const y = (e.clientY - this.card.getBoundingClientRect().y - (h / 2)) * (h > w ? (w / h) : 1);

        // the angle and the direction from where the mouse came in/went out clockwise TRBL (0/1/2/3)
        // first calculate the angle of the point,
        // add 180 deg to get rid of the negative values
        // divide by 90 to get the quadrant
        // add 3 and do a modulo by 4 to shift the quadrants to a proper clockwise TRBL (top/right/bottom/left)
        const d = Math.round((((Math.atan2(y, x) * (180 / Math.PI)) + 180) / 90) + 3) % 4;
        return Directions.get(d);
    }
}

window.addEventListener('load', () => {
    animateCardOverlays();
});

function animateCardOverlays() {
    let cards = document.getElementsByClassName(ClassName.CARD);
    for (let card of cards) {
        let overlay = card.querySelector('.' + ClassName.OVERLAY);
        if (overlay) {
            new CardOverlay(card, overlay);
        }        
    }
}

'use strict';

const CarouselSlideOrder = {
    NONE: 0,
    PREV: 1,
    NEXT: 2
};

const CarouselSlidingDirection = {
    NONE: 0,
    LEFT: 1,
    RIGHT: 2
};

const CarouselClassName = {
    ACTIVE_BULLET: 'active-bullet',
    ACTIVE_SLIDE: 'active-slide',
    BULLET: 'bullet',
    CAROUSEL: 'carousel',
    NEXT_SLIDE: 'next-slide',
    PREV_SLIDE: 'prev-slide',
    SLIDE: 'slide',
    SLIDE_LEFT: 'slide-left',
    SLIDE_RIGHT: 'slide-right'
};

const CarouselSelector = {
    ACTIVE_SLIDE: '.active-slide',
    BULLETS: '.active-bullet, .bullet',
    NEXT_SLIDE_CONTROL: '.next-slide-control',
    PREV_SLIDE_CONTROL: '.prev-slide-control',
    SLIDES: '.active-slide, .slide'
};

class Carousel {
    constructor(element) {
        this._rootElement = element;
        this._slides = this.getElementsAsArray(CarouselSelector.SLIDES);
        this._bullets = this.getElementsAsArray(CarouselSelector.BULLETS);
        this._isSliding = false;
        this.addEventListeners();
    }

    getElementsAsArray(selector) {
        const elements = this._rootElement.querySelectorAll(selector);
        return Array.from(elements);
    }

    addEventListeners() {
        let prevSlideControl = this._rootElement.querySelector(CarouselSelector.PREV_SLIDE_CONTROL);
        if (prevSlideControl) {
            prevSlideControl.onclick = () => {
                this.slideIn(CarouselSlideOrder.PREV);
            };
        }

        let nextSlideControl = this._rootElement.querySelector(CarouselSelector.NEXT_SLIDE_CONTROL);
        if (nextSlideControl) {
            nextSlideControl.onclick = () => {
                this.slideIn(CarouselSlideOrder.NEXT);
            };
        }

        for (let slideIndex = 0; slideIndex < this._slides.length; slideIndex++) {
            if (slideIndex < this._bullets.length) {
                this._bullets[slideIndex].onclick = () => {
                    this.slideTo(slideIndex);
                };
            }
        }
    }

    slideIn(targetSlideOrder) {
        let activeSlide = this.getActiveSlide();
        let targetSlide = this.getTargetSlide(activeSlide, targetSlideOrder);
        this.slide(activeSlide, targetSlide, targetSlideOrder);
    }

    slideTo(targetSlideIndex) {
        if (targetSlideIndex < 0 || targetSlideIndex >= this._slides.length) {
            throw `Slide index out of range: ${targetSlideIndex}.`;
        }

        let activeSlide = this.getActiveSlide();
        const activeSlideIndex = this.getSlideIndex(activeSlide);
        const targetSlideOrder = this.getTargetSlideOrder(activeSlideIndex, targetSlideIndex);

        if (targetSlideOrder !== CarouselSlideOrder.NONE) {
            let targetSlide = this._slides[targetSlideIndex];
            this.slide(activeSlide, targetSlide, targetSlideOrder);
        }
    }

    getActiveSlide() {
        return this._rootElement.querySelector(CarouselSelector.ACTIVE_SLIDE);
    }

    getTargetSlide(activeSlide, targetSlideOrder) {
        const activeSlideIndex = this.getSlideIndex(activeSlide);
        const targetSlideIndex = this.getTargetSlideIndex(activeSlideIndex, targetSlideOrder);
        return this._slides[targetSlideIndex];
    }

    getSlideIndex(slide) {
        return this._slides.indexOf(slide);
    }

    getTargetSlideIndex(activeSlideIndex, targetSlideOrder) {
        let targetSlideIndex = 0;

        if (targetSlideOrder === CarouselSlideOrder.PREV) {
            targetSlideIndex = activeSlideIndex - 1;
            if (targetSlideIndex < 0) {
                targetSlideIndex = this._slides.length - 1;
            }
        } else if (targetSlideOrder === CarouselSlideOrder.NEXT) {
            targetSlideIndex = activeSlideIndex + 1;
            if (targetSlideIndex === this._slides.length) {
                targetSlideIndex = 0;
            }
        } else {
            throw `Invalid slide order: ${targetSlideOrder}.`;
        }

        return targetSlideIndex;
    }

    getTargetSlideOrder(activeSlideIndex, targetSlideIndex) {
        if (targetSlideIndex < activeSlideIndex) {
            return CarouselSlideOrder.PREV;
        } else if (targetSlideIndex > activeSlideIndex) {
            return CarouselSlideOrder.NEXT;
        } else {
            return CarouselSlideOrder.NONE;
        }
    }

    slide(activeSlide, targetSlide, targetSlideOrder) {
        const slideOrderClassName = this.getSlideOrderClassName(targetSlideOrder);
        const slidingDirectionClassName = this.getSlidingDirectionClassName(targetSlideOrder);

        targetSlide.classList.replace(CarouselClassName.SLIDE, slideOrderClassName);
        this.reflow(targetSlide);

        activeSlide.classList.add(slidingDirectionClassName);
        targetSlide.classList.add(slidingDirectionClassName);

        activeSlide.addEventListener('transitionend', () => {
            targetSlide.classList.remove(slidingDirectionClassName);
            targetSlide.classList.replace(slideOrderClassName, CarouselClassName.ACTIVE_SLIDE);

            activeSlide.classList.remove(slidingDirectionClassName);
            activeSlide.classList.replace(CarouselClassName.ACTIVE_SLIDE, CarouselClassName.SLIDE);
        }, { once: true });

        this.updateBullets(activeSlide, targetSlide);
    }

    getSlideOrderClassName(slideOrder) {
        return slideOrder === CarouselSlideOrder.PREV ?
            CarouselClassName.PREV_SLIDE : CarouselClassName.NEXT_SLIDE;
    }

    getSlidingDirectionClassName(slideOrder) {
        return slideOrder === CarouselSlideOrder.PREV ?
            CarouselClassName.SLIDE_RIGHT : CarouselClassName.SLIDE_LEFT;
    }

    reflow(element) {
        element.offsetHeight;
    }

    updateBullets(activeSlide, targetSlide) {
        const activeSlideIndex = this.getSlideIndex(activeSlide);
        if (activeSlideIndex < this._bullets.length) {
            let activeBullet = this._bullets[activeSlideIndex];
            activeBullet.classList.replace(CarouselClassName.ACTIVE_BULLET, CarouselClassName.BULLET);
        }

        const targetSlideIndex = this.getSlideIndex(targetSlide);
        if (targetSlideIndex < this._bullets.length) {
            let targetBullet = this._bullets[targetSlideIndex];
            targetBullet.classList.replace(CarouselClassName.BULLET, CarouselClassName.ACTIVE_BULLET);
        }
    }
}

window.addEventListener('load', () => {
    let rootElements = document.getElementsByClassName(CarouselClassName.CAROUSEL);
    for (let element of rootElements) {
        new Carousel(element);
    }
});

'use strict';

class Gradient {
    static colorize(elements, options) {
        let shadeValues = this.calculateShadeValues(elements, options);
        for (let elementIndex = 0; elementIndex < elements.length; elementIndex++) {
            let element = elements[elementIndex];
            let shadeValue = shadeValues[elementIndex];
            element.style.backgroundColor = `rgb(${shadeValue}, ${shadeValue}, ${shadeValue})`;
        }
    }

    static calculateShadeValues(elements, options = {}) {
        options = this.getAllOptions(options);

        let shadeValues = new Uint8Array(elements.length);
        let currentColumn = 1;
        let currentShadeValue = options.initialShadeValue;

        for (let elementIndex = 0; elementIndex < elements.length; elementIndex++) {
            if (elementIndex < options.columnCount) {
                // first row
                currentShadeValue -= options.shadeDelta;
            } else {
                // remaining rows
                if (currentColumn === options.columnCount) {
                    currentShadeValue -= options.shadeDelta;
                } else {
                    let referenceElementIndex = elementIndex - options.columnCount + 1;
                    currentShadeValue = shadeValues[referenceElementIndex];
                }
            }

            shadeValues[elementIndex] = currentShadeValue;

            if (currentColumn === options.columnCount) {
                currentColumn = 1;
            } else {
                currentColumn += 1;
            }
        }

        return shadeValues;
    }

    // See: https://stackoverflow.com/a/9602718
    static getAllOptions(options) {
        let defaultOptions = {
            initialShadeValue: 252,
            shadeDelta: 3,
            columnCount: 4
        };

        return Object.assign({}, defaultOptions, options);
    }
}

window.addEventListener('load', () => {
    //var elements = document.getElementsByClassName('bg-gradient');    
    //Gradient.colorize(elements);
});
'use strict';

function animateButtons() {
    let elements = document.querySelectorAll('nav .btn');
    expand(elements);

    document.addEventListener('scroll', () => {
        collapse(elements);
    }, { once: true });
}

function expand(elements) {
    let transitionDelay = 0;
    for (let element of elements) {
        element.style.transitionDelay = transitionDelay + 's';
        element.style.left = '0';
        transitionDelay += 0.1;
    }
}

function collapse(elements) {
    for (let element of elements) {
        element.style.transitionDelay = '0s';
        element.style.left = 'calc(60px - 100%)';

        element.onmouseenter = () => {
            element.style.left = '0';
        };

        element.onmouseleave = () => {
            element.style.left = 'calc(60px - 100%)';
        };
    }
}
