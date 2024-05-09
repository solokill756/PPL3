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


//Remove hotel

const removeGeneralBtn = document.querySelector('.remove_btn_general')
const removeBtns = document.querySelectorAll('.remove_btn button')
const checkBoxs = document.querySelectorAll('.check_box')
const checkRemoves = document.querySelectorAll('.check_remove')
let currentRemove = 0;
removeGeneralBtn.addEventListener('click', () => {
    currentRemove++;
    if (currentRemove % 2 != 0) {
        for (var checkRemove of checkRemoves) {
            checkRemove.classList.add('open')
        }
    } else {
        const checkeds = document.querySelectorAll('.checked');
        for (var checked of checkeds) {
            const parent = checked.parentNode;
            const parent1 = parent.parentNode;
            const hotelContainer = parent1.parentNode;
            hotelContainer.classList.add('close');
        }
        for (var checkRemove of checkRemoves) {
            checkRemove.classList.remove('open')
        }
    }
})
let currentCheck = 0;
checkBoxs.forEach(function (button) {
    button.addEventListener('click', function () {
        currentCheck++;
        if (currentCheck % 2 != 0) {
            this.classList.add('checked');
            this.style.color = 'white';
        } else {
            this.classList.remove('checked');
            this.style.color = '';
        }
    });
});

removeBtns.forEach(function (removeBtn) {
    removeBtn.addEventListener('click', () => {
        const parentR = removeBtn.parentNode;
        const parentR1 = parentR.parentNode;
        const hotelContainerR = parentR1.parentNode;
        hotelContainerR.classList.add('close')
    })
})