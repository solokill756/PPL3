//Show about detail
let aboutHotelDetail = document.getElementById('about_hotel_detail')
let aboutDetailContainer = document.querySelector('.about_detail_container')
let showMoreAbout = document.querySelector('.show_more_about')
let aboutBox = document.querySelector('.about_box')
showMoreAbout.addEventListener('click', () => {
    aboutHotelDetail.classList.add('active')
})

aboutHotelDetail.addEventListener('click', () => {
    aboutHotelDetail.classList.remove('active')
})

aboutBox.addEventListener('click', (e) => {
    e.stopPropagation();
})

aboutDetailContainer.addEventListener('click', (e) => {
    e.stopPropagation();
})
//show offerDetail
let offerDetail = document.getElementById('offer_detail')
let offerDetailContainer = document.querySelector('.offer_detail_container')
let showAll = document.querySelector('.show_all')
let offerBox = document.querySelector('.offer_box')
showAll.addEventListener('click', () => {
    offerDetail.classList.add('active')
})

offerDetail.addEventListener('click', () => {
    offerDetail.classList.remove('active')
})

offerBox.addEventListener('click', (e) => {
    e.stopPropagation();
})

offerDetailContainer.addEventListener('click', (e) => {
    e.stopPropagation();
})
//Show comment detail
let commentDetail = document.getElementById('comment_detail')
let commentDetailContainer = document.querySelector('.comment_detail_container')
let showComment = document.querySelector('.show_comment')
let commentBox = document.querySelector('.comment_box')
showComment.addEventListener('click', () => {
    commentDetail.classList.add('active')
})

commentDetail.addEventListener('click', () => {
    commentDetail.classList.remove('active')
})

commentBox.addEventListener('click', (e) => {
    e.stopPropagation();
})

commentDetailContainer.addEventListener('click', (e) => {
    e.stopPropagation();
})