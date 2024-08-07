const listMess = document.querySelectorAll('.list_message li')
const listBody = document.querySelector('.list_body')
const listBodyMess = document.querySelectorAll('.list_body .body_mess')
const avatar = document.querySelectorAll('.avatar_user')
const nameUsers = document.querySelectorAll('.name_user')
const messDetail = document.querySelector('.mess_detail')
const userDetail = document.querySelector('.user_detail')
const btnDetail = document.querySelector('.btn_detail button')
listMess.forEach((mess)=>{
    mess.addEventListener('click', ()=>{
        //Open Mess of user with you
        listMess.forEach((me) => {
            if (me.classList.contains('selected')) {
                me.classList.remove('selected');
            }
        })
        mess.classList.add('selected');
        if (mess.classList.contains('Unread')) {
            mess.classList.remove('Unread');
        }
        var imgUser = mess.firstElementChild.style.background;
        var name = mess.querySelector('.name p').textContent;
        avatar.forEach((avt) =>{
            avt.style.background = imgUser
        })
        nameUsers.forEach((nameU) =>{
            nameU.textContent = name;
        })
        var nameMess = mess.querySelector('.message').className.split(' ')
        var bodyMess = listBody.querySelector('.' + nameMess[1])
        listBodyMess.forEach((body) =>{
            if(body.classList.contains('open')){
                body.classList.remove('open')
            }
        })
        bodyMess.classList.add('open')
        messDetail.classList.add('open')
        messDetail.classList.remove('l-8')
        messDetail.classList.add('l-12')
    })
})

//Send Message
const sendinput = document.getElementById('message_send')
//const sendbtn = document.queryselector('.send_btn')

//sendbtn.addeventlistener('click', ()=>{
//    listbodymess.foreach((bd) =>{
//        if(bd.classlist.contains('open')){
//            const chatsection = bd.queryselector('.chat_section')
//            var text = sendinput.value;
//            if(text !== ''){
//                chatsection.innerhtml += `<div class="text text_you">
//                                        <p>${text}</p>
//                                    </div>`
//            }
//        }
//    })
//    sendinput.value = ''
//})

//document.addEventListener('keydown', (event)=>{
//    if(event.key === 'Enter'){
//        listBodyMess.forEach((bd) =>{
//            if(bd.classList.contains('open')){
//                const chatSection = bd.querySelector('.chat_section')
//                var text = sendInput.value;
//                if(text !== ''){
//                    chatSection.innerHTML += `<div class="text text_you">
//                                            <p>${text}</p>
//                                        </div>`
//                    setTimeout(()=>{
//                        chatSection.innerHTML += `<div class="text text_user">
//                                            <p>oke bro</p>
//                                        </div>`
//                    }, 3000)
//                }
//            }
//        })
//        sendinput.value = ''
//    }
//})

btnDetail.addEventListener('click', () =>{
    if(userDetail.classList.contains('open')){
        messDetail.classList.remove('l-8')
        messDetail.classList.add('l-12')
        userDetail.classList.remove('open')
    }else{
        messDetail.classList.add('l-8')
        messDetail.classList.remove('l-12')
        userDetail.classList.add('open')
        // Lấy NodeList chứa tất cả các phần tử <li> trong .user_detail.open .nav_bar
        const listItems = document.querySelectorAll('.user_detail.open .nav_bar li');

        // Lấy phần tử <li> đầu tiên (index 0)
        const firstListItem = listItems[0];

        firstListItem.addEventListener('click', () => {
            let host_id = document.querySelector('.body_mess.open').className;
            window.location.href = `/user/homeuser/profile?id=${host_id.split(" ")[2]}`
        })

        // Lấy phần tử <li> thứ hai (index 1)
        const secondListItem = listItems[1];


        secondListItem.addEventListener('click', () => {
            var chatSection = document.querySelector('.body_mess.open');
            chatSection.scrollTop = 0;
            messDetail.classList.remove('l-8')
            messDetail.classList.add('l-12')
            userDetail.classList.remove('open');
        })

        // Lấy phần tử <li> thứ ba (index 2)
        const thirdListItem = listItems[2];

        thirdListItem.addEventListener('click', () => {
            let host_id = document.querySelector('.body_mess.open').className;
            window.location.href = `/user/homeuser/BlockUser?userID=${host_id.split(" ")[2]}`
        })


    }
})
