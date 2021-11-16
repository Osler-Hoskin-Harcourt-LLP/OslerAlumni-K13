import debounce from 'lodash.debounce';
import SVGInjector from 'svg-injector';

import './ultilities/polyfills.js';
import bLazy from './ultilities/blazy.js';
import './components/nav.js';
import './components/tables.js';

import Forms from './components/form.js'
import RadioToggle from './components/radio-toggle';
import TranscriptToggle from './components/transcript-toggle';
import FocusWithin from './components/form-focus';
import Resize from './ultilities/resize';
import Listing from './components/listing';
import RemoveHash from './ultilities/removehash';
import GlobalSearch from './components/global-search';
import ImageCropRedirect from './ultilities/image-crop-redirect';
import ScrollTo from './ultilities/scroll-to.js';

const OSLERALUMNI = {
    common: {
        init: () => {
            if (document.querySelector('img.c-svg')) {
                var SVGsToInject = document.querySelectorAll('img.c-svg');
                SVGInjector(SVGsToInject);
            }
            GlobalSearch.init();
            Forms.init();
            bLazy.init();
            // Picture element HTML5 shiv
            document.createElement( "picture" );
        }
    },

    Account: {
        LogIn: () => {
            RemoveHash.init();
        }
    },

    BoardOpportunity: {
        Index: () => {
            Listing.init();
        }
    },

    ContactUs: {
        Index: () => {
            RadioToggle.init();
            FocusWithin.init();
        }
    },

    News: {
        Index: () => {
            Listing.init();
        }
    },

    Jobs: {
        Index: () => {
            Listing.init();
        }
    },

    Events: {
        Index: () => {
            Listing.init();
        }
    },

    Search: {
        Index: () => {
            Listing.init();
        }
    },
    Resources: {
        Index: () => {
            Listing.init();
        }
    },

    DevelopmentResources: {
        Index: () => {
            Listing.init();
        }
    },

    Profiles: {
        Index: () => {
            Listing.init();
        }
    },
    Membership: {
        Index: () => {
            ScrollTo.init();
            ImageCropRedirect.init();
            FocusWithin.init();
        }
    }
};

const UTIL = {
    exec: (controller, action) => {
        const namespace = OSLERALUMNI;
        action = (action === undefined) ? 'init' : action;

        if (controller !== '' && namespace[controller] && typeof namespace[controller][action] == 'function') {
            namespace[controller][action]();
        }

    },

    init: () => {
        const body = document.body;
        const controller = body.getAttribute('data-controller');
        const action = body.getAttribute('data-action');

        UTIL.exec('common');
        UTIL.exec(controller);
        UTIL.exec(controller, action);
    }
};

document.addEventListener('DOMContentLoaded', () => {
    UTIL.init();
});
