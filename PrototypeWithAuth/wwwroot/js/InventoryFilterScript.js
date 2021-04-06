$('.filter-col').mouseover(function () {
    console.log('hurray');
    $('.filter-col').focus();
});

$('#invFilterPopover').on('shown.bs.popover', function () {
    $('body').addClass('popover-open');
})
