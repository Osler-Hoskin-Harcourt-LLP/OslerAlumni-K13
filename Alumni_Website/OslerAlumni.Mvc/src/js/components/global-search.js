const GlobalSearch = (() => {

    const globalSearchFormField = document.querySelector('.c-form-field-global');

    return {
        init: () => {
            if (globalSearchFormField) {
                const input = globalSearchFormField.querySelector('input');
                const searchUrl = globalSearchFormField.getAttribute("data-searchurl");
                const submit = globalSearchFormField.querySelector("button[type='submit']");

                if (input && submit && searchUrl) {
                    input.addEventListener("keyup",
                        function(event) {
                            // Number 13 is the "Enter" key on the keyboard
                            if (event.keyCode === 13) {
                                event.preventDefault();
                                submit.click();
                            }
                        });
                    submit.onclick = () => {
                        window.location.href = searchUrl + "?pageNumber=1&keywords=" + encodeURIComponent(input.value);
                    }
                }
            }
        }
    }
})();

export default GlobalSearch;
