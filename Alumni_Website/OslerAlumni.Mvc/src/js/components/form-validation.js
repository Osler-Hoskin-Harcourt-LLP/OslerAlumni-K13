import axios from 'axios';
import SmoothScroll from '../ultilities/smoothscroll';

const validateFields = (() => {

    const handleError = (name, validationErrorList, fieldErrorMessage) => {

        const field = document.getElementById(name);

        if (field) {
            const parent = field.closest('.c-form-field');

            const fieldErrorMessageNode = parent.querySelector('.c-form-error-message');
            fieldErrorMessageNode.setAttribute('id', `error-${name}`);
            fieldErrorMessageNode.innerHTML = fieldErrorMessage;

            parent.classList.add('c-form-error');
            field.setAttribute('aria-describedby', `error-${name}`);

            const error = document.getElementById(`error-${name}`),
                errorMsg = error.innerHTML,
                validationErrorListItem = document.createElement('li'),
                errorLink = document.createElement('a');

            errorLink.setAttribute('href', `#${name}`);
            errorLink.innerHTML = errorMsg;

            errorLink.addEventListener('click', (e) => {
                e.preventDefault();
                field.focus();
            });

            validationErrorListItem.appendChild(errorLink);

            validationErrorList.appendChild(validationErrorListItem);
        }
    }

    const isValidElement = (element) => {
        return (!['radio', 'submit'].indexOf(element.type) <= 0 && element.name !== '__RequestVerificationToken');
    };

    const getRadioValue = (element) => {
        const options = Array.from(element.querySelectorAll('input[type="radio"]')),
              selected = options.find((option) => option.checked == true);
        return selected ? selected.value : '';
    }

    const formatTel = (num) => {
        return num.replace(/\D+/g, "");
    }

    const getAttachment = (element) => {
        const hasFile = element.value != '';
        return hasFile ? element.files[0] : '';
    }

    // credit: https://code.lengstorf.com/get-form-values-as-json/
    const formToJSON = (elements) => [].reduce.call(elements, (data, element) => {

        if (isValidElement(element)) {
            if (element.getAttribute('role') == 'radiogroup') {
                data[element.id] = getRadioValue(element);
            } else if (element.type =='checkbox') {
                data[element.name] = element.checked;
            } else if (element.type == 'select-one') {
                data[element.name] = element.options[element.selectedIndex].value;
            } else if (element.type == 'tel') {
                data[element.name] = formatTel(element.value);
            } else if (element.type == 'file') {
                data[element.name] = getAttachment(element);
            } else if (element.name == 'g-recaptcha-response') {
                const res = grecaptcha.getResponse();
                data['GoogleCaptchaUserResponse'] = res;
            } else {
                data[element.name] = element.value;
            }
        }
        return data;
    }, {});

    return {
        init: (formId, api) => {
            const form = document.getElementById(formId),
                  data = formToJSON(form.elements),
                  fields = Array.from(form.querySelectorAll('input[type="text"], input[type="email"], input[type="password"], input[type="tel"], input[type="file"], textarea, select, fieldset[role="radiogroup"], .g-recaptcha')),
                  token = form.querySelector('[name="__RequestVerificationToken"]').value,
                  encType = form.enctype,
                  validationErrorSummary = form.querySelector('.c-form-error-summary'),
                  validationErrorList = validationErrorSummary.querySelector('ul'),
                  captcha = document.querySelector('.g-recaptcha');
            let hasAttachment = false;
            validationErrorList.innerHTML = '';
            validationErrorSummary.setAttribute('aria-hidden', true);

            fields.forEach((field) => {
                const parent = field.closest('.c-form-field');
                parent.classList.remove('c-form-error');
                field.removeAttribute('aria-describedby');
            });

            const config = {
                headers: {
                    '__requestverificationtoken': token,
                }
            }

            const attachmentData = new FormData();
            if (encType == 'multipart/form-data' && data.FileUpload != '') {
                hasAttachment = true;

                for (var key in data) {
                    attachmentData.append(key, data[key]);
                }

                config.headers['Content-Type'] = encType;
            }

            return axios
                .post(api, hasAttachment ? attachmentData : data, config)
                .then((response) => {
                    if(captcha) {
                        grecaptcha.reset();
                    }
                    response.fields = data;
                    return response;
                })
                .catch((error) => {
                    if(captcha) {
                        grecaptcha.reset();
                    }

                    var response = error.response;

                    // Validation error code
                    if (response.status === 400) {

                        validationErrorSummary.setAttribute('aria-hidden', false);
                        SmoothScroll(validationErrorSummary);
                        
                        window.setTimeout(function ()
                        {
                            validationErrorSummary.focus();

                        }, 150);

                        const errorResult = response.data.result;

                        if (errorResult) {
                            const fieldsWithErrors = [];

                            Object.entries(errorResult).forEach(([name, message]) => {
                                if (message.length) {
                                    fieldsWithErrors.push(name);
                                }
                            });

                            fields.filter((field) => {
                                return fieldsWithErrors.indexOf(field.id) >= 0;
                            }).forEach((field) => {
                                const fieldErrorMessage = errorResult[field.id];
                                handleError(field.id, validationErrorList, fieldErrorMessage);
                            });
                        }
                    }

                    throw error;
                });
        }
    }

})();

export default validateFields;
