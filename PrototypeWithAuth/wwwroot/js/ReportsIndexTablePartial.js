$(function () {
	$(".change-month").on("change", function () {
		$.fn.$.fn.ajaxPartialIndexTable();
	});
	$(".change-year").on("change", function () {
		var years = $("#select-years").val();
		var currency = $("#select-months").val();
		console.log("year: " + year + " , currency: " + currency);
		var url = "/Protocols/_ReportsIndexTable?months=" + months + "&years=" + years;
		$.fn.$.fn.ajaxPartialIndexTable(url, "._IndexTable", "GET" );
	});

	$(".create-report").click(function (e) {
		var sidebarType = $('#masterSidebarType').val()
		var reportCategory = $("#ReportsIndexObject_ReportCategoryID").val()
		$.ajax({
			url: "/Protocols/NewReportModal?reportCategoryId=" + reportCategory + "&sidebarType=" + sidebarType,
			type: 'GET',
			cache: false,
			success: function (data) {
				$.fn.OpenModal('new-report-modal', "new-report", data)
			}
		});
	})

	$(".edit-report").click(function (e) {
		var reportID = $(this).attr("value")
		$.ajax({
			url: "/Protocols/CreateReport?reportID=" + reportID,
			type: 'GET',
			cache: false,
			success: function (data) {
				$(".render-body").html(data)
			}
		});
    })
});