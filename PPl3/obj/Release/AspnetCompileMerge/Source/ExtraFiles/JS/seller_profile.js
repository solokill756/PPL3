const profileBtns = document.querySelectorAll('.profile_seller')
const pageWhite = document.querySelector('.page_white')

for (let i = 0; i < profileBtns.length; ++i) {
    //console.log(document.querySelector('.seller_profile_' + i));
    document.querySelector('.profile_seller_' + i).addEventListener('click', () => {
        document.querySelector('.seller_profile_' + i).classList.add('open1')
        setTimeout(() => {
            document.querySelector('.seller_profile_container_left_' + i).classList.add('openProfile')
        }, 500)
        setTimeout(() => {
            document.querySelector('.profile_left_' + i).classList.add('turn')
        }, 1170)
    });
}

for (let i = 0; i < profileBtns.length; ++i) {
    document.querySelector('.profile_seller_' + i).addEventListener('click', () => {
        document.querySelector('.seller_profile_' + i).addEventListener('click', () => {
            document.querySelector('.seller_profile_' + i).classList.remove('open1')
            document.querySelector('.seller_profile_container_left_' + i).classList.remove('openProfile')
            document.querySelector('.profile_left_' + i).classList.remove('turn')
        });
    });
}


for (let i = 0; i < profilebtns.length; ++i) {
    document.querySelector('.profile_seller_' + i).addEventListener('click', () => {
        document.querySelector('.seller_' + i).addEventListener('click', (e) => {
            e.stoppropagation();
        });
    });
}