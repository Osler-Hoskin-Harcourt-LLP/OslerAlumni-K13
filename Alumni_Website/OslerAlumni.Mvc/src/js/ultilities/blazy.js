import Blazy from 'blazy';

const bLazy = {};
const hasDefaultImage = document.querySelector('[data-defaultimage]');


bLazy.init = () => { 
  const blazyer = new Blazy({
    error: function(ele, msg){
      if(hasDefaultImage){
        const defaultImage = hasDefaultImage.getAttribute('data-defaultimage');
        ele.src = defaultImage;
        ele.classList.remove('b-error');
        ele.classList.add('b-loaded');
      }
    }
  });
};

bLazy.checkImages = () => {
  const blazyer = new Blazy({
    error: function(ele, msg){
      if(hasDefaultImage){
        const defaultImage = hasDefaultImage.getAttribute('data-defaultimage');
        ele.src = defaultImage;
        ele.classList.remove('b-error');
        ele.classList.add('b-loaded');
      }
    }
  });
  blazyer.revalidate();
}

export default bLazy;