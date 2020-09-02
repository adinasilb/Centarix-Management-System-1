
$("#LocationInstances_0__Height").on("change", function (e) {
	console.log("shelves changed to: " + $(this).val());
	var select = $(".locationinstance");
	var numSelect = select.length;
	if (numSelect > 0) {
		console.log("empty shelves 80 exists");
		var numOptions = select.length;
		console.log("numoptions: " + numOptions);
		var difference = parseInt($(this).val()) - parseInt(numOptions);
		console.log("difference: " + difference);
		var x = numOptions + 1;
		if (difference > 0) {
			for (m = 0; m < difference; m++) {
				//$('.mdb-select').material_select('destroy');
				var newOption = "<li value='"+x+"' >Shelf " + x + "</li>";
				select.append(newOption);
				//('.mdb-select').material
				x++;
			}
		}
		else if (difference < 0) {
			for (n = 0; n < difference; n++) {
				console.log("subtract one");
			}
		}
	}
});
