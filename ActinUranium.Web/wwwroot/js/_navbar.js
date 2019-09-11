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
