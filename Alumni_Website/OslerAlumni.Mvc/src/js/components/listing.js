import 'es6-promise/auto';
import axios from 'axios';
import Vue from 'vue';
import bLazy from '../ultilities/blazy';
import debounce from 'lodash.debounce';
import * as pagination from "../vendor/vuejs-uib-pagination-custom";
Vue.use(pagination);


import Filters from '../vue/components/filters';
import EventAttendeesModal from '../vue/components/eventattendees-modal';
import SmoothScroll from '../ultilities/smoothscroll';

const Listing = (() => {
    return {
        init: () => {

            // main list component (news, events, jobs, resources, development resources)
            Vue.component('main-list', {
                template: '#list-template',
                props: ['results'],
                methods: {
                    handleModal(result, event) {
                        this.$emit('open-modal', result, event);
                    },
                }
            });

            // used only for community news.
            Vue.component('recent-list', {
                template: '#recent-template',
                props: ['recent']
            });

            //Stackoverflow: https://stackoverflow.com/a/1199420
            Vue.filter('truncate', (value, limit) => {
                if (value.length <= limit) { return value; }
                var subString = value.substr(0, limit-4);
                return (subString.substr(0, subString.lastIndexOf(' '))) + " ...";
            });

            Vue.filter('trimSpace', (value) => {
                return value.trim();
            });

            const List = new Vue({
                el: '.c-listing',
                components: {
                    'filters' : Filters,
                    'eventattendees-modal' : EventAttendeesModal,
                },
                data: () => {
                    return {
                        searchRequest: {
                            culture: '',
                            pageSize: '',
                            pageNumber: '',
                            skip: 0,
                            orderBy: '',
                            orderByDirection: '',
                            excludedNodeGuids: [],
                            isPreviewMode: false,
                            includeFilters: '',
                            keywords: '',
                        },
                        api: '',
                        results: [],
                        recent: [],
                        filters: [],
                        keywords: '',
                        defaultImage: '',
                        totalItemCount: '',
                        totalPageCount: 0,
                        pagination: {},
                        paginationSize: 0,
                        focused: '',
                        recentList: '',
                        mainList: '',
                        hasFilters: false,
                        loading: false,
                        hasError: false,
                        hasResults: true,
                        totalResultsCountDisplay: '',
                        params: '',
                        queryParams: '',
                        queryParamsObj: {},
                        queryParamsFiltersObj: {},
                        hasParam: false,
                        selectedFilters: {},
                        clearCultureParams: 'false',
                        activeItem: {},
                        showModal: false,
                        focused: '',
                        IsKeywordOrFilteredSearch: false,
                    };
                },

                beforeMount() {
                    this.loading = true;

                    const uri = decodeURIComponent(location.search.substr(1));
                    this.getUrlParams(uri);
                    this.browserBackRefresh();
                },

                mounted() {
                    let request = this.searchRequest;
                    request.culture = this.$el.dataset.culture;
                    request.pageNumber = this.$el.dataset.pagenumber;
                    request.orderBy = this.$el.dataset.orderby;
                    request.orderByDirection = this.$el.dataset.orderbydirection;
                    request.isPreviewMode = this.$el.dataset.ispreviewmode;
                    request.pageSize = this.$el.dataset.mainpagesize;
                    request.includeFilters = this.$el.dataset.includefilters;

                    let excludedNodeGuids = this.$el.dataset.excludednodeguids;
                    if (excludedNodeGuids) {
                        request.excludedNodeGuids = excludedNodeGuids.split(';');
                    }

                    this.api = this.$el.dataset.type;
                    this.mainList = this.$el.dataset.mainlist;


                    // set page number based on query param or default (1)
                    const pageNum = 'pageNumber';
                    if (this.queryParamsObj.hasOwnProperty(pageNum)) {
                        this.pagination.currentPage = Number(this.queryParamsObj.pageNumber);
                        request.pageNumber = this.pagination.currentPage;
                    } else {
                        this.pagination.currentPage = 1;
                    }

                    this.params = window.location.search.substr(1);

                    this.clearCultureParams = this.$el.dataset.clearcultureparams || 'false';
                    this.clearLangToggle();

                    let hasRecent = this.$el.dataset.recentlist;
                    if (hasRecent) {
                        request.pageSize = this.$el.dataset.recentpagesize;
                        this.recentList = this.$el.dataset.recentlist;
                        this.defaultImage = this.$el.dataset.defaultimage;

                        this.getMostRecent(request);
                    }

                    this.setPagination();

                    window.addEventListener('resize', debounce(() => {
                        this.setPagination();
                    }, 150));

                    this.getListing(request);
                },

                methods: {

                    getMostRecent(params) {
                        params = Object.assign({}, params, {
                            pageSize: this.$el.dataset.recentpagesize
                        });
                        axios.post(`/api/search/${this.api}`, params, {
                            headers: {'Content-type': 'application/json'}
                        })
                        .then((response) => {
                            setTimeout(() => {
                                this.recent = this.checkImage(response.data.items);
                                this.totalItemCount = response.data.totalItemCount;
                            }, 1000);
                        })
                        .catch(error => {
                            this.hasError = true;
                        })
                    },

                    getListing(params) {
                        params = Object.assign({}, params, {
                            pageSize: this.$el.dataset.mainpagesize,
                            skip: this.$el.dataset.recentpagesize || 0
                        });
                        axios.post(`/api/search/${this.api}`, params, {
                            headers: {'Content-type': 'application/json'}
                        })
                        .then((response) => {
                            this.loading = false;
                            this.hasError = false;

                            // check for images in each result
                            this.results = this.checkImage(response.data.items);

                            // check for filters
                            this.filters = response.data.filters || [];
                            if (this.filters.length) {
                                this.hasFilters = true;
                            }

                            // set total result count
                            this.totalItemCount = response.data.totalItemCount;

                            // display no results message
                            this.hasResults = this.totalItemCount > 0;

                            // set results count string
                            this.totalResultsCountDisplay = response.data.totalResultsCountDisplay;

                            // revalidate for images when listings have loaded
                            setTimeout(() => {
                                bLazy.checkImages();
                            }, 500)

                            // if the selectedFilters object is NOT empty, show filters
                            if (Object.keys(this.selectedFilters).length === 0 && this.selectedFilters.constructor === Object) {
                                this.initializeFilters();
                            }

                            // show selected filters if any exist in the url
                            if (Object.keys(this.queryParamsFiltersObj)[0] != '') {
                                Object.entries(this.queryParamsFiltersObj).forEach(([name, selected]) => {
                                    this.selectedFilters[name] = selected;
                                });
                            }

                            // show keywords and jump to page number if they exist in the url
                            if (Object.keys(this.queryParamsObj)[0] != '') {
                                this.keywords = this.queryParamsObj.keywords;
                            }

                            this.IsKeywordOrFilteredSearch = response.data.IsKeywordOrFilteredSearch;

                        })
                        .catch(error => {
                            this.loading = false;
                            this.hasError = true;
                            this.totalItemCount = '';
                            this.totalResultsCountDisplay = '';
                            this.results =  []
                        })
                    },

                    initializeFilters() {
                        const filterGroups = this.filters;
                        filterGroups.forEach(filterGroup => {
                            this.$set(this.selectedFilters, filterGroup.fieldName, filterGroup.type == 'MultipleChoice' ? [] : '');
                        });
                    },

                    checkImage(items) {
                        items.forEach(item => {
                            if (!item.imageUrl || !item.imageUrl.length) {
                                item.imageUrl = this.defaultImage;
                                item.imageAltText = '';
                            }
                        }, this);
                        return items;
                    },

                    onPageChange() {
                        this.searchRequest.pageNumber = this.pagination.currentPage;
                        this.updatePage(this.searchRequest.pageNumber);
                        this.loading = true;
                        this.getListing(this.searchRequest);
                    },

                    updatePage(page) {
                        this.searchRequest.pageNumber = page;
                        this.pagination.currentPage = this.searchRequest.pageNumber;
                        this.queryParams = this.setUrlParams(this.selectedFilters, this.keywords, this.pagination.currentPage);

                        this.setBrowserHistory();

                        setTimeout(function(){
                            const listing = document.querySelector('.c-listings-top');
                            SmoothScroll(listing);
                        }, 500);
                    },

                    setUrlParams(filterParams, keywords, currentPage) {
                        let output = [];

                        const additionalParams = {
                            pageNumber: currentPage,
                            keywords: keywords
                        }

                        for (var prop in filterParams) {
                            if (filterParams.hasOwnProperty(prop) && filterParams[prop].length) {
                                if (Array.isArray(filterParams[prop])) {
                                    output.push(prop + '=' + filterParams[prop].join(','));
                                } else {
                                    output.push('_' + prop + '=' + filterParams[prop]);
                                }
                            }
                        }

                        for (var prop in additionalParams) {
                            if (additionalParams.hasOwnProperty(prop) && additionalParams[prop]) {
                                output.push(prop + '=' + encodeURIComponent(additionalParams[prop]));
                            }
                        }

                        const sources = [filterParams, additionalParams];
                        this.queryParamsFiltersObj = filterParams;
                        this.queryParamsObj = Object.assign.apply(Object, [{}].concat(sources));

                        return output.join('&');
                    },

                    getUrlParams(uri) {
                        // get all of the query string parameters - based on:
                        // https://stackoverflow.com/questions/8648892/convert-url-parameters-to-a-javascript-object#answer-15383092
                        var chunks = uri.split('&');
                        var params = {};

                        for (var i = 0; i < chunks.length; i++) {
                            var chunk = chunks[i].split('=');
                            if (chunk[0].search("\\[\\]") !== -1) {
                                if (typeof params[chunk[0]] === 'undefined') {
                                    params[chunk[0]] = [chunk[1]];
                                } else {
                                    params[chunk[0]].push(chunk[1]);
                                }
                            } else {
                                if (chunk[0] != '' || chunk[0] === 'undefined') {
                                    if (chunk[0] === 'pageNumber' || chunk[0] === 'keywords') {
                                        params[chunk[0]] = chunk[1];
                                    } else if (chunk[0].charAt(0) === '_') {
                                        let key = chunk[0].substr(1);
                                        params[key] = chunk[1];
                                        this.queryParamsFiltersObj[key] = chunk[1];
                                    } else {
                                        params[chunk[0]] = chunk[1].split(',');
                                        this.queryParamsFiltersObj[chunk[0]] = chunk[1].split(',');
                                    }
                                }
                            }
                        }
                        this.queryParamsObj = params;

                        Object.entries(this.queryParamsObj).forEach(([name, selected]) => {
                            this.searchRequest[name] = selected;
                        });
                    },

                    setBrowserHistory() {
                        if (history.pushState) {
                            var newUrl = `${window.location.protocol}//${window.location.host}${window.location.pathname}?${this.queryParams}`;
                            window.history.pushState({ path: newUrl }, '', newUrl);
                        }
                    },

                    browserBackRefresh() {
                        // this.setBrowserHistory();
                        window.onpopstate =  function() {
                            location.reload(true);
                        };
                    },

                    filterPage(selectedFilters) {
                        //this.setBrowserHistory();

                        Object.entries(selectedFilters).forEach(([name, selected]) => {
                            if (selected.length) {
                                this.searchRequest[name] = selected;
                            } else {
                                delete this.searchRequest[name];
                            }
                        });

                        this.updatePage(1);
                        this.loading = true;
                        this.getListing(this.searchRequest);
                    },

                    keywordSearch() {
                        this.loading = true;
                        this.searchRequest.keywords = this.keywords;
                        this.queryParams = this.setUrlParams(this.selectedFilters, this.keywords, this.pagination.currentPage);
                        this.updatePage(1);
                        //this.setBrowserHistory();
                        this.getListing(this.searchRequest);
                    },

                    clearKeywords(){
                        this.keywords = '';
                    },

                    clearLangToggle() {
                        if(this.clearCultureParams === 'true') {
                            const langToggle = document.querySelector('.c-nav-link-culture');

                            if (langToggle) {
                                let langHref = langToggle.href;

                                if (langHref.indexOf('?')!=-1) {
                                    langHref = langHref.substring(0, langHref.indexOf('?'));

                                    langToggle.setAttribute('href', langHref)
                                }
                            }


                        }
                    },

                    openModal(result) {

                        // store the listing items data
                        this.activeItem = result;

                        // show modal
                        this.showModal = true;

                        // lock the document body via css
                        document.body.classList.add('no-scroll');

                        // store the last focused element. When the modal closes, focus returns here
                        this.focused = document.activeElement;
                    },

                    closeModal() {

                        // hide the modal
                        this.showModal = false;

                        // remove the lock we set to document body
                        document.body.classList.remove('no-scroll');

                        // set focus back
                        this.focused.focus();
                    },

                    setPagination() {
                        window.innerWidth < 640 ? this.paginationSize = 3 : this.paginationSize = 5;
                    },
                }
            });
        }
    }
})();

export default Listing;
