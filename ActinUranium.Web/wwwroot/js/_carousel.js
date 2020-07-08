(function () {
    "use strict";

    const SlideOrder = {
        NONE: 0,
        PREV: 1,
        NEXT: 2
    };

    const INTERVAL_IN_MILLISECONDS = 5000;

    const ClassName = {
        ACTIVE_SLIDE: "active-slide",
        ACTIVE_SLIDE_BULLET: "active-slide-bullet",
        CAROUSEL: "carousel",
        NEXT_SLIDE: "next-slide",
        PREV_SLIDE: "prev-slide",
        SLIDE: "slide",
        SLIDE_BULLET: "slide-bullet",
        SLIDE_LEFT: "slide-left",
        SLIDE_RIGHT: "slide-right"
    };

    const Selector = {
        ACTIVE_SLIDE: `.${ClassName.ACTIVE_SLIDE}`,
        NEXT_SLIDE_CONTROL: ".next-slide-control",
        PREV_SLIDE_CONTROL: ".prev-slide-control",
        SLIDE_BULLETS: `.${ClassName.ACTIVE_SLIDE_BULLET}, .${ClassName.SLIDE_BULLET}`,
        SLIDES: `.${ClassName.ACTIVE_SLIDE}, .${ClassName.SLIDE}`
    };

    class Carousel {
        constructor(element) {
            this.rootElement = element;
            this.slides = this.getElementsAsArray(Selector.SLIDES);
            this.slideBullets = this.getElementsAsArray(Selector.SLIDE_BULLETS);
            this.isSliding = false;
            this.interval = null;

            this.cycle();
            this.handleSlideControls();
            this.handleSlideBullets();
        }

        static getTargetSlideOrder(activeSlideIndex, targetSlideIndex) {
            if (targetSlideIndex < activeSlideIndex) {
                return SlideOrder.PREV;
            } else if (targetSlideIndex > activeSlideIndex) {
                return SlideOrder.NEXT;
            } else {
                return SlideOrder.NONE;
            }
        }

        static getSlideOrderClassName(slideOrder) {
            return slideOrder === SlideOrder.PREV ? ClassName.PREV_SLIDE : ClassName.NEXT_SLIDE;
        }

        static getSlidingDirectionClassName(slideOrder) {
            return slideOrder === SlideOrder.PREV ? ClassName.SLIDE_RIGHT : ClassName.SLIDE_LEFT;
        }

        static reflow(element) {
            element.offsetHeight;
        }

        getElementsAsArray(selector) {
            const elements = this.rootElement.querySelectorAll(selector);
            return [...elements];
        }

        cycle() {
            if (this.slides.length > 1) {
                this.interval = setInterval(this.slideIn.bind(this), INTERVAL_IN_MILLISECONDS, SlideOrder.NEXT);
            }
        }

        handleSlideControls() {
            const prevSlideControl = this.rootElement.querySelector(Selector.PREV_SLIDE_CONTROL);
            if (prevSlideControl) {
                prevSlideControl.onclick = () => {
                    clearInterval(this.interval);
                    this.slideIn(SlideOrder.PREV);
                };
            }

            const nextSlideControl = this.rootElement.querySelector(Selector.NEXT_SLIDE_CONTROL);
            if (nextSlideControl) {
                nextSlideControl.onclick = () => {
                    clearInterval(this.interval);
                    this.slideIn(SlideOrder.NEXT);
                };
            }
        }

        handleSlideBullets() {
            for (let slideIndex = 0; slideIndex < this.slides.length; slideIndex++) {
                if (slideIndex < this.slideBullets.length) {
                    this.slideBullets[slideIndex].onclick = () => {
                        clearInterval(this.interval);
                        this.slideTo(slideIndex);
                    };
                }
            }
        }

        slideIn(targetSlideOrder) {
            if (!this.isSliding) {
                const activeSlide = this.getActiveSlide();
                const targetSlide = this.getTargetSlide(activeSlide, targetSlideOrder);
                this.slide(activeSlide, targetSlide, targetSlideOrder);
            }
        }

        slideTo(targetSlideIndex) {
            if (targetSlideIndex < 0 || targetSlideIndex >= this.slides.length) {
                throw `Slide index out of range: ${targetSlideIndex}.`;
            }

            if (!this.isSliding) {
                const activeSlide = this.getActiveSlide();
                const activeSlideIndex = this.getSlideIndex(activeSlide);
                const targetSlideOrder = Carousel.getTargetSlideOrder(activeSlideIndex, targetSlideIndex);

                if (targetSlideOrder !== SlideOrder.NONE) {
                    const targetSlide = this.slides[targetSlideIndex];
                    this.slide(activeSlide, targetSlide, targetSlideOrder);
                }
            }
        }

        getActiveSlide() {
            return this.rootElement.querySelector(Selector.ACTIVE_SLIDE);
        }

        getTargetSlide(activeSlide, targetSlideOrder) {
            const activeSlideIndex = this.getSlideIndex(activeSlide);
            const targetSlideIndex = this.getTargetSlideIndex(activeSlideIndex, targetSlideOrder);
            return this.slides[targetSlideIndex];
        }

        getSlideIndex(slide) {
            return this.slides.indexOf(slide);
        }

        getTargetSlideIndex(activeSlideIndex, targetSlideOrder) {
            let targetSlideIndex = 0;

            if (targetSlideOrder === SlideOrder.PREV) {
                targetSlideIndex = activeSlideIndex - 1;
                if (targetSlideIndex < 0) {
                    targetSlideIndex = this.slides.length - 1;
                }
            } else if (targetSlideOrder === SlideOrder.NEXT) {
                targetSlideIndex = activeSlideIndex + 1;
                if (targetSlideIndex === this.slides.length) {
                    targetSlideIndex = 0;
                }
            } else {
                throw `Invalid slide order: ${targetSlideOrder}.`;
            }

            return targetSlideIndex;
        }

        slide(activeSlide, targetSlide, targetSlideOrder) {
            this.isSliding = true;

            this.updateSlideBullets(activeSlide, targetSlide);

            const slideOrderClassName = Carousel.getSlideOrderClassName(targetSlideOrder);
            const slidingDirectionClassName = Carousel.getSlidingDirectionClassName(targetSlideOrder);

            targetSlide.classList.replace(ClassName.SLIDE, slideOrderClassName);
            Carousel.reflow(targetSlide);

            activeSlide.classList.add(slidingDirectionClassName);
            targetSlide.classList.add(slidingDirectionClassName);

            activeSlide.addEventListener("transitionend", () => {
                targetSlide.classList.remove(slidingDirectionClassName);
                targetSlide.classList.replace(slideOrderClassName, ClassName.ACTIVE_SLIDE);

                activeSlide.classList.remove(slidingDirectionClassName);
                activeSlide.classList.replace(ClassName.ACTIVE_SLIDE, ClassName.SLIDE);

                this.isSliding = false;
            }, { once: true });
        }

        updateSlideBullets(activeSlide, targetSlide) {
            const activeSlideIndex = this.getSlideIndex(activeSlide);
            if (activeSlideIndex < this.slideBullets.length) {
                const activeSlideBullet = this.slideBullets[activeSlideIndex];
                activeSlideBullet.classList.replace(ClassName.ACTIVE_SLIDE_BULLET, ClassName.SLIDE_BULLET);
            }

            const targetSlideIndex = this.getSlideIndex(targetSlide);
            if (targetSlideIndex < this.slideBullets.length) {
                const targetSlideBullet = this.slideBullets[targetSlideIndex];
                targetSlideBullet.classList.replace(ClassName.SLIDE_BULLET, ClassName.ACTIVE_SLIDE_BULLET);
            }
        }
    }

    window.addEventListener("load", () => {
        const rootElements = document.getElementsByClassName(ClassName.CAROUSEL);
        for (let element of rootElements) {
            new Carousel(element);
        }
    });
}());
