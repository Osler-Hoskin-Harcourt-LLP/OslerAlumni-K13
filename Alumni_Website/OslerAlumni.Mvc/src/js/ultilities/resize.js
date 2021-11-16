const Resize = (() => {

    const resizeNodes = Array.from(document.querySelectorAll('[data-breakpoint]'));

    const small = (resizeNode) => {
        if (window.innerWidth < 1024) {
            resizeNode.setAttribute('aria-hidden', 'true');
        } else {
            resizeNode.setAttribute('aria-hidden', 'false');
        }
    };

    const large = (resizeNode) => {
        if (window.innerWidth > 1024) {
            resizeNode.setAttribute('aria-hidden', 'true');
        } else {
            resizeNode.setAttribute('aria-hidden', 'false');
        }
    };

    return {
        init: () => {
            if (resizeNodes.length > 0) {
                resizeNodes.forEach((resizeNode) => {
                    const breakpoint = resizeNode.dataset.breakpoint;
                    if (breakpoint == 'large') {
                        small(resizeNode);
                    } else if (breakpoint == 'small') {
                        large(resizeNode);
                    }
                });
            }
        }
    }
})();

export default Resize;
