function getUserTime(time) {
    var userTime = new Date(Date.parse(time))
    var hours = userTime.getHours();
    var ampm = "AM";
    if (hours > 12) {
        hours = hours - 12;
        ampm = "PM";
    }
    if (hours === 12) {
        ampm = "PM"
    }
    if (hours === 0) {
        hours = 12;
    }
    var minutes = userTime.getMinutes();
    if (minutes < 10) {
        minutes = "0" + minutes;
    }
    var seconds = userTime.getSeconds();
    if (seconds < 10) {
        seconds = "0" + seconds;
    }
    var correctTime = (userTime.getMonth() + 1).valueOf() + "/" + (userTime.getDate()).valueOf() + "/" + userTime.getFullYear().valueOf() + " " + hours.valueOf() + ":" + minutes + ":" + seconds + " " + ampm
    return (correctTime)
}

function transDate2(count, created, updated) {
    var time1 = getUserTime(created);
    $("#transaction_create_" + count).append(time1);
    var time2 = getUserTime(updated);
    $("#transaction_update_" + count).append(time2);
}
function transDate(count, created) {
    var time1 = getUserTime(created);
    $("#transaction_create_" + count).append(time1);
}

function DeleteCategory(id,name,description,budgetId,inDefault) {
    $("#delete_category_modal").modal("show");
    $("#delete_category_id").val(id);
    $("#delete_category_name").html(name);
    $("#delete_category_description").html(description);
    if(budgetId != 0)
    {
        $("#delete_category_budgetId").val(budgetId);
    }
    if(inDefault == 'True')
    {
        $("#delete_category_default").val(true);
    }
    else
    {
        $("#delete_category_default").val(false);
    }
};

function EditCategory(id, name, description, budgetId, inDefault) {
    $("#edit_category_modal").modal("show");
    $("#edit_category_id").val(id);
    $("#edit_category_name").val(name);
    $("#edit_category_description").val(description);
    if (budgetId != 0)
    {
        $("#EditBudgetId").val(budgetId);
    }
    if(inDefault == 'True')
    {
        $("#edit_category_default").val(true);
    }
    else
    {
        $("#edit_category_default").val(false);
    }
};

function DeleteTrans(id, name, description, expense, value) {
    $("#delete-trans-modal").modal("show");
    $("#delete_trans_id").val(id);
    $("#delete_trans_name").html(name);
    $("#delete_trans_description").html(description);
    if (expense == "True") {
        $("#delete_trans_value").removeClass("text-success");
        $("#delete_trans_value").html("-$" + value);
        $("#delete_trans_value").addClass("text-danger");
    }
    else {
        $("#delete_trans_value").removeClass("text-danger");
        $("#delete_trans_value").html("+$" + value);
        $("#delete_trans_value").addClass("text-success");
    }
}

function EditTrans(id, bankId, name, description, value, expense, reconciled, reconciledValue, categoryId, typeId) {
    $("#edit-trans-modal").modal("show");
    $("#edit_transaction_id").val(id);
    $("#edit_transaction_id").val(bankId);
    $("#edit_transaction_name").val(name);
    $("#edit_transaction_description").val(description);
    $("#edit_transaction_value").val(value);
    if (expense == "True") {
        $("#edit_transaction_expense").prop("checked", true);
    }
    else {
        $("#edit_transaction_expense").prop("checked", false);
    }
    if (reconciled == "True") {
        $("#edit_transaction_reconciled").prop("checked", true);
        $("#edit_transaction_reconciledvalue").prop('disabled', false)
    }
    else {
        $("#edit_transaction_reconciled").attr("checked", false);
    }
    $("#edit_transaction_reconciledvalue").val(reconciledValue);
    $("#EditCategoryId").val(categoryId);
    $("#EditTypeId").val(typeId);
}

function EditBankTrans(id,name,description,value,expense,reconciled,reconciledValue,categoryId,typeId) {
    $("#edit-trans-modal").modal("show");
    $("#edit_transaction_id").val(id);
    $("#edit_transaction_name").val(name);
    $("#edit_transaction_description").val(description);
    $("#edit_transaction_value").val(value);
    if (expense == "True")
    {
        $("#edit_transaction_expense").prop("checked", true);
    }
    else
    {
        $("#edit_transaction_expense").prop("checked", false);
    }
    if (reconciled == "True")
    {
        $("#edit_transaction_reconciled").prop("checked", true);
        $("#edit_transaction_reconciledvalue").prop('disabled', false)
    }
    else
    {
        $("#edit_transaction_reconciled").attr("checked", false);
    }
    $("#edit_transaction_reconciledvalue").val(reconciledValue);
    $("#EditCategoryId").val(categoryId);
    $("#EditTypeId").val(type);
}

