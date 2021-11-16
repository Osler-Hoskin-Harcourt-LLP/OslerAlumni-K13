const RadioToggle = (() => {

    const toggleGroups = Array.from(document.querySelectorAll('fieldset[role=radiogroup]'));

    const show = (toggle, el) => {
        toggle.setAttribute('aria-expanded', true);
        el.setAttribute('aria-hidden', false);
    };

    const hide = (toggleControl, el) => {
        toggleControl.setAttribute('aria-expanded', false);
        el.setAttribute('aria-hidden', true);
    };

    const reset = (el, submit) => {
        el.classList.remove('c-form-error');
        el.querySelector('label').setAttribute('data-file', '');
        el.querySelector('input[type="file"]').value = '';
        el.querySelector('.c-form-error-message').classList.add('hide');

        submit.disabled = false;
    };

    return {
        init: () => {
            if (toggleGroups.length > 0) {
                toggleGroups.forEach(toggleGroup => {

                    const toggles = Array.from(toggleGroup.querySelectorAll('[data-toggle]'));
                    const toggleControl = toggleGroup.querySelector('[aria-expanded]');

                    if (toggles.length > 0) {
                        toggles.forEach(toggle => {

                            let els = Array.from(document.querySelectorAll(`[data-toggled-by=${toggle.getAttribute('data-toggle')}]`));

                            toggle.onclick = () => {
                                    
                                if (toggle.hasAttribute('aria-expanded')) {
                                    els.forEach(el => {
                                        show(toggle, el);
                                    })
                                } else {
                                    els.forEach(el => {
                                        const isFormAttachment = el.querySelector('input[type="file"]');

                                        if (isFormAttachment) {
                                            const submit = el.parentElement.querySelector('.c-form-submit');
                                            reset(el, submit);
                                        }
                                        hide(toggleControl, el);
                                    })
                                }
                            }
                        });
                    }
                });
            }
        }
    }
})();

export default RadioToggle;
