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
        'topojson'
        ],
        function($, d3, topojson) {

    // This will be the module we return.
    var pub = {};

    var width = 1000;
    var height = 1000;

    var svg = d3.select('body').append('svg')
        .attr('width', width)
        .attr('height', height);

    //queue()
        //.defer(d3.json, 'data/maps/india.json')
        //.await(ready);

    //function ready(error, india) {
        //svg.append('g')
                //.attr('class', 'states')
            //.selectAll('path')
                //.data(topojson.feature(india, india.objects.states).features)
            //.enter().append('path')
                //.attr('class', 'testClass')
                //.attr('d', path);
    //}
    
    d3.json('data/maps/india_IN_State.json', function(error, india) {
        var subunits = topojson.feature(india, india.objects.subunits);

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

        //svg.append('path')
            //.datum(topojson.feature(india, india.objects.subunits))
            //.attr('d', d3.geo.path().projection(d3.geo.mercator()));
    });

    /**
     * This function is called in main.js and is where we begin the
     * customization of the page.
     */
    pub.onReady = function onReady() {
        // Do something here
        console.log("the onReady function of content.js was called. " +
            "You're up and running!");
        var welcome = $('<h1>');
        welcome.text('Welcome! Let\'s get started.');
        $('body').append(welcome);
    };

    return pub;

});

