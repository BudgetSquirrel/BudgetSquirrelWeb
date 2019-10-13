(function($) {
    function toggleSubBudgets(budgetId) {
        let budgetItem = $(".budget-item[data-budget-id=" + budgetId + "]");
        let subBudgetContainer = $(".budget-container[data-budget-id=" + budgetId + "]");
        let hiddenArrowSvg = budgetItem.find(".budget-item__expand-toggle > svg.hidden");
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

    $(document).on("click", ".budget-item__btn-delete", function() {
        $(this).find("form").submit();
    });

    $(document).on("click", "*[data-toggle]", function() {
        /* Toggle the div that this button controls.
         *
         * This button could either be a button that only toggles
         * on, or it could be a button that only toggles off, or
         * it could be a button that toggles both on and off.
         *
         * Both the on and off toggle buttons will be toggled
         * themselves if they exist.
         *
         * To hook a button up to toggle a div, set the data attribute
         * of 'data-toggle' to a selector query for the element you
         * want the button to toggle.
         *
         * To use a toggle on and a toggle off button, Choose a group
         * name for this toggle that is unique on the page. Then set the
         * attribute of 'data-toggle-group' to that group name and then
         * the 'data-toggle-on' attribute on the button that will toggle
         * the element on. Then do the same with the button that will
         * toggle the section off but this time, set the 'data-toggle-off'
         * attribute instead of the 'data-toggle-on' attribute.
         */
        let divToToggleQuery = $(this).data("toggle");
        $(divToToggleQuery).toggleClass("hidden");

        let groupName = $(this).data("toggleGroup");
        console.log($(this));
        let toggleOffButton = $("*[data-toggle-group='" + groupName + "'][data-toggle-off]");
        console.log(toggleOffButton);
        let toggleOnButton = $("*[data-toggle-group='" + groupName + "'][data-toggle-on]");
        console.log(toggleOnButton);
        toggleOnButton.toggleClass("hidden");
        toggleOffButton.toggleClass("hidden");
    });
})(jQuery);
