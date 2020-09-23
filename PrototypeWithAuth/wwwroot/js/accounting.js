$(function () {
	$(".form-check.accounting-select").on("change", function (e) {
		var addToSelectedButton = $("#add-to-selected");
		if ($(".form-check.accounting-select .form-check-input:checked").length) {
			console.log("is checked");
			if (addToSelectedButton.hasClass("hidden")) {
				console.log("has class hidden");
				addToSelectedButton.removeClass("hidden");
			}
		}
		else {
			console.log("is not checked");
			if (!addToSelectedButton.hasClass("hidden")) {
				console.log("does not have class hidden");
				addToSelectedButton.addClass("hidden");
			}
		}
	});
});