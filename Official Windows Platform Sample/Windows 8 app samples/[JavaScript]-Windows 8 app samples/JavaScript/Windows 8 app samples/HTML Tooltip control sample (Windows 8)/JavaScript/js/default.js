﻿//// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
//// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
//// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//// PARTICULAR PURPOSE.
////
//// Copyright (c) Microsoft Corporation. All rights reserved

(function () {
    "use strict";

    var sampleTitle = "Tooltip control sample";

    var scenarios = [
        { url: "/html/basics.html", title: "Basics" },
        { url: "/html/styling.html", title: "Styling" },
        { url: "/html/scrubbing.html", title: "Scrubbing Example" }
    ];

    function activated(eventObject) {
        if (eventObject.detail.kind === Windows.ApplicationModel.Activation.ActivationKind.launch) {
            // Use setPromise to indicate to the system that the splash screen must not be torn down
            // until after processAll and navigate complete asynchronously.
            eventObject.setPromise(WinJS.UI.processAll().then(function () {
                // Navigate to either the first scenario or to the last running scenario
                // before suspension or termination.
                var url = WinJS.Application.sessionState.lastUrl || scenarios[0].url;
                return WinJS.Navigation.navigate(url);
            }));
        }
    }

    WinJS.Navigation.addEventListener("navigated", function (evt) {
        var url = evt.detail.location;
        var host = document.getElementById("contentHost");
        // Call unload method on current scenario, if there is one
        host.winControl && host.winControl.unload && host.winControl.unload();
        WinJS.Utilities.empty(host);
        WinJS.UI.Pages.render(url, host).then(function () {
            WinJS.Application.sessionState.lastUrl = url;
        });
    });

    WinJS.Namespace.define("SdkSample", {
        sampleTitle: sampleTitle,
        scenarios: scenarios
    });

    WinJS.Application.addEventListener("activated", activated, false);
    WinJS.Application.start();
})();

function switchStyleDark() {
    document.styleSheets[0].disabled = true;
    document.styleSheets[3].disabled = false;

    // switch the class on body tag as a master selector for dark/light versions of styling examples
    WinJS.Utilities.removeClass(document.body, "ui-light");
    WinJS.Utilities.addClass(document.body, "ui-dark");
}

function switchStyleLight() {
    document.styleSheets[0].disabled = false;
    document.styleSheets[3].disabled = true;

    WinJS.Utilities.removeClass(document.body, "ui-dark");
    WinJS.Utilities.addClass(document.body, "ui-light");
}
