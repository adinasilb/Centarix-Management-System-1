$(function () {
	$('.createProtocolForm').validate({
		normalizer: function (value) {
			return $.trim(value);
		},
		rules: {

			"Protocol.Name": {
				required: true
			},
			"Protocol.UniqueCode": {
				required: true
			},
			"Protocol.ProtocolSubCategory.ProtocolCategoryTypeID": {
				selectRequired: true
			},
			"Protocol.ProtocolSubCategoryID": {
				selectRequired: true
			}
		}
	});
});

