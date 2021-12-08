const AttachFields = (() => {
    return {
        init: (attachment, submit) => {
            attachment.onchange = (e) => {
                e.preventDefault();

                const file = attachment.files[0],
                             parent = attachment.closest('.c-form-field'),
                             label = parent.querySelector('label'),
                             attachmentMsg = parent.querySelector('.c-form-error-message');

                if (file) {
                    const attachmentSize = parseInt(parent.getAttribute('data-max-file-size'), 10) || 0,
                          attachmentTypes = (parent.getAttribute('data-allowed-file-types') || '').split(',');

                    label.setAttribute('data-file', attachment.value.replace(/.*(\/|\\)/, ''));

                    if (attachmentTypes.indexOf(file.type) == -1 || file.size > attachmentSize) {
                        submit.disabled = true;
                        parent.classList.add('c-form-error');
                        attachmentMsg.classList.remove('hide');
                    } else {
                        submit.disabled = false;
                        parent.classList.remove('c-form-error');
                        attachmentMsg.classList.add('hide');
                    }
                }
            };
        }
    }
})();

export default AttachFields;
