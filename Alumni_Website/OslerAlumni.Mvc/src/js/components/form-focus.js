const FocusWithin = (() => {
    const fields = Array.from(document.querySelectorAll('.c-form-field-file'));

    return {
        init: () => {
            if (fields.length > 0) {
                fields.forEach(field => {
                    let input = field.querySelector('input');

                    input.addEventListener('focus', () => {
                        field.classList.add('is-focused');
                    });

                    input.addEventListener('blur', () => {
                        field.classList.remove('is-focused');
                    });
                })
            }
        }
    }
})();

export default FocusWithin;
