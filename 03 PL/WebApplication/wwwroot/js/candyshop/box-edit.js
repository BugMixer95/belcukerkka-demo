// constructor for an object that is sent to receive available weight types
function weightParameters(weight, boxParentId) {
    this.weight = weight;
    this.boxParentId = boxParentId;
};

// getting list of weight type names which are related to the chosen weight
$(document).ready(function () {
    $('#weight-select').change(function () {
        var boxParentId = $('#box-parent-id').val();

        var selectedWeight = $('#weight-select').val();
        var weightTypeNamesSelect = $('#weight-type-name-select');
        
        weightTypeNamesSelect.empty();
        weightTypeNamesSelect.prop("disabled", false);

        var paramsToSend = new weightParameters(selectedWeight, boxParentId);
        var paramsJson = JSON.stringify(paramsToSend);

        if (paramsJson != null && paramsJson != '') {
            $.ajax({
                type: "POST",
                url: "/Admin/Boxes/EditChild?handler=GetWeightTypeNames",
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                data: paramsJson,
                contentType: "json; charset=utf-8",
                success: function (responseData) {
                    weightTypeNamesSelect.append($('<option/>', {
                        value: -1,
                        text: "Выберите тип состава",
                        class: "font-weight-bold"
                    }));

                    let weightTypeMainOption = $("#weight-type-name-select option[value=-1]");
                    weightTypeMainOption.prop("disabled", "disabled");
                    weightTypeMainOption.prop("selected", "selected");
                    
                    $.each(responseData, function (index, element) {
                        weightTypeNamesSelect.append("<option value='" + element.value + "'>" + element.text + "</option>");
                    });
                },
                failure: function (response) {
                    alert(response.responseText);
                },
                error: function (response) {
                    alert(response.responseText);
                },
            });
        };
    });
});

// setting weight type name selector disabled if null
$(document).ready(function () {
    if ($("#weight-select").val() == null) {
        $("#weight-type-name-select").prop("disabled", true);
    }
    else {
        $("#weight-type-select-name").prop("disabled", false);
    };
});

// rounding value to 2 decimals
function round(value) {
    value = +value;

    if(isNaN(value))
        return NaN;

    value = value.toString().split('e');
    value = Math.round(+(value[0] + 'e' + (value[1] ? (+value[1] + 2) : 2)));

    value = value.toString().split('e');
    return (+(value[0] + 'e' + (value[1] ? (+value[1] - 2) : -2))).toFixed(2);
};

// changing full price on price input change
$(document).ready(function () {
    $("#price-input").change(function () {
        var price = $("#price-input").val();
        $("#price-input").val(round(price));
        var newFullPrice = price * 1.25;
        $("#full-price-input").val(round(newFullPrice));
    });
});

// validate weight name select field
$(document).ready( function () {
    $("button[type=submit]").on("click", function (e) {
        var field = $("#weight-type-name-select");
        var value = $(field).val();

        if (value == null) {
            $(field).addClass("cs-invalid");
            return false;
        }

        return true;
    });
});