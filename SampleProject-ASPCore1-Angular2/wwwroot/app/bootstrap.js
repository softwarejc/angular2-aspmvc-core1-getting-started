System.register(['angular2/platform/browser', 'angular2/http', './hello/hello.component', 'rxjs/Rx'], function(exports_1) {
    var browser_1, http_1, hello_component_1;
    return {
        setters:[
            function (browser_1_1) {
                browser_1 = browser_1_1;
            },
            function (http_1_1) {
                http_1 = http_1_1;
            },
            function (hello_component_1_1) {
                hello_component_1 = hello_component_1_1;
            },
            function (_1) {}],
        execute: function() {
            // Bootstrap Angular2 loading Licenses controller
            browser_1.bootstrap(hello_component_1.Hello, [http_1.HTTP_PROVIDERS]).then(function (success) { return console.log('app bootstrapped...'); }, function (error) { return console.log(error); });
        }
    }
});
//# sourceMappingURL=bootstrap.js.map