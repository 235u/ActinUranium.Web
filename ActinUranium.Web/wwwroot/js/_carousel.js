(function () {
    'use strict';

    const SlideOrder = {
        NONE: 0,
        PREV: 1,
        NEXT: 2
    };

    const INTERVAL_IN_MILLISECONDS = 5000;

    const ClassName = {
        ACTIVE_SLIDE: 'active-slide',
        ACTIVE_SLIDE_BULLET: 'active-slide-bullet',
        CAROUSEL: 'carousel',
        NEXT_SLIDE: 'next-slide',
        PREV_SLIDE: 'prev-slide',
        SLIDE: 'slide',
        SLIDE_BULLET: 'slide-bullet',
        SLIDE_LEFT: 'slide-left',
        SLIDE_RIGHT: 'slide-right'
    };

    const Selector = {
        ACTIVE_SLIDE: '.active-slide',
        NEXT_SLIDE_CONTROL: '.next-slide-control',
        PREV_SLIDE_CONTROL: '.prev-slide-control',
        SLIDE_BULLETS: '.active-slide-bullet, .slide-bullet',
        SLIDES: '.active-slide, .slide'
    };    

    class Carousel {
        constructor(element) {
            this._rootElement = element;
            this._slides = this.getElementsAsArray(Selector.SLIDES);                        
            this._slideBullets = this.getElementsAsArray(Selector.SLIDE_BULLETS);
            this._isSliding = false;
            this._interval = null;

            this.cycle();
            this.handleSlideControls();
            this.handleSlideBullets();
        }

        getElementsAsArray(selector) {
            const elements = this._rootElement.querySelectorAll(selector);
            return Array.from(elements);
        }

        cycle() {
            if (this._slides.length > 1) {
                this._interval = setInterval(this.slideIn.bind(this), INTERVAL_IN_MILLISECONDS, SlideOrder.NEXT);
            }
        }

        handleSlideControls() {
            let prevSlideControl = this._rootElement.querySelector(Selector.PREV_SLIDE_CONTROL);
            if (prevSlideControl) {
                prevSlideControl.onclick = () => {
                    clearInterval(this._interval);
                    this.slideIn(SlideOrder.PREV);
                };
            }

            let nextSlideControl = this._rootElement.querySelector(Selector.NEXT_SLIDE_CONTROL);
            if (nextSlideControl) {
                nextSlideControl.onclick = () => {
                    clearInterval(this._interval);
                    this.slideIn(SlideOrder.NEXT);
                };
            }
        }

        handleSlideBullets() {
            for (let slideIndex = 0; slideIndex < this._slides.length; slideIndex++) {
                if (slideIndex < this._slideBullets.length) {
                    this._slideBullets[slideIndex].onclick = () => {
                        clearInterval(this._interval);
                        this.slideTo(slideIndex);
                    };
                }
            }
        }

        slideIn(targetSlideOrder) {
            if (!this._isSliding) {
                let activeSlide = this.getActiveSlide();
                let targetSlide = this.getTargetSlide(activeSlide, targetSlideOrder);
                this.slide(activeSlide, targetSlide, targetSlideOrder);
            }
        }

        slideTo(targetSlideIndex) {
            if (targetSlideIndex < 0 || targetSlideIndex >= this._slides.length) {
                throw `Slide index out of range: ${targetSlideIndex}.`;
            }

            if (!this._isSliding) {
                let activeSlide = this.getActiveSlide();
                const activeSlideIndex = this.getSlideIndex(activeSlide);
                const targetSlideOrder = this.getTargetSlideOrder(activeSlideIndex, targetSlideIndex);

                if (targetSlideOrder !== SlideOrder.NONE) {
                    let targetSlide = this._slides[targetSlideIndex];
                    this.slide(activeSlide, targetSlide, targetSlideOrder);
                }                
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
            this._isSliding = true;

            this.updateSlideBullets(activeSlide, targetSlide);

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

                this._isSliding = false;
            }, { once: true });            
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

        updateSlideBullets(activeSlide, targetSlide) {
            const activeSlideIndex = this.getSlideIndex(activeSlide);
            if (activeSlideIndex < this._slideBullets.length) {
                let activeSlideBullet = this._slideBullets[activeSlideIndex];
                activeSlideBullet.classList.replace(ClassName.ACTIVE_SLIDE_BULLET, ClassName.SLIDE_BULLET);
            }

            const targetSlideIndex = this.getSlideIndex(targetSlide);
            if (targetSlideIndex < this._slideBullets.length) {
                let targetSlideBullet = this._slideBullets[targetSlideIndex];
                targetSlideBullet.classList.replace(ClassName.SLIDE_BULLET, ClassName.ACTIVE_SLIDE_BULLET);
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
