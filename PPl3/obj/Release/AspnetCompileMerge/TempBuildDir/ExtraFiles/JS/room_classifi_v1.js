const listRoomClassifi = document.querySelector('.room_classifi_list')
const btnLeftRoom = document.querySelector('.btn_left_room')
const btnRightRoom = document.querySelector('.btn_right_room')
const roomClassifiItem = document.querySelectorAll('.room_classifi_item')
let length1 = roomClassifiItem.length
let current1 = 0;
btnRightRoom.addEventListener('click', () =>{
    if(current1 == length1 - 21){
        btnRightRoom.classList.add('close')
    }else{
        btnRightRoom.classList.remove('close')
        btnLeftRoom.classList.remove('close')
        current1++
        let width1 = roomClassifiItem[0].offsetWidth;
        listRoomClassifi.style.transform = `translateX(${width1 * -1 * current1}px)`
    }
})

btnLeftRoom.addEventListener('click', () =>{
    if(current1 == 0){
        btnLeftRoom.classList.add('close')
    }else{
        btnLeftRoom.classList.remove('close')
        btnRightRoom.classList.remove('close')
        current1--
        let width1 = roomClassifiItem[0].offsetWidth;
        listRoomClassifi.style.transform = `translateX(${width1 * -1 * current1}px)`
    }
})

var temp = 0;
for (let i = 1; i < roomClassifiItem.length; i++){
    document.querySelector('.room_classifi_item_' + (i + 1)).addEventListener('click', () =>{
        temp++;
        if (temp == 1){
            document.querySelector('.room_classifi_item_' + (i + 1) + ' span').classList.add('activeListRoom')
        }else{
            document.querySelector('.activeListRoom').classList.remove('activeListRoom')
            document.querySelector('.room_classifi_item_' + (i + 1) + ' span').classList.add('activeListRoom')
        }
    })
}