$(function () {
    $(".employee-type").on("click", function () {
        var value = $(this).children("input").attr("id");

        var repairDays = $("#Repair_Days");
        var repairMonths = $("#Repair_Months");
        var hiddenIsRepeat = $("#Repair_IsRepeat");
        switch (value) {
            case "once":
                repairDays.attr("disabled", true);
                repairMonths.attr("disabled", true);
                hiddenIsRepeat.val("false");
                break;
            case "repeat":
                repairDays.attr("disabled", false);
                repairMonths.attr("disabled", false);
                hiddenIsRepeat.val("true");
                break;
        }
    });

    $(".saveRepairs").on("click", function (e) {
        e.preventDefault();
        e.stopPropagation();
        if (!$(this).hasClass("disabled-submit")) {
            console.log("save repairs clicked");

			var url = "/Calibrations/_Repairs";
			console.log("url : " + url);
			var formData = new FormData($(".RepairsPartialViews")[0]);

			$.ajax({
				url: url,
				method: 'POST',
				data: formData,
				success: (result) => {
				},
				processData: false,
				contentType: false
			});
        }
    });
});