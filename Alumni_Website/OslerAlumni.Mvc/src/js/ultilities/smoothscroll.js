import smoothscroll from '../ultilities/smoothscroll-polyfill';

const SmoothScroll = ((element) => {
    window.__forceSmoothScrollPolyfill__ = true;
    smoothscroll.polyfill();
    element.scrollIntoView({
        top: 0,
        left: 0,
        behavior: "smooth",
        block: "start",
        inline: "nearest"
    });
    element.focus(); 
});

export default SmoothScroll;