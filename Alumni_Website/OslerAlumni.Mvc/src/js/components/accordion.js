const Accordion = (() => {

    const show = (toggle, el) => {
        toggle.setAttribute('aria-expanded', true);
        el.setAttribute('aria-hidden', false);
    };

    const hide = (toggleControl, el) => {
        toggleControl.setAttribute('aria-expanded', false);
        el.setAttribute('aria-hidden', true);
    };

    return {
        init: (trigger, el) => {

            let isExpanded = trigger.getAttribute('aria-expanded');
            isExpanded == 'true' ? hide(trigger, el) : show(trigger, el);
        }
    }
})();

export default Accordion;
