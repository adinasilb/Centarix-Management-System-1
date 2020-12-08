$(function () {
	$(".vendor-contact-close").on("click", function () {
		var contactcard = $(this).attr("ContactCardID");
		console.log("closing contactcard: " + contactcard);
		var contactcardid = "#contact-info-" + contactcard;
		console.log("ContactCardID: " + contactcardid);
		$(".contact-info").remove(contactcardid);
		$('#contact-index').val(parseInt($('#contact-index').val()) - 1);
	});
});