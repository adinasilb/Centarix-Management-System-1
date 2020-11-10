$(function () {
    //$(".employee-type").on("click", function () {
    //    var value = $(this).children("input").attr("id");

    //    var repairDays = $("#Repair_Days");
    //    var repairMonths = $("#Repair_Months");
    //    var hiddenIsRepeat = $("#Repair_IsRepeat");
    //    switch (value) {
    //        case "once":
    //            repairDays.attr("disabled", true);
    //            repairMonths.attr("disabled", true);
    //            hiddenIsRepeat.val("false");
    //            break;
    //        case "repeat":
    //            repairDays.attr("disabled", false);
    //            repairMonths.attr("disabled", false);
    //            hiddenIsRepeat.val("true");
    //            break;
    //    }
    //});

	$(".repeat-type label").off("click").on("click", function () {
		var value = $(this).attr("for");
		var index = value.substr(value.length - 1, 1);
		console.log("value: " + value + ", index: " + index);
		var first2Letters = value.substr(0, 2);
		console.log("first2Letters: " + first2Letters);
		var repairDays = $("#Repairs_" + index + "__Days");
		var repairMonths = $("#Repairs_" + index + "__Months");
		var hiddenInput = $("#Repairs_" + index + "__IsRepeat");
		switch (first2Letters) {
			case "on":
				repairDays.attr("disabled", true);
				repairMonths.attr("disabled", true);
				hiddenInput.val("false");
				break;
			case "re":
				repairDays.attr("disabled", false);
				repairMonths.attr("disabled", false);
				hiddenInput.val("true");
				break;
        }
	});

	$(".removeNewRepair").off("click").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		var newDivClass = "RepairDiv" + $(this).attr("index");
		console.log("newDivClass: " + newDivClass);
		$("." + newDivClass).remove();

		$.fn.UpdateIds($(this).attr("index"));
	});

	$.fn.UpdateIds = function (deletedIndex) {
		console.log("update ids. deleted div: " + deletedIndex);
		$(".repair-outer-div").each(function (i, obj) {
			var divIndex = $(this).attr("repairIndex");
			console.log("DivIndex: " + divIndex);
			if (parseInt(divIndex) > parseInt(deletedIndex)) {
				console.log("change ids with #" + divIndex);
				$("#Repairs_" + divIndex + "__Date")
            }
		});
	};

    $(".addRepair").off("click").on("click", function (e) {
        e.preventDefault();
		e.stopPropagation();
		var repairIndexInput = $("#repairIndex");
		var repairIndex = repairIndexInput.val();
		$.ajax({
			async: true,
			url: "/Calibrations/_Repairs?RepairIndex=" + repairIndex,
			type: 'GET',
			cache: false,
			success: function (data) {
				var newRepair = $(data);
				$('#repairsListDiv').append(newRepair);
				var newRepairIndex = parseInt(repairIndex) + 1;
				console.log("new repair index: " + newRepairIndex);
		repairIndexInput.val(newRepairIndex);
				//$("#visualZoomModal").modal({
				//	backdrop: true,
				//	keyboard: true,
				//});
				//$(".modal").modal('show');
				////$('.modal-backdrop').remove()
				//var firstTDFilled = $("td.lab-man-50-background-color");
				//var height = firstTDFilled.height();
				//var width = firstTDFilled.width();
				//console.log("h: " + height + "------ w: " + width);
				////$("td").height(height);
				////$("td").width(width);
				//$(".visualzoom td").css('height', height);
				//$(".visualzoom td").css('width', width);
				////$("td").addClass("danger-color");
			}
		});
    });

   // $(".saveRepairs").on("click", function (e) {
   //     e.preventDefault();
   //     e.stopPropagation();
   //     if (!$(this).hasClass("disabled-submit")) {
   //         console.log("save repairs clicked");

			//var url = "/Calibrations/_Repairs";
			//console.log("url : " + url);
			//var formData = new FormData($(".RepairsPartialViews")[0]);

			//$.ajax({
			//	url: url,
			//	method: 'POST',
			//	data: formData,
			//	success: (result) => {
			//	},
			//	processData: false,
			//	contentType: false
			//});
   //     }
   // });
});