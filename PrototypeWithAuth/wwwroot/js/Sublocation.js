//import { options } from "../lib/bootstrap/dist/src/js/vendor/free/chart";

$("#LocationInstances_0__Height").on("change", function (e) {
	var select = $(".locationinstances");
	select.addClass("order-inv-color");
	var numSelect = select.length;
	if (numSelect > 0) {
		var numOptions = $(".empty-shelf-check").length;
		var difference = parseInt($(this).val()) - parseInt(numOptions);
		var x = numOptions + 1;
		if (difference > 0) {
			for (m = 0; m < difference; m++) {
				//$('.mdb-select').material_select('destroy');
				var idNum = parseInt(x) - 1;
				var optName = "EmptyShelves80[" + idNum + "]";
				var optId = "EmptyShelves80_" + idNum + "_";
				var newOption = "<div class='form-check locations'>";
				newOption += "<input type='checkbox' class='form-check-input empty-shelf-check emptyShelf' data-val='false' data-num='" + idNum + "' id='" + optId + "' name='" + optName + "' value='true'>";
				newOption += "<label class='form-check-label' for='" + optId + "'>";
				newOption += "Shelf " + x;
				newOption += "</label></div>";
				select.append(newOption);
				//('.mdb-select').material
				x++;
			}
		}
		else if (difference < 0) {
			console.log("subtract difference: " + difference);
			for (n = 0; n > difference; n--) {
				var lastDiv = $(".form-check.locations:last");
				console.log("last div id: " + lastDiv.attr("id"));
				//select.remove(lastDiv);
				lastDiv.remove();
			}
		}
	}
});

$(".empty-shelf-check").on("click", function () {
	alert("empty shelf clicked!");
});

$(".empty-shelf-check").on("change", function () {
	alert("empty shelf changed!");
});

$("#labPartDDL").on("change", function () {
	if($(this).val()!="")
	{
	$.ajax({
            async: true,
            type: 'GET',
            cache: false,
            url: "/Locations/HasShelfBlock?id="+$(this).val(),
            success: function (data) {
                $(".hasShelfBlock").html(data);
            }
    });
		}
	
});
//DROPDOWN MENU


$('.modal').off('change', '.emptyShelf').on('change', '.emptyShelf', function () {
	console.log('emptyShelf change')
	var val = $(this).prop('checked');
	var id = $(this).attr('id');
	var num = parseInt( $(this).attr('data-num'))+1;
	if (val == false) {
		//remove from
		$('.' + id).remove()
	} else {
		//add to label
		var span = "<span class='addLocationFont " + id + "'>" + num +", &nbsp</span>"
		$('.custom-multipleSpan').show();
		$('.select').append(span)
	}


});