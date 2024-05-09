$(document).ready(function() {


    $('.profile_seller').click(function() {

        var userId = $(this).data('userid');



        $.ajax({
            url: '@Url.Action("FindHost", "Home")',

            type: 'GET',

            data: { userId: userId },

            success: function(data) {

                $('#seller_profile').html(data);

            }
        });

    });

});
