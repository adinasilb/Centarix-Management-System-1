$(function () {
	$(".form-check.accounting-select").on("change", function (e) {
		console.log("checkbox checked");
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
			//var arrayOfSelected = [];
			//$(".form-check.accounting-select .form-check-input:checked").not($(this).find(".form-check-input")).map(function () {
			//	//return $(this).attr("vendorid")
			//	arrayOfSelected.push($(this).attr("vendorid"));
			//}).get()

			//console.log("arrayofselected: " + arrayOfSelected);
			//console.log("array of selected length: " + arrayOfSelected.length);
			//console.log("vendor id of first selected checkbox: " + arrayOfSelected[0]);
			//console.log("vendor id of this 1 try 1: " + $(this).attr("vendorid"));

			//if (arrayOfSelected.length > 1 && arrayOfSelected[0] == $(this).attr("vendorid")) {

				if (selectedButton.hasClass("hidden")) {
					selectedButton.removeClass("hidden");
			}

			var arrayOfOtherVendorsCheckboxes = [];
			$(".form-check.accounting-select .form-check-input").map(function () {
				arrayOfOtherVendorsCheckboxes.push($(this));
			});
			console.log("arrayOfOtherVendorsCheckboxes: " + arrayOfOtherVendorsCheckboxes);
			//arrayOfOtherVendorsCheckboxes.ea
			//}
			//else {
			//	$(this).find(".form-check-input").prop("checked", false);
			//	alert("can only select requests from the same vendor");
			//}

			//arrayOfSelected.splice(arrayOfSelected, $(this).attr("vendorid"));
			//console.log("arrayofselected: " + arrayOfSelected);

			//if (arrayOfSelected[0])

		}
		else {
			if (!selectedButton.hasClass("hidden")) {
				selectedButton.addClass("hidden");
			}
		}
	});

});