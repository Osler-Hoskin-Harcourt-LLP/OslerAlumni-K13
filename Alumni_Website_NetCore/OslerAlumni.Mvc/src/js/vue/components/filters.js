// local Filter Group component.
// assign to an object that defines the component object to a variable
import Accordion from '../../components/accordion';
const Filters = {
    template: '#filters',
    props: ['filters', 'selectedfilters', 'searchRequest'],
    methods: {
        toggle(trigger) {
            let el = document.getElementById(trigger.getAttribute('aria-controls'));
            Accordion.init(trigger, el);
        },

        resetFilters() {
            const filterGroups = this.filters;
            filterGroups.forEach(filterGroup => {
                this.$set(this.selectedfilters, filterGroup.fieldName, filterGroup.type == 'MultipleChoice' ? [] : '');
            });
        },

        handleFilters() {
            this.$emit('filter-data', this.selectedfilters);
        },

        handleClearAllFilters() {
            this.resetFilters();
        },

        handleClearFilterGroup(filterGroupName) {
            this.$set(this.selectedfilters, filterGroupName, []);
        },
    },
}

export default Filters;

