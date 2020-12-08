$(function () {
	$(".vendor-contact-close").on("click", function () {
		var contactcard = $(this).attr("ContactCardID");
		console.log("closing contactcard: " + contactcard);
		var contactcardid = "#contact-info-" + contactcard;
		console.log("ContactCardID: " + contactcardid);
		$(".contact-info").remove(contactcardid);
		$('#contact-index').val(parseInt($('#contact-index').val()) - 1);
		var deletedid = "VendorContacts_" + contactcard + "__Delete";
		console.log("vendor contact deleted hidden id: " + deletedid);
		$("#" + deletedid).val("true");
		//$("#VendorContacts_" + contactcard + "__VendorContactID").remove();
	});
});