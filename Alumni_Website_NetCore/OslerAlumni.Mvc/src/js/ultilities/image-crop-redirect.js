const ImageCropRedirect = (() => {
    return {
        init: () => {
            const linkElement = document.querySelector('.e-image-crop-link');

            if(linkElement) {
                const baseURL = window.location.href;
                linkElement.href = baseURL + '&edit=true';
            }
        }
    }
})();

export default ImageCropRedirect;