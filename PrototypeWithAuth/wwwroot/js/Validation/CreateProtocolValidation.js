$(function () {
	$('.createProtocolForm').validate({
		normalizer: function (value) {
			return $.trim(value);
		},
		rules: {

			"ProtocolVersion.Protocol.Name": {
				required: true
			},
			"ProtocolVersion.Protocol.ProtocolSubCategory.ProtocolCategoryTypeID": {
				selectRequired: true
			},
			"ProtocolVersion.Protocol.ProtocolSubCategoryID": {
				selectRequired: true
			},
			
		},
	

	});
});

