$(function () {
	$(".form-check.accounting-select").on("change", function (e) {
		var addToSelectedButton = $("#add-to-selected");
		if ($(".form-check.accounting-select .form-check-input:checked").length) {
			if (addToSelectedButton.hasClass("hidden")) {
				addToSelectedButton.removeClass("hidden");
			}
		}
		else {
			if (!addToSelectedButton.hasClass("hidden")) {
				addToSelectedButton.addClass("hidden");
			}
		}
	});

});