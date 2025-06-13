mergeInto(LibraryManager.library, {
    InitVisibilityListener: function (callback) {
        window.unityVisibilityCallback = callback;
        document.addEventListener("visibilitychange", function () {
            if (document.hidden) {
                window.unityVisibilityCallback(0); // Вкладка скрыта
            } else {
                window.unityVisibilityCallback(1); // Вкладка видима
            }
        });
    }
});