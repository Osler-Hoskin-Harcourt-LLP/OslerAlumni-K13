import debounce from 'lodash.debounce';

const nav = document.querySelector('.c-nav');
const focusable = Array.from(nav.querySelectorAll('a[href]:not(.c-header-logo), area[href], input:not([disabled]), select:not([disabled]), textarea:not([disabled]), button:not([disabled]), object, embed, *[tabindex], *[contenteditable]'));
const mqMediumDown = 1024;
let previousActiveElement;
let navInitialized = false;

const initNav = () => {
    if (nav) {
        const toggleNode = document.querySelector('.c-nav-toggle');
        toggleNode.addEventListener('click', toggleNav);
        window.addEventListener('resize', debounce(() => {
            if (window.innerWidth < mqMediumDown && !navInitialized) {
                nav.setAttribute('aria-hidden', 'true');
                toggleNode.setAttribute('aria-expanded', 'false');
            } else if (window.innerWidth < mqMediumDown && navInitialized){
                nav.setAttribute('aria-hidden', 'false');
                toggleNode.setAttribute('aria-expanded', 'true');
            }
            else if (window.innerWidth >= mqMediumDown) {
                nav.setAttribute('aria-hidden', 'false');
                toggleNode.setAttribute('aria-expanded', 'true');
            }
        }, 150));
    }
};

const toggleNav = (event) => {
    previousActiveElement = document.activeElement;
    const targetNode = event.currentTarget;
    const isExpanded = targetNode.getAttribute('aria-expanded');

    // find the ID of the expandable element this button controls
    const controlsId = targetNode.getAttribute('aria-controls');
    const menuNode = document.getElementById(controlsId);

    isExpanded === 'true' ? destroyNav(targetNode, menuNode) : createNav(targetNode, menuNode);
};

const createNav = (trigger, menu) => {

    trigger.setAttribute('aria-expanded', 'true');
    trigger.classList.add('open');
    menu.setAttribute('aria-hidden', 'false');
    document.body.classList.add('hide-scroll');
    menu.onkeydown = (e) => { trapFocus(e, trigger, menu); };
    navInitialized = true;
};

const destroyNav = (trigger, menu) => {

    trigger.setAttribute('aria-expanded', 'false');
    trigger.classList.remove('open');
    menu.setAttribute('aria-hidden', 'true');
    navInitialized = false;
    document.body.classList.remove('hide-scroll');
    previousActiveElement.focus();
};

const trapFocus = (event, trigger, menu) => {
    const lastFocus = focusable[focusable.length - 1];
    const firstFocus = focusable[0];
    const keycode = event.which || event.keyCode;

    if (keycode == 27) {
        destroyNav(trigger, menu);
    }
    lastFocus.onkeydown = (e) => {
        if (keycode === 9 && !e.shiftKey) {
            e.preventDefault();
            firstFocus.focus();
        }
    };
    firstFocus.onkeydown = (e) => {
        if (keycode === 9 && e.shiftKey) {
            e.preventDefault();
            lastFocus.focus();
        }
    };
};

initNav();
