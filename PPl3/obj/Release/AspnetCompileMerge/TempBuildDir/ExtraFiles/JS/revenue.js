//Xử lý My Hotel

const statuses = document.querySelectorAll('.box_item span')
const hotels = document.querySelectorAll('.box_item')
statuses.forEach((st) =>{
    if(st.textContent === 'In Progress'){
        st.style.backgroundColor = 'chartreuse'
    }
    else if (st.textContent === 'Return'){
        st.style.backgroundColor = 'red'
    }
    else if (st.textContent === 'Pending'){
        st.style.backgroundColor = 'rgb(228, 109, 25)'
    }
})

hotels.forEach((hotel) =>{
    hotel.addEventListener('click', () =>{
        document.querySelector('.focus').classList.remove('focus')
        hotel.classList.add('focus')
    })
})

//Xử lý mess, notifi

const btnMess = document.querySelector('.btn_mess')
const btnNotifi = document.querySelector('.btn_notifi')
const header = document.getElementById('header')
const content = document.querySelector('.main')
const modalMess = document.querySelector('.mess')
const  modalNotifi = document.querySelector('.notifi')

function showModal(modal){
    modal.classList.add('open')
}

function hideModal(modal){
    modal.classList.remove('open')
}

btnMess.addEventListener('click', (e) =>{
    if(modalMess.classList.contains('open')){
        hideModal(modalMess)
    }else{
        showModal(modalMess)
        hideModal(modalNotifi)
    }
    e.stopPropagation();
})

btnNotifi.addEventListener('click', (e)=>{
    if(modalNotifi.classList.contains('open')){
        hideModal(modalNotifi)
    }else{
        showModal(modalNotifi)
        hideModal(modalMess)
    }
    e.stopPropagation();
})

modalMess.addEventListener('click', (e)=>{
    e.stopPropagation();
})

modalNotifi.addEventListener('click', (e)=>{
    e.stopPropagation();
})

header.addEventListener('click', ()=>{
    if(modalMess.classList.contains('open')){
        hideModal(modalMess)
    }
    else if(modalNotifi.classList.contains('open')){
        hideModal(modalNotifi)
    }
})

content.addEventListener('click', () =>{
    if(modalMess.classList.contains('open')){
        hideModal(modalMess)
    }
    else if(modalNotifi.classList.contains('open')){
        hideModal(modalNotifi)
    }
})


//Show all comment 
let commentDetail = document.getElementById('comment_detail')
let commentDetailContainer = document.querySelector('.comment_detail_container')
let showComment = document.querySelector('.show_all_comment')
let commentBox = document.querySelector('.comment_box')
showComment.addEventListener('click', () =>{
    commentDetail.classList.add('active')
})

commentDetail.addEventListener('click', () =>{
    commentDetail.classList.remove('active')
})

commentBox.addEventListener('click', (e) =>{
    e.stopPropagation();
})

commentDetailContainer.addEventListener('click', (e) =>{
    e.stopPropagation();
})

//Show all list room
let listRoom = document.getElementById('list_room')
let btnListRoom = document.querySelector('.show_all_list_room')
let listRoomContainer = document.querySelector('.list_room_container')

btnListRoom.addEventListener('click', ()=>{
    listRoom.classList.add('active')
})

listRoom.addEventListener('click', () =>{
    listRoom.classList.remove('active')
})

listRoomContainer.addEventListener('click', (e)=>{
    e.stopPropagation();
})


//Show all list current of tenants

let listCurrent = document.getElementById('list_current')
let btnListCurrent = document.querySelector('.show_all_list_current')
let listCurrentBox = document.querySelector('.current_box')

btnListCurrent.addEventListener('click', ()=>{
    listCurrent.classList.add('active')
})

listCurrent.addEventListener('click', () =>{
    listCurrent.classList.remove('active')
})

listCurrentBox.addEventListener('click', (e)=>{
    e.stopPropagation();
})