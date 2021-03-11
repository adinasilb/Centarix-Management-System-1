$(function () {
	$(".form-check.accounting-select .form-check-input ").on("click", function (e) {
		 if (!$(this).is(':checked')) 
		 {
                 $(this).closest("tr").removeClass("clicked-border-acc");			
         }
		var activeVendor = $(".activeVendor").val();
		if(activeVendor == "" && $(this).is(":checked"))
		{
		//	alert("reset vendor")
			 $(".activeVendor").val($(this).attr("vendorid"))
		}
		var addToSelectedButton = $("#add-to-selected");
		var paySelectedButton = $("#pay-selected");
	

		var selectedButton;
		if (addToSelectedButton) {
			selectedButton = addToSelectedButton;
		}
		else if (paySelectedButton) {
			selectedButton = paySelectedButton;
		}
	
		if ($(".form-check.accounting-select .form-check-input:checked").length) {
			if( $(".activeVendor").val() !=$(this).attr("vendorid"))
			{
				//alert("active vendors are ot equal - not doing anything")
				$(this).removeAttr("checked");
				$(this).prop("checked", false);
				//alert("count checked: "+$(".form-check.accounting-select .form-check-input:checked").length)
				return false;
			}
	
			//alert("after if -continuing with if ")
			 $(this).closest("tr").addClass("clicked-border-acc");

			if (selectedButton.hasClass("hidden")) {
				selectedButton.removeClass("hidden");
			}

		}
		else {
			if (!selectedButton.hasClass("hidden")) {
				selectedButton.addClass("hidden");
			}
			$(".activeVendor").val($(this).attr(""))
		}
	});

});


        $("table tr .form-check-input.fci-acc").click(function () {
            if ($(this).is(':checked')) {
               
            }
            else {
              
            }
        });