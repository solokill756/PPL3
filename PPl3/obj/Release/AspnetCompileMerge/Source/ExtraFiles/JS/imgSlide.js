const contentContainer = document.querySelectorAll('.content_container')
for (let index = 0; index < contentContainer.length; index++) {
    const listImg = document.querySelector('.list_img_' + index)
    const btnLeft = document.querySelector('.btn_left_' + index)
    const btnRight = document.querySelector('.btn_right_' + index)
    const imgs = document.querySelectorAll('.img_' + index)
    const heartIcon = document.querySelector('.heart_icon_' + index + ' i')
    const heart = document.querySelector('.heart_icon' + index)
    let length = imgs.length
    let current = 0;
    btnRight.addEventListener('click', () => {
        if (current == length - 1) {
            current = 0
            let width = imgs[0].offsetWidth
            listImg.style.transform = 'translateX(0px)'
            document.querySelector('.active_' + index).classList.remove('active_' + index)
            document.querySelector('.index-item-' + current + '_' + index).classList.add('active_' + index)
        } else {
            current++
            let width = imgs[0].offsetWidth;
            listImg.style.transform = `translateX(${width * -1 * current}px)`
            document.querySelector('.active_' + index).classList.remove('active_' + index)
            document.querySelector('.index-item-' + current + '_' + index).classList.add('active_' + index)
        }
    })

    btnLeft.addEventListener('click', () => {
        if (current == 0) {
            current = length - 1
            let width = imgs[0].offsetWidth
            listImg.style.transform = `translateX(${width * -1 * current}px)`
            document.querySelector('.active_' + index).classList.remove('active_' + index)
            document.querySelector('.index-item-' + current + '_' + index).classList.add('active_' + index)
        } else {
            current--
            let width = imgs[0].offsetWidth;
            listImg.style.transform = `translateX(${width * -1 * current}px)`
            document.querySelector('.active_' + index).classList.remove('active_' + index)
            document.querySelector('.index-item-' + current + '_' + index).classList.add('active_' + index)
        }
    })
    let heartCurrent = 0;
    if (heartIcon) {
        heartIcon.addEventListener('click', (e) => {
            heartCurrent++
            if (heartCurrent % 2 != 0) {
                e.target.classList.add('loved');
            } else {
                e.target.classList.remove('loved');
            }
        })
    }
    
}


