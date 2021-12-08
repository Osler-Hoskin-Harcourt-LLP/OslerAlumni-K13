const ImageCrop = (() => {
    return {
        init: () => {
            
            const hasProfileImage = document.getElementById('profile-image'),
                ImageX = document.getElementById('ImageX'), 
                ImageY = document.getElementById('ImageY'),
                ImageWidth = document.getElementById('ImageWidth'),
                ImageHeight = document.getElementById('ImageHeight'),
                rectPointX = 0,
                rectPointY = 0,
                rectPointW = 380,
                rectPointH = 380,
                ImageAspectRatio = 1;
                
            if(hasProfileImage){

                let jcp = [];

                Jcrop.load('profile-image').then(img => {
                    jcp = Jcrop.attach(img, {multi:false});
                    const rect = Jcrop.Rect.fromPoints([rectPointX,rectPointY],[rectPointW,rectPointH]);
                    jcp.newWidget(rect,{aspectRatio: ImageAspectRatio});

                    //set coordinates in hidden fields
                    setInitialCrop(jcp);
                                
                    jcp.focus();
                    
                    //update coordinates in hidden fields
                    cropUpdate(jcp);
                });
            
            }

            const setInitialCrop = (stage) => {
                ImageX.setAttribute('value', stage.active.pos.x);
                ImageY.setAttribute('value', stage.active.pos.y);
                ImageWidth.setAttribute('value', stage.active.pos.w);
                ImageHeight.setAttribute('value', stage.active.pos.h);
            }

            const cropUpdate = (stage) => {
                stage.listen('crop.change',(widget,e) => {
                    ImageX.setAttribute('value', stage.active.pos.x);
                    ImageY.setAttribute('value', stage.active.pos.y);
                    ImageWidth.setAttribute('value', stage.active.pos.w);
                    ImageHeight.setAttribute('value', stage.active.pos.h);
                });
            }
            
        }
    }
})();

export default ImageCrop;