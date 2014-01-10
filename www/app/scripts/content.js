/**
 * This file should be customized.
 */
define(['jquery', 
        'd3'
        ],
        function($, d3) {

    // This will be the module we return.
    var pub = {};

    /**
     * This function is called in main.js and is where we begin the
     * customization of the page.
     */
    pub.onReady = function onReady() {
        // Do something here
        console.log("the onReady function of content.js was called. " +
            "You're up and running!");
    };

    return pub;

});

