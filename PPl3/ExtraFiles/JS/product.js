var menuUserBtns = document.querySelectorAll('.js_menuUser_btn');
var subNavGuest = document.querySelector('.js_sub_nav_guest');
var menuUser = document.querySelector('.js_menu_user');
const firstHeader = document.querySelector('#first_header');
const headerProduct = document.querySelector('#headerProduct');
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

// Gắn lắng nghe sự kiện click vào header để ẩn menu
headerProduct.addEventListener('click', function () {
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

const guestBtn = document.querySelector('.guest_btn')
const guestBox = document.querySelector('.guest_box')
const chevron = document.querySelector('.chevron')
const newIcon = document.createElement('i');
const OldIcon = document.createElement('i');
newIcon.classList.add('fa-solid', 'fa-chevron-up');
OldIcon.classList.add('fa-solid', 'fa-chevron-down');
let checkbox = 0;
guestBtn.addEventListener('click', () => {
    checkbox++;
    if (checkbox % 2 == 0) {
        guestBox.classList.remove('open')
        chevron.innerHTML = '<i class="fa-solid fa-chevron-down"></i>'
    } else {
        chevron.innerHTML = '<i class="fa-solid fa-chevron-up"></i>'
        guestBox.classList.add('open')
    }
})

//Guest number
const minusAdultsBtn = document.querySelector('.minus_adults_btn')
const plusAdultsBtn = document.querySelector('.plus_adults_btn')
const minusChildBtn = document.querySelector('.minus_child_btn')
const plusChildBtn = document.querySelector('.plus_child_btn')
const minusInfantsBtn = document.querySelector('.minus_infants_btn')
const plusInfantsBtn = document.querySelector('.plus_infants_btn')
const numberAdults = document.querySelector('.number_adult')
const numberChild = document.querySelector('.number_child')
const numberInfants = document.querySelector('.number_infants')
const numberGuest = document.querySelector('.guest_item p')
let numberGuest1 = parseInt(numberAdults.textContent);
let numberGuest2 = parseInt(numberChild.textContent);
let numberGuest3 = parseInt(numberInfants.textContent);

console.log(numberGuest1);
console.log(numberGuest2);
console.log(numberGuest3);

function minusBtn(minus, plus, number, numberG) {
    Object.assign(plus.style, {
        color: '',
        border: '',
    });
    if (numberG < 0) {
        Object.assign(minus.style, {
            color: '',
            border: '',
        });
        numberG = 0;
    } else {
        number.innerHTML = `${numberG}`
    }
}


function plusBtn(minus, plus, number, numberG) {
    Object.assign(minus.style, {
        color: '#000',
        border: '1px solid #000',
    });
    if (numberG > 5) {
        Object.assign(plus.style, {
            color: '#ccc',
            border: '1px solid #ccc',
        });
        numberG = 5;
    } else {
        number.innerHTML = `${numberG}`
    }
}


minusAdultsBtn.addEventListener('click', () => {
    numberGuest1--;
    Object.assign(plusAdultsBtn.style, {
        color: '',
        border: '',
    });
    if (numberGuest1 < 1) {
        Object.assign(minusAdultsBtn.style, {
            color: '',
            border: '',
        });
        numberGuest1 = 1;
    } else {
        numberAdults.innerHTML = `${numberGuest1}`
    }
    if (numberGuest1 >= 1 && numberGuest2 == 0 && numberGuest3 == 0) {
        numberGuest.innerHTML = `${numberGuest1} Adults`;
    }
    else if (numberGuest1 >= 1 && numberGuest2 > 0 && numberGuest3 == 0) {
        numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest2} childs`
    }
    else if (numberGuest1 >= 1 && numberGuest2 > 0 && numberGuest3 > 0) {
        numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest2} childs, ${numberGuest3} infants`
    }
    else if (numberGuest1 >= 1 && numberGuest2 == 0 && numberGuest3 > 0) {
        numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest3} infants`
    }
})

plusAdultsBtn.addEventListener('click', () => {
    if (numberGuest1 <= 5) {
        numberGuest1++;
        plusBtn(minusAdultsBtn, plusAdultsBtn, numberAdults, numberGuest1)
        if (numberGuest1 == 6) {
            numberGuest1 = 5;
            if (numberGuest1 >= 1 && numberGuest2 == 0 && numberGuest3 == 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults`;
            }
            else if (numberGuest1 >= 1 && numberGuest2 > 0 && numberGuest3 == 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest2} childs`
            }
            else if (numberGuest1 >= 1 && numberGuest2 > 0 && numberGuest3 > 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest2} childs, ${numberGuest3} infants`
            }
            else if (numberGuest1 >= 1 && numberGuest2 == 0 && numberGuest3 > 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest3} infants`
            }
        } else {
            if (numberGuest1 >= 1 && numberGuest2 == 0 && numberGuest3 == 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults`;
            }
            else if (numberGuest1 >= 1 && numberGuest2 > 0 && numberGuest3 == 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest2} childs`
            }
            else if (numberGuest1 >= 1 && numberGuest2 > 0 && numberGuest3 > 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest2} childs, ${numberGuest3} infants`
            }
            else if (numberGuest1 >= 1 && numberGuest2 == 0 && numberGuest3 > 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest3} infants`
            }
        }
    }
})

minusChildBtn.addEventListener('click', () => {
    if (numberGuest2 >= 0) {
        numberGuest2--;
        minusBtn(minusChildBtn, plusChildBtn, numberChild, numberGuest2)
        if (numberGuest2 == 0) {
            numberGuest2 = 0;
            if (numberGuest1 >= 1 && numberGuest2 == 0 && numberGuest3 == 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults`;
            }
            else if (numberGuest1 >= 1 && numberGuest2 > 0 && numberGuest3 == 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest2} childs`
            }
            else if (numberGuest1 >= 1 && numberGuest2 > 0 && numberGuest3 > 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest2} childs, ${numberGuest3} infants`
            }
            else if (numberGuest1 >= 1 && numberGuest2 == 0 && numberGuest3 > 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest3} infants`
            }
        } else {
            if (numberGuest1 >= 1 && numberGuest2 == 0 && numberGuest3 == 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults`;
            }
            else if (numberGuest1 >= 1 && numberGuest2 > 0 && numberGuest3 == 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest2} childs`
            }
            else if (numberGuest1 >= 1 && numberGuest2 > 0 && numberGuest3 > 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest2} childs, ${numberGuest3} infants`
            }
            else if (numberGuest1 >= 1 && numberGuest2 == 0 && numberGuest3 > 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest3} infants`
            }
        }
    }
})

plusChildBtn.addEventListener('click', () => {
    if (numberGuest2 <= 5) {
        numberGuest2++;
        plusBtn(minusChildBtn, plusChildBtn, numberChild, numberGuest2)
        if (numberGuest2 == 6) {
            numberGuest2 = 5;
            if (numberGuest1 >= 1 && numberGuest2 == 0 && numberGuest3 == 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults`;
            }
            else if (numberGuest1 >= 1 && numberGuest2 > 0 && numberGuest3 == 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest2} childs`
            }
            else if (numberGuest1 >= 1 && numberGuest2 > 0 && numberGuest3 > 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest2} childs, ${numberGuest3} infants`
            }
            else if (numberGuest1 >= 1 && numberGuest2 == 0 && numberGuest3 > 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest3} infants`
            }
        } else {
            if (numberGuest1 >= 1 && numberGuest2 == 0 && numberGuest3 == 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults`;
            }
            else if (numberGuest1 >= 1 && numberGuest2 > 0 && numberGuest3 == 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest2} childs`
            }
            else if (numberGuest1 >= 1 && numberGuest2 > 0 && numberGuest3 > 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest2} childs, ${numberGuest3} infants`
            }
            else if (numberGuest1 >= 1 && numberGuest2 == 0 && numberGuest3 > 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest3} infants`
            }
        }
    }
})

minusInfantsBtn.addEventListener('click', () => {
    if (numberGuest3 >= 0) {
        numberGuest3--;
        minusBtn(minusInfantsBtn, plusInfantsBtn, numberInfants, numberGuest3)
        if (numberGuest3 == 0) {
            numberGuest3 = 0;
            if (numberGuest1 >= 1 && numberGuest2 == 0 && numberGuest3 == 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults`;
            }
            else if (numberGuest1 >= 1 && numberGuest2 > 0 && numberGuest3 == 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest2} childs`
            }
            else if (numberGuest1 >= 1 && numberGuest2 > 0 && numberGuest3 > 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest2} childs, ${numberGuest3} infants`
            }
            else if (numberGuest1 >= 1 && numberGuest2 == 0 && numberGuest3 > 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest3} infants`
            }
        } else {
            if (numberGuest1 >= 1 && numberGuest2 == 0 && numberGuest3 == 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults`;
            }
            else if (numberGuest1 >= 1 && numberGuest2 > 0 && numberGuest3 == 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest2} childs`
            }
            else if (numberGuest1 >= 1 && numberGuest2 > 0 && numberGuest3 > 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest2} childs, ${numberGuest3} infants`
            }
            else if (numberGuest1 >= 1 && numberGuest2 == 0 && numberGuest3 > 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest3} infants`
            }
        }
    }
})

plusInfantsBtn.addEventListener('click', () => {
    if (numberGuest3 <= 5) {
        numberGuest3++;
        plusBtn(minusInfantsBtn, plusInfantsBtn, numberInfants, numberGuest3)
        if (numberGuest3 == 6) {
            numberGuest3 = 5;
            if (numberGuest1 >= 1 && numberGuest2 == 0 && numberGuest3 == 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults`;
            }
            else if (numberGuest1 >= 1 && numberGuest2 > 0 && numberGuest3 == 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest2} childs`
            }
            else if (numberGuest1 >= 1 && numberGuest2 > 0 && numberGuest3 > 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest2} childs, ${numberGuest3} infants`
            }
            else if (numberGuest1 >= 1 && numberGuest2 == 0 && numberGuest3 > 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest3} infants`
            }
        } else {
            if (numberGuest1 >= 1 && numberGuest2 == 0 && numberGuest3 == 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults`;
            }
            else if (numberGuest1 >= 1 && numberGuest2 > 0 && numberGuest3 == 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest2} childs`
            }
            else if (numberGuest1 >= 1 && numberGuest2 > 0 && numberGuest3 > 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest2} childs, ${numberGuest3} infants`
            }
            else if (numberGuest1 >= 1 && numberGuest2 == 0 && numberGuest3 > 0) {
                numberGuest.innerHTML = `${numberGuest1} Adults, ${numberGuest3} infants`
            }
        }
    }
})


