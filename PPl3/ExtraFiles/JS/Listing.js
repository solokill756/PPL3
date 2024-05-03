const todayBtn = document.querySelector('.today_btn');
const calendarBtn = document.querySelector('.calendar_btn');
const listingBtn = document.querySelector('.listing_btn');
const pageListing = document.querySelector('.page_listing')
const inboxBtn = document.querySelector('.inbox_btn')
const searchBtn = document.querySelector('.search_btn')
const search = document.querySelector('.search')
const closeBtn = document.querySelector('.fa-circle-xmark')
window.addEventListener('load', (event) => {
    Object.assign(todayBtn.style, {
        backgroundColor: 'rgba( 245, 245, 245, 1 )',
        color: '#000',
        fontWeight: '600'
    });
});

listingBtn.addEventListener('click', () => {
    Object.assign(todayBtn.style, {
        backgroundColor: '',
        color: '',
        fontWeight: ''
    });
    Object.assign(listingBtn.style, {
        backgroundColor: 'rgba( 245, 245, 245, 1 )',
        color: '#000',
        fontWeight: '600'
    });
})

listingBtn.addEventListener('click', () => {
    pageListing.classList.add('open');
})

searchBtn.addEventListener('click', () => {
    search.classList.add('open')
    searchBtn.classList.add('close')
})

closeBtn.addEventListener('click', () => {
    search.classList.remove('open')
    searchBtn.classList.remove('close')
})



const layoutBtn = document.querySelector('.layout_btn')
const layout1 = document.querySelector('.list_hotel_1')
const layout2 = document.querySelector('.list_hotel_2')
let checkLayout = 0;

layoutBtn.addEventListener('click', () => {
    checkLayout++;
    if (checkLayout % 2 != 0) {
        layoutBtn.innerHTML = '<i class="fa-solid fa-table-cells-large"></i>';
        layout1.classList.add('close')
        layout2.classList.add('changeLayout')
    } else {
        layoutBtn.innerHTML = '<i class="fa-solid fa-bars-progress"></i>';
        layout1.classList.remove('close')
        layout2.classList.remove('changeLayout')
    }
})

const removeBtns = document.querySelectorAll('.js_remove_btn')
let btns;
console.log(removeBtns)
for (let i = 0; i < removeBtns.length; i++) {
    removeBtns[i].addEventListener('click', () => {
        if (i < (removeBtns.length / 2)) {
            btns = document.querySelectorAll('.btn_' + (i + 1));
        } else {
            btns = document.querySelectorAll('.btn_' + ((i + 1) - (removeBtns.length / 2)));
        }
        console.log(btns)
        for (var btn of btns) {
            for (let j = 0; j < 2; j++) {
                const parent = btns[j].parentNode;
                const parent1 = parent.parentNode;
                const parent2 = parent1.parentNode;
                const grandParent = parent2.parentNode;
                grandParent.classList.add('close');
            }
            break;
        }
    })
}