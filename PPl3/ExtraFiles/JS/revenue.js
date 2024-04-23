const peopleNumber = document.querySelector('.people')
const reviewNumber = document.querySelector('.review')
const earningNumber = document.querySelector('.earning')
// Số đã chọn
const targetNumber = parseFloat(peopleNumber.textContent, 10);
const targetNumber1 = Number.parseInt(reviewNumber.textContent, 10);
const targetNumber2 = parseFloat(earningNumber.textContent);
console.log(targetNumber2)
// Thời gian delay giữa các bước
const delay = 8; // milliseconds
// Số bước để đạt được số đã chọn
const steps = 150;
const steps1 = 150;
const steps2 = 150;
// Bước tăng giá trị sau mỗi lần cập nhật
const stepValue = targetNumber / steps;
const stepValue1 = targetNumber1 / steps1;
const stepValue2 = targetNumber2 / steps2;
console.log(stepValue2)
// Lấy phần tử div hiển thị số
// Biến đếm hiện tại
var currentNumber = 0;
var currentNumber1 = 0;
var currentNumber2 = 0;
// Hàm cập nhật số
function updateCount() {
    // Tăng giá trị hiện tại
    currentNumber += stepValue;
    // Đảm bảo không vượt quá giá trị đã chọn
    currentNumber = Math.min(currentNumber, targetNumber);
    // Hiển thị giá trị hiện tại
    peopleNumber.textContent = currentNumber.toFixed(3);
    // Kiểm tra nếu đã đạt được số đã chọn
    if (currentNumber >= targetNumber) {
        clearInterval(interval);
    }
}

function updateCount1() {
    // Tăng giá trị hiện tại
    currentNumber1 += stepValue1;
    // Đảm bảo không vượt quá giá trị đã chọn
    currentNumber1 = Math.min(currentNumber1, targetNumber1);
    // Hiển thị giá trị hiện tại
    reviewNumber.textContent = Math.round(currentNumber1);
    // Kiểm tra nếu đã đạt được số đã chọn
    if (currentNumber1 >= targetNumber1) {
        clearInterval(interval1);
    }
}

function updateCount2() {
    // Tăng giá trị hiện tại
    currentNumber2 += stepValue2;
    // Đảm bảo không vượt quá giá trị đã chọn
    currentNumber2 = Math.min(currentNumber2.toFixed(3), targetNumber2);
    // Hiển thị giá trị hiện tại
    earningNumber.textContent = currentNumber2.toFixed(3)
    // Kiểm tra nếu đã đạt được số đã chọn
    if (currentNumber2 >= targetNumber2) {
        clearInterval(interval2);
    }
}
// Bắt đầu cập nhật số sau mỗi khoảng thời gian
const interval = setInterval(updateCount, delay);
const interval1 = setInterval(updateCount1, delay);
const interval2 = setInterval(updateCount2, delay);
