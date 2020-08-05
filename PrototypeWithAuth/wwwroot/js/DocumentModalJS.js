//$(".upload-file").on("click", function (e) {
//	e.preventDefault();
//	e.stopPropagation();
//	return false;
//});

$(".save-document-files").on("click", function (e) {
    e.preventDefault();
    e.stopPropagation();
    var filetype = $(this).data("string");
    console.log("filetype: " + filetype);
    var files = $(".file-select").val();
    console.log("files: " + files);
    console.log("files.filetype : " + files.filetype);
    console.log("files.files : " + files.files);
    $.ajax({
        async: true,
        url: "AppData/FileSaver/SaveFileUploads?FileType=" + filetype + "&Files=" + files,
    });
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