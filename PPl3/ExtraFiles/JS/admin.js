let sidebar = document.querySelector(".sidebar");
  let closeBtn = document.querySelector("#btn");
  let searchBtn = document.querySelector(".bx-search");
let data_statics = []; 
let current_id = -1;
  closeBtn.addEventListener("click", ()=>{
    sidebar.classList.toggle("open");
    menuBtnChange();//calling the function(optional)
  });

  searchBtn.addEventListener("click", ()=>{ // Sidebar open when you click on the search iocn
    sidebar.classList.toggle("open");
    menuBtnChange(); //calling the function(optional)
  });

  // following are the code to change sidebar button(optional)
  function menuBtnChange() {
   if(sidebar.classList.contains("open")){
     closeBtn.classList.replace("bx-menu", "bx-menu-alt-right");//replacing the iocns class
   }else {
     closeBtn.classList.replace("bx-menu-alt-right","bx-menu");//replacing the iocns class
   }
  }

  //Circle
  function setCircleProgress(percentage , check) {
      if (check == 1) {
          const circles = document.querySelectorAll('.circle-progress');
          circles.forEach((circle, index) => {
              if (index % 2 != 0) {
                  const radius = circle.r.baseVal.value;
                  const circumference = 2 * Math.PI * radius;
                  const offset = circumference - (percentage / 100) * circumference;

                  circle.style.strokeDasharray = `${circumference} ${circumference}`;
                  circle.style.strokeDashoffset = offset;
              }

          })
      }
      else {
          const circles = document.querySelectorAll('.circle-progress');
          circles.forEach((circle, index) => {
              if (index % 2 == 0) {
                  const radius = circle.r.baseVal.value;
                  const circumference = 2 * Math.PI * radius;
                  const offset = circumference - (percentage / 100) * circumference;

                  circle.style.strokeDasharray = `${circumference} ${circumference}`;
                  circle.style.strokeDashoffset = offset;
              }

          })
      }


      if (check == 1) {
          var circleTexts = document.querySelectorAll('#circleText');

          circleTexts.forEach((item, index) => {
              if (index % 2 != 0) {
                  item.textContent = `${percentage}%`;
              }

          })
      }
      else {
          var circleTexts = document.querySelectorAll('#circleText');
          circleTexts.forEach((item, index) => {
              if (index % 2 == 0) {
                  item.textContent = `${percentage}%`;
              }

          })
      }
}
//Upload %
function animateCircleProgress(targetPercentage_host, targetPercentage_hotel, duration) {
    let start_host = 0;
    const increment_host = targetPercentage_host / (duration / 10); // Tính toán gia tăng mỗi 10ms

    const interval_host = setInterval(() => {
        start_host += increment_host;
        if (start_host >= targetPercentage_host) {
            start_host = targetPercentage_host;
            clearInterval(interval_host);
        }
        setCircleProgress(Math.round(start_host), 1);
    }, 10); // Cập nhật mỗi 10ms

    let start_hotel = 0;
    const increment_hotel = targetPercentage_hotel / (duration / 10); // Tính toán gia tăng mỗi 10ms

    const interval_hotel = setInterval(() => {
        start_hotel += increment_hotel;
        if (start_hotel >= targetPercentage_hotel) {
            start_hotel = targetPercentage_hotel;
            clearInterval(interval_hotel);
        }
        setCircleProgress(Math.round(start_hotel), 2);
    }, 10); // Cập nhật mỗi 10ms
}

// Example: Set progress to 75%


//Change Page
var homepage = document.getElementById('home');
var centorshipPage = document.getElementById('centorship');
var page_btns = document.querySelectorAll('.nav-list li>a')
var homeSection = document.querySelector('.home-section')
page_btns.forEach((btn) =>{
  btn.addEventListener('click', ()=>{
    var namePage = btn.querySelector('span').textContent.toLowerCase();
    var pageShow = homeSection.querySelector('.active');
    pageShow.classList.remove('active')
    var page = document.getElementById(namePage);
    page.classList.add('active');
  })
})

//Xử lý button detail infor

