
$('.termsModalForm').validate({
	rules: {
		 normalizer: function( value ) {
    return $.trim( value );
  },
		"SelectedTerm": "selectRequired",
		"Installments" :"required"
	}

});