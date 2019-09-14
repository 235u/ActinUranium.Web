(function () {
    'use strict';

    const ClassName = {
        CARD: 'card'        
    };

    const Selector = {
        OVERLAY: '.card-overlay'
    };

    const Directions = new Map([
        [0, 'top'],
        [1, 'right'],
        [2, 'bottom'],
        [3, 'left']
    ]);

    const ClassNamePostfix = {
        IN: '-in',
        OUT: '-out'
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

    class CardOverlay {
        constructor(card, overlay) {
            this.card = card;
            this.overlay = overlay;

            this.card.addEventListener('mouseenter', (e) => this.update(e, ClassNamePostfix.IN));
            this.card.addEventListener('mouseleave', (e) => this.update(e, ClassNamePostfix.OUT));
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
            // gets the x value relative to the center of the card and "normalize" it
            const x = (e.clientX - this.card.getBoundingClientRect().x - w / 2) * (w > h ? h / w : 1);
            const y = (e.clientY - this.card.getBoundingClientRect().y - h / 2) * (h > w ? w / h : 1);

            // the angle and the direction from where the mouse came in/went out clockwise TRBL (0/1/2/3)
            // first calculate the angle of the point,
            // add 180 deg to get rid of the negative values,
            // divide by 90 to get the quadrant,
            // add 3 and do a modulo by 4 to shift the quadrants to a proper clockwise TRBL (top/right/bottom/left)
            const d = Math.round((Math.atan2(y, x) * (180 / Math.PI) + 180) / 90 + 3) % 4;
            return Directions.get(d);
        }
    }

    window.addEventListener('load', () => {
        let cards = document.getElementsByClassName(ClassName.CARD);
        for (let card of cards) {
            let overlay = card.querySelector(Selector.OVERLAY);
            if (overlay) {
                new CardOverlay(card, overlay);
            }
        }
    });
}());

(function () {
    'use strict';

    const SlideOrder = {
        NONE: 0,
        PREV: 1,
        NEXT: 2
    };

    const ClassName = {
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

    const Selector = {
        ACTIVE_SLIDE: '.active-slide',
        BULLETS: '.active-bullet, .bullet',
        NEXT_SLIDE_CONTROL: '.next-slide-control',
        PREV_SLIDE_CONTROL: '.prev-slide-control',
        SLIDES: '.active-slide, .slide'
    };

    class Carousel {
        constructor(element) {
            this._rootElement = element;
            this._slides = this.getElementsAsArray(Selector.SLIDES);
            this._bullets = this.getElementsAsArray(Selector.BULLETS);
            this._isSliding = false;
            this.addEventListeners();
        }

        getElementsAsArray(selector) {
            const elements = this._rootElement.querySelectorAll(selector);
            return Array.from(elements);
        }

        addEventListeners() {
            let prevSlideControl = this._rootElement.querySelector(Selector.PREV_SLIDE_CONTROL);
            if (prevSlideControl) {
                prevSlideControl.onclick = () => {
                    this.slideIn(SlideOrder.PREV);
                };
            }

            let nextSlideControl = this._rootElement.querySelector(Selector.NEXT_SLIDE_CONTROL);
            if (nextSlideControl) {
                nextSlideControl.onclick = () => {
                    this.slideIn(SlideOrder.NEXT);
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

            if (targetSlideOrder !== SlideOrder.NONE) {
                let targetSlide = this._slides[targetSlideIndex];
                this.slide(activeSlide, targetSlide, targetSlideOrder);
            }
        }

        getActiveSlide() {
            return this._rootElement.querySelector(Selector.ACTIVE_SLIDE);
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

            if (targetSlideOrder === SlideOrder.PREV) {
                targetSlideIndex = activeSlideIndex - 1;
                if (targetSlideIndex < 0) {
                    targetSlideIndex = this._slides.length - 1;
                }
            } else if (targetSlideOrder === SlideOrder.NEXT) {
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
                return SlideOrder.PREV;
            } else if (targetSlideIndex > activeSlideIndex) {
                return SlideOrder.NEXT;
            } else {
                return SlideOrder.NONE;
            }
        }

        slide(activeSlide, targetSlide, targetSlideOrder) {
            const slideOrderClassName = this.getSlideOrderClassName(targetSlideOrder);
            const slidingDirectionClassName = this.getSlidingDirectionClassName(targetSlideOrder);

            targetSlide.classList.replace(ClassName.SLIDE, slideOrderClassName);
            this.reflow(targetSlide);

            activeSlide.classList.add(slidingDirectionClassName);
            targetSlide.classList.add(slidingDirectionClassName);

            activeSlide.addEventListener('transitionend', () => {
                targetSlide.classList.remove(slidingDirectionClassName);
                targetSlide.classList.replace(slideOrderClassName, ClassName.ACTIVE_SLIDE);

                activeSlide.classList.remove(slidingDirectionClassName);
                activeSlide.classList.replace(ClassName.ACTIVE_SLIDE, ClassName.SLIDE);
            }, { once: true });

            this.updateBullets(activeSlide, targetSlide);
        }

        getSlideOrderClassName(slideOrder) {
            return slideOrder === SlideOrder.PREV ?
                ClassName.PREV_SLIDE : ClassName.NEXT_SLIDE;
        }

        getSlidingDirectionClassName(slideOrder) {
            return slideOrder === SlideOrder.PREV ?
                ClassName.SLIDE_RIGHT : ClassName.SLIDE_LEFT;
        }

        reflow(element) {
            element.offsetHeight;
        }

        updateBullets(activeSlide, targetSlide) {
            const activeSlideIndex = this.getSlideIndex(activeSlide);
            if (activeSlideIndex < this._bullets.length) {
                let activeBullet = this._bullets[activeSlideIndex];
                activeBullet.classList.replace(ClassName.ACTIVE_BULLET, ClassName.BULLET);
            }

            const targetSlideIndex = this.getSlideIndex(targetSlide);
            if (targetSlideIndex < this._bullets.length) {
                let targetBullet = this._bullets[targetSlideIndex];
                targetBullet.classList.replace(ClassName.BULLET, ClassName.ACTIVE_BULLET);
            }
        }
    }

    window.addEventListener('load', () => {
        let rootElements = document.getElementsByClassName(ClassName.CAROUSEL);
        for (let element of rootElements) {
            new Carousel(element);
        }
    });
}());

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