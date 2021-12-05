$(document).ready(function () {
	var docType = $("#document-type-select").val();

	if (docType != null) $(".cs-doc-" + docType).show(0);

	$("#document-type-select").on("change", function (e) {
		e.preventDefault();

		docType = $(e.target).val();

		$(".cs-doc-form-hidden").fadeOut(0);
		$(".cs-doc-" + docType).fadeIn(125);
	});
});