// Tạo một MutationObserver
var observer = new MutationObserver(function () {
    togglePayFooterButton();
});
// Thuộc tính và cài đặt cho MutationObserver
var config = { childList: true, subtree: true };
// Đăng ký MutationObserver cho #checkInDate và #checkOutDate
if (document.getElementById("checkInDate") != null && document.getElementById("checkOutDate") != null) {
    observer.observe(document.getElementById("checkInDate"), config);
    observer.observe(document.getElementById("checkOutDate"), config);
    function togglePayFooterButton() {
        var checkInDateValue = document.getElementById("checkInDate").textContent;
        var checkOutDateValue = document.getElementById("checkOutDate").textContent;
        var payFooterButton = document.getElementById("checkAvailabilityButton");
        if (checkInDateValue !== "Add date" && checkOutDateValue !== "Add date") {

            document.querySelector(".pay_footer").classList.remove("disapear");
            //payFooterButton.addEventListener("click", function () {
            //    //window.location.href = '/user/homeuser/payhotel?checkBook=true';
            //});
        } else {
            document.querySelector(".pay_footer").classList.add("disapear");
            //payFooterButton.removeEventListener("click", function () {
            //    //window.location.href = '/user/homeuser/payhotel?checkBook=true';
            //});
        }
    }
}




