System.register(['angular2/core', './hello.service'], function(exports_1) {
    var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
        var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
        if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
        else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
        return c > 3 && r && Object.defineProperty(target, key, r), r;
    };
    var __metadata = (this && this.__metadata) || function (k, v) {
        if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
    };
    var core_1, hello_service_1;
    var Hello;
    return {
        setters:[
            function (core_1_1) {
                core_1 = core_1_1;
            },
            function (hello_service_1_1) {
                hello_service_1 = hello_service_1_1;
            }],
        execute: function() {
            Hello = (function () {
                function Hello(_helloService) {
                    this._helloService = _helloService;
                }
                Hello.prototype.ngOnInit = function () {
                    this.name = "";
                    this.message = "";
                };
                Hello.prototype.sayHello = function () {
                    var _this = this;
                    // Use hello service to call the API and bind result
                    this._helloService.sayHello(this.name).subscribe(function (response) {
                        _this.message = response;
                    });
                };
                Hello = __decorate([
                    core_1.Component({
                        // Declare the tag name in index.html to where the component attaches
                        selector: 'hello',
                        // Location of the template for this component
                        templateUrl: 'views/hello.view.html',
                        // Dependencies
                        providers: [hello_service_1.HelloService]
                    }), 
                    __metadata('design:paramtypes', [hello_service_1.HelloService])
                ], Hello);
                return Hello;
            })();
            exports_1("Hello", Hello);
        }
    }
});
//# sourceMappingURL=hello.component.js.map