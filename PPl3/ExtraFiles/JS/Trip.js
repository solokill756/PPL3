const checkIn = document.querySelectorAll('.check_in')
const checkOut = document.querySelectorAll('.check_out')
let currentDate = new Date();

let dayNow = currentDate.getDate();
let monthNow = currentDate.getMonth() + 1;
let yearNow = currentDate.getFullYear();

for (let i = 0; i < checkOut.length; i++) {
    let DateCheckOut = checkOut[i].textContent;
    let parts = DateCheckOut.split('/');
    let day = parseInt(parts[0], 10);
    let month = parseInt(parts[1], 10);
    let year = parseInt(parts[2], 10);
    if ((year < yearNow) || (year == yearNow && month < monthNow) || (year == yearNow && month == monthNow && day < dayNow)) {
        let parent = checkOut[i].parentNode;
        let parent1 = parent.parentNode;
        let parent2 = parent1.parentNode;
        let parent3 = parent2.parentNode;
        let hotelRemove = parent3.parentNode;
        hotelRemove.classList.add('close')
    }
}


//Menu_Header


var menuUserBtns = document.querySelectorAll('.js_menuUser_btn');
var subNavGuest = document.querySelector('.js_sub_nav_guest');
var menuUser = document.querySelector('.js_menu_user');
const Header = document.getElementById('header');
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
        if (current % 2 != 0) {
            e.stopPropagation(); // Ngăn chặn sự kiện click từ việc phát tán ra ngoài
            showMenuUser(); // Hiển thị menu
        } else {
            hideMenuUser();
        }
    });
}

// Gắn lắng nghe sự kiện click vào firstHeader để ẩn menu
Header.addEventListener('click', function () {
    hideMenuUser();
    current = 0;
});

content.addEventListener('click', () => {
    hideMenuUser();
    current = 0;
})

// Ngăn chặn sự kiện click từ menuUser phát tán ra ngoài
menuUser.addEventListener('click', function (e) {
    e.stopPropagation();
});