function DeleteBudget(id, name, description, expense, value) {
    $("#delete-budget-modal").modal("show");
    $("#delete_budget_id").val(id);
    $("#delete_budget_name").html(name);
    $("#delete_budget_description").html(description);
    if (expense == "True") {
        $("#delete_budget_value").removeClass("text-success");
        $("#delete_budget_value").html("-$" + value);
        $("#delete_budget_value").addClass("text-danger");
    }
    else {
        $("#delete_budget_value").removeClass("text-danger");
        $("#delete_budget_value").html("+$" + value);
        $("#delete_budget_value").addClass("text-success");
    }
}

function EditBudget(id, name, description, value, expense, categories,disabled) {
    $("#edit-budget-modal").modal("show");
    $("#edit_budget_id").val(id);
    $("#edit_budget_name").val(name);
    $("#edit_budget_description").val(description);
    $("#edit_budget_value").val(value);
    if (expense == "True") {
        $("#edit_budget_expense").prop("checked", true);
    }
    else {
        $("#edit_budget_expense").prop("checked", false);
    }
    $("#EditCategoryId").val(categories);
    $("#EditCategoryId").multiselect("refresh");
    $("EditCategoryId option").removeAttr("disabled");
    $("#EditCategoryId").multiselect("refresh");
    disabled.forEach(function(i){
        $('#EditCategoryId option[value='+i+']').prop('disabled', true);
        $("#EditCategoryId").multiselect("refresh");
    });
}

function transColor(expense) {
    if (expense == "True") {
        $("#balance").removeClass("panel-success")
        $("#balance").addClass("panel-warning")
    }
    else {
        $("#balance").removeClass("panel-success")
        $("#balance").addClass("panel-danger")
    }
}

function remainColor(value, spent, count) {
    if ((value - spent) / value > 0.25) {
        $("#remaining_" + count).removeClass("progress-bar-success", "progress-bar-warning", "progress-bar-info", "progress-bar-danger");
        $("#remaining_" + count).addClass("progress-bar-success")
    }
    else if ((value - spent) / value < 0.25 && (value - spent) / value > 0.05) {
        $("#remaining_" + count).removeClass("progress-bar-success", "progress-bar-warning", "progress-bar-info", "progress-bar-danger");
        $("#remaining_" + count).addClass("progress-bar-warning")
    }
    else if ((value - spent) / value < 0.05 && (value - spent) / value > -0.02) {
        $("#remaining_" + count).removeClass("progress-bar-success", "progress-bar-warning", "progress-bar-info", "progress-bar-danger");
        $("#remaining_" + count).removeClass("progress-bar-info");
    }
    else {
        $("#remaining_" + count).removeClass("progress-bar-success", "progress-bar-warning", "progress-bar-info", "progress-bar-danger");
        $("#remaining_" + count).removeClass("progress-bar-danger");
    }
}

function spendColor(value, spent, count) {
    if (spent / value < 0.75) {
        $("#spent_" + count).removeClass("progress-bar-success", "progress-bar-warning", "progress-bar-info", "progress-bar-danger");
        $("#spent_" + count).addClass("progress-bar-success")
    }
    else if (spent / value > 0.75 && spent / value < 0.9) {
        $("#spent_" + count).removeClass("progress-bar-success", "progress-bar-warning", "progress-bar-info", "progress-bar-danger");
        $("#spent_" + count).addClass("progress-bar-warning")
    }
    else if (spent / value > 0.9 && spent / value < 1.01) {
        $("#spent_" + count).removeClass("progress-bar-success", "progress-bar-warning", "progress-bar-info", "progress-bar-danger");
        $("#spent_" + count).addClass("progress-bar-info");
    }
    else {
        $("#spent_" + count).removeClass("progress-bar-success", "progress-bar-warning", "progress-bar-info", "progress-bar-danger");
        $("#spent_" + count).addClass("progress-bar-danger");
    }
}

$(document).ready(function () {
    $("#CategoryId").multiselect({
        maxHeight: 300,
        enableFiltering: true,
        enableCaseInsensitiveFiltering: true
    });

    $("#EditCategoryId").multiselect({
        maxHeight: 300,
        enableFiltering: true,
        enableCaseInsensitiveFiltering: true
    });

    if (top.location.pathname === '/Home/Index') {
        $("#side-menu li").removeClass("selected");
        $("#home-nav").addClass("selected");
    }
    if (top.location.pathname === '/Home') {
        $("#side-menu li").removeClass("selected");
        $("#home-nav").addClass("selected");
    }
    if (top.location.pathname === '/Home/') {
        $("#side-menu li").removeClass("selected");
        $("#home-nav").addClass("selected");
    }
    if (top.location.pathname === '/') {
        $("#side-menu li").removeClass("selected");
        $("#home-nav").addClass("selected");
    }
    if (top.location.pathname === '/Households/') {
        $("#side-menu li").removeClass("selected");
        $("#house-nav").addClass("selected");
    }
    if (top.location.pathname === '/Households') {
        $("#side-menu li").removeClass("selected");
        $("#house-nav").addClass("selected");
    }
    if (top.location.pathname === '/Households/Index') {
        $("#side-menu li").removeClass("selected");
        $("#house-nav").addClass("selected");
    }
});
