$('.reorderForm').validate({
	rules: {
		 normalizer: function( value ) {
    return $.trim( value );
  },

		"Requests[0].Unit": {
			required: true,
			number: true,
			min: 1,
			integer: true
		},
		"Requests[0].Product.SubUnit": {
			required: true,
			number: true,
			greaterThan: 0
		},
		"Requests[0].Product.SubSubUnit": {
			required: true,
			number: true,
			greaterThan: 0
		},
		"Requests[0].Cost": {
			required: true,
			number: true,
			min: 1
		},

		"Requests[0].Product.UnitTypeID": "selectRequired",
		"Requests[0].Product.SubUnitTypeID": "selectRequired",
		"Requests[0].Product.SubSubUnitTypeID": "selectRequired"

	},

});