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

	$(".open-sublocations-types").on("click", function () {
		console.log("select location")
		var id = $(this).attr("id");
		console.log(id)
		loadReceivedModalSubLocations(id);
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
	//$(".SLI-click").on("click", function () {
	//    alert("sli clicked!");
	//    SLI($(this));
	//});
	$("body").off("click",".SLI-click").on("click",".SLI-click", function (e) {
		//alert("clicked SLI");
		console.log("clicked SLI")
		SLI($(this));
	});

	function SLI(el) {
		//alert("in SLI function");
		//ONE ---> GET THE NEXT DROPDOWNLIST
		var nextSelect = $(el).parents('.form-group').nextAll().first().find('.dropdown-menu')
		$(nextSelect).html('');
		console.log(nextSelect)
		console.log("selected")
		var locationInstanceParentId = $(el).val();
		var url = "/Requests/GetSublocationInstancesList";/*/?LocationInstanceParentID=" + locationInstanceParentId;*/

		if (nextSelect != undefined) { //if there is another one
			$(nextSelect).html('');
			$(nextSelect).parents('.dropdown-main').find('span:not(.caret)').text('select');
			FillNextSelect(nextSelect, locationInstanceParentId);
			//alert("items: " + items);
			//nextSelect.html(items);
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
					url: "/Requests/ReceivedModalVisual/?LocationInstanceID=" + oldSelected,
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
				url: "/Requests/ReceivedModalVisual/?LocationInstanceID=" + locationInstanceParentId,
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
	};

	function FillNextSelect(nextSelect, locationInstanceParentId) {
		var url = "/Requests/GetSublocationInstancesList?locationInstanceParentId=" + locationInstanceParentId;
		$.getJSON(url, { locationInstanceParentId, locationInstanceParentId }, function (result) {
			var item = "<li>Select Location Instance</li>";
			$.each(result, function (i, field) {
				var emptyText = "";
				if (field.isEmptyShelf && field.labPartID<=0) {
					emptyText = " (nr)";
				}
				item += '<li value="' + field.locationInstanceID + '" id="' + field.locationInstanceID + ' "  class="SLI-click" >' + field.locationInstanceName + emptyText + '</li>'
			});
			nextSelect.append(item);
		});
		return false;
	};


	//RECEIVED MODAL VISUAL
	$(".open-new-visual").on("click", function () {
		var childlocationinstanceid = $(this).parent().attr("id");
		var parentlocationinstanceid = $("#ParentLocationInstance_LocationInstanceID").val();

		//get the name of the parentlocationinstance
		var name = $(this).parents().parents().children(".LocationInstanceName").val();
		//fill the shelves dropdown with the name
		$("#1").parents('.dropdown-main').find('span:not(.caret)').text(name);
		$("#1").parents('.dropdown-main').find('input').attr('value', childlocationinstanceid);

		//now send a new visual
	});

	$(".visual-locations td").on("click", function () {
		if (!$(this).hasClass("not-clickable")) {
			if ($(this).has("i").length) {

				var locationInstanceId = $(this).children('div').first().children("input").first().attr("liid");
				var lip = $(".liid." + locationInstanceId);
				console.log("lip val: " + lip.val());
				$(".complete-order").removeClass("disabled-submit")
				if (lip.val() == "true") {
					console.log("TRUE!");
					lip.val("false"); //IMPT: sending back the true value to controller to place it here

					$(this).children('div').first().children(".row-1").children("i").addClass("icon-add_circle_outline-24px1");
					$(this).children('div').first().children(".row-1").children("i").removeClass("icon-delete-24px");
					$(this).removeClass('location-selected')
					var hasLocationSelected = $('.liid[value="true"]').length;
					if (hasLocationSelected <= 0) {
						$('#locationSelected').val('');
					}

				}
				else {
					console.log("FALSE!");
					lip.val("true"); //IMPT: sending back the true value to controller to place it here

					$(this).children('div').first().children(".row-1").children("i").removeClass("icon-add_circle_outline-24px1");
					$(this).children('div').first().children(".row-1").children("i").addClass("icon-delete-24px");
					$(this).addClass('location-selected')
					$('#locationSelected').val(true);
					$('#locationSelected').removeClass("error")
					$("#locationSelected-error").replaceWith('');
				}
			}
		}
	});

});