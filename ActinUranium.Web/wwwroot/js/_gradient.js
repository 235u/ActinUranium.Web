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