(function () {
    "use strict";
    const dialogs = $("[data-message]");
    
    dialogs.each((idx, dialog) => {
        const owner = dialog.dataset.dialogOwner !== undefined ? $(`[${dialog.dataset.dialogOwner}]`) : window;

        $(dialog).dialog({
            dialogClass: "dialog",
            modal: dialog.dataset.dialogModal !== undefined ? true : false,
            position: { my: "center", at: "center", of: owner }
        });
    });
})();