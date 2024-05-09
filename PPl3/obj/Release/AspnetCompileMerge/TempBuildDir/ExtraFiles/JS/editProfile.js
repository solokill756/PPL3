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


//place went

const openBtn = document.querySelector('.open_btn')
const btn = document.querySelector('.open_btn button')
const places = document.querySelectorAll('.place')
let currentClicked = 0
openBtn.addEventListener('click', () => {
    currentClicked++;
    if (currentClicked % 2 != 0) {
        Object.assign(btn.style, {
            transform: 'translateX(28px)',
            color: '#000'
        })
        openBtn.style.opacity = 1;

        for (var place of places) {
            place.style.opacity = 1;
        }
    } else {
        Object.assign(btn.style, {
            transform: 'translateX(0)',
            color: ''
        })
        openBtn.style.opacity = '';

        for (var place of places) {
            place.style.opacity = '';
        }
    }
})

//interest of user

const interestBtns = document.querySelectorAll('.interest_list li>button')
const saveBtn = document.querySelector('.save_btn')
const quantity = document.querySelector('.number')
const listAdd = document.querySelector('.list_button_add')
let parts = quantity.textContent.split('/');
let number = parseInt(parts[0], 10);
let currentCheck = 0;
let selectedButtons = [];
let addBtns = document.querySelectorAll('.add_inter_btn');

const inButtons = document.querySelectorAll('.interest-button');

for (var addBtn of addBtns) {
    addBtn.addEventListener('click', () => {
        interestMenu.classList.add('active')
    })
}

inButtons.forEach((button, index) => {
    if (button.getAttribute('data-selected') === 'true') {
        button.classList.add('focus');
        number++; 
        currentCheck++;
        quantity.textContent = `${currentCheck}/7 selected`;
        if (currentCheck >= 3) {
            listAdd.innerHTML += '<button type="button" class="add_inter_btn"><i class="fa-solid fa-plus"></i></button>';
        }
        addBtns = document.querySelectorAll('.add_inter_btn');
        var temp = button.innerHTML;
        addBtns[currentCheck - 1].innerHTML = temp;
        Object.assign(addBtns[currentCheck - 1].style, {
            border: '2px solid #000',
            color: '#000'
        });
        addBtns.forEach((addBtn) => {
            addBtn.addEventListener('click', () => {
                interestMenu.classList.add('active');
            });
        });
    }
});

if (number === 7) {
    for (let i = 0; i < interestBtns.length; i++) {
        if (!interestBtns[i].classList.contains('focus')) {
            interestBtns[i].style.opacity = 0.3;
        }
    }
}

for (let index = 0; index < interestBtns.length; index++) {
    interestBtns[index].addEventListener('click', (e) => {
        if (currentCheck < 7) {
            if (!e.target.classList.contains('focus')) {
                number++;
                interestBtns[index].classList.add('focus')
                quantity.textContent = `${number}/7 selected`
                if (currentCheck >= 3)
                    listAdd.innerHTML += '<button type="button" class="add_inter_btn"><i class="fa-solid fa-plus"></i></button>'
                addBtns = document.querySelectorAll('.add_inter_btn');
                var temp = interestBtns[index].innerHTML;
                addBtns[currentCheck].innerHTML = temp
                Object.assign(addBtns[currentCheck].style, {
                    border: '2px solid #000',
                    color: '#000'
                })
                for (var addBtn of addBtns) {
                    addBtn.addEventListener('click', () => {
                        interestMenu.classList.add('active')
                    })
                }
                currentCheck++;
                selectedButtons.push(interestBtns[index])
            } else {
                number--;
                e.target.classList.remove('focus')
                quantity.textContent = `${number}/7 selected`
                var a = interestBtns[index].children[0].className;
                var iconPlus = '<i class="fa-solid fa-plus"></i>'
                var btn = listAdd.querySelector('.' + a.slice(9, a.length)).parentNode;
                Object.assign(btn.style, {
                    border: '',
                    color: ''
                })
                if (currentCheck > 3) {
                    btn.remove();
                } else {
                    listAdd.querySelector('.' + a.slice(9, a.length)).parentNode.innerHTML = iconPlus;
                }
                currentCheck--;
                selectedButtons.splice(selectedButtons.indexOf(this), 1);
            }
            for (let i = 0; i < interestBtns.length; i++) {
                if (!interestBtns[i].classList.contains('focus')) {
                    interestBtns[i].style.opacity = 1;
                }
            }
        }
        else if (e.target.classList.contains('focus')) {
            number--;
            e.target.classList.remove('focus')
            quantity.textContent = `${number}/7 selected`
            var a = interestBtns[index].children[0].className;
            var iconPlus = '<i class="fa-solid fa-plus"></i>'
            var btn = listAdd.querySelector('.' + a.slice(9, a.length)).parentNode;
            Object.assign(btn.style, {
                border: '',
                color: ''
            })
            if (currentCheck > 3) {
                btn.remove();
            } else {
                listAdd.querySelector('.' + a.slice(9, a.length)).parentNode.innerHTML = iconPlus;
            }
            currentCheck--;
            selectedButtons.splice(selectedButtons.indexOf(this), 1);
            for (let i = 0; i < interestBtns.length; i++) {
                if (!interestBtns[i].classList.contains('focus')) {
                    interestBtns[i].style.opacity = 1;
                }
            }
        }
        if (number === 7) {
            for (let i = 0; i < interestBtns.length; i++) {
                if (!interestBtns[i].classList.contains('focus')) {
                    interestBtns[i].style.opacity = 0.3;
                }
            }
        }
    })
}





const interestMenu = document.querySelector('.interest_menu')
const interestBox = document.querySelector('.interest_box')
const interestContainer = document.querySelector('.interest_container')
const saveeBtn = document.getElementById('saveButton');

saveBtn.addEventListener('click', (event) => {
    event.stopPropagation();
    interestMenu.classList.remove('active');
});

interestMenu.addEventListener('click', (event) => {
    event.target.classList.remove('active')
})

interestBox.addEventListener('click', (event) => {
    interestMenu.classList.remove('active')
})

interestContainer.addEventListener('click', (event) => {
    event.stopPropagation();
})

