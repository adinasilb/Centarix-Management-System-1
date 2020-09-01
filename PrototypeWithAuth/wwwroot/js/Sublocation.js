
$("#LocationInstances_0__Height").on("change", function (e) {
	console.log("shelves changed to: " + $(this).val());
	var select = $("#select-options-EmptyShelves80");
	var numSelect = select.length;
	console.log("numSelect: " + numSelect);
	if (numSelect > 0) {
		console.log("empty shelves 80 exists");
		var numOptions = $("#select-options-EmptyShelves80 li").length;
		console.log("numoptions: " + numOptions);
		var difference = parseInt($(this).val()) - parseInt(numOptions);
		console.log("difference: " + difference);
		var x = numOptions + 1;
		if (difference > 1) {
			for (m = 0; m < difference; m++) {
				var newOption = "<li class role='option' aria-selected='false' aria-disabled='false'>";
				newOption += "<span class='filtrable'>";
				newOption += "<input type='checkbox' class='form-check-input'>";
				newOption += "<label></label>";
				newOption += " Shelf " + x;
				newOption += "</span> </li>";
				select.append(newOption);
				$('.mdb-select-empty').material_select('destroy');
				('.mdb-select-empty').material_select();
				x++;
			}
		}
		else if (difference < 1) {
			for (n = 0; n < difference; n++) {
				console.log("subtract one");
			}
		}
	}
});
