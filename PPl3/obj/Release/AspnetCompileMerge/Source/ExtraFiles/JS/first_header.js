const stayBtn = document.querySelector(".js_stays_btn");
const bookStays = document.querySelector(".js_book_stays")
const experienceBtn = document.querySelector(".js_experience_btn");
const bookExperience = document.querySelector(".js_book_experience");

experienceBtn.addEventListener('click', (e) =>{
    Object.assign(e.target.style,{
        // backgroundColor: 'rgba( 245, 245, 245, 1 )',
        color: '#000',
        fontWeight: '600'
    });
    Object.assign(stayBtn.style,{
        backgroundColor: '',
        color: '',
        fontWeight: '500'
    });
    bookStays.classList.add('close')
    bookExperience.classList.add('open');
})

stayBtn.addEventListener('click', (e) =>{
    Object.assign(experienceBtn.style,{
       backgroundColor: '',
       color: '',
       fontWeight: '500'
    });

    Object.assign(e.target.style,{
        // backgroundColor: 'rgba( 245, 245, 245, 1 )',
        color: '#000',
        fontWeight: '600'
    });

    bookExperience.classList.remove('open')
    bookStays.classList.remove('close')
})

window.addEventListener('load', (event) => {
    Object.assign(stayBtn.style,{
        // backgroundColor: 'rgba( 245, 245, 245, 1 )',
        color: '#000',
        fontWeight: '600'
    });
});