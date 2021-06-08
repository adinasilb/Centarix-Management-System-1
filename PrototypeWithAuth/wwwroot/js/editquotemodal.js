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
    console.log($(this).attr("checked"))
    $(this).attr("checked", !$(this).prop("checked"));
    $(this).val($(this).prop("checked"))
})

$("#currency").change(function (e) {
    var currencyType = $("#currency").val();
    switch (currencyType) {
        case "USD":
            $('.shekel-group').addClass('d-none');
            $('.dollar-group').removeClass('d-none');
            break;
        case "NIS":
            $('.shekel-group').removeClass('d-none');
            $('.dollar-group').addClass('d-none');
            break;
    }
});
$('#sum-dollars').change(function (e) {
    var exchangeRate = $('#exchangeRate').val();
    //console.log(exchangeRate);
    var shekelPrice = $('#sum-dollars').val() * exchangeRate;
    //console.log($('#sum-dollars').val());
    console.log('shekel price: ' + shekelPrice)
    $('#cost').val(shekelPrice);
    //console.log('value: ' + $('#cost').val());
});