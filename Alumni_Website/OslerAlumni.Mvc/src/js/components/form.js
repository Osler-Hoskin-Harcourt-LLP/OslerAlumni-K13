import inert from 'wicg-inert';
import validateFields from './form-validation';
import attachFields from './form-attachment';
import ImageCrop from './image-crop';

const Forms = (() => {
    const forms = Array.from(document.querySelectorAll('[data-form]'));
    const toggleServerError = (serverError, isShow) => {
        if (isShow) {
            serverError.setAttribute('aria-hidden', false);
            serverError.focus();
        } else {
            serverError.setAttribute('aria-hidden', true);
        }
    };

    const toggleOverlay = (overLay, isActive) => {
        const body = document.body;
        if (isActive) {

            overLay.classList.add('show-overlay'); 
            body.classList.add('no-scroll');
            body.inert = true;
        } else {
            overLay.classList.remove('show-overlay'); 
            body.classList.remove('no-scroll');
            body.inert = false;
        }
    };

    
    const readURL = (input) => {
        if (input.files && input.files[0]) {
            let reader = new FileReader();

            reader.onload = function(e)  {
                const imgDiv = document.getElementById('image-preview');
                const oldImage = document.getElementById('profile-image');
                
                if (oldImage){
                    imgDiv.innerHTML = ""
                } 
                
                let image = document.createElement("img");
                image.src = e.target.result;
                image.setAttribute('id', 'profile-image')
                
                imgDiv.appendChild(image);
                image.onload = function () { 
                    setTimeout(function(){
                        ImageCrop.init(); 
                    }, 200)
                }
            }
            reader.readAsDataURL(input.files[0]);

        }

    };

    return {
        init: () => {
            if (forms.length > 0) {
                forms.forEach((formWrapper) => {
                    const form = formWrapper.querySelector('.c-form'),
                          submit = formWrapper.querySelector('.c-form-submit'),
                          attachment = formWrapper.querySelector('input[type=file]'),
                          imageAttachment = formWrapper.querySelector('.image input[type=file]'),
                          successMsg = document.querySelector('.c-form-success'),
                          serverError = formWrapper.querySelector('.c-form-error-server'),
                          overLay = document.querySelector('.o-overlay');

                    submit.onclick = (e) => {
                        e.preventDefault();

                        toggleServerError(serverError, false);

                        if (overLay) {
                            toggleOverlay(overLay, true);
                        }

                        validateFields
                            .init(form.id, form.action)
                            .then((response) => {
                                if (response.data.status === 'Success') {

                                    if (response.data.refreshOnSuccess) {
                                        window.location.reload(true); 
                                    }

                                    if (response.data.RedirectToUrl) {
                                        window.location = response.data.RedirectToUrl;
                                    }

                                    successMsg.classList.remove('hide');
                                    if (formWrapper.classList.contains('is-full')) {
                                        formWrapper.setAttribute('aria-hidden', 'true');
                                        window.scrollTo(0,0);
                                    } else {
                                        successMsg.focus();
                                    }

                                    if (successMsg.classList.contains('c-form-success-fade-out')){
                                        setTimeout(function () { successMsg.classList.add('hide'); }, 3000);
                                    }

                                } else {
                                    toggleServerError(serverError, true);
                                }
                                if (overLay) {
                                    toggleOverlay(overLay, false);
                                }
                            })
                            .catch((error) => {
                                if (successMsg) {
                                    successMsg.classList.add('hide');
                                }
                                if (overLay) {
                                    toggleOverlay(overLay, false);
                                }
                                var response = error.response;

                                // Validation error code
                                if (response.status === 400) {
                                    return;
                                }

                                toggleServerError(serverError, true);
                            });
                    };

                    if (attachment) {
                        attachFields.init(attachment, submit);
                    }

                    if (imageAttachment) {
                        attachFields.init(attachment, submit);
                        
                        imageAttachment.addEventListener("change", function(){
                            readURL(this);   
                        });
                    }
                })
            }
        }
    }
})();

export default Forms;

