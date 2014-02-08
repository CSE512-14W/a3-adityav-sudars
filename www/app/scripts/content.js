/**
 * Does the muscle of the visualization. A big thanks to Mike Bostock for the
 * example below:
 * http://bl.ocks.org/mbostock/4060606
 *
 * Also here, where he uses NaturalEarth to do some amazing things:
 * http://bost.ocks.org/mike/map/
 */
define(['jquery', 
        'd3',
        'topojson',
        'ui-slider'
        ],
        function($, d3, topojson, slider) {
            
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
                var width = 1000;
        var height = 1000;

        var svg = d3.select('#map').append('svg')
            .attr('width', width)
            .attr('height', height);

       
        d3.json('data/maps/india_IN_State_Delhi_Gujarat.json', function(error, india) {
            var subunits = topojson.feature(india, india.objects.places_IN_State_Delhi_Gujarat);

            // According to: http://teczno.com/squares/#4.39/22.83/79.83,
            // centering at 23 80 is ok. The above url is lat/lon. However, the
            // api appears to take lng/lat.
            var projection = d3.geo.mercator()
                .scale(1200)
                .translate([-1250, 900]);

            var path = d3.geo.path()
                .projection(projection);

            svg.append('path')
                .datum(subunits)
                .attr('d', path);

            svg.selectAll('.subunit')
                .data(topojson.feature(india, india.objects.places_IN_State_Delhi_Gujarat).features)
                .enter().append('path')
                .attr('class', function(d) {
                    console.log('d.id: ' + d.id);
                    return 'state ' + d.id;
                })
                .attr('d', path);

        });

        // This will load the csv.
        d3.csv('data/WeeklyData_v2.csv', function(csv) {
            // this will hold the time headings. In the case of weeks it should be
            // things like "Week 1, Week 2".
            var timeNames = [];
            var titleRow = d3.keys(csv[0]);
            for (var i = 1; i < titleRow.length - 1; i++) {
                timeNames.push(titleRow[i]);
            }
            console.log('timeNames: ' + timeNames);
            console.log('timeNames.length: ' + timeNames.length);
            // We're going to make the range of the slider 1 to timeNames.length.
            // If i is selected on the slider, that means we'll index into the
            // timeNames array at i-1.

            $('#slider-div').slider({
                range: 'min',
                value: 1,
                min: 1,
                max: 11,
                slide: function(event, ui) {
                    console.log(ui.value);
                }
            });
            $('#slider-div').slider('values', 0, 11);

            // Now that we have them, we'll set up the slider.
            //$('#slider').simpleSlider({
                //'simple-slider-snap': 'true',
                //'simple-slider-range': '1,10',
                //'simple-slider-step': '1',
                //'simple-slider-steps': 'true'
            //});
            //$('#slider').simpleSlider();
            //$('#slider').bind('slider:changed', function(event, data) {
                //console.log('slider changed to: ' + data.value);
            //});
            //$('#slider').attr('data-slider-snap', 'true');
            //$('#slider').attr('data-slider-range', '1,10');
            //$('#slider').attr('data-slider-step', '1');
            //$('#slider').attr('data-slider-steps', 'true');


        });

    };

    return pub;

});