var btnDetailsHost = document.querySelectorAll('.details_host')
var inforHostDetail = document.querySelector('.info_host_detail')
var containerInforHost = document.querySelector('.container_info_host')
console.log(btnDetailsHost);
btnDetailsHost.forEach((btn) =>{
  btn.addEventListener('click', ()=>{
    containerInforHost.classList.remove('animationInfor')
    inforHostDetail.classList.add('openHost');
  })
})

if (inforHostDetail != null) {
    inforHostDetail.addEventListener('click', () => {
        if (inforHostDetail.classList.contains('openHost')) {
            containerInforHost.classList.add('animationInfor')
            setTimeout(() => {
                inforHostDetail.classList.remove('openHost');
            }, 1100);
        }
    })
}

if (containerInforHost != null) {
    containerInforHost.addEventListener('click', (e) => {
        e.stopPropagation();
    }) 
}


var btnDetailHotel = document.querySelectorAll('.details_hotel')
var inforHotelDetail = document.querySelector('.info_hotel_detail')
var containerInforHotel = document.querySelector('.container_info_hotel')
console.log(btnDetailsHost);
btnDetailHotel.forEach((btn) =>{
  btn.addEventListener('click', ()=>{
    containerInforHotel.classList.remove('animationInfor')
    inforHotelDetail.classList.add('openHost');
  })
})

if (inforHotelDetail != null) {
    inforHotelDetail.addEventListener('click', () => {
        if (inforHotelDetail.classList.contains('openHost')) {
            containerInforHotel.classList.add('animationInfor')
            setTimeout(() => {
                inforHotelDetail.classList.remove('openHost');
            }, 1100);
        }
    })
}

if (containerInforHotel != null) {
    containerInforHotel.addEventListener('click', (e) => {
        e.stopPropagation();
    })
}


//Xử lý ảnh hotel

const imgHotels = document.querySelectorAll('.img_hotel img');
const imgStyle = document.getElementById('img-style');
imgHotels.forEach((img, index) => {
  if(index !== imgHotels.length - 1){
    img.addEventListener('click', () => {
      const translateYValue = -150;
      const dynamicKeyframes = `
        @keyframes slideUpRotate {
          0% {
            transform: translateY(0) rotateX(0);
          }
          25% {
            transform: translateY(${translateYValue}px) rotateX(0);
          }
          50% {
            transform: translateY(${translateYValue}px) rotateX(30deg);
          }
          75% {
            transform: translateY(${translateYValue + index * 20}px) rotateX(30deg);
          }
          100% {
            transform: translateY(0) rotateX(0);
          }
        }
      `;
      imgStyle.textContent = dynamicKeyframes;
      img.style.animation = 'slideUpRotate 2s ease-in-out infinite';
      setTimeout(()=>{
        if(img.classList.contains('zIndex')){
          img.classList.remove('zIndex')
          imgHotels.forEach((img) =>{
            img.style.pointerEvents = ''
          })
        }
        else{
          img.classList.add('zIndex')
          imgHotels.forEach((img) =>{
            if(img.classList.contains('zIndex')){

            }else{
              img.style.pointerEvents = 'none'
            }
          })
        }
      }, 1000)
      setTimeout(() => {
        img.style.animation = 'none';
      }, 2000);
    });
    
  }else{
    img.addEventListener('click', () =>{
      if(img.classList.contains('zoomWidth')){
        img.classList.remove('zoomWidth')
      }
      else{
        img.classList.add('zoomWidth')
      }
    })
  }
  img.addEventListener('mouseover', () => {
    const translateYValue = -(10); // Dịch chuyển lên trên theo index * 40
    img.style.transform = `translateY(${translateYValue}px)`;
  });

  img.addEventListener('mouseout', () => {
    img.style.transform = `translateY(0px)`; // Trở lại vị trí ban đầu
  });
});
const btnZoom = document.querySelector('.btn_zoom button')
const imgZoomHotel = document.querySelector('.img_zoom_hotel')
const imgZoom = document.querySelectorAll('.list_img img')

if (btnZoom != null) {
    btnZoom.addEventListener('click', () => {
        imgZoomHotel.classList.add('openImg')
    })
}

const btnLeft = document.querySelector('.btn_left')
const btnRight = document.querySelector('.btn_right')
const btnClose = document.querySelector('.btn_close')

