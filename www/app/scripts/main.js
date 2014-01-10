require.config({
    paths: {
        "jquery": "vendor/jquery/jquery",
        "underscore": "vendor/underscore-amd/underscore",
        "backbone": "vendor/backbone-amd/backbone",
        "d3": "vendor/d3/d3",
        "content": "content"
    },
    shim: {
        d3: { exports: 'd3' }
    }
});

requirejs(['jquery',
           'content',
           'd3',
           'backbone',
           'underscore'
          ],
          function($, content, d3, backbone, _) {
              var testd3 = d3;
              $(content.onReady());
          }
);
