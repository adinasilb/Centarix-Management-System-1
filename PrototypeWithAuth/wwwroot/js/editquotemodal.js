$(".save-quote-details").click(function (e) {
    e.preventDefault();
    $("#myForm").data("validator").settings.ignore = "";
    var valid = $("#myForm").valid();
    console.log("valid form: " + valid)
    var formdata = new FormData($("#myForm")[0]);
    console.log(formdata);
    if (!valid) {

        if (!$('.activeSubmit').hasClass('disabled-submit')) {
            $('.activeSubmit').addClass('disabled-submit')
        }

    }
    else {
        $('.activeSubmit ').removeClass('disabled-submit')
        var formData = new FormData($("#myForm")[0]);
        $.ajax({
            processData: false,
            contentType: false,
            async: true,
            url: "/Requests/EditQuoteDetails",
            type: 'POST',
            data: formData,
            cache: false,
            success: function (data) {
                $.fn.CloseModal('edit-quote');
                $('._IndexTableDataByVendor').html(data);
            },
            error: function (jqxhr) {
                $.fn.OpenModal('modal', 'edit-quote', jqxhr.responseText)
                $('.mdb-select').materialSelect();
            }
        });
    }
    $("#myForm").data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible)';

})

$(".vat-check").click(function (e) {
    var checked = $(this).prop("checked");
    console.log(checked)
    $(this).attr("checked", !checked);
    $(this).val(checked);
    var classes;
    if ($('#currency').val() === "NIS") {
        $('.cost').each(function (index) {
            $.fn.CalculatePriceWithVAT('.price-with-vat-shekel.' + index, $('.cost.' + index).val());
        })
    } else {
        $('.sum-dollars').each(function (index) {
            $.fn.CalculatePriceWithVAT('.price-with-vat-dollar.' + index, $('.sum-dollars.' + index).val());
        })
    }
})

$("#currency").change(function (e) {
    var currencyType = $("#currency").val();
    switch (currencyType) {
        case "USD":
            $('.shekel-group').addClass('d-none');
            $('.dollar-group').removeClass('d-none');
            $('.dollar-group div input').each(function () {
                $(this).val(0);
            });
            break;
        case "NIS":
            $('.shekel-group').removeClass('d-none');
            $('.dollar-group').addClass('d-none');
            $('.shekel-group div input').each(function () {
                $(this).val(0);
            });
            break;
    }
});
$('.sum-dollars').change(function (e) {
    var index = $(this).attr("index");
    var costDollars = $('.sum-dollars.' + index).val()
    $.fn.CalculateShekelPrice(index, costDollars);
    $.fn.CalculatePriceWithVAT('.price-with-vat-dollar.' + index, costDollars);
});
$.fn.CalculatePriceWithVAT = function (totalPriceClass, cost) {
    var totalPrice = cost;
    console.log($('.include-vat').attr('value'));
    if ($('.include-vat').attr('value').toLowerCase() === 'true') {
        console.log('in if');
        totalPrice = cost * 1.17;
    }
    console.log(totalPrice)
    $(totalPriceClass).val(Number(totalPrice).toFixed(2));
}
$('.cost').change(function (e) {
    //console.log('shekel cost change function');
    $.fn.CalculatePriceWithVAT('.price-with-vat-shekel.' + $(this).attr("index"), $(this).val());
})
$('#exchangeRate').change(function (e) {
    if ($("#currency").val() === "USD") {
        $('.sum-dollars').each(function (index) {
            var costDollars = $('.sum-dollars.' + index).val();
            $.fn.CalculateShekelPrice(index, costDollars);
        })
    }
})

$.fn.CalculateShekelPrice = function (index, costDollars) {
    var exchangeRate = $('#exchangeRate').val();
    console.log(costDollars);
    var shekelPrice = costDollars * exchangeRate;
    console.log('shekel price: ' + shekelPrice)
    $('.cost.' + index).val(shekelPrice);
}
