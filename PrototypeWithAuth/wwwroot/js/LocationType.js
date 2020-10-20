$(function () {


	$(".load-add-location-type").on("click", function (e) {
		console.log("load add location type 3");
		e.preventDefault();
		e.stopPropagation();
		var $itemurl = "Locations/AddLocationType";
		var $modal = $(".location-modal-view");
		//$("location-modal-view").css("display", "none");
		//IMPT REOPEN ON CLOSING
		$.fn.CallPageRequest($itemurl, "addlocation");
	});

	//$(".locationinstance-locationtypeid").on("click", function (e) {
	//	e.preventDefault();
	//	e.stopPropagation();
	//	console.log("value of locationinstance: " + $(this).val());
	//	$("#LocationInstance_LocationTypeID").val($(this).val());
	//});

	//$(".btnAddSublocation").click(e){
	//    console.log("Location manage location view js");
	//    e.preventDefault();
	//    e.stopPropagation();
	//    $.fn.AddSublocation();
	//});

	$(".load-add-sublocation").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		console.log("load add sublocation");
		$.fn.AddSublocation();
	});


	var $sublocationCounter = 1;
	$.fn.AddSublocation = function () {
		if ($sublocationCounter == 0 && !$(".nameSublocation").val()) {
			$(".nameError").html("Please input a name first");
			return;
		}
		else if (false) {
			//check if last sublocation is blank and make sure any other spans are blank
		}
		//check that the one on top is filled out
		else {
			$("span").html("");
		}
		console.log("Location site.js");

		var newSublocationID = 'Sublocations_' + $sublocationCounter + '_';
		console.log("newSublocationID: " + newSublocationID);
		var newSublocationName = 'Sublocations[' + $sublocationCounter + ']';
		console.log("newSublocationName: " + newSublocationName);
		var newSublocationClass = 'sublocationName' + $sublocationCounter;
		console.log("newSublocationClass: " + newSublocationClass);
		var sublocationHtml = '<div class="col-md-4">';
		sublocationHtml += '<label class="control-label">Sublocation ' + $sublocationCounter + ':</label>';
		sublocationHtml += '<input type="text" class="form-control" id="' + newSublocationID + '" name="' + newSublocationName + '" class="' + newSublocationClass + '" />';
		//sublocationHtml += '<input type="text" class="form-control" ' + newSublocationClass + '  />';
		var spanClass = 'spanSublocation' + $sublocationCounter;
		sublocationHtml += '<span class="text-danger-centarix ' + spanClass + '></span>"';
		sublocationHtml += '</div>';
		$(".addSublocation").append(sublocationHtml);
		$(".addSublocation").show();
		$sublocationCounter++;
	}


	//AJAX partial view submit for addLocations --> Sublocations
	$(function () {
		$("li").on("click", function () {
			console.log("clicked li-li : " + $(this).id);
		});
	});



	$(".locationinstance li").click(function () {
		//$("#LocationInstance_LocationTypeID").change(function () {

		//$(document).ajaxStart(function () {
		//	$("#loading").show();
		//});

		//$(document).ajaxComplete(function () {
		//	$("#loading").hide();
		//});
	});

	//$(".empty-shelf-check").on("click", function () {
	//	alert("empty shelf check!");
	//});

	//$(".form-check-label").on("click", function () {
	//	alert("form check label clicked!");
	//});

	//$(".form-check.locations").on("click", function () {
	//	alert("form check locations div clicked!");
	//});

});