$(function () {

	//FROM THE RECEIVED MODAL
	$("input:checkbox.p-c").on('click', function () {
		// in the handler, 'this' refers to the box clicked on
		var $box = $(this);
		if (!$box.is(":checked")) {
			console.log('in box checked')
			$box.prop("checked", false);
		} else {
			switch ($box.attr('name')) {
				case "PartialDelivery":
					console.log('in partial')
					$("#PartialDelivery").prop("checked", true);
					$("#Clarify").prop("checked", false);
					break;
				case "Clarify":
					console.log('in clarify')
					$("#PartialDelivery").prop("checked", false);
					$("#Clarify").prop("checked", true);
					break;
			}
		}
	});

	$(".open-sublocations-types").off('click').on("click", function () {
		$("#LocationTypeID").val($(this).attr("id"))
		$("#locationTypeSelected").attr('data-val', true);
		if (!$(".temporary-check").is(":checked")) {
			console.log("select location")
			var id = $(this).attr("id");
			console.log(id)
			loadReceivedModalSubLocations(id);
		}
		else {
			$("#myForm").data("validator").settings.ignore = "";
			var valid = $("#myForm").valid();
			if (valid) {
				$('.activeSubmit').removeClass('disabled-submit')
			}
		}
		$(".locationFullName").html("");
	});

	//AJAX load full partial view for modalview manage locations
	function loadReceivedModalSubLocations(val) {
		var myDiv = $(".divSublocations");
		console.log(myDiv)
		$.ajax({
			//IMPORTANT: ADD IN THE ID
			url: "/Requests/ReceivedModalSublocations/?LocationTypeID=" + val,
			type: 'GET',
			cache: false,
			context: myDiv,
			success: function (result) {
				$(this).html(result);
				$('.visualView').html('');
			}
		});
	};

	//FROM THE RECEIVED MODAL SUBLOCATIONS
	$("form").off("click", ".SLI-click").on("click", ".SLI-click", function (e) {
		//alert("clicked SLI");
		console.log("clicked SLI")
		SLI($(this));
		/*var prevName = $(".locationFullName").html()
		var name = $(this).html();

		$(".locationFullName").html(prevName + name);*/
		$.ajax({
			url: "/Locations/GetLocationName?locationId=" + $(this).attr('id'),
			type: 'GET',
			cache: false,
			success: function (data) {
				$(".locationFullName").html(data);
			}
		});
	})

	function SLI(el) {
		//alert("in SLI function");
		//ONE ---> GET THE NEXT DROPDOWNLIST
		if($(el).attr("isNoRack")=="true")
		{
			$(".hasRackBlock").addClass("d-none");
		}
		else 
		{
			if($(".hasRackBlock").hasClass("d-none"))
			{
				$(".hasRackBlock").removeClass("d-none");
			}
		}
		var nextSelect = $(el).parents('.form-group').nextAll(".form-group").first().find('.dropdown-menu')
		$(nextSelect).html('');
		//clear all the next selects
		$(el).parents('.form-group').nextAll(".form-group").each(function(){
			$(this).find('.dropdown-menu').html('');
			$(this).find('.dropdown-main').find('span:not(.caret)').text('select');
		});
		console.log(nextSelect)
		console.log("selected")
		var locationInstanceParentId = $(el).val();
		var requestID = $("#Requests_0__RequestID").val();

		if (nextSelect != undefined) { //if there is another one
			$(nextSelect).html('');
			$(nextSelect).parents('.dropdown-main').find('span:not(.caret)').text('select');
			FillNextSelect(nextSelect, locationInstanceParentId);
		}
			
		//TWO ---> FILL VISUAL VIEW
		var myDiv = $(".visualView");
		if (locationInstanceParentId == 0) { //if zero was selected
			console.log("selected was 0");
			//check if there is a previous select box
			var oldSelectClass = name.replace(place.toString(), (place - 1).toString());
			var oldSelect = $("select[name='" + oldSelectClass + "']");
			if (oldSelect.length) {
				console.log("oldSelectClass " + oldSelectClass + " exists and refilling with that");
				var oldSelected = $("." + oldSelect).children("option:selected").val();
				console.log("oldSelected: " + oldSelected);
				$.ajax({
					url: "/Requests/ReceivedModalVisual/?LocationInstanceID=" + oldSelected + "&RequestID=" + requestID + "&ShowIcons=true",
					type: 'GET',
					cache: false,
					context: myDiv,
					success: function (result) {
						$(this).html(result);
					}
				});
			}
			else {
				console.log("oldSelectClass " + oldSelectClass + " does not exist and clearing");
				myDiv.html("");
			}
		}
		else {
		
			console.log("regular visual");
			$.ajax({
				url: "/Requests/ReceivedModalVisual/?LocationInstanceID=" + locationInstanceParentId + "&RequestID=" + requestID + "&ShowIcons=true",
				type: 'GET',
				cache: false,
				context: myDiv,
				success: function (result) {
					$(this).html(result);

					console.log("____________________________________");
					var visualBox = $(".visual-box");
					var visualShelf = $(".visual-shelf");
					var table = $("table.visual-locations");
					//table.css("width", "100%");
					//var tableWidth = table.width();
					//console.log("tableWidth: " + tableWidth);
					//var tblH = tableWidth + "px";
					//table.css("height", tblH);

					var cols = 0;
					$("table.visual-locations tr:nth-child(1) td").each(function () {
						cols++; //no colspans so don't count for that here
					});
					console.log("cols: " + cols);
					var perc = parseFloat(100) / parseFloat(cols);
					console.log("perc: " + parseFloat(perc));
					visualBoxWidth = perc + "%";
					visualBox.css('width', visualBoxWidth);
					if (cols > 0) {
						$('#subLocationSelected').attr('data-val', true);
					} else {
						$('#subLocationSelected').attr('data-val', false);
                    }
					//var width = visualBox.width();
					//console.log("width: " + width);
					//var heightPx = width + "px";
					//visualBox.css('overflow', "hidden");
					//visualBox.css('height', heightPx);

					//var row1 = $(".visual-box .row-1");
					//var row2 = $(".visual-box .row-2");
					//var height = visualBox.height();
					//console.log("height: " + height);
					//var row1H = parseFloat(height, 10) * .75;
					//console.log("row1H: " + row1H);
					//var row2H = parseFloat(height, 10) * .25;
					//console.log("row2H: " + row2H);
				}
			});
		}

		$(el).parents('.dropdown-main').find('span:not(.caret)').text($(el).text());
		$(el).parents('.dropdown-main').find('span:not(.caret)').attr("description", $(el).attr("data-string"));
		$(el).parents('.dropdown-main').find('span:not(.caret)').attr("parentID", locationInstanceParentId);
	};

	
	$("select.part-type").off("change").change(function () {
		$(".visualView").html("")
			var parentID = $(this).parents('.parent-group').prev(".form-group").find('.dropdown-main').find('span:not(.caret)').attr("parentID");
		var nextSelect = $(this).parents('.parent-group').nextAll(".form-group").first().find('.dropdown-menu');
		console.log(nextSelect)
			$(nextSelect).html('');
		FillNextSelect(nextSelect, parentID);
		
	});

	function FillNextSelect(nextSelect, locationInstanceParentId) {
		var url = "/Requests/GetSublocationInstancesList?locationInstanceParentId=" + locationInstanceParentId+"&labPartID="+$("select.part-type").val();
		$.getJSON(url, { locationInstanceParentId, locationInstanceParentId }, function (result) {
			var item = "<li>Select Location Instance</li>";
			$.each(result, function (i, field) {
				var emptyText = "";
				var description =field.locationInstanceAbbrev;
				if(description ==null ||description =='')
				{
					description =field.locationInstanceName
				}
				if (field.isEmptyShelf && field.labPartID<=0) {
					emptyText = " (nr)";
					item += '<li value="' + field.locationInstanceID + '" id="' + field.locationInstanceID + ' "  class="SLI-click" isNoRack="true" data-string="' + description +'">' + field.locationInstanceAbbrev + emptyText + '</li>'
				}
				else if (field.locationTypeID == 501)
				{
					item += '<li value="' + field.locationInstanceID + '" id="' + field.locationInstanceID + ' "  class="SLI-click" data-string="' + description + '" >' + field.labPart.labPartName + " " + field.locationNumber + emptyText + '</li>'
                }
				else
				{
					item += '<li value="' + field.locationInstanceID + '" id="' + field.locationInstanceID + ' "  class="SLI-click" data-string="' + description +'">' + field.locationInstanceAbbrev + emptyText + '</li>'
				}
				
			});
			nextSelect.append(item);
		});
		return false;
	};


	////RECEIVED MODAL VISUAL
	//$(".open-new-visual").on("click", function () {
	//	alert('$(".open-new-visual").on("click"')
	//	var childlocationinstanceid = $(this).parent().attr("id");
	//	var parentlocationinstanceid = $("#ParentLocationInstance_LocationInstanceID").val();

	//	//get the name of the parentlocationinstance
	//	var name = $(this).parents().parents().children(".LocationInstanceName").val();
	//	//fill the shelves dropdown with the name
	//	$("#1").parents('.dropdown-main').find('span:not(.caret)').text(name);
	//	$("#1").parents('.dropdown-main').find('input').attr('value', childlocationinstanceid);

	//	//now send a new visual
	//});

	$(".visual-locations td").off("click").on("click", function (e) {
		console.log("clicked")
		if (!$(this).hasClass("not-clickable")) {
			
			if ($(this).has("i").length) {
				var locationInstanceId = $(this).children('div').first().children("input").first().attr("liid");
				var lip = $(".liid." + locationInstanceId);
				console.log("lip val: " + lip.val());
				$(".submit-received").removeClass("disabled-submit")
				if (lip.val().toLowerCase() == "true") {
					//console.log("TRUE!");
					lip.val("false"); //IMPT: sending back the true value to controller to place it here

					$(this).children('div').first().children(".row-1").children("i").addClass("icon-add_circle_outline-24px1");
					$(this).children('div').first().children(".row-1").children("i").removeClass("icon-delete-24px");
					$(this).removeClass('location-selected')
					var hasLocationSelected = $('.liid[value="true"]').length;
					console.log("locations selected " + hasLocationSelected)
					if (hasLocationSelected <= 0) {
						$(".submit-received").addClass("disabled-submit")
						$('#locationVisualSelected').attr('data-val', false);
					}

				}
				else {
					console.log("empty")
					$(this).children(".css-checkbox").addClass("first");
					//console.log("FALSE!");
					lip.val("true"); //IMPT: sending back the true value to controller to place it here

					$(this).children('div').first().children(".row-1").children("i").removeClass("icon-add_circle_outline-24px1");
					$(this).children('div').first().children(".row-1").children("i").addClass("icon-delete-24px");
					$(this).addClass('location-selected')
					$('#locationVisualSelected').attr('data-val', true);
					$('#locationVisualSelected').removeClass("error")
					//$("#locationSelected-error").replaceWith('');
				}
			}
		}
		else{
			console.log("not clickable")
			}
	});
	$(".visual-locations td").on("click", function (e) {
		var element = $(this);
		if (!element.hasClass("not-clickable")) {
	
			if (e.shiftKey) {
				MultipleLocation(element, ToggleBoxUnitSelected);
			}
			$(".first-selected").removeClass("first-selected");
			element.addClass("first-selected");
		}
		else{
			console.log("not clickable")
			}
	});
	function ToggleBoxUnitSelected(element, select) {
		var locationInstanceId = element.children('div').first().children("input").first().attr("liid");
		var lip = $(".liid." + locationInstanceId);
		if (select) {
			element.addClass('location-selected')
			element.children('div').first().children(".row-1").children("i").removeClass("icon-add_circle_outline-24px1");
			element.children('div').first().children(".row-1").children("i").addClass("icon-delete-24px");
			lip.val("true")			
		}
		else {
			element.removeClass('location-selected')
			element.children('div').first().children(".row-1").children("i").addClass("icon-add_circle_outline-24px1");
			element.children('div').first().children(".row-1").children("i").removeClass("icon-delete-24px");
			lip.val("false")			
        }
	}

	$(".temporary-check").click(function (e) {
		var checked = $(this).is(":checked");
		if (checked) {
			$(".divSublocations").html("")
			console.log($("#LocationTypeID").val())
			/*if ($("#LocationTypeID").val() != 0)
			{
				$("#locationTypeSelected").attr('data-val', true)
			}*/
			$('.visualView').html('');
			$(".locationFullName").html("");
		}
		else {
			if ($("#LocationTypeID").val() != 0) {
				//$("#locationTypeSelected").attr('data-val', false)
				var id = $('#LocationTypeID').val();
				console.log(id);
				loadReceivedModalSubLocations(id);
            }

		}
    })


});

