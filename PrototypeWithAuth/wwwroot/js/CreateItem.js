$(function () {
    $("#submitCategory").on("click", function (e) {
        console.log("submit category")
        e.preventDefault();
        $("#myForm").data("validator").settings.ignore = "";
        var valid = $("#myForm").valid();
		console.log("valid form: " + valid)
        if (!valid) {

            e.stopPropagation();
            if (!$('input[type="submit"], button[type="submit"] ').hasClass('disabled-submit')) {
                $('input[type="submit"], button[type="submit"] ').addClass('disabled-submit')
            }

        }
		else {
            //var categoryId = $("#categorylist").val();
            $('input[type="submit"], button[type="submit"] ').removeClass('disabled-submit')
            $("#loading").show();
            var formData = new FormData($("#myForm")[0]);
            console.log(...formData)
			$.ajax({
				processData: false,
				contentType: false,
                async: true,
                url: "/Requests/CreateItemTabs",
                type: 'GET',
                data: formData,
				cache: false,
				success: function (data) {
                    $(".outer-partial").html(data);
                    $("#loading").hide();
                    var category = $("#categoryDescription").val();
                    $("." + category).removeClass("d-none");
				}
             
			})
        }
        $("#myForm").data("validator").settings.ignore = ':not(select:hidden, input:visible, textarea:visible)';

        return false;
    });
    $("#sublist").off("change").on("change", function (e) {
        console.log("subcategory change")
        var subcategoryID = $("#sublist").val()
        var pageType = $("#masterPageType").val()
        var itemName = $("#Request_Product_ProductName").val()
        if (subcategoryID != "") {
        $.ajax({
            //processData: true,
            //contentType: true,
            async: true,
            url: "/Requests/CreateItemTabs/?productSubCategoryId=" + subcategoryID + "&PageType=" + pageType,
            type: 'GET',
            cache: false,
            //data: formData,
            success: function (data) {
                $(".outer-partial").html(data);
                $("#loading").hide();
                var category = $("#categoryDescription").val();
                console.log("category "+ category)
                $("." + category).removeClass("d-none");
                $("." + category).prop("disabled", false);
                $.fn.DisableMaterialSelectWithOutMaterialSelect("#parentlist", 'select-options-parentlist');
                $.fn.DisableMaterialSelectWithOutMaterialSelect("#sublist", 'select-options-sublist');
                $(".proprietryHidenCategory").attr("disabled", false);
                $(".mdb-select").materialSelect();
            }
        })
    })
   
	$(".categoryForm").validate({
		rules: {
			"SelectedCategory": "required"
        }
    })
     $(".order-tab").on("click", function (e) {
        if ($(".all-mail").length == 0) {
            let data = [
                $("#vendor-primary-email").val()
            ]
            $("#allEmails").email_multiple({
                data: data
            })
            var newEmailLine = $(".enter-mail-id");
            newEmailLine.removeAttr('placeholder');
            newEmailLine.addClass('form-control-plaintext');
            newEmailLine.addClass('border-bottom');

            $(".email-ids:first span").attr("disabled", true);
            $(".email-ids:first span").css("display", "none");
            $(".email-ids:first").addClass("supplier-email");
        }
    });
    

    $.fn.UpdatePrimaryOrderEmail = function () {
        if ($(".supplier-email").length > 0) {
            $(".supplier-email").html($("#vendor-primary-email").val());
            $(".isSupplier").val($("#vendor-primary-email").val());
        }
        else {
            $('.all-mail').prepend('<span class="email-ids supplier-email">' + $("#vendor-primary-email").val() + '</span>');
            $(".isSupplier").val($("#vendor-primary-email").val());
        }
    };

    $.fn.CheckListLength = function () {
        //Disable if 5 emails
        var listlength = $(".email-ids").length;
        if (listlength >= 5 || (listlength == 4 && $(".supplier-email").length == 0)) {
            $(".enter-mail-id").attr("disabled", true);
        }
        else {
            $(".enter-mail-id").attr("disabled", false);
        }
    }

    $.fn.RemoveFromHiddenIds = function (emailValue) {
        $(".emailaddresses[value='" + emailValue + "']:first").val('');
    }

    $.fn.AddToHiddenIds = function (emailValue) {
        $(".emailaddresses[value='']:first").val(emailValue);
    }
})