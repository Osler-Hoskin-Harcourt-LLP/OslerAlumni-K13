import SmoothScroll from './smoothscroll';

const ScrollTo = (() => {

    return {
        init: () => {

            // assign query string parameters to a variable
            const uri = decodeURIComponent(location.search.substr(1));

            // check if query string contains a specified string and if viewing page on small width screen
            if (uri.includes('subpageAlias') && window.innerWidth < 768) {

                // scroll to specified element (top)
                const scrollEl = document.querySelector('[data-scrollto]');
                SmoothScroll(scrollEl);
            }
        }
    }
})();

export default ScrollTo;
