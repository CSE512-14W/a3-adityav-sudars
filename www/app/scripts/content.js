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
        'ui-slider',
        'queue'
        ],
        function($, d3, topojson, slider, queue) {

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

       
        // We need to do two ajax calls--one to get the csv and one to get the
        // TopoJSON. To do this we're going to use queue. It is YET ANOTHER
        // mbostock library that appears to be nice and easy.
        queue()
            .defer(d3.json, 'data/maps/india_IN_State_Delhi_Gujarat.json')
            .defer(d3.csv, 'data/WeeklyData_v2.csv')
            .await(function(error, topoJson, csv) {
                doViz(topoJson, csv);
            });
        
        // This is the function that actually performs the visualization. It
        // expects to get the loaded TopoJSON of the states as well as the 
        // csv with the time series.
        function doViz(topoJson, csv) {
            drawMap(topoJson);
            setUpSlider(csv);
        }

        // This will map the state ids to the actual state object.
        var states = {};
        // this will hold the time headings. In the case of weeks it should be
        // things like "Week 1, Week 2".
        var timeNames = [];

        function drawMap(india) {
            var subunits = topojson.feature(
                    india,
                    india.objects.places_IN_State_Delhi_Gujarat);

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
                .data(topojson.feature(
                            india,
                            india.objects.places_IN_State_Delhi_Gujarat)
                        .features)
                .enter().append('path')
                .attr('class', function(d) {
                    console.log('d.id: ' + d.id);
                    return 'state ' + d.id;
                })
                .attr('d', path);

        }

        // This will load the csv.
        function setUpSlider(csv) {
            var titleRow = d3.keys(csv[0]);
            for (var i = 1; i < titleRow.length - 1; i++) {
                timeNames.push(titleRow[i]);
            }
            console.log('timeNames: ' + timeNames);
            console.log('timeNames.length: ' + timeNames.length);
            // We're going to make the range of the slider 1 to
            // timeNames.length. If i is selected on the slider, that means
            // we'll index into the timeNames array at i-1.
            // First handle the error case where there are no names. This
            // likely would mean something hasn't parsed correctly.
            if (timeNames.length < 1) {
                alert('no names for time periods, big problems');
            }
            // And we'll set the label the first time even before a slide.
            $('#time-label').text(timeNames[0]);
            $('#slider-div').slider({
                range: 'min',
                value: 1,
                min: 1,
                max: timeNames.length,
                slide: onSlide
            });

        }

        function onSlide(event, ui) {
            $('#time-label').text(timeNames[ui.value - 1]);
            console.log(ui.value);

        }
        
        console.log('returning pub');
        d3.select('svg').select('#Rajasthan').style('fill', '#dce');

    };

    return pub;

});

