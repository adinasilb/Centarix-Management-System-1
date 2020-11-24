$(function () {
	$(".parent-project-button").off("click").on("click", function () {
		var projectId = $(this).val();
		var months = [];
		$(".months-selected").each(function () {
			months.push($(this).val());
		});
		var year = $("#Year").val();

		//set the view:
		var colorClass = "graduated-table-background-color";
		var borderLeftClass = "statistics-project-selected";
		var arrowClass = "expenses-filter";
		$("tr").removeClass(borderLeftClass);
		$("button").removeClass(colorClass);
		$("i").removeClass(arrowClass);
		$(this).parent().parent().children("td").children("button").addClass(colorClass);
		$(this).parent().parent().children("td").children(".right-arrow-icon").children("i").addClass(arrowClass)
		$(this).parent().parent().addClass(borderLeftClass);

		//fill the second div:
		var url = "/Expenses/_StatisticsSubProjects?ProjectID=" + projectId + "&Months=" + months + "&Year=" + year;

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

	$("#Months").off("change").on("change", function () {
		//call projects
		$.fn.CallProjectsPartialView();

		//remove subprojects
		$(".subprojects-table").html("");
	});

	$("#select-years").off("change").on("change", function () {
		//call projects
		$.fn.CallProjectsPartialView();

		//remove subprojects
		$(".subprojects-table").html("");
	});

	$.fn.CallProjectsPartialView = function () {
		var months = [];
		months = $("#Months").val();
		var year = $("#select-years").val();

		var url = "/Expenses/_StatisticsProjects";

		$.ajax({
			async: true,
			url: url,
			type: 'GET',
			traditional: true,
			cache: false,
			data: { Months: months, Year: year },
			success: function (data) {
				$(".projects-table").empty();
				$(".projects-table").html(data);
			}
		});
	};
});