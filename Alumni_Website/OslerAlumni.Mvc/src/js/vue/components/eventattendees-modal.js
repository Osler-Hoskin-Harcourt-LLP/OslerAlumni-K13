import 'es6-promise/auto';
import axios from 'axios';
import debounce from 'lodash.debounce';
import { stringify } from 'querystring';
import unorm from 'unorm';

// todo:
// separate modal and attendees functionality.
// these should be two separate components

const EventAttendeesModal = {
    template: '#EventAttendeesModal',
    props: {
        culture: String,
        activeItem: Object,
    },

    data: () => {
        return {
            searchRequest: {
                culture: '',
                isPreviewMode: false,
                onePlaceFunctionId: '',
            },
            api: 'functionAttendees',
            activeOnePlaceFunctionId: '',
            attendees: [],
            attendeesCount: 0,
            focusableChildren: [],
            input: '',
            inputArr: [],
            filteredList: [],
            results: {},
            isMatch: false,
            showResults: false,
            loading: true,
            showModal: false,
        }
    },

    created() {

        // store OnePlaceFunctionId to use in searchRequest object later
        this.activeOnePlaceFunctionId = this.activeItem.onePlaceFunctionId;

        // call api
        this.getEventAttendees();
    },

    mounted() {
        this.showModal = true;
        this.positionModal();

        window.addEventListener('resize', debounce(() => {
            this.positionModal();
        }, 150));

        // this.setFocusableChildren();

    },

    updated() {
        this.focusableChildren = [];

        this.$nextTick(() => {
            this.getFocusableChildren();
        })
    },

    methods: {
        getEventAttendees() {

            // build the search request object
            let eventAttendeesRequest = this.searchRequest;
            eventAttendeesRequest.culture = this.culture;
            eventAttendeesRequest.onePlaceFunctionId = this.activeOnePlaceFunctionId;

            // get the data
            axios.post(`/api/oneplace/${this.api}`, eventAttendeesRequest, {
                headers: {'Content-type': 'application/json'}
            })
            .then((response) => {

                // store and set data
                this.attendees = response.data.items;
                this.attendeesCount = this.attendees.length;

                // hide the loader, if its showing
                this.loading = false;

                // set and show the list of attendees
                this.showList();

                // set up tab trapping and set focus to the first focusable element in the modal
                this.getFocusableChildren();
                this.focusableChildren[0].focus();
            })
            .catch(error => {
                this.loading = true;
                this.hasError = true;
            })
        },

        positionModal() {

            // on large screens, sets the modals left most position to always match the listing containers right most position, with some padding for fun
            const guide = document.querySelector('.c-list');

            const guideWidth = `${guide.offsetWidth}px`;
            window.innerWidth > 1023 ? this.$el.style.width = guideWidth : this.$el.style.width = '100%';

            // sets the modals max height based on the height of the list
            const guideHeight = `${guide.offsetHeight}px`;
            this.$refs.modalWindow.style.maxHeight = guideHeight;
        },

        showList() {
            this.filteredList = this.attendees;
        },

        filterList() {

            this.filteredList = [];
            this.inputVals = this.normalizeInput(this.input);
            console.log(this.inputVals)
            this.inputArr = this.inputVals.toLowerCase().split(' ');

            this.attendees.forEach((attendee, i) => {

                attendee.searchField = this.normalizeInput(attendee.searchField);

                this.$set(this.results, i, []);

                this.results[i].push(...attendee.searchField.toLowerCase().split(' '));
                this.results[i] = this.results[i].filter(Boolean);

                this.isMatch = this.inputArr.every(input => this.results[i].includes(input));

                if (this.isMatch) {
                    this.filteredList.push(attendee);
                }
            });

            this.showResults = true;

            if (!this.filteredList.length && this.input == '') {
                this.filteredList = this.attendees;
                this.showResults = false;
            }
        },

        normalizeInput(value) {
            return unorm.nfd(value).replace(/[\u0300-\u036f]/g, "").replace(/,/g, ""). replace(/\./g, "").trim();
        },
        handleCloseModal() {
            this.attendees = [];
            this.$emit('close-modal');
        },

        getFocusableChildren() {
            this.focusableChildren = Array.from(this.$el.querySelectorAll("a[href], area[href], input:not([disabled]), select:not([disabled]), textarea:not([disabled]), button:not([disabled]), object, embed, *[tabindex], *[contenteditable]"));
            this.$el.onkeydown = (e) => { this.trapFocus(e, this.focusableChildren); };
        },

        trapFocus(e, focusableChildren) {
            const lastFocus = focusableChildren[focusableChildren.length - 1],
                  firstFocus = focusableChildren[0],
                  keycode = e.which || e.keycode;

            // 27 is the escape key
            if (keycode == 27) {
                this.handleCloseModal();
            }

            // 9 is the tab key
            if (keycode === 9 && !e.shiftKey) {
                if (e.target === lastFocus) {
                    e.preventDefault();
                    firstFocus.focus();
                }
            }

            // 9 is the tab key
            if (keycode === 9 && e.shiftKey) {
                if (e.target === firstFocus) {
                    e.preventDefault();
                    lastFocus.focus();
                }
            }
        },
    },
}

export default EventAttendeesModal;
