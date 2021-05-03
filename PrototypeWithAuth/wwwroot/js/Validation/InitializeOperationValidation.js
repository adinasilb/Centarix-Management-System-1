$(function () {
$('.ordersItemForm').validate({
     normalizer: function( value ) {
    return $.trim( value );
  },

});
    });

