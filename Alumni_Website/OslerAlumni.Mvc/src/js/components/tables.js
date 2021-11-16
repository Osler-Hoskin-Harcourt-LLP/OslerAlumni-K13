const basicTables = Array.prototype.slice.call(document.querySelectorAll(".table-scroll"));

function wrap(el, wrapper) {
    el.parentNode.insertBefore(wrapper, el);
    wrapper.appendChild(el);
};

const initTables = (() => {
    if(document.querySelector(".table-scroll") !== null)  {
        const div = document.createElement('div');
        wrap(document.querySelector('.table-scroll'), div);
        div.setAttribute('class', 'table-scroll-wrapper');
    }
});


initTables();