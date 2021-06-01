$('.dropdown-main').off("click").on("click", function () {
		$(this).attr('tabindex', 1).focus();
		//$(this).toggleClass('active');
		$(this).find('.dropdown-menu').slideToggle(300);
	});
	$('.dropdown-main').focusout(function () {
		$(this).removeClass('active');
		$(this).find('.dropdown-menu').slideUp(300);
	});
$("body").on("click",'.dropdown-main .dropdown-menu li', function () {
		$(this).parents('.dropdown-main').find('span:not(.caret)').text($(this).text());
		$(this).parents('.dropdown-main').find('input').attr('value', $(this).attr('id'));
	});
	/*End Dropdown Menu*/


$("body").off("click", '.dropdown-menu li').on("click", '.dropdown-menu li', function () {
		var input = '<strong>' + $(this).parents('.dropdown').find('input').val() + '</strong>',
			msg = '<span class="msg">Hidden input value: ';
		$('.msg').html(msg + input + '</span>');
	});



	$('.dropdown-multiple').click(function () {
		$(this).attr('tabindex', 1).focus();
		$(this).addClass('active');
		$(this).find('.dropdown-menu-multiple').slideToggle(300);
	});
	$('.dropdown-multiple').focusout(function () {
		$(this).removeClass('active');
		$(this).find('.dropdown-menu-multiple').slideUp(300);
	});
	$('.dropdown-multiple .dropdown-menu div label').click(function () {
		//$(this).parents('.dropdown').find('span').text($(this).text());
		$(this).parents('.dropdown-multiple').find('input').attr('value', $(this).attr('id'));
		$(this).parents('.dropdown-multiple').addClass('active');
	});
	$('.dropdown-multiple .dropdown-menu div .form-check-input').click(function () {
		//$(this).parents('.dropdown').find('span').text($(this).text());
		console.log("in multiple")
		//alert("in multiple");
		$(this).parents('.dropdown-multiple').find('span:not(.caret)').append($(this).find('label').text());
	});