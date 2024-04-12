let calendar = document.querySelector('.calendar')
let checkIn = document.querySelector('.check_in span')
let checkOut = document.querySelector('.check_out span')
let currentDay = 0;
const month_names = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December']

isLeapYear = (year) => {
    return (year % 4 === 0 && year % 100 !== 0) || (year % 400 === 0)
}

getFebDays = (year) => {
    return isLeapYear(year) ? 29 : 28
}

generateCalendar = (month, year) => {

    let calendar_days = calendar.querySelector('.calendar-days')
    let calendar_header_year = calendar.querySelector('#year')

    let days_of_month = [31, getFebDays(year), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]

    calendar_days.innerHTML = ''

    let currDate = new Date()
    if (!year) year = currDate.getFullYear()

    let curr_month = `${month_names[month]}`
    month_picker.innerHTML = curr_month
    calendar_header_year.innerHTML = year

    // get first day of month
    let first_day = new Date(year, month, 1)

    let dayCheckIn = 0;
    let monthCheckIn = 0;
    let yearCheckIn = 0;
    let dayCheckOut = 0;
    let monthCheckOut = 0;
    let yearCheckOut = 0;
    for (let i = 0; i <= days_of_month[month] + first_day.getDay() - 1; i++) {
        let day = document.createElement('div')
        if (i >= first_day.getDay()) {
            day.classList.add('calendar-day-hover')
            day.innerHTML = i - first_day.getDay() + 1
            day.innerHTML += `<span></span>
                            <span></span>
                            <span></span>
                            <span></span>`
            day.addEventListener('click', () => {
                currentDay++;
                console.log(currentDay)
                if (currentDay <= 2) {
                    if (currentDay == 1) {
                        day.classList.add('curr-date')
                        dayCheckIn = Number.parseInt(day.outerText);
                        monthCheckIn = month + 1;
                        yearCheckIn = year;
                        checkIn.innerHTML = `${day.outerText}/${month + 1}/${year}`
                        let daysBeforeCheckIn = calendar.querySelectorAll('.calendar-days div');
                        for (let j = 0; j < i; j++) {
                            daysBeforeCheckIn[j].classList.add('before-check-in');
                        }
                        for (let j = i + 10; j <= days_of_month[month] + first_day.getDay() - 1; j++) {
                            daysBeforeCheckIn[j].classList.add('before-check-in');
                        }

                    } else if (currentDay == 2) {
                        dayCheckOut = Number.parseInt(day.outerText);
                        monthCheckOut = month + 1;
                        yearCheckOut = year;
                        if ((yearCheckIn <= yearCheckOut && monthCheckIn < monthCheckOut) || (yearCheckIn <= yearCheckOut && monthCheckIn == monthCheckOut && dayCheckIn < dayCheckOut) && (yearCheckIn <= yearCheckOut && monthCheckIn == monthCheckOut && (dayCheckOut - dayCheckIn) <= 10)) {
                            day.classList.add('curr-date')
                            checkOut.innerHTML = `${day.outerText}/${month + 1}/${year}`
                        }
                        else {
                            currentDay = 1;
                        }
                    }
                } else {
                    calendar.querySelectorAll('.calendar-days div').forEach(day => {
                        day.classList.remove('curr-date'); // Xóa lớp 'curr-date' cho tất cả các ngày
                    });
                    let daysBeforeCheckIn = calendar.querySelectorAll('.calendar-days div');
                    for (let j = 0; j <= days_of_month[month] + first_day.getDay() - 1; j++) {
                        daysBeforeCheckIn[j].classList.remove('before-check-in');
                    }
                    currentDay = 0;
                    checkOut.innerHTML = 'Add date'
                }
            })
        }
        calendar_days.appendChild(day)
    }
    let clearDate = document.querySelector('.clear_date_btn')

    clearDate.addEventListener('click', () => {
        calendar.querySelectorAll('.calendar-days div').forEach(day => {
            day.classList.remove('curr-date'); // Xóa lớp 'curr-date' cho tất cả các ngày
        });
        let daysBeforeCheckIn = calendar.querySelectorAll('.calendar-days div');
        for (let j = 0; j <= days_of_month[month] + first_day.getDay() - 1; j++) {
            daysBeforeCheckIn[j].classList.remove('before-check-in');
        }
        currentDay = 0;
        checkOut.innerHTML = 'Add date'
    })
}

let month_list = calendar.querySelector('.month-list')

month_names.forEach((e, index) => {
    let month = document.createElement('div')
    month.innerHTML = `<div data-month="${index}">${e}</div>`
    month.querySelector('div').onclick = () => {
        month_list.classList.remove('show')
        curr_month.value = index; // Cập nhật giá trị của curr_month.value
        generateCalendar(index, curr_year.value)
    }
    month_list.appendChild(month)
})

let month_picker = calendar.querySelector('#month-picker')

month_picker.onclick = () => {
    month_list.classList.add('show')
}

let currDate = new Date()

let curr_month = { value: currDate.getMonth() }
let curr_year = { value: currDate.getFullYear() }
generateCalendar(curr_month.value, curr_year.value)
let currTemp = 0;


document.querySelector('#prev-year').onclick = () => {
    // currentDay = 1;
    if (currTemp.value > currDate.getFullYear()) {
        --curr_year.value
    }
    generateCalendar(curr_month.value, curr_year.value)
}

document.querySelector('#next-year').onclick = () => {
    currentDay = 1;
    ++curr_year.value
    currTemp = curr_year
    generateCalendar(curr_month.value, curr_year.value)
}