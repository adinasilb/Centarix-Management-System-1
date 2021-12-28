

$(function () {
	$(".contact-name").rules("add", "required");

	$(".contact-email").rules("add", {
				required: true,
				email: true
			});
	$(".contact-phone").rules("add", {
				required: true,
				minlength: 9
			});

});