// nhập discount và giá phòng
var elements = document.getElementsByClassName('_10');

for (var i = 0; i < elements.length; i++) {
  elements[i].contentEditable = 'true';

  elements[i].addEventListener('input', function(e) {
    var newNumber = parseInt(contentWithoutPercent, 10);

    if (contentWithoutPercent === '' || isNaN(newNumber)) {
      this.textContent = '10';
    } else {
      this.textContent = newNumber;
    }
  });

  elements[i].addEventListener('keydown', function(e) {
    if (e.key === 'Enter') {
      e.preventDefault();
      this.blur();
    }
  });

  elements[i].addEventListener('blur', function(e) {
    var contentWithoutPercent = this.textContent.slice(0, -1);
    var newNumber = parseInt(contentWithoutPercent, 10);

    if (contentWithoutPercent === '') {
      this.textContent = '10';
    }
  });
}


var elements = document.getElementsByClassName('_102');

for (var i = 0; i < elements.length; i++) {
  elements[i].contentEditable = 'true';

  elements[i].addEventListener('input', function(e) {
    var contentWithoutPercent = this.textContent.slice(0, -1);
    var newNumber = parseInt(contentWithoutPercent, 10);

    if (contentWithoutPercent === '' || isNaN(newNumber)) {
      this.textContent = '%';
    } else {
      this.textContent = newNumber + '%';
    }
  });

  elements[i].addEventListener('keydown', function(e) {
    if (e.key === 'Enter') {
      e.preventDefault();
      this.blur();
    }
  });

  elements[i].addEventListener('blur', function(e) {
    var contentWithoutPercent = this.textContent.slice(0, -1);
    var newNumber = parseInt(contentWithoutPercent, 10);

    if (contentWithoutPercent === '') {
      this.textContent = '0%';
    }
  });
}

//Chuyển trang

