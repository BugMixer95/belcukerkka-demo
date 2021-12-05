// loading more catalog items on load more button click
$(document).ready(function () {
	var loadCount = $(".cs-product-grid .cs-product").length;

	$("#load-more").on("click", function (e) {
		e.preventDefault();

		var isCatalogFiltered = $(".filter-toggle").attr("checked") != undefined;

		var weightsFilter = [];
		var minPriceFilter = 0;
		var maxPriceFilter = 0;

		if (isCatalogFiltered)
		{
			$(".filter-weight-check:checked").each(function (index, el) {
				var elementValue = parseInt(el.value);
				weightsFilter.push(elementValue); 
			});

			minPriceFilter = $(".cs-filter-price__min").val();
			maxPriceFilter = $(".cs-filter-price__max").val();
		}

		$.ajax({
			type: "POST",
			url: "/Catalog/Index?handler=LoadMore",
			beforeSend: function (xhr) {
				xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
			},
			data: { size: loadCount, weights: weightsFilter, minPrice: minPriceFilter, maxPrice: maxPriceFilter },
			cache: false,
			success: function (responseData) {
				if (responseData.length != 0) {
					$.each(responseData.itemsToLoad, function (index, element) {
						$("#catalog").append($(element).hide().fadeIn(500));
					});

					loadCount += responseData.itemsToLoad.length;
				}

				var ajaxModelCount = responseData.itemsCount - (loadCount);

				if (ajaxModelCount <= 0) {
					$("#load-more").hide().fadeOut(1000);
				};
			},
			error: function (xhr, status, error) {
				console.log(xhr.responseText);
			}
		});
	});
});