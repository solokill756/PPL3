const imgBtnFront = document.querySelector('.img_btn_front')
const imgBtnBehind = document.querySelector('.img_btn_behind')
const fileUpLoadFront = document.getElementById('file_upload_left')
const fileUpLoadBehind = document.getElementById('file_upload_right')
const imgFront = document.querySelector('.img_front')
const imgBehind = document.querySelector('.img_behind')

//Up load img front
imgBtnFront.addEventListener('click', () => {
    fileUpLoadFront.click()
})

fileUpLoadFront.addEventListener('change', () => {
    var file = fileUpLoadFront.files[0];
    if (file) {
        var reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = function (e) {
            var imageUrl = e.target.result;
            imgFront.classList.add('img')
            imgBtnFront.querySelector('i').classList.add('close');
            imgFront.style.backgroundImage = "url('" + imageUrl + "')";
            imgFront.style.backgroundPosition = "top center";
            imgFront.style.backgroundSize = "cover";
            imgFront.style.backgroundRepeat = "no-repeat";
        };
        reader.onerror = function (e) {
            console.error("Error reading file:", e.target.error);
        };
    }
})


//Up load img behind
imgBtnBehind.addEventListener('click', () => {
    fileUpLoadBehind.click()
})

fileUpLoadBehind.addEventListener('change', () => {
    var file = fileUpLoadBehind.files[0];
    if (file) {
        var reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = function (e) {
            var imageUrl = e.target.result;
            imgBehind.classList.add('img')
            imgBtnBehind.querySelector('i').classList.add('close');
            imgBehind.style.backgroundImage = "url('" + imageUrl + "')";
            imgBehind.style.backgroundPosition = "top center";
            imgBehind.style.backgroundSize = "cover";
            imgBehind.style.backgroundRepeat = "no-repeat";
        };
        reader.onerror = function (e) {
            console.error("Error reading file:", e.target.error);
        };
    }
})

var alpha = 0;
const Btns = document.querySelectorAll('.btn')
Btns.forEach((btn) => {
    btn.addEventListener('click', () => {
        alpha++;
        var parent = btn.parentNode;
        var grandParent = parent.parentNode;
        var box = grandParent.lastElementChild;
        if (alpha % 2 != 0) {
            box.classList.add('open');
            Btns.forEach((btn1) => {
                if (btn1 !== btn) {
                    var parent = btn1.parentNode;
                    var grandParent = parent.parentNode;
                    grandParent.classList.add('active')
                }
            })
        } else {
            if (errorMessCCCD.classList.contains('open') || errorMessPhone.classList.contains('open')) {

            } else {
                box.classList.remove('open');
                Btns.forEach((btn1) => {
                    if (btn1 !== btn) {
                        var parent = btn1.parentNode;
                        var grandParent = parent.parentNode;
                        grandParent.classList.remove('active')
                    }
                })
            }
        }
    });
})

const saveBtns = document.querySelectorAll('.save_btn')
const firstName = document.getElementById('first_name')
const lastName = document.getElementById('last_name')
const city = document.getElementById('city')
const district = document.getElementById('district')
const country = document.getElementById('country')
const cccdNumber = document.getElementById('cccd_number')
const phoneNumber = document.getElementById('phone_number')
const errors = document.querySelectorAll('.error')
const errorMessCCCD = document.querySelector('.error_mess')
const errorMessPhone = document.querySelector('.error_mess_phone')

cccdNumber.addEventListener('change', () => {
    if (cccdNumber.value.length !== 12) {
        errorMessCCCD.classList.add('open')
    } else {
        errorMessCCCD.classList.remove('open')
    }
})

phoneNumber.addEventListener('change', () => {
    if (phoneNumber.value.length !== 10) {
        errorMessPhone.classList.add('open')
    } else {
        errorMessPhone.classList.remove('open')
    }
})

saveBtns.forEach((saveBtn) => {
    saveBtn.addEventListener('click', () => {
        var inforName = saveBtn.className.split(' ')
        var parent = saveBtn.parentNode;
        var grandParent = parent.parentNode;
        var inforContainer = grandParent.firstElementChild;
        var inforItem = inforContainer.firstElementChild;
        var textName = inforItem.lastElementChild;

        if (errorMessCCCD.classList.contains('open') || errorMessPhone.classList.contains('open')) {

        } else {
            parent.classList.remove('open')
            Btns.forEach((btn1) => {
                var parent = btn1.parentNode;
                var grandParent = parent.parentNode;
                if (grandParent.classList.contains('active')) {
                    grandParent.classList.remove('active')
                }
            })
        }
        if (inforName[1] === 'name_btn') {
            textName.textContent = `${firstName.value} ${lastName.value}`
        } else if (inforName[1] === 'address_btn') {
            /*textName.textContent = `Live in ${city.value}, ${district.value}, ${country.value}`*/
            var countryName = $('#country').find(':selected').text(); 
            var stateName = $('#district').find(':selected').text(); 
            var cityName = $('#city').find(':selected').text(); 
            textName.textContent = `Live in ${cityName}, ${stateName}, ${countryName}`; 
        }
    })
})


// check gía trị của các thành phần được nhập