function MultipleLocation(element, ToggleBoxUnitSelected) {

        console.log(element);
        var firstSelected = $(".first-selected");
        console.log(firstSelected);
        var currentBox = element;
        var boxIsPrevious = false;
        var select = firstSelected.hasClass('location-selected');
	if (!firstSelected.parent("tr").is(element.parent("tr"))) {
		console.log("different row")
		if (element.parent("tr").prevAll().filter(firstSelected.parent("tr")).length == 0) {
			console.log("Earlier row");
			boxIsPrevious = true;
		}
		var end = false;
		if (boxIsPrevious) {
			firstSelected.parent("tr").prevUntil(element.parent("tr").prev()).each(function () {
				var reversed = $(this).children("td").toArray().reverse();
				reversed.forEach(td => {
					if (!end) {
						if ($(td).is(currentBox)) {
							end = true;
						}
						else {
							if (!$(td).hasClass("not-clickable")) {
							
								ToggleBoxUnitSelected($(td), select);
							}
							else{

									console.log("not clickable")
								}
						}
					}
				});
			});
		}
		else {
			firstSelected.parent("tr").nextUntil(element.parent("tr").next()).each(function () {
				$(this).children("td").each(function () {
					if (!end) {
						if ($(this).is(currentBox)) {
							end = true;
						}
						else {
							if (!$(this).hasClass("not-clickable")) {
								ToggleBoxUnitSelected($(this), select);
							}
							else{

									console.log("not clickable")
								}
						}
					}
				});
			});
		}
	}
	else {
		console.log("same row")
		if (element.prevAll().filter(firstSelected).length == 0) {
			console.log("Earlier box");
			boxIsPrevious = true;
		}
	}
	if (boxIsPrevious) {
		firstSelected.prevUntil(element).each(function () {
			if (!$(this).hasClass("graduated-table-background-color")) {
				ToggleBoxUnitSelected($(this), select);
			}
		});
	}
	else {
		console.log("element" +$(element).html())
		firstSelected.nextUntil(element).each(function () {
			if (!$(this).hasClass("graduated-table-background-color")) {
				console.log("this" + $(this))
				ToggleBoxUnitSelected($(this), select);
			}
		});
	}
}
