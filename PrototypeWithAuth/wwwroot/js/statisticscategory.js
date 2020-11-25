$(function () {
	$(".parent-category-button").off("click").on("click", function () {
		alert("Parent cat clicked");
		var parentCatID = $(this).val();
		var months = [];
		$(".months-selected").each(function () {
			months.push($(this).val());
		});
		var catTypes = [];
		catTypes = $("#CategoryTypesSelected").val();
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
		var url = "/Expenses/_SubCategoryTypes?ParentCategoryId=" + parentCatID + "&Months=" + months + "&Year=" + year;

		$.ajax({
			async: true,
			url: url,
			type: 'GET',
			cache: false,
			success: function (data) {
				$(".subcat-table").empty();
				$(".subcat-table").html(data);
			}
		});
	});
});