if (btnClose != null) {
    btnClose.addEventListener('click', () => {
        imgZoomHotel.classList.remove('openImg')
    })
}
if (btnLeft != null) {
    btnLeft.style.pointerEvents = 'none'
}

var current = 0;
if (btnRight) {
    btnRight.addEventListener('click', () => {
        btnLeft.style.pointerEvents = ''
        if (current === imgZoom.length - 1) {
            btnRight.style.pointerEvents = 'none'
        }
        else {
            current++;
            imgZoom.forEach((img) => {
                img.style.transform = `translateX(${710 * -1 * current}px)`
            })
        }
    })
}

if (btnLeft) {
    btnLeft.addEventListener('click', () => {
        btnRight.style.pointerEvents = ''
        if (current === 0) {
            btnLeft.style.pointerEvents = 'none'
        }
        else {
            current--;
            imgZoom.forEach((img) => {
                img.style.transform = `translateX(${710 * -1 * current}px)`
            })
        }
    })

}

const btnAlls = document.querySelectorAll('.list_item table tbody tr td:first-child button');

btnAlls.forEach((btn) =>{
  btn.addEventListener('click', ()=>{
    if(btn.classList.contains('ticked')){
      btn.classList.remove('ticked')
    }else{
      btn.classList.add('ticked')
    }
  })
})


//Trang Analytics




function updateChartData(newData, type) {
    if (type == 1) {
        // Cập nhật dữ liệu của biểu đồ
        hotel_chart.data.datasets[0].data = newData;

        // Gọi phương thức update() để cập nhật biểu đồ
        hotel_chart.update();
    }
    else if (type == 2) {
        // Cập nhật dữ liệu của biểu đồ
        admin_month_revenue.data.datasets[0].data = newData;

        // Gọi phương thức update() để cập nhật biểu đồ
        admin_month_revenue.update();
    }
    else if (type == 3) {
        // Cập nhật dữ liệu của biểu đồ
        admin_year_revenue.data.datasets[0].data = newData;

        // Gọi phương thức update() để cập nhật biểu đồ
        admin_year_revenue.update();
    }
    
}

const hotels = document.querySelectorAll('.item_7 table tbody tr')
for(let i = 0; i < hotels.length; i++){
  hotels[i].addEventListener('click', () =>{
    var name = hotels[i].firstElementChild.textContent;
    const nameHotel = document.querySelector('.name_hotel_analytics span')
      nameHotel.textContent = name;
      current_id = hotels[i].classList.value.split(" ")[0].split("_")[1];
      HotelRevenueStatistics(current_id);
      
     
  })
}


document.querySelector('#by_year_hotel').addEventListener('change', function () {
    // Lấy giá trị được chọn từ thẻ select
    const selectedValue = this.value;

    // Lấy dữ liệu mới dựa trên giá trị được chọn
    if (current_id != -1) {
        HotelRevenueByYear(selectedValue, current_id);
    }
    

});



document.querySelector('#by_year_admin').addEventListener('change', function () {
    // Lấy giá trị được chọn từ thẻ select
    const selectedValue = this.value;
    console.log(1);
    // Lấy dữ liệu mới dựa trên giá trị được chọn
    AdminRevenueByYear(selectedValue);


});

document.querySelector('.btn_search').addEventListener('click', function () {
    SearchHotel();
})

const selectElement = document.getElementById('search_by_revenue');
const btnRevenue = document.querySelector('.btn_revenue')
const revenues = document.querySelectorAll('.item_7 table tbody tr td:last-child')

var revenuesArr = [];

for(let i = 0; i < revenues.length; i++){
  var money = revenues[i].textContent.slice(0, -1);
  revenuesArr.push(money, i)
}

