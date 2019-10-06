(function($) {
    function toggleSubBudgets(budgetId) {
        let budgetItem = $(".budget-item[data-budget-id=" + budgetId + "]");
        let subBudgetContainer = $(".budget-container[data-budget-id=" + budgetId + "]");
        let hiddenArrowSvg = budgetItem.children("svg.hidden");
        let shownArrowSvg = $(hiddenArrowSvg).siblings("svg");

        subBudgetContainer.toggleClass("hidden");
        hiddenArrowSvg.toggleClass("hidden");
        shownArrowSvg.toggleClass("hidden");
    }

    $(document).on("click", ".budget-item__transaction-form-toggle", function() {
        $(this).siblings(".transaction-form").toggleClass("hidden");
    });

    $(document).on("click", ".budget-item__expand-toggle", function() {
        let budgetId = $(this).parents(".budget-item").data("budgetId");

        toggleSubBudgets(budgetId);
    });
})(jQuery);