const nextBtn = document.querySelector('.next_btn button')
const saveBtn = document.querySelector('.save_btn button')
const backBtn = document.querySelector('.back_btn button')
const doneBtn = document.querySelector('.done_btn')
const barStep1 = document.querySelector('.bg-black_step_1')
const barStep2 = document.querySelector('.bg-black_step_2')
const barStep3 = document.querySelector('.bg-black_step_3')
var video1 = document.getElementById('myVideo-1')
var video2 = document.getElementById('myVideo-5')
var video3 = document.getElementById('myVideo-9')
var page1 = document.getElementById('page_1');
console.log(index);
console.log(saveBtn);
var barWith;
var step;
if (index == 1) {
    barWith = 0;
}
else {
    page1.classList.add('close');
    var pageshow = document.getElementById(`page_${index}`);
    pageshow.classList.add('open');
    if (index < 5) {
        step = 0;
        barWith = (index - 1) * 33.333;
    }
    else if (index < 9) {
        step = 1;
        barWith = (index % 5) * 33.333;
    }
    else {
        step = 2;
        barWith = (index % 9) * 33.333;
    }
    for (var i = 1; i <= step; ++i) {
        if (i === 1) {
            barStep1.style.width = '99.999%';
        }
        if (i === 2) {
            barStep2.style.width = '99.999%';
        }
    }
   
    console.log(barWith + " " + step);
    if (step + 1 === 1) {
        barStep1.style.width = `${barWith}%`;
    }
    else if (step + 1 === 2) {
        barStep2.style.width = `${barWith}%`;
    }
    else if (step + 1 === 3) {
        barStep3.style.width = `${barWith}%`;
    }
    
}
var current = 0;
nextBtn.addEventListener('click', () => {
    index++;
    backBtn.style.pointerEvents = ''
    var pageShow = document.querySelector('#page_' + index);
    var pageHide = document.querySelector('#page_' + (index - 1));
    pageHide.style.transform = 'translateX(-100%)'
    pageShow.style.transform = ''
    setTimeout(() => {
        pageHide.classList.remove('open')
        pageShow.classList.add('open')
    }, 700)
    if (index > 1 && index < 5) {
        barWith = barWith + 33.333;
        barStep1.style.width = `${barWith}%`
        nextBtn.style.pointerEvents = 'none'
        saveBtn.style.pointerEvents = 'none'
        if (index === 2) {
            const placeBtn = document.querySelectorAll('.list_place button')
            placeBtn.forEach((place) => {
                if (place.classList.contains('selected')) {
                    nextBtn.style.pointerEvents = ''
                    saveBtn.style.pointerEvents = ''
                }
                    
                place.addEventListener('focus', () => {
                    placeBtn.forEach((pla) => {
                        if (pla.classList.contains('selected')) {
                            pla.classList.remove('selected')
                            nextBtn.style.pointerEvents = ''
                            saveBtn.style.pointerEvents = ''
                        }
                    })
                    place.classList.add('selected')
                    nextBtn.style.pointerEvents = ''
                    saveBtn.style.pointerEvents = ''
                })
            })
        } else if (index === 3) {
            const listTypeOf = document.querySelectorAll('.list_type_of button')
            listTypeOf.forEach((type) => {
                if (type.classList.contains('selected')) {
                    nextBtn.style.pointerEvents = ''
                    saveBtn.style.pointerEvents = ''
                }
                   
                type.addEventListener('focus', () => {
                    listTypeOf.forEach((typ) => {
                        if (typ.classList.contains('selected')) {
                            typ.classList.remove('selected')
                        }
                    })
                    type.classList.add('selected')
                    nextBtn.style.pointerEvents = ''
                    saveBtn.style.pointerEvents = ''
                })
            })
        } else if (index === 4) {
            const listInput = document.querySelectorAll('.input_list input')
            var checkInput = 0
            listInput.forEach((inputs) => {
                if (inputs.value.trim() !== '') {
                    checkInput++
                }
                console.log(checkInput)
                if (checkInput === listInput.length) {
                    saveBtn.style.pointerEvents = ''
                    nextBtn.style.pointerEvents = ''
                }
                    
            })
            checkInput = 0
            listInput.forEach((input) => {
                input.addEventListener('change', () => {
                    const listInput = document.querySelectorAll('.input_list input')
                    listInput.forEach((inp) => {
                        console.log(inp.value)
                        if (inp.value.trim() !== '')
                            checkInput++;
                        else
                            checkInput--;
                    })
                    if (checkInput === 4) {
                        saveBtn.style.pointerEvents = ''
                        nextBtn.style.pointerEvents = ''
                    }
                    else {
                        nextBtn.style.pointerEvents = 'none'
                        saveBtn.style.pointerEvents = 'none'
                    }
                        
                    checkInput = 0
                })
            })
        }
    }
    else if (index === 5) {
        if (barWith > 66.660 && barWith < 66.667) {
            barWith = barWith + 33.333;
            barStep1.style.width = `${barWith}%`
        }
        barWith = 0;
        video2.addEventListener('ended', () => {
            video2.currentTime = 0
            video2.playbackRate = 1.0
            video2.play();
        })
    }
    else if (index > 5 && index < 9) {
        nextBtn.style.pointerEvents = 'none'
        saveBtn.style.pointerEvents = 'none'
        if (index === 6) {
            barWith = 0;
            barWith = barWith + 33.333;
            var check1 = 1;
            const remove = document.querySelectorAll('.remove')
            remove.forEach((rm) => {
                if (rm.classList.contains('open')) {
                    ++check1;
                }
            })
            console.log(check1)
            if (check1 === 6) {
                nextBtn.style.pointerEvents = ''
                saveBtn.style.pointerEvents = ''
            }
        } else {
            barWith = barWith + 33.333;
        }
        if (index === 7) {
            nextBtn.style.pointerEvents = ''
            saveBtn.style.pointerEvents = ''
        }
        else if (index === 8) {
            offerBtns.forEach((offer) => {
                if (offer.classList.contains('clicked')) {
                    nextBtn.style.pointerEvents = ''
                    saveBtn.style.pointerEvents = 'none'
                    checkOffer++;
                }
            })
            if (checkOffer === 0) {
                nextBtn.style.pointerEvents = 'none'
                saveBtn.style.pointerEvents = 'none'
            }
                
            checkOffer = 0
        }
        barStep2.style.width = `${barWith}%`
            
    }

        else if (index === 9) {
            if (barWith > 66.660 && barWith < 66.667) {
                barWith = barWith + 33.333;
                barStep2.style.width = `${barWith}%`
            }
            barWith = 0;
            video3.addEventListener('ended', () => {
                video3.currentTime = 0
                video3.playbackRate = 1.0
                video3.play();
            })
        }
        else if (index > 9 && index < 13) {
        if (index === 10) {
                barWith = 0;
                barWith = barWith + 33.333;
                nextBtn.style.pointerEvents = 'none'
                const nameHotel = document.getElementById('property_name').value;
                const hightlightsBtns = document.querySelectorAll('.highlights_list button')
                const descrideHotel = document.getElementById('property_describe').value;
                const startDate = document.getElementById('Check_in').value;
                const endDate = document.getElementById('Check_out').value;
                var currentHigh = document.querySelectorAll('.highlights_list .clicked').length;
                console.log(currentHigh);
                console.log(startDate);
                if (currentHigh === 2) {
                    hightlightsBtns.forEach((btn1) => {
                        if (btn1.classList.contains('clicked')) {

                        }
                        else {
                            btn1.style.opacity = 0.4
                            btn1.style.pointerEvents = 'none'
                        }
                    })
                   
                }
                
               
                current = currentHigh;
               
               
                

            if (nameHotel.trim() !== '' && currentHigh === 2 && descrideHotel.trim() !== '' && startDate.trim() !== '' && endDate.trim() !== '') {
                nextBtn.style.pointerEvents = ''
                saveBtn.style.pointerEvents = ''
            }
                else {
                nextBtn.style.pointerEvents = 'none'
                saveBtn.style.pointerEvents = 'none'
                }
                   
            } else {
                barWith = barWith + 33.333;
            }
            if (index === 11) {
                nextBtn.style.pointerEvents = 'none'
                saveBtn.style.pointerEvents = 'none'
                const listConfirm = document.querySelectorAll('.confirm_list button')
                listConfirm.forEach((confirm) => {
                    if (confirm.classList.contains('selected')) {
                        nextBtn.style.pointerEvents = ''
                        saveBtn.style.pointerEvents = ''
                    }
                        
                    confirm.addEventListener('focus', () => {
                        listConfirm.forEach((cf) => {
                            if (cf.classList.contains('selected')) {
                                cf.classList.remove('selected')
                            }
                        })
                        confirm.classList.add('selected')
                        nextBtn.style.pointerEvents = ''
                        saveBtn.style.pointerEvents = ''
                    })
                })
            }
            barStep3.style.width = `${barWith}%`
        }


    if (index === 1) {
        page1.classList.remove('close');
            backBtn.style.pointerEvents = 'none'
            video1.addEventListener('ended', () => {
                video1.currentTime = 0
                video1.playbackRate = 1.0
                video1.play();
            })
        }
        else if (index === 12) {
            nextBtn.classList.add('close')
            doneBtn.classList.add('open')
        } else {
            nextBtn.classList.remove('close')
            doneBtn.classList.remove('open')
        }
})





