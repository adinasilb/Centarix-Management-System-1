//import { options } from "../lib/bootstrap/dist/src/js/vendor/free/chart";

$("#LocationInstances_0__Height").on("change", function (e) {
	console.log("shelves changed to: " + $(this).val());
	var select = $(".locationinstance");
	var numSelect = select.length;
	if (numSelect > 0) {
		console.log("empty shelves 80 exists");
		var numOptions = $(".locationinstance div").length;
		console.log("numoptions: " + numOptions);
		var difference = parseInt($(this).val()) - parseInt(numOptions);
		console.log("difference = val - options");
		console.log(difference + " = " + $(this).val() + " - " + numOptions);
		var x = numOptions + 1;
		if (difference > 0) {
			for (m = 0; m < difference; m++) {
				//$('.mdb-select').material_select('destroy');
				var idNum = parseInt(x) - 1;
				var optName = "EmptyShelves80[" + idNum + "]";
				var optId = "EmptyShelves80_" + idNum + "_";
				var newOption = "<div class='form-check locations'>";
				newOption += "<input type='checkbox' class='form-check-input' data-val='false' id='" + optId + "' name='" + optName + "' value='true'>";
				newOption += "<label class='form-check-label' for='" + optId + "'>::before ESTest ::after";
				newOption += "</label></div>";
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

