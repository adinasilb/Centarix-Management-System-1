
$('.saveOffDayForm').validate({
	rules: {
		 normalizer: function( value ) {
    return $.trim( value );
  },
		"dateSelected": {
			required:true
		}


	}, 
	messages : {
		"dateSelected": {
			required: "must select at least one date"
		},
	}
		
});
