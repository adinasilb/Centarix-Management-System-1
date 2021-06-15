
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

$(".share-object").on("click", function (e) {
		var url = "/Protocols/ShareModal?ID=" + $(this).val() + "&ModelsEnum=Protocol"; 
		$.ajax({
			async: true,
			url: url,
			type: 'GET',
			cache: true,
			success: function (data) {
				$.fn.OpenModal("shared-modal", "share-modal", data);
				$.fn.EnableMaterialSelect('#ApplicationUserIDs', 'select-options-ApplicationUserIDs')
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
		var requestFavorite = $(this);
		//alert(" in favorite request fx");
		var emptyHeartClass = "icon-favorite_border-24px";
		var fullHeartClass = "icon-favorite-24px";
		var unfav = "protocol-unlike";
		var title = "Favorite";
		var FavType = "favorite";
		var sidebarType = $('#masterSidebarType').val();
		if (requestFavorite.hasClass("request-unlike")) {
			FavType = "unlike";
			$.ajax({
				async: true,
				url: "/Protocols/ProtocolFavorite/?protocolID=" + requestFavorite.attr("value") + "&Favtype=" + FavType + '&sidebarType=' + sidebarType,
				traditional: true,
				type: "GET",
				cache: false,
				success: function (data) {
					protocolFavoritesHasRun = false;
					requestFavorite.children("i").addClass(emptyHeartClass);
					requestFavorite.children("i").removeClass(fullHeartClass);
					requestFavorite.attr("data-original-title", title);
					requestFavorite.removeClass(unfav);
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
				url: "/Requests/RequestFavorite/?requestID=" + requestFavorite.attr("value") + "&Favtype=" + FavType + '&sidebarType=' + sidebarType,
				traditional: true,
				type: "GET",
				cache: false,
				success: function (data) {
					protocolFavoritesHasRun = false;
					requestFavorite.children("i").removeClass(emptyHeartClass);
					requestFavorite.children("i").addClass(fullHeartClass);
					requestFavorite.attr("data-original-title", title);
					requestFavorite.addClass(unfav);
					$("#loading").hide();

				}
			})
		}
		//$.fn.FavoriteRequests(requestFavorite);
	}
});

