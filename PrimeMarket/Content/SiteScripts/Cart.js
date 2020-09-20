

$(document).ready(function () {
    
    $('.txt_Quantity_Details').change(function () {
        var txtQuantity = $(this);
        var Quantity = txtQuantity.val().trim();
        // alert(Quantity);
        if ($.isNumeric(Quantity)) {
            if (Quantity > 0) {
                var StockAmount = txtQuantity.parent().find('#StockAmount').val();
                StockAmount = StockAmount - 1;
                StockAmount = StockAmount + 1;
                if (Quantity > StockAmount) {
                    alert('الكمية المطلوبة غير متاحة');
                    return;
                }
            }
            else {
                alert("أدخل كمية صحيحة");
            }
        }
        else {
            alert("أدخل كمية صحيحة");
        }
        
    });
    //--------------------------------------------
    $('.txt_Quantity').change(function () {
        var txtQuantity = $(this);
        var Quantity = txtQuantity.val().trim();
        // alert(Quantity);
        if ($.isNumeric(Quantity)) {
            if (Quantity > 0) {
                var StockAmount = txtQuantity.parent().find('#StockAmount').val();
                StockAmount = StockAmount - 1;
                StockAmount = StockAmount + 1;
                if (Quantity > StockAmount) {
                    alert('الكمية المطلوبة غير متاحة');
                    return;
                }
                txtQuantity.closest("form").submit();
            }
            else
            {
                alert("أدخل كمية صحيحة");
            }
        }
        else
        {
            alert("أدخل كمية صحيحة");
        }
        
        
        
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
    
        
});