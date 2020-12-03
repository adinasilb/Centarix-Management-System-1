$(function () {
	$(".parent-category-button").off("click").on("click", function () {
		var parentCatID = $(this).val();
		var months = [];
		$(".months-selected").each(function () {
			months.push($(this).val());
		});
		var catTypes = [];
		catTypes = $("#CategoryTypeSelected").val();
		var years = [];
		$(".years-selected").each(function () {
			years.push($(this).val());
		});

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
		var url = "/Expenses/_SubCategoryTypes";

		$.ajax({
			async: true,
			url: url,
			type: 'GET',
			traditional: true,
			cache: false,
			data: { ParentCategoryId: parentCatID, categoryTypes: catTypes, Months: months, years, years },
			success: function (data) {
				$(".subcat-table").empty();
				$(".subcat-table").html(data);
			}
		});
	});


	$.fn.GetStatisticsCategoryPartial = function () {
		var years = [];
		years = $("#select-years").val();
		var months = [];
		months = $("#Months").val();
		var catTypes = [];
		catTypes = $("#CategoryTypeSelected").val();

		var url = "/Expenses/_CategoryTypes";

		$.ajax({
			async: true,
			url: url,
			type: 'GET',
			traditional: true,
			cache: false,
			data: { categoryTypes: catTypes, months: months, years: years },
			success: function (data) {
				$(".subcat-table").empty();
				$(".cat-table").empty();
				$(".cat-table").html(data);
			}
		});
	};
});