backBtn.addEventListener('click', () =>{
    index--;
    nextBtn.style.pointerEvents = ''
    saveBtn.style.pointerEvents = ''
    var pageShow = document.querySelector('#page_' + index);
    var pageHide = document.querySelector('#page_' + (index + 1));
    pageShow.classList.add('active')
    pageShow.style.transform = ''
    pageHide.style.transform = 'translateX(100%)'
    
    setTimeout(() => {
      pageHide.classList.remove('active')
      pageHide.classList.remove('open')
      pageShow.classList.add('open')
    }, 700)
    if (index >= 1 && index < 5){
      if(index === 4){
        barWith = 133.332;
        barWith = barWith - 33.333;
      }else{
        barWith = barWith - 33.333;
      }
      barStep1.style.width = `${barWith}%`
      video1.currentTime = 0
    }
    else if(index === 5){
      barWith = barWith - 33.333;
      barStep2.style.width = `${barWith}%`
      barWith = 133.332;
      video2.addEventListener('ended', () =>{
        video2.currentTime = 0
        video2.playbackRate = 1.0
        video2.play();
      })
    }
    else if(index > 5 && index < 9){
      if(index === 8){
        barWith = 133.332;
        barWith = barWith - 33.333;

      }
      else{
        barWith = barWith - 33.333;
      }
      barStep2.style.width = `${barWith}%`
    }
    else if(index === 9){
      barWith = barWith - 33.333;
      barStep3.style.width = `${barWith}%`
      barWith = 133.332;
      video3.addEventListener('ended', () =>{
        video3.currentTime = 0
        video3.playbackRate = 1.0
        video3.play();
      })
    }
    else if(index > 9 && index < 13){
      barWith = barWith - 33.333;
      barStep3.style.width = `${barWith}%`
      video3.currentTime = 0
    }

    if (index === 1) {
        page1.classList.remove('close');
      backBtn.style.pointerEvents = 'none'
      video1.addEventListener('ended', () =>{
        video1.currentTime = 0
        video1.playbackRate = 1.0
        video1.play();
      })
    }
    else if(index === 12){
      nextBtn.classList.add('close')
      doneBtn.classList.add('open')
    }else{
      nextBtn.classList.remove('close')
      doneBtn.classList.remove('open')
    }
})


