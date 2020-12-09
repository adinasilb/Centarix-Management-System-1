$(function () {
	$(".vendor-contact-close").off("click").on("click", function () {
		var amountOpen = 0;
		$(".contact-deletes").each(function () {
			if ($(this).val() == "false") {
				amountOpen++;
			}
		});
		if (amountOpen > 1) {
			var contactcard = $(this).attr("ContactCardID");
			console.log("closing contactcard: " + contactcard);
			var contactcardid = "#contact-info-" + contactcard;
			console.log("ContactCardID: " + contactcardid);
			//$(".contact-info").remove(contactcardid);
			$(contactcardid).hide();
			//$("#" + contactcard).attr("display", "none");
			var x = 0;
			$(contactcardid + " input").each(function () {
				if (x > 1) {
					$(this).attr("disabled", "true"); //skipping the first 2 hiddens but allowing the rest so the id and delete won't be disabled but the rest will allowing the validation to go through while saving it's place
				}
				x++;
			});
			//$('#contact-index').val(parseInt($('#contact-index').val()) - 1);
			var deletedid = "VendorContacts_" + contactcard + "__Delete";
			console.log("vendor contact deleted hidden id: " + deletedid);
			$("#" + deletedid).val("true");
		//$("#VendorContacts_" + contactcard + "__VendorContactID").remove();
		}
	});
});