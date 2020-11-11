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

	$(".repeat-type label.repair").off("click").on("click", function () {
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

	$(".repeat-type label.externalCalibration").off("click").on("click", function () {
		var value = $(this).attr("for");
		var index = value.substr(value.length - 1, 1);
		console.log("value: " + value + ", index: " + index);
		var first2Letters = value.substr(0, 2);
		console.log("first2Letters: " + first2Letters);
		var ecDays = $("#ExternalCalibrations_" + index + "__Days");
		var ecMonths = $("#ExternalCalibrations_" + index + "__Months");
		var hiddenInput = $("#ExternalCalibrations_" + index + "__IsRepeat");
		switch (first2Letters) {
			case "on":
				ecDays.attr("disabled", true);
				ecMonths.attr("disabled", true);
				hiddenInput.val("false");
				break;
			case "re":
				ecDays.attr("disabled", false);
				ecMonths.attr("disabled", false);
				hiddenInput.val("true");
				break;
		}
	});

	$(".repeat-type label.internalCalibration").off("click").on("click", function () {
		var value = $(this).attr("for");
		var index = value.substr(value.length - 1, 1);
		console.log("value: " + value + ", index: " + index);
		var first2Letters = value.substr(0, 2);
		console.log("first2Letters: " + first2Letters);
		var icDays = $("#InternalCalibration_" + index + "__Days");
		var icMonths = $("#InternalCalibration_" + index + "__Months");
		var hiddenInput = $("#InternalCalibration_" + index + "__IsRepeat");
		switch (first2Letters) {
			case "on":
				icDays.attr("disabled", true);
				icMonths.attr("disabled", true);
				hiddenInput.val("false");
				break;
			case "re":
				icDays.attr("disabled", false);
				icMonths.attr("disabled", false);
				hiddenInput.val("true");
				break;
		}
	});

	$(".removeNewRepair").off("click").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		var index = $(this).attr("index");
		var newDivClass = "RepairDiv" + index;
		console.log("newDivClass: " + newDivClass);
		$("." + newDivClass).hide();

		$("#Repairs_" + index + "__IsDeleted").val("true");
		//$.fn.UpdateIds($(this).attr("index"));
	});

	$(".removeNewExternalCalibration").off("click").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		var index = $(this).attr("index");
		var newDivClass = "ECDiv" + index;
		console.log("newDivClass: " + newDivClass);
		$("." + newDivClass).hide();

		$("#ExternalCalibrations_" + index + "__IsDeleted").val("true");
		//$.fn.UpdateIds($(this).attr("index"));
	});

	$(".removeNewInternalCalibration").off("click").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		var index = $(this).attr("index");
		var newDivClass = "ICDiv" + index;
		console.log("newDivClass: " + newDivClass);
		$("." + newDivClass).hide();

		$("#InternalCalibration_" + index + "__IsDeleted").val("true");
	});

	//$.fn.UpdateIds = function (deletedIndex) {
	//	console.log("update ids. deleted div: " + deletedIndex);
	//	$(".repair-outer-div").each(function (i, obj) {
	//		var divIndex = $(this).attr("repairIndex");
	//		console.log("DivIndex: " + divIndex);
	//		if (parseInt(divIndex) > parseInt(deletedIndex)) {
	//			console.log("change ids with #" + divIndex);
	//			$("#Repairs_" + divIndex + "__Date")
 //           }
	//	});
	//};

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
			}
		});
    });

	$(".addExternalCalibration").off("click").on("click", function (e) {
		console.log("in EXTERNAL calibration");
		e.preventDefault();
		e.stopPropagation();
		var ecIndexInput = $("#externalCalibrationIndex");
		var ecIndex = ecIndexInput.val();
		$.ajax({
			async: true,
			url: "/Calibrations/_ExternalCalibration?ECIndex=" + ecIndex,
			type: 'GET',
			cache: false,
			success: function (data) {
				var newEC = $(data);
				$('#ecListDiv').append(newEC);
				var newECIndex = parseInt(ecIndex) + 1;
				console.log("new ec index: " + newECIndex);
				ecIndexInput.val(parseInt(ecIndex) + 1);
			}
		});
	});

	$(".addInternalCalibration").off("click").on("click", function (e) {
		e.preventDefault();
		e.stopPropagation();
		var icIndexInput = $("#icIndex");
		var icIndex = icIndexInput.val();
		$.ajax({
			async: true,
			url: "/Calibrations/_InternalCalibration?ICIndex=" + icIndex,
			type: 'GET',
			cache: false,
			success: function (data) {
				var newIC = $(data);
				$('#icListDiv').append(newIC);
				var newICIndex = parseInt(icIndex) + 1;
				console.log("new ec index: " + newICIndex);
				icIndexInput.val(newICIndex);
			}
		});
	});

});