//Xử lý button page 7

const minusBtns = document.querySelectorAll('.minus_btn')
const plusBtns = document.querySelectorAll('.plus_btn')

minusBtns.forEach((btn) =>{
  btn.addEventListener('click', () =>{
    var parent = btn.parentNode;
    var textNumber = parent.querySelector('span');
    var number = Number.parseInt(parent.querySelector('span').textContent, 10);
    if(number > 0){
      number--;
      textNumber.textContent = `${number}`
    }else{
      btn.style.pointerEvents = 'none'
      btn.style.color = '#ccc'
      btn.style.border = '1px solid #ccc'
    }
  })
})


plusBtns.forEach((btn) =>{
  btn.addEventListener('click', () =>{
    var parent = btn.parentNode;
    var textNumber = parent.querySelector('span');
    var minusBtn = parent.firstElementChild;
    var number = Number.parseInt(parent.querySelector('span').textContent, 10);
    number++;
    textNumber.textContent = `${number}`
    minusBtn.style.color = '#000'
    minusBtn.style.border = '1px solid #000'
    minusBtn.style.pointerEvents = ''
  })
})


//Xử lý button ở page 8

const offerBtns = document.querySelectorAll('.list_offer button')
const amenitiesBtns = document.querySelectorAll('.amenities_list button')
var checkOffer = 0
offerBtns.forEach((btn) =>{
  btn.addEventListener('click',()=>{
    if(btn.classList.contains('clicked')){
      btn.classList.remove('clicked')
    }else{
      btn.classList.add('clicked')
    }
    offerBtns.forEach((offer)=>{
      if(offer.classList.contains('clicked')){
          nextBtn.style.pointerEvents = ''
          saveBtn.style.pointerEvents = ''
        checkOffer++;
      }
    })
    if(checkOffer === 0)
      nextBtn.style.pointerEvents = 'none'
      saveBtn.style.pointerEvents = 'none'
    checkOffer = 0
  })
})

amenitiesBtns.forEach((btn) =>{
  btn.addEventListener('click',()=>{
    if(btn.classList.contains('clicked')){
      btn.classList.remove('clicked')
        nextBtn.style.pointerEvents = ''
        saveBtn.style.pointerEvents = ''
    }else{
      btn.classList.add('clicked')
    }
  })
})

//Xử lý button page 10

const hightlightsBtns = document.querySelectorAll('.highlights_list button')
var checkHigthLight = false
const descrideHotel = document.getElementById('property_describe')
const startDate = document.getElementById('Check_in')
const endDate = document.getElementById('Check_out')
const nameHotel = document.getElementById('property_name')

nameHotel.addEventListener('change', () => {

    if (nameHotel.value.trim() !== '' && document.querySelectorAll('.highlights_list .clicked').length == 2 && descrideHotel.value.trim() !== '' && startDate.value.trim() !== '' && endDate.value.trim() !== '') {
        nextBtn.style.pointerEvents = ''
        saveBtn.style.pointerEvents = '';
    }

    else {
        nextBtn.style.pointerEvents = 'none'
        saveBtn.style.pointerEvents = 'none'
    }
})

startDate.addEventListener('change', () => {
    
    if (nameHotel.value.trim() !== '' && document.querySelectorAll('.highlights_list .clicked').length == 2 && descrideHotel.value.trim() !== '' && startDate.value.trim() !== '' && endDate.value.trim() !== '') {
        nextBtn.style.pointerEvents = ''
        saveBtn.style.pointerEvents = '';
    }

    else {
        nextBtn.style.pointerEvents = 'none'
        saveBtn.style.pointerEvents = 'none'
    }
})

