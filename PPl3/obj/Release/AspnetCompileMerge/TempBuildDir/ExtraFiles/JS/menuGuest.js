var menuUserBtns = document.querySelectorAll('.js_menuUser_btn');
var subNavUser = document.querySelector('.js_sub_nav_user');
var menuUser = document.querySelector('.js_menu_user');
const Header = document.getElementById('header');
const navLogin = document.querySelectorAll('nav_login')
const content = document.querySelector('.main')
let current = 0;
function showMenuUser() {
    subNavUser.classList.add('open');
}

function hideMenuUser() {
    subNavUser.classList.remove('open');
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


const alertOut = document.querySelector('.alert_out');


if (alertOut) {
    setInterval(() => {
        alertOut.classList.add('offActive')
    }, 3000)

}