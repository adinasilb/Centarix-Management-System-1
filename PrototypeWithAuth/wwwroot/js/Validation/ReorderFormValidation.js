$('.reorderForm').validate({
	rules: {
		 normalizer: function( value ) {
    return $.trim( value );
  },

		"Request.Unit": {
			required: true,
			number: true,
			min: 1,
			integer: true
		},
		"Request.Product.SubUnit": {
			required: true,
			number: true,
			min: 1
		},
		"Request.Product.SubSubUnit": {
			required: true,
			number: true,
			min: 1,
		},
		"Request.Cost": {
			required: true,
			number: true,
			min: 1
		},

		"Request.Product.UnitTypeID": "selectRequired",
		"Request.Product.SubUnitTypeID": "selectRequired",
		"Request.Product.SubSubUnitTypeID": "selectRequired"

	},

});