$(function () {
	$(".parent-project-button").on("click", function () {
		var projectId = $(this).val();
		console.log("parent project button clicked with value of: " + projectId);

		//set the view:
		var colorClass = "graduated-table-background-color";
		var borderLeftClass = "statistics-project-selected";
		$("tr").removeClass(borderLeftClass);
		$("button").removeClass(colorClass);
		$(this).parent().parent().children("td").children("button").addClass(colorClass);
		$(this).parent().parent().addClass(borderLeftClass);

		//fill the second div:
		var url = "/Expenses/_StatisticsSubProjects?ProjectID=" + projectId;

		$.ajax({
			async: true,
			url: url,
			type: 'GET',
			cache: false,
			success: function (data) {
				$(".subprojects-table").empty();
				$(".subprojects-table").html(data);
			}
		});
	});
});