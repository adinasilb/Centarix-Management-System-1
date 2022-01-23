/**
 * Created by Malal91 and Haziel
 * Select multiple email by jquery.email_multiple
 * **/

(function ($) {

	$.fn.email_multiple = function (options) {

		let defaults = {
			reset: false,
			fill: false,
			data: null
		};

		let settings = $.extend(defaults, options);
		let email = "";

		return this.each(function () {
			//$(this).after("<span class=\"to-input\">Email :</span>\n" +
			//    "<div class=\"all-mail\"></div>\n" +
			//    "<input type=\"text\" name=\"email\" class=\"enter-mail-id\" placeholder=\"Enter Email ...\" />");

			//Adina replaced...
			$(this).after("" +
				"<div class=\"all-mail\"></div>\n" +
				"<input type=\"text\" name=\"email\" class=\"enter-mail-id\" placeholder=\"Enter Email ...\" />");
			let $orig = $(this);
			let $element = $('.enter-mail-id');

			var dangerClass = "danger-box-shadow";//Adina

			$element.keydown(function (e) {
				$element.css('border', '');
				if (e.keyCode === 13 || e.keyCode === 32) {
					let getValue = $element.val().toString().toLowerCase();
					if (/^[a-z0-9._-]+@[a-z0-9._-]+\.[a-z]{2,6}$/.test(getValue)) {
						$('.all-mail').append('<span class="email-ids">' + getValue + '<span class="remove-email">&nbsp;&times;&nbsp;</span></span>');
						$element.val('');

						email += getValue + ';'
						$element.removeClass(dangerClass);

						$.fn.CheckListLength();
						$.fn.AddToHiddenIds(getValue);
					} else {
						$element.addClass(dangerClass);
					}

					e.preventDefault(); //Adina
					e.stopPropagation(); //Adina
				}

				$orig.val(email.slice(0, -1))

			});

			$(document).on('click', '.remove-email', function () {
				if (!$(this).attr("disabled")) {
					$(this).parent().remove();
					$.fn.CheckListLength();
					$.fn.RemoveFromHiddenIds($(this).parent().html().substr(0, $(this).parent().html().indexOf('<')));
				}
			});

			if (settings.data) {
				$.each(settings.data, function (x, y) {
					y = y.toString().toLowerCase();
					if (/^[a-z0-9._-]+@[a-z0-9._-]+\.[a-z]{2,6}$/.test(y)) {
						$('.all-mail').append('<span class="email-ids">' + y + '<span class="remove-email">&times;</span></span>');
						$element.val('');

						email += y + ';'
					} else {
						$element.css('border', '1px solid red')
					}
				})

				$orig.val(email.slice(0, -1))
			}

			if (settings.reset) {
				$('.email-ids').remove()
			}

			return $orig.hide()
		});

	};

})(jQuery);
