var orderTotal = 0;

// disable submit button when order total is less than 300
function checkOrderTotal(total) {
	var submitButton = $("#submit-order")
	if (total < 300) return submitButton.prop("disabled", true);
	return submitButton.prop("disabled", false)
};

// recalculate order total
function recalculateOrderTotal() {
	var arrQuantity = $("[aria-label=quantity]");
	var arrAmount = $(".cs-cart-product__amount > p:first-child span");

	orderTotal = 0;

	arrAmount.each(function (index, el) {
		let price = parseFloat(el.textContent.split(" ")[0]);
		let subtotal = price * parseFloat(arrQuantity[index].value);

		$("#subtotal-" + index).text("Итого: " + subtotal.toFixed(2) + " руб.");

		orderTotal += subtotal;
	});

	var orderTotalText = $(".cs-cart-total-text > p").text("Итого за заказ: " + orderTotal.toFixed(2) + " руб.");

	checkOrderTotal(orderTotal);
};

// send ajax request to change quantity of product in cart
function changeProductQuantity(id, quantity) {
	
	$.ajax({
		type: "POST",
		url: "/Cart?handler=ChangeProductQuantity",
		beforeSend: function (xhr) {
			xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
		},
		data: { itemId: id, newQuantity: quantity },
		cache: false,
		success: function () {
			enableAllInputs();
			$(".spinner").css("opacity", "0");
			recalculateOrderTotal();
		}
	});
};

function disableNotActiveInputs(index) {
	$(".cs-btn-minus").attr("disabled", true);
	$(".cs-btn-plus").attr("disabled", true);
	$(".input-number").attr("disabled", true);
	$("#submit-order").attr("disabled", true);

	$(".cs-btn-minus")[index].disabled = false;
	$(".cs-btn-plus")[index].disabled = false;
	$(".input-number")[index].disabled = false;
}

function enableAllInputs() {
	$(".cs-btn-minus").attr("disabled", false);
	$(".cs-btn-plus").attr("disabled", false);
	$(".input-number").attr("disabled", false);
	$("#submit-order").attr("disabled", false);
}

$(document).ready(function () {
	recalculateOrderTotal();

	var inputTimer;
	var doneInputInterval = 750;

	$("[aria-label=quantity").on("cut copy paste contextmenu", function (e) {
		e.preventDefault();
	});

	$("[aria-label=quantity]").on("input", function (e) {
		e.preventDefault();

		clearTimeout(inputTimer);

		var wasFocused = $(e.target).is(":focus") ? true : false;

		let input = e.target;
		let targetId = e.target.getAttribute("id").split("-")[1];

		disableNotActiveInputs(targetId);
		if (wasFocused) $(e.target).focus();

		let invalid = input.value == '' || input.value == 0 || input.value > parseInt($(input).attr("max"))

		if (invalid) input.value = 1;
		
		var spinner = $(".spinner");
		$(spinner[targetId]).css("opacity", "1");

		inputTimer = setTimeout(changeProductQuantity, doneInputInterval, targetId, input.value);
	});
});

// hiding / showing legal entity info form
$(document).ready(function() {
	$("input:radio[data-cs-radio=customer]").on("change", function (e) {
		e.preventDefault();

		if (!$("#radio-legal").prop("checked")) {
			$(".cs-cart-form-legal-header").fadeOut(200);
			$(".cs-cart-form-legal").fadeOut(200);
			$("label[for=customer-name]").text("ФИО");

			$("label[for=customer-contact-person").removeClass("cs-required");
			$("label[for=customer-contact-person").fadeOut(200);
			$("#customer-contact-person").fadeOut(200);
		};

		if ($("#radio-legal").prop("checked")) {
			$(".cs-cart-form-legal-header").fadeIn(200);
			$(".cs-cart-form-legal").fadeIn(200);
			$("label[for=customer-name]").text("Название компании");

			$("label[for=customer-contact-person").addClass("cs-required");
			$("label[for=customer-contact-person").fadeIn(200);
			$("#customer-contact-person").fadeIn(200);
		}
	})
});

// deleting product from cart
$(document).ready(function () {
	$(".product-remove").on("click", function (e) {
		e.preventDefault();

		let targetId = e.target.getAttribute("id").split("-")[2];

		$.ajax({
			type: "POST",
			url: "/Cart?handler=RemoveProductFromCart",
			beforeSend: function (xhr) {
				xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
			},
			data: { itemId: targetId },
			cache: false,
			success: function () {
				location.reload(true);
			}
		});
	});
});

// validate customer model manually because jquery validation doesn't work somewhy
$(document).ready(function () {
	$(".cs-required ~ input").on("keyup", function () {
		$(this).removeClass("cs-invalid");
	});	

	$("#submit-order").on("click", function (e) {
		var invalid = false;

		$(".cs-required ~ input").each(function () {
			if ($(this).val().trim() == "") {
				$(this).addClass("cs-invalid");
				invalid = true;
			};
		});

		if (invalid) {
			alert("Пожалуйста, заполните обязательные поля!");
			return false;
		};

		return true;
	});
});