endDate.addEventListener('change', () => {
    console.log(endDate.value);
    if (nameHotel.value.trim() !== '' && document.querySelectorAll('.highlights_list .clicked').length == 2 && descrideHotel.value.trim() !== '' && startDate.value.trim() !== '' && endDate.value.trim() !== '') {
        nextBtn.style.pointerEvents = ''
        saveBtn.style.pointerEvents = '';
    }

    else {
        nextBtn.style.pointerEvents = 'none'
        saveBtn.style.pointerEvents = 'none'
    }
})
hightlightsBtns.forEach((btn) =>{
  btn.addEventListener('click',()=>{
    if(btn.classList.contains('clicked')){
      current--;
      btn.classList.remove('clicked')
    }else{
      current++
      btn.classList.add('clicked')
    }
    if(current === 2){
      hightlightsBtns.forEach((btn1) =>{
        if(btn1.classList.contains('clicked')){
          
        }else{
          btn1.style.opacity = 0.4
          btn1.style.pointerEvents = 'none'
        }
      })
        if (nameHotel.value.trim() !== '' && current == 2 && descrideHotel.value.trim() !== '' && startDate.value.trim() !== '' && endDate.value.trim() !== '') {
            nextBtn.style.pointerEvents = ''
            saveBtn.style.pointerEvents = '';
        }

        else {
            nextBtn.style.pointerEvents = 'none'
            saveBtn.style.pointerEvents = 'none'
        }
    }else{
      hightlightsBtns.forEach((btn1) =>{
        btn1.style.opacity = 1
        btn1.style.pointerEvents = ''
      })
        nextBtn.style.pointerEvents = 'none'
        saveBtn.style.pointerEvents = 'none'
    }
  })
})


//UpLoad ảnh hotel
const img = document.querySelectorAll('.img')
const remove = document.querySelectorAll('.remove')
for (let i = 1; i <= img.length; i++){
  const fileUpLoad = document.getElementById('up_img_' + i)
  const addImgBtn = document.querySelector('.btn_img_' + i)
  const imgProfile = document.querySelector('.img_' + i)
  const btnRemove = document.querySelector('.remove_' + i)
  
  imgProfile.addEventListener('click', () =>{
    fileUpLoad.click();
  })

  btnRemove.addEventListener('click', (e) => {
    imgProfile.style.backgroundImage = "";
    imgProfile.style.backgroundPosition = "";
    imgProfile.style.backgroundSize = "";
    imgProfile.style.backgroundRepeat = "";
    btnRemove.classList.remove('open')
    addImgBtn.classList.remove('close')
      nextBtn.style.pointerEvents = 'none'
      saveBtn.style.pointerEvents = 'none'
    fileUpLoad.value = null;
    e.stopPropagation();
  })
  
  fileUpLoad.addEventListener('change', () =>{

      var file = fileUpLoad.files[0];

      if (file) {
        var reader = new FileReader();
        reader.readAsDataURL(file);
          reader.onload = function (e) {
             
           var imageUrl = e.target.result;
            imgProfile.style.backgroundImage = "url('" + imageUrl + "')";
            imgProfile.style.backgroundPosition = "top center";
            imgProfile.style.backgroundSize = "cover";
            imgProfile.style.backgroundRepeat = "no-repeat";
            addImgBtn.classList.add('close')
            btnRemove.classList.add('open')
        };
        reader.onerror = function(e) {
          console.error("Error reading file:", e.target.error);
        };
        var check = 1;
        const remove = document.querySelectorAll('.remove')
        remove.forEach((rm)=>{
          if(rm.classList.contains('open')){
            ++check;
          }
        })
        console.log(check)
        if(check === 5 || check === 6){
            nextBtn.style.pointerEvents = ''
            saveBtn.style.pointerEvents = ''
        }
    }
  })
}



// pages 2
const list_btns = document.querySelectorAll('.list_place .place_item button');

list_btns.forEach(function (item) {
    item.addEventListener('click', () => {
        var selected_btns = document.querySelectorAll('.list_place .place_item .selected');

        // Lặp qua từng phần tử đã chọn và loại bỏ class 'selected'

        selected_btns.forEach(function (btn) {

            btn.classList.remove('selected');

        });
        item.classList.add("selected");
    })
})

// pages 3

document.querySelectorAll('.list_type_of .type_of_item button').forEach(function (item) {
    item.addEventListener('click', () => {
        var selected_btns = document.querySelectorAll('.list_type_of .type_of_item button');

        // Lặp qua từng phần tử đã chọn và loại bỏ class 'selected'

        selected_btns.forEach(function (btn) {

            btn.classList.remove('selected');

        });
        item.classList.add("selected");
    })
})

// pages 9

document.querySelectorAll('.confirm_list .confirm_item button').forEach(function (item) {
    item.addEventListener('click', () => {
        var selected_btns = document.querySelectorAll('.confirm_list .confirm_item button');

        // Lặp qua từng phần tử đã chọn và loại bỏ class 'selected'

        selected_btns.forEach(function (btn) {

            btn.classList.remove('selected');

        });
        item.classList.add("selected");
    })
})



