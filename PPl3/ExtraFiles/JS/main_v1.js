var menuUserBtns = document.querySelectorAll('.js_menuUser_btn');
var subNavGuest = document.querySelector('.js_sub_nav_guest');
var menuUser = document.querySelector('.js_menu_user');
const firstHeader = document.getElementById('first_header');
const header = document.getElementById('header');
const navLogin = document.querySelectorAll('nav_login')
const content = document.querySelector('.main')
let current = 0;
function showMenuUser() {
    subNavGuest.classList.add('open');
}

function hideMenuUser() {
    subNavGuest.classList.remove('open');
}

// Lặp qua mỗi nút menuUserBtn và gắn lắng nghe sự kiện click
for (var menuUserBtn of menuUserBtns) {
    menuUserBtn.addEventListener('click', function (e) {
        current++
        if(current % 2 != 0){
            e.stopPropagation(); // Ngăn chặn sự kiện click từ việc phát tán ra ngoài
            showMenuUser(); // Hiển thị menu
        }else{
            hideMenuUser();
        }
    });
}

// Gắn lắng nghe sự kiện click vào firstHeader để ẩn menu
firstHeader.addEventListener('click', function () {
    hideMenuUser();
    current = 0;
});

// Gắn lắng nghe sự kiện click vào header để ẩn menu
header.addEventListener('click', function () {
    hideMenuUser();
    current = 0;
});

content.addEventListener('click', () =>{
    hideMenuUser();
    current = 0;
})

// Ngăn chặn sự kiện click từ menuUser phát tán ra ngoài
menuUser.addEventListener('click', function (e) {
    e.stopPropagation();
});

const alertComplete = document.querySelector('.alert_complete');
const userLogOut = document.querySelector('.user_log_out');
const alertOut = document.querySelector('.alert_out');
userLogOut.addEventListener('click', function (e) {
    alertOut.style.cssText = "display: flex !important";
    window.location.href = "/user/homeuser/logout";
});
if (alertComplete) {
    setInterval(() => {
        alertComplete.classList.add('offActive')
    }, 3000)
}
if (alertOut) {
    setInterval(() => {
        alertOut.classList.add('offActive')
    }, 3000)

}



