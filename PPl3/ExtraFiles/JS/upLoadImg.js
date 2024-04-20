const fileUpLoad = document.getElementById('file_upload')
const addImgBtn = document.querySelector('.add_img_btn')
const imgProfile = document.querySelector('.img_profile')
addImgBtn.addEventListener('click', () => {
    fileUpLoad.click();
})

fileUpLoad.addEventListener('change', () => {
    var file = fileUpLoad.files[0];
    if (file) {
        var reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = function (e) {
            var imageUrl = e.target.result;
            var imageUrlString = imageUrl.toString();
            imgProfile.style.backgroundImage = "url('" + imageUrl + "')";
            imgProfile.style.backgroundPosition = "top center";
            imgProfile.style.backgroundSize = "cover";
            imgProfile.style.backgroundRepeat = "no-repeat";
            $.ajax({
                url: '/user/homeuser/addUserAvatar',
                type: 'POST',
                dataType: 'json',
                data: { image: imageUrlString },
                success: function (response) {
                    console.log(response);
                },
                error: function (xhr, status, error) {
                    console.error(error);
                }
            });
        };
        reader.onerror = function (e) {
            console.error("Error reading file:", e.target.error);
        };
    }
})

