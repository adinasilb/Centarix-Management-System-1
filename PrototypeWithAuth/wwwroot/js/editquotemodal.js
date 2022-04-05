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
        $(this).prop('disabled', true);
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
                $(this).prop('disabled', false);
                $.fn.CloseModal('edit-quote');
                $('._IndexTableDataByVendor').html(data);
            },
            error: function (jqxhr) {
                //$(this).prop('disabled', false);
                $.fn.OpenModal('modal', 'edit-quote', jqxhr.responseText)
                $('.mdb-select').materialSelect();
            }
        });
    }
    $("#myForm").data("validator").settings.ignore = ':not(select:hidden, .location-error:hidden,input:visible, textarea:visible)';

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
    $.fn.CheckForVendorCurrencyWarning($("#VendorCurrencyID").val(), currencyType);
});

$('.sum-dollars').change(function (e) {
    var index = $(this).attr("index");
    var costDollars = $('.sum-dollars.' + index).val()
    var vatClass = '.vat-dollar.' + index;
    $.fn.CalculateShekelPrice(index, costDollars);
    $.fn.CalculatePriceWithVAT('.price-with-vat-dollar.' + index, costDollars, vatClass);
    $.fn.ClearDiscountAndUpdateSums('.sum-dollars')
});
$.fn.CalculatePriceWithVAT = function (totalPriceClass, cost, vatClass) {
    var totalPrice = cost;
    console.log($('.include-vat').attr('value'));
    if ($('.include-vat').attr('value').toLowerCase() === 'true') {
        console.log('in if');
        vat = cost * .17;
        totalPrice = Number(cost) + vat;
    }
    console.log(totalPrice)
    console.log(vat)
    $(totalPriceClass).val(Number(totalPrice).toFixed(2));
    if ($(vatClass)) {
        $(vatClass).val(Number(vat).toFixed(2));
    }
}
$.fn.ClearDiscountAndUpdateSums = function(totalPriceClass) {
    if ($('.add-invoice').length) {
        $('#Invoice_InvoiceDiscount').val(0)
        $('.discount-amount').val(0)
        console.log('clear discount')
        var totalPrice = 0;
        $(totalPriceClass).each(function (index, element) {
            totalPrice += (Number($(element).val()))
        })
        $('.total-cost').val(totalPrice)
        $('.original-cost').val(totalPrice)
        $.fn.UpdateVATAndTotal()
    }
}
$('.cost').change(function (e) {
    //console.log('shekel cost change function');
    var index= $(this).attr("index")
    $.fn.CalculatePriceWithVAT('.price-with-vat-shekel.' + index, $(this).val(), '.vat-shekel.' + index)
    $.fn.ClearDiscountAndUpdateSums('.cost');
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
    console.log('cost dollar ' + costDollars);
    console.log('exchange rate ' + exchangeRate)
    var shekelPrice = costDollars * exchangeRate;
    console.log('shekel price: ' + shekelPrice)
    $('.cost.' + index).val(shekelPrice);
}
$.fn.UpdateVATAndTotal = function () {
    if ($('.include-vat').val() === 'True') {
        console.log('In if')
        var vat = $('.total-cost').val() * .17
        $('.total-vat').val(vat.toFixed(2))
    } //else stays 0
    var grandTotal = parseFloat($('.total-cost').val()) + parseFloat($('.total-vat').val()) + parseFloat($('.total-shipping').val()) + parseFloat($('.shipping-vat').val())
    console.log('total: ' + grandTotal)
    $('.grand-total').val(grandTotal.toFixed(2))

}