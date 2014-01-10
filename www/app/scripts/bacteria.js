/**
 * This file should draw a visualization based on the bacteria dataset.
 */
define(['jquery', 
        'd3'
        ],
        function($, d3) {

    // This will be the module we return.
    var pub = {};

    pub.onReady = function onReady() {
        var data = d3.csv.parse('../../resources/dataset_a1-burtin.csv');
        console.log(data);
    
    };

    return pub;


});

