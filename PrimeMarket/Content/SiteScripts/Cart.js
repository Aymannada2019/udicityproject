

$(document).ready(function () {

    $('#txt_Quantity').change(function () {
        alert('v');
        var v = $('#txt_Quantity').value;
        alert(v);
    //    $.ajax({
    //    url: 'https://api.joind.in/v2.1/talks/10889',
    //    data: {
    //        format: 'json'
    //    },
    //    error: function () {
    //        $('#info').html('<p>An error has occurred</p>');
    //    },
    //    dataType: 'jsonp',
    //    success: function (data) {
    //        var $title = $('<h1>').text(data.talks[0].talk_title);
    //        var $description = $('<p>').text(data.talks[0].talk_description);
    //        $('#info')
    //            .append($title)
    //            .append($description);
    //    },
    //    type: 'GET'
    //});
    });
    
    
    var itemClose = $('.shoping__cart__item__close');
    itemClose.on('click', '.icon_close', function () {
        var closebutton = $(this);
        var CartId = closebutton.parent().find('#hdn_CartId').val();
        // ---- call ajax to delete this item from cart -----------
        alert('CartId=' + CartId);
    });




});