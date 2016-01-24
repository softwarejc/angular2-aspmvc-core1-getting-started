System.register(['angular2/http', 'angular2/core'], function(exports_1) {
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var __metadata = (this && this.__metadata) || function (k, v) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
    };
    var http_1, core_1;
    var HelloService;
    return {
        setters:[
            function (http_1_1) {
                http_1 = http_1_1;
            },
            function (core_1_1) {
                core_1 = core_1_1;
            }],
        execute: function() {
            HelloService = (function () {
                function HelloService(http) {
                    this.http = http;
                    this._sayHelloServiceUrl = '/api/hello/';
                }
                // Gets the activations of an specified license from the server
                HelloService.prototype.sayHello = function (name) {
                    var url = this._sayHelloServiceUrl + "?name=" + name;
                    return this.http.get(url).map(function (res) { return res.json().message; });
                };
                HelloService = __decorate([
                    core_1.Injectable(), 
                    __metadata('design:paramtypes', [http_1.Http])
                ], HelloService);
                return HelloService;
            })();
            exports_1("HelloService", HelloService);
        }
    }
});
//# sourceMappingURL=hello.service.js.map