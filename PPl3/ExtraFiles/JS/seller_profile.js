const profileBtns = document.querySelectorAll('.profile_seller')
const sellerProfile = document.getElementById('seller_profile')
const seller = document.querySelector('.seller')
const containerLeft = document.querySelector('.seller_profile_container_left')
const profileLeft = document.querySelector('.profile_left')
const pageWhite = document.querySelector('.page_white')
for (var profileBtn of profileBtns) {
    profileBtn.addEventListener('click', () =>{
        sellerProfile.classList.add('open1')
        setTimeout(()=>{
            containerLeft.classList.add('openProfile')
        },500)
        setTimeout(()=>{
            profileLeft.classList.add('turn')
        },1170)
    });
}

sellerProfile.addEventListener('click',() =>{
    sellerProfile.classList.remove('open1')
    containerLeft.classList.remove('openProfile')
    profileLeft.classList.remove('turn')
})

seller.addEventListener('click', (e)=>{
    e.stopPropagation();
})