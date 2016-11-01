import { inject, customAttribute } from "aurelia-framework";

@customAttribute("datepicker")
@inject(Element)
export class DatePicker {
    element: any;

    constructor(element) {
        this.element = element;
    }

    attached() {
        jQuery.noConflict();
        jQuery(this.element).datepicker().on("change", e => fireEvent(e.target, "input"));
    }

    detached() {
        jQuery(this.element).datepicker("destroy").off("change");
    }
}

function createEvent(name) {
    const event = document.createEvent("Event");
    event.initEvent(name, true, true);
    return event;
}

function fireEvent(element, name) {
    const event = createEvent(name);
    element.dispatchEvent(event);
}