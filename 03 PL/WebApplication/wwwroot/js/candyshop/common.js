// initializing all tooltips on the page
$(document).ready(function () {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl)
    });
});

// setting the name of chosen file in the input field
$(document).ready(function () {
    $('.custom-file-input').on("change", function () {
        let fileName = $(this).val().split("\\").pop();
        $(this).next('.custom-file-label').html(fileName);
    });
});

// highlighting hovered table row
$(document).ready(function () {
    $("tbody tr td").on("mouseover", function () {
        var attr = $(this).attr("colspan");
        if (!(attr > 0)) {
            $(this).parent().addClass("table-active");
        };
    });
    $("tbody tr td").on("mouseout", function () {
        $(this).parent().removeClass("table-active");
    });
});

// adding or subtracting the value on button click + restricting user to enter only numbers
$(document).ready(function () {
    $(".input-decimal").on("keyup", function () {
        var val = $(this).val();

        if (isNaN(val)) {
            val = val.replace(/[^0-9\.]/g, '');
            if (val.split('.').length > 2) {
                val = val.replace(/\.+$/, '');
            };
        };
    });

    $(".input-number").on("keypress", function (evt) {
        var ASCIICode = (evt.which) ? evt.which : evt.keyCode
        if (ASCIICode > 31 && (ASCIICode < 48 || ASCIICode > 57))
            return false;
        return true;
    });

    var input = $("[aria-label=quantity]");

    $(".cs-btn-plus").on("click", function (e) {
        e.preventDefault();

        let inputToChange;

        if (input.length > 1) {
            let targetId = e.target.getAttribute("id").split("-")[2];
            inputToChange = input[targetId];
        }
        else {
            inputToChange = input[0];
        }

        var currentValue = parseInt(inputToChange.value);

        inputToChange.value = currentValue + 1;
        inputToChange.dispatchEvent(new Event("input"));
    });

    $(".cs-btn-minus").on("click", function (e) {
        e.preventDefault();

        let inputToChange;

        if (input.length > 1) {
            let targetId = e.target.getAttribute("id").split("-")[2];
            inputToChange = input[targetId];
        }
        else {
            inputToChange = input[0];
        }

        var currentValue = parseInt(inputToChange.value);

        if (currentValue > inputToChange.min) {
            inputToChange.value = currentValue - 1;
        }

        inputToChange.dispatchEvent(new Event("input"));
    });
});

$(document).ready(function () {
    $(".cs-navbar__burger").on("click", function (e) {
        e.preventDefault();

        $(".cs-navbar__burger, .cs-navbar__menu").toggleClass("menu-active");
        $("body").toggleClass("lock");
    });
});

$(document).ready(function () {
    $.ajax({
        type: "POST",
        url: "/Cart?handler=CheckCart",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        cache: false,
        success: function (responseData) {
            if (!responseData.isEmpty) $(".cs-navbar__cart").attr("data-cs-empty", "false");
            else $(".cs-navbar__cart").attr("data-cs-empty", "true");
        }
    });
});

$(document).ready(function () {
    $(".contact__main").on("click", function (e) {
        e.preventDefault();

        $(e.target).toggleClass("contacts-active");
    });

    $(".cs-index-weights > div").on("click", function (e) {
        e.preventDefault();

        if ($(e.target).attr("disabled") != undefined) return;

        $(".cs-index-weights > div").attr("disabled", false);
        $(e.target).attr("disabled", true);

        var targetWeight = $(e.target).attr("id").split("-")[2];

        $.ajax({
            type: "POST",
            url: "/Index?handler=ChangeWeights",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: { weight: targetWeight },
            cache: false,
            success: function (responseData) {
                $("#catalog").children().fadeOut(250);
                $("#catalog").children().remove();
                if (responseData.length != 0) {
                    $.each(responseData.itemsToLoad, function (index, element) {
                        $("#catalog").append(element).hide().fadeIn(500);
                    });
                }
            }
        });
    });
});