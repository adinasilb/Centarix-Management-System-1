
function ajaxPartialIndexTable(url, viewClass, type, formdata, modalClass = "") {

	var contentType = true;
	var processType = true;
	if (formdata == undefined) {
		console.log("formdata is undefined");
		formdata = {
			PageNumber: $('.page-number').val(),
			PageType: $('#masterPageType').val(),
			SectionType: $('#masterSectionType').val(),
			SidebarType: $('#masterSidebarType').val(),
			SidebarFilterID: $('.sideBarFilterID').val(),
		};
		console.log(formdata);
	}
	else {
		$.fn.CloseModal(modalClass);
		contentType = false;
		processType = false;
	}

	$.ajax({
		contentType: contentType,
		processData: processType,
		async: true,
		url: url,
		data: formdata,
		traditional: true,
		type: type,
		cache: false,
		success: function (data) {
			$(viewClass).html(data);
			$("#loading").hide();
			return true;
		}
	});

	return false;
}

$(".load-protocol").click(function(e){
	var val = $(this).val();
	$.ajax({
		url: "/Protocols/_IndexTableWithEditProtocol?protocolID="+val,
		async: true,
		type: "GET",
		success: function (data) {
			$("._IndexTable").html(data);
			$(".mdb-select").materialSelect();
			return true;
		}
	});


});

var protocolFavoritesHasRun = false; //This is preventing the double click
$(".protocol-favorite").off("click").on("click", function (e) {
	//$(this).off("click");
	//alert("in click fr");
	if (!protocolFavoritesHasRun) {
		protocolFavoritesHasRun = true;
		$("#loading").show();
		var protocolFavorite = $(this);
		//alert(" in favorite request fx");
		var emptyHeartClass = "icon-favorite_border-24px";
		var fullHeartClass = "icon-favorite-24px";
		var unfav = "protocol-unlike";
		var title = "Favorite";
		var FavType = "favorite";
		var sidebarType = $('#masterSidebarType').val();
		if (protocolFavorite.hasClass("protocol-unlike")) {
			FavType = "unlike";
			$.ajax({
				async: true,
				url: "/Protocols/FavoriteProtocol/?protocolID=" + protocolFavorite.attr("value") + "&Favtype=" + FavType + '&sidebarType=' + sidebarType,
				traditional: true,
				type: "GET",
				cache: false,
				success: function (data) {
					protocolFavoritesHasRun = false;
					protocolFavorite.children("i").addClass(emptyHeartClass);
					protocolFavorite.children("i").removeClass(fullHeartClass);
					protocolFavorite.attr("data-original-title", title);
					protocolFavorite.removeClass(unfav);
					$("#loading").hide();
					if (sidebarType == 'Favorites') {
						$('[data-toggle="tooltip"]').tooltip('dispose'); //is this the right syntax?
						$('._IndexTable').html(data);
					}
				}
			})
		}
		else {
			title = "Unfavorite";
			$.ajax({
				async: true,
				url: "/Protocols/FavoriteProtocol/?protocolID=" + protocolFavorite.attr("value") + "&Favtype=" + FavType + '&sidebarType=' + sidebarType,
				traditional: true,
				type: "GET",
				cache: false,
				success: function (data) {
					protocolFavoritesHasRun = false;
					protocolFavorite.children("i").removeClass(emptyHeartClass);
					protocolFavorite.children("i").addClass(fullHeartClass);
					protocolFavorite.attr("data-original-title", title);
					protocolFavorite.addClass(unfav);
					$("#loading").hide();

				}
			})
		}
		//$.fn.FavoriteRequests(requestFavorite);
	}
});

