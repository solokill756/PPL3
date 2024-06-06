
const img = document.querySelectorAll('.list_img li')
const summit_btn = document.querySelector('.Summit_btn');
const skip_btn = document.querySelector('.Skip_btn');
const Comments = document.getElementById('comments');
summit_btn.disabled = false;


for(let i = 1; i <= img.length; i++){
    const imgBr = document.querySelector('.img_' + i)
    const fileUpLoad = document.querySelector('.file_' + i);
    const btn = document.querySelector('.btn_' + i)
    const xbtn = document.querySelector('.remove_' + i)
    imgBr.addEventListener('click', ()=>{
        fileUpLoad.click();
    })
    xbtn.addEventListener('click', (e)=>{
        e.stopPropagation();
        imgBr.style.backgroundImage = "";
        imgBr.style.backgroundPosition = "";
        imgBr.style.backgroundSize = "";
        imgBr.style.backgroundRepeat = "";
        imgBr.style.objectFit = '';
        btn.classList.remove('close')
        xbtn.classList.remove('open')
    })
    fileUpLoad.addEventListener('change', () =>{
        var file = fileUpLoad.files[0];
        if (file) {
            var reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = function(e) {
                var imageUrl = e.target.result;
                imgBr.style.backgroundImage = "url('" + imageUrl + "')";
                imgBr.style.backgroundPosition = "top center";
                imgBr.style.backgroundSize = "cover";
                imgBr.style.backgroundRepeat = "no-repeat";
                imgBr.style.objectFit = 'fill';
                btn.classList.add('close')
                xbtn.classList.add('open')
            };
            reader.onerror = function(e) {
              console.error("Error reading file:", e.target.error);
            };
        }
    })

    Comments.addEventListener('change', () => {
        if (Comments.textContent.trim() != "") {
            summit_btn.disabled = true;
            summit_btn.style.cursor = "pointer";
        } 
    })
}

const star = document.querySelectorAll('.list_star i')

for(let i = 0; i < star.length; i++){
    star[i].addEventListener('click', () => {
        summit_btn.disabled = false;
        summit_btn.style.cursor = "pointer";
        star.forEach((str) =>{
            str.classList.replace('fa-solid', 'fa-regular')
            str.style.color = ''
            str.classList.remove("select");
            
        })
        for(let j = 0; j <= i; j++){
            star[j].classList.replace('fa-regular', 'fa-solid')
            star[j].style.color = '#ffbf00'
            star[j].classList.add("select");
        }
    })
}