btnRevenue.addEventListener('click', () =>{
  var index = selectElement.selectedIndex;
  hotels.forEach((ht) =>{
    ht.classList.remove('close')
  })
  if(index === 1){
    hotels.forEach((ht) =>{
      ht.classList.add('close')
    })
    for(let j = 0; j < revenuesArr.length; j++){
      if(j % 2 === 0){
        if(revenuesArr[j] < 10000){
          var row = revenuesArr[j + 1];
          hotels[row].classList.remove('close')
        }
      }
    }
  }
  else if(index === 2){
    hotels.forEach((ht) =>{
      ht.classList.add('close')
    })
    for(let j = 0; j < revenuesArr.length; j++){
      if(j % 2 === 0){
        if(revenuesArr[j] >= 10000 && revenuesArr[j] < 30000){
          var row = revenuesArr[j + 1];
          hotels[row].classList.remove('close')
        }
      }
    }
  }
  else if(index === 3){
    hotels.forEach((ht) =>{
      ht.classList.add('close')
    })
    for(let j = 0; j < revenuesArr.length; j++){
      if(j % 2 === 0){
        if(revenuesArr[j] >= 30000 && revenuesArr[j] < 50000){
          var row = revenuesArr[j + 1];
          hotels[row].classList.remove('close')
        }
      }
    }
  }
  else if(index === 4){
    hotels.forEach((ht) =>{
      ht.classList.add('close')
    })
    for(let j = 0; j < revenuesArr.length; j++){
      if(j % 2 === 0){
        if(revenuesArr[j] >= 50000){
          var row = revenuesArr[j + 1];
          hotels[row].classList.remove('close')
        }
      }
    }
  }
});

const selectElementArrange = document.getElementById('arrange');
const btnArrange = document.querySelector('.btn_arrange')
var arrArrange = [];

btnArrange.addEventListener('click', () =>{
  hotels.forEach((ht, index) =>{
    if(ht.className === ''){
        var revenue = ht.lastElementChild;
        var money = revenue.textContent.slice(0, -1);
        arrArrange.push(money, index)
    }
  })
  
  console.log(arrArrange)
  var index = selectElementArrange.selectedIndex;
  if(index === 1){
    // Lấy danh sách các thẻ <tr> từ bảng
    const tableBody = document.querySelector('.list_item.item_7 table tbody');
    const rows = Array.from(tableBody.querySelectorAll('tr'));

    // Sắp xếp mảng các thẻ <tr> dựa trên doanh thu (revenue)
    rows.sort((a, b) => {
        const revenueA = parseInt(a.querySelector('td:nth-child(4)').textContent.replace('$', '').replace(',', ''));
        const revenueB = parseInt(b.querySelector('td:nth-child(4)').textContent.replace('$', '').replace(',', ''));
        return revenueA - revenueB;
    });

    // Xóa tất cả các thẻ <tr> hiện có từ bảng
    tableBody.innerHTML = '';

    // Thêm các thẻ <tr> đã sắp xếp vào bảng
    rows.forEach(row => {
        tableBody.appendChild(row);
    });
  }
  else if(index === 2){
    // Lấy danh sách các thẻ <tr> từ bảng
    const tableBody = document.querySelector('.list_item.item_7 table tbody');
    const rows = Array.from(tableBody.querySelectorAll('tr'));

    // Sắp xếp mảng các thẻ <tr> dựa trên doanh thu (revenue)
    rows.sort((a, b) => {
        const revenueA = parseInt(a.querySelector('td:nth-child(4)').textContent.replace('$', '').replace(',', ''));
        const revenueB = parseInt(b.querySelector('td:nth-child(4)').textContent.replace('$', '').replace(',', ''));
        return revenueB - revenueA;
    });

    // Xóa tất cả các thẻ <tr> hiện có từ bảng
    tableBody.innerHTML = '';

    // Thêm các thẻ <tr> đã sắp xếp vào bảng
    rows.forEach(row => {
        tableBody.appendChild(row);
    });
  }
  else if(index === 3){
    // Lấy danh sách các thẻ <tr> từ bảng
    const tableBody = document.querySelector('.list_item.item_7 table tbody');
    const rows = Array.from(tableBody.querySelectorAll('tr'));

    // Sắp xếp mảng các thẻ <tr> dựa trên tên khách sạn (theo bảng chữ cái A-Z)
    rows.sort((a, b) => {
        const nameA = a.querySelector('td:nth-child(1)').textContent.trim().toLowerCase();
        const nameB = b.querySelector('td:nth-child(1)').textContent.trim().toLowerCase();
        return nameA.localeCompare(nameB);
    });

    // Xóa tất cả các thẻ <tr> hiện có từ bảng
    tableBody.innerHTML = '';

    // Thêm các thẻ <tr> đã sắp xếp vào bảng
    rows.forEach(row => {
        tableBody.appendChild(row);
    });

  }
  
})
