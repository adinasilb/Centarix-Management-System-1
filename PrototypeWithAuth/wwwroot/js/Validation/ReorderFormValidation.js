$('.reorderForm').validate({
	rules: {
		 normalizer: function( value ) {
    return $.trim( value );
  },

		"RequestItemViewModel.Requests[0].Unit": {
			required: true,
			number: true,
			min: 1,
			integer: true
		},
		"RequestItemViewModel.Requests[0].Product.SubUnit": {
			required: true,
			number: true,
			min: 0
		},
		"RequestItemViewModel.Requests[0].Product.SubSubUnit": {
			required: true,
			number: true,
			min: 0,
		},
		"RequestItemViewModel.Requests[0].Cost": {
			required: true,
			number: true,
			min: 1
		},

		"RequestItemViewModel.Requests[0].Product.UnitTypeID": "selectRequired",
		"RequestItemViewModel.Requests[0].Product.SubUnitTypeID": "selectRequired",
		"RequestItemViewModel.Requests[0].Product.SubSubUnitTypeID": "selectRequired"

	},

});