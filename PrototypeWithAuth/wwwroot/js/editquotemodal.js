$(".save-quote-details").click(function (e) {
    e.preventDefault();
    $("#myForm").data("validator").settings.ignore = "";
    var valid = $("#myForm").valid();
    console.log("valid form: " + valid)
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
    $(this).data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible)';

})

$(".vat-check").click(function (e) {
    var checked = $(this).prop("checked");
    console.log(checked)
    $(this).attr("checked", !checked);
    $(this).val(checked);
    if ($('#currency').val() === "NIS") {
        $.fn.CalculatePriceWithVAT('.price-with-vat-shekel', $('#cost').val());
    } else {
        $.fn.CalculatePriceWithVAT('.price-with-vat-dollar', $('#sum-dollars').val());
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
$('#sum-dollars').change(function (e) {
    var exchangeRate = $('#exchangeRate').val();
    var costDollars = $('#sum-dollars').val()
    var shekelPrice = costDollars * exchangeRate;
    //console.log('shekel price: ' + shekelPrice)
    $('#cost').val(shekelPrice);
    $.fn.CalculatePriceWithVAT('.price-with-vat-dollar', costDollars);
});
$.fn.CalculatePriceWithVAT = function (totalPriceClass, cost) {
    var totalPrice = cost;
    //console.log($('.vat-check').attr('value'));
    if ($('.vat-check').attr('value') === 'true') {
        totalPrice = cost * 1.17;
    }
    console.log(totalPrice)
    $(totalPriceClass).val(totalPrice);
}
$('#cost').change(function (e) {
    $.fn.CalculatePriceWithVAT('.price-with-vat-shekel', $('#cost').val());
})