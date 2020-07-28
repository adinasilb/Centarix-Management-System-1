$(".upload-file").on("click", function (e) {
	e.preventDefault();
	e.stopPropagation();
	return false;
});

//MDBootstrap Carousel
$('.carousel.carousel-multi-item.v-2 .carousel-item').each(function () {
    var next = $(this).next();
    if (!next.length) {
        next = $(this).siblings(':first');
    }
    next.children(':first-child').clone().appendTo($(this));

    for (var i = 0; i < 4; i++) {
        next = next.next();
        if (!next.length) {
            next = $(this).siblings(':first');
        }
        next.children(':first-child').clone().appendTo($(this));
    }
});