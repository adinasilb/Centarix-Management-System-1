$(function () {
	$('.createReportForm').validate({
		normalizer: function (value) {
			return $.trim(value);
		},
		rules: {
			"Report.ReportTitle": {
				required: true
			}
		}
	});
});