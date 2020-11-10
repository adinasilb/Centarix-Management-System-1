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

	$(".isRepeat").off("click").on("click", function () {
		console.log("is repeat clicked");
	});

	$("body").off("click").on("click", ".repeat-type input", function () {
		console.log("clicked");
		var value = $(this).children("input").attr("id");
		console.log("value: " + value);
		var index = value.substr(value.length - 1, 1);
		console.log("value: " + value + ", index: " + index);
	});

	$(".removeNewRepair").off("click").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		var newDivClass = "RepairDiv" + $(this).attr("index");
		console.log("newDivClass: " + newDivClass);
		$("." + newDivClass).remove();
	});

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