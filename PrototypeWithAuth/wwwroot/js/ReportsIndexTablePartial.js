$(function () {
	$(".change-month").on("change", function () {
		$.fn.ajaxPartialIndexTable();
	});
	$(".change-year").on("change", function () {
		var years = $("#select-years").val();
		var currency = $("#select-months").val();
		console.log("year: " + year + " , currency: " + currency);
		var url = "/Protocols/_ReportsIndexTable?months=" + months + "&years=" + years;
		$.fn.ajaxPartialIndexTable(url, "._IndexTable", "GET